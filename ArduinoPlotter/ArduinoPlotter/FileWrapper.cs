using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Xml.Serialization;

namespace ArduinoPlotter {
    class FileWrapper : DataStream {
        private StreamReader reader;
        private GestureData data;
        private string fileName;
        private int curPointIndex;
        public FileWrapper(string fileName) {
            this.fileName = fileName;
        }

        public bool TryToConnect() {
            try {
                curPointIndex = -1;
                reader = new StreamReader(fileName);
                XmlSerializer serializer = new XmlSerializer(typeof(GestureData));
                data = (GestureData)serializer.Deserialize(reader);
                return true;
            } catch (Exception e) {
                return false;
            }
        }

        public Measurement NextSlice() {
            curPointIndex++;
            curPointIndex %= data.Points.Count;
            return data.Points[curPointIndex];
        }

        public void BreakConnection() {
            reader.Close();
        }

        public bool IsConnected() {
            return true;
        }

    }
}
