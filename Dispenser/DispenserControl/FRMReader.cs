using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;

namespace DispenserControl
{
    public class FRMReader
    {
        private SerialPort sp;
        private int[] RTX = new int[32];
        private const int intDelay = 500;
        int n;

        private bool RXComplete = false;
        private bool booChkHead = false;
        private bool booOK = false;

        private int intResult;
        private int intByterRX;

        public FRMReader()
        {
            sp = new SerialPort();
            sp.DataReceived += new SerialDataReceivedEventHandler(sp_DataReceived);
        }

        private bool waitRXComplete()
        {
            for (int i = 0; i < 100; i++)
            {
                delay(4);
                if (RXComplete)
                {
                    return true;
                }
            }
            return false;
        }

        public void setLED(int b)
        {
            if (b == 0)
            {
                RTX[3] = 0x04;
                RTX[4] = 0x6A;
                RTX[5] = 0;
                sendByte(8);
                waitRXComplete();
            }
            else
            {
                RTX[3] = 0x04;
                RTX[4] = 0x6A;
                RTX[5] = 0x10;
                RTX[6] = 0x3;
                sendByte(9);
                waitRXComplete();
            }
        }

        public void setSound(int b)
        {
            setSound(true);
            delay(b * 10);
            setSound(false);
        }

        public void setSound(bool booSound)
        {
            if (!booSound)
            {
                RTX[3] = 0x04;
                RTX[4] = 0x6A;
                RTX[5] = 1;
                sendByte(8);
                waitRXComplete();
            }
            else
            {
                RTX[3] = 0x04;
                RTX[4] = 0x6A;
                RTX[5] = 0x10;
                RTX[6] = 0x2;
                sendByte(9);
                waitRXComplete();
            }
        }

        public bool chkCard()
        {
            RTX[3] = 0x04;
            RTX[4] = 0x46;
            RTX[5] = 0x52;
            sendByte(8);
            waitRXComplete();
            return booOK;
        }

        public string Init1()
        {
            string strTagNo = "";
            RTX[3] = 0x04;
            RTX[4] = 0x47;
            RTX[5] = 0x04;
            sendByte(8);
            waitRXComplete();
            if (booOK)
            {
                for (int i = 6; i < 10; i++)
                {
                    string strTmp = Convert.ToString(RTX[i], 16).ToUpper();//.PadLeft(2, '0').PadRight(3, ' ')
                    if (strTmp.Length < 2)
                    {
                        strTmp = "0" + strTmp;
                    }
                    strTagNo += strTmp;
                }
            }
            return strTagNo;
        }

        public bool Connect()
        {
            setLED(1);
            delay(10);
            setSound(10);
            delay(200);
            setLED(0);
            delay(20);
            setLED(1);
            delay(10);
            setSound(10);
            delay(200);
            setLED(0);
            return true;
        }

        private void sendByte(int nb)
        {
            booChkHead = true;
            n = 0;
            booOK = false;
            RXComplete = false;
            RTX[0] = 0x02;
            RTX[1] = 0;
            RTX[2] = 0;
            RTX[nb - 2] = 0;
            RTX[nb - 2] = calSum();
            RTX[nb - 1] = 3;
            byte[] sendByte = new byte[nb];
            for (int i = 0; i < nb; i++)
            {
                sendByte[i] = (byte)RTX[i];
                RTX[i] = 255;
            }
            sp.Write(sendByte, 0, nb);
        }

        //private byte calSum()
        private int calSum()
        {
            byte b1 = 0;
            int b = 0;
            int no = RTX[3] + 3;
            bool booCheckChar = false;
            b = RTX[3];
            if (b == 0x10)
            {
                b = 0;
                no = RTX[4] + 3;
            }
            for (int i = 4; i < no; i++)
            {
                if (!booCheckChar && RTX[i] == 0x10)
                {
                    booCheckChar = true;
                    continue;
                }
                b = b + RTX[i];
                b1 = (byte)b;
                b = (int)b1;
                booCheckChar = false;
            }
            return b1;
        }

        private bool booChar = false;

        private void sp_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            int intBytes = sp.BytesToRead;
            byte[] bytes = new byte[intBytes];
            sp.Read(bytes, 0, intBytes);

            for (int cnt = 0; cnt < intBytes; cnt++)
            {
                RTX[n] = bytes[cnt];
                n++;
                if (booChkHead == true && n > 2)
                {
                    if (RTX[n - 1] == 0 && RTX[n - 2] == 0 && RTX[n - 3] == 2)
                    {
                        booChkHead = false;
                    }
                }
                else if (booChkHead == false)
                {
                    if (!booChar)
                    {
                        if (bytes[cnt] == 0x10)
                            booChar = true;
                    }
                    else
                    {
                        n = n - 2;
                        RTX[n] = bytes[cnt];
                        n++;
                        booChar = false;
                        continue;
                    }
                    if (!booChar && bytes[cnt] == 3)
                    {

                        //string RxString = "";
                        //for (int i = 0; i < n; i++)
                        //{
                        //    string strTmp = Convert.ToString(RTX[i], 16).ToUpper();//.PadLeft(2, '0').PadRight(3, ' ')
                        //    if (strTmp.Length < 2)
                        //    {
                        //        strTmp = "0" + strTmp;
                        //    }
                        //    RxString += strTmp + " ";
                        //}

                        if (RTX[3] == 16 && RTX[6] == 0)
                            booOK = true;
                        else if (RTX[5] == 0)
                            booOK = true;

                        //RxString = "Tx: " + RxString;
                        n = 0;
                        RXComplete = true;
                        //booChkHead = true;
                        //this.BeginInvoke(new SetTextDeleg(si_DataReceived), new object[] { RxString });
                    }
                }
            }
        }

        public bool Open(string strPort)
        {
            bool booOpen = false;
            sp.Close();
            sp.BaudRate = 38400;
            sp.PortName = strPort;
            try
            {
                sp.Open();
                booOpen = true;
            }
            catch (Exception)
            {
            }
            return booOpen;
        }

        public void Close()
        {
            sp.Close();
        }

        public void InLift()
        {
            sp.DtrEnable = true;
            delay(intDelay);
            sp.DtrEnable = false;
        }

        public void OutLift()
        {
            sp.RtsEnable = true;
            delay(intDelay);
            sp.RtsEnable = false;
        }

        private void delay(int i)
        {
            System.Threading.Thread.Sleep(i);
        }

    }
}
