using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;

namespace DispenserControl
{
    public class DispenserControllor
    {
        SerialPort controlSerialPort;
        string portName;
        /// <summary>
        /// Dispenser Instance 
        /// Default SerialPort : COM1
        /// </summary> 
        public DispenserControllor()
        {
            portName = "COM1";
        }

        /// <summary>
        /// /// Dispenser Instance 
        /// // parameter set port name string for SerialPort
        /// </summary> 
        public DispenserControllor(string portName)
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

        public SerialPort portHandle;
        /// <summary>
        /// Open serial port to use dispenser controllor
        /// </summary> 
        public bool openPort()
        {
            controlSerialPort = new SerialPort(portName);
            controlSerialPort.BaudRate = 19200;
            controlSerialPort.Parity = Parity.None;
            controlSerialPort.StopBits = StopBits.One;
            controlSerialPort.DataBits = 8;
            controlSerialPort.Handshake = Handshake.None;
            controlSerialPort.Close();
            controlSerialPort.Open();
            portHandle = controlSerialPort;
            return controlSerialPort.IsOpen;
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
        /// pay out command
        /// </summary> 
        public void payOut(){
            if (controlSerialPort.IsOpen)
                controlSerialPort.Write("CP\r");
        }

        /// <summary>
        /// reset command
        /// </summary> 
        public void reset() {
            if (controlSerialPort.IsOpen)
                controlSerialPort.Write("CR\r");
        }

        /// <summary>
        /// callback card command
        /// </summary> 
        public void callBack() {
            if (controlSerialPort.IsOpen)
                controlSerialPort.Write("CB\r");
        }

        /// <summary>
        /// lift in command
        /// </summary> 
        public void liftIn() {
            if (controlSerialPort.IsOpen)
                controlSerialPort.Write("LI\r");
        }

        /// <summary>
        /// lift out command
        /// </summary> 
        public void liftOut() {
            if (controlSerialPort.IsOpen)
                controlSerialPort.Write("LO\r");
        }
    }
}
