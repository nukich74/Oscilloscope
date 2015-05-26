using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace ArduinoPlotter {

    class Logger {
        private const string TargetPath = ".\\Logs\\";
        private string CurrentLoggingPath;

        public Logger() {
            if (!Directory.Exists(TargetPath)) {
                Directory.CreateDirectory(TargetPath);
            } else {
                string targetName = DateTime.Now.ToString("yyyy_MM_dd");
                Directory.CreateDirectory(TargetPath + targetName);
                CurrentLoggingPath = TargetPath + targetName;
            }
        }

        public void DumpToBinFile(GestureData data, string experimentName = null) {
            if (experimentName == null) {
                experimentName = CurrentLoggingPath + "\\" + DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss") + ".bin";

            }
            using (Stream stream = File.Open(experimentName, FileMode.Create)) {
                BinaryFormatter bin = new BinaryFormatter();
                bin.Serialize(stream, data);
            }
        }

        public void DumpToXmlFile(GestureData data, string experimentName = null) {
            if (experimentName == null) {
                experimentName = CurrentLoggingPath + "\\" + DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss") + ".bin";

            }
            using (Stream stream = File.Open(experimentName, FileMode.Create)) {
                XmlSerializer serializer = new XmlSerializer(typeof(GestureData));
                serializer.Serialize(stream, data);
            }
        }

        public void DumpToSimpleFile(GestureData data, string experimentName = null) {
            if (experimentName == null) {
                experimentName = CurrentLoggingPath + "\\" + DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss") + ".bin";

            }
            using (StreamWriter stream = new StreamWriter(experimentName)) {
                stream.WriteLine(data.Points.Count);
                for (int i = 0; i < data.Points.Count; i++) {
                    StringBuilder builder = new StringBuilder();
                    for (int j = 0; j < data.Points[i].Value.Length; j++) {
                        builder.Append(data.Points[i].Value[j]);
                        builder.Append(" ");
                    }
                    stream.WriteLine(builder.ToString());
                }
            }
        }
    }

    [Serializable()]
    public class GestureData {
        [XmlArray("Points"), XmlArrayItem("Event")]
        public List<Measurement> Points {
            get;
            set;
        }

        [XmlElement("Type")]
        public string Type {
            set;
            get;
        }

        public GestureData(string type, List<Measurement> points) {
            Points = points;
            Type = type;
        }

        public GestureData() {
            Type = "EmpyType";
            Points = new List<Measurement>();
        }

        public void AddMeasurement(Measurement measure) {
            Points.Add(measure);
        }
    }
}
