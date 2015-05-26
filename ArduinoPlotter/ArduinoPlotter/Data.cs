using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ArduinoPlotter {

    [Serializable()]
    public class Measurement {

        [XmlElement("Value")]
        public int[] Value {
            get;
            set;
        }

        [XmlElement("Time")]
        public DateTime DateTime {
            get;
            set;
        }

        [XmlIgnore]
        public string ProxyDateTime {
            get {
                return this.DateTime.ToString("yyyy-MM-dd HH:mm:ss:fff");
            }
            set {
                this.DateTime = DateTime.Parse(value);
            }
        }

        public Measurement() {
            DateTime = System.DateTime.Now;
            Value = null;
        }

        public Measurement(int[] initValue, DateTime occurTime) {
            this.DateTime = occurTime;
            this.Value = initValue;
        }

        public int this[int key] {
            get {
                return Value[key];
            }
            set {
                Value[key] = value;
            }
        }
    }
}
