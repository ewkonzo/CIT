using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;

namespace DispenserControl
{
    public class LEDDisplay
    {
        SerialPort controlSerialPort;
        string portName;
        public bool sendData = false;
        public LEDDisplay()
        {
            portName = "COM1";
        }

        /// <summary>
        /// /// Dispenser Instance 
        /// // parameter set port name string for SerialPort
        /// </summary> 
        public LEDDisplay(string portName)
        {
            this.portName = portName;
        }

        /// <summary>
        /// set port name string
        /// </summary> 
        public void setPortName(string portName)
        {
            this.portName = portName;
        }
        /// <summary>
        /// Open serial port to use dispenser controllor
        /// </summary> 
        public bool openPort()
        {
            try
            {
                controlSerialPort = new SerialPort(portName);
                controlSerialPort.BaudRate = 9600;
                controlSerialPort.Parity = Parity.None;
                controlSerialPort.StopBits = StopBits.One;
                controlSerialPort.DataBits = 8;
                controlSerialPort.Handshake = Handshake.None;
                controlSerialPort.DataReceived += controlSerialPort_DataReceived;
                controlSerialPort.Close();
                controlSerialPort.Open();
                if (controlSerialPort.IsOpen)
                    setText("Test", 3);
                Thread.Sleep(500);
                return sendData;
            }
            catch (Exception) {
                return false;
            }
        }

        private void controlSerialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            sendData = true;
        }

        /// <summary>
        /// Close serial port
        /// </summary> 
        public bool closePort()
        {
            if (controlSerialPort.IsOpen)
                controlSerialPort.Close();
            return !controlSerialPort.IsOpen;
        }

        /// <summary>
        /// return boolean status serial port is open
        /// </summary> 
        public bool isOpen()
        {
            return controlSerialPort.IsOpen;
        }

        /// <summary>
        /// string max length 128 character
        /// int mode 1-6
        /// [1] Right to Left
        /// [2] Left to Right
        /// [3] Stop and Right to Left
        /// [4] Right to Left Split Word
        /// [5] Left to Right and Right to Left 8 character
        /// [6] Up and Right to Left
        /// </summary> 
        public bool setText(string text, int mode) {
            bool result = false;
            try
            {
                string runPattern = "30";
                if (mode == 1) runPattern = "30";
                if (mode == 2) runPattern = "31";
                if (mode == 3) runPattern = "32";
                if (mode == 4) runPattern = "33";
                if (mode == 5) runPattern = "34";
                if (mode == 6) runPattern = "35";

                string strHex = "00,00,FE,00,32,32,30," + runPattern + ",30,30,30,";//Headers
                string input = text;// Data Text 
                char[] values = input.ToCharArray();
                foreach (char letter in values)
                {
                    // Get the integral value of the character. 
                    int value = Convert.ToInt32(letter);
                    // Convert the decimal value to a hexadecimal value in string form. 
                    string hexOutput = String.Format("{0:X}", value);
               //     Console.WriteLine("Hexadecimal value of {0} is {1}", letter, hexOutput);
                    strHex += hexOutput + ",";
                }
                strHex += "0D,55,AA,32,01,32,30,30,38,30,36,30,31,30,30,38,34,34,34,38,31";//Tails

                string[] boxHex = strHex.Split(','); //Data Hex Split by ,
                byte[] test3 = new byte[boxHex.Length];
                int i = 0;
                foreach (string hex in boxHex)
                {
                    int value = Convert.ToInt32(hex, 16);
                    test3[i] = (byte)value;
                    i++;
                }
                controlSerialPort.Write(test3, 0, i);
                result = true;
            }
            catch (Exception ex)
            {
                result = false;
            }
            return result;
        }

    }
}
