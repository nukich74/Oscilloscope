using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArduinoPlotter {

    interface DataStream {
        bool TryToConnect();
        Measurement NextSlice();
        void BreakConnection();
        bool IsConnected();
    }

    class ArduinoWrapper : DataStream {

        SerialPort port;
        private string portName;
        private bool connectionStatus;

        public bool IsConnected() {
            return connectionStatus;
        }
        String errorStatus {
            get;
            set;
        }
        public ArduinoWrapper(string portName) {
            this.portName = portName;
        }

        public bool TryToConnect() {
            try {
                port = new SerialPort(portName, 115200, Parity.None, 8, StopBits.One);
                port.Handshake = Handshake.None;
                port.ReadTimeout = 2000;
                port.WriteTimeout = 2000;
                port.Open();
                port.WriteLine(String.Format("{0}", 0));
                connectionStatus = false;
                //TODO это какой-то бред. Нужно формализованное рукопожание. 
                //http://playground.arduino.cc/Csharp/SerialCommsCSharp
                port.ReadLine();
                port.WriteLine(String.Format("{0}", 1));
                connectionStatus = true;
            } catch (Exception e) {
                errorStatus = e.Message;
                connectionStatus = false;
                return false;
            }
            return true;
        }
        public Measurement NextSlice() {
            return new Measurement( Array.ConvertAll(port.ReadLine().Split(' '), int.Parse), DateTime.Now );
        }

        public void BreakConnection() {
            port.Close();
        }
    }
}
