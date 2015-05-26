using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ArduinoPlotter {
    class Configuration {
        static public int CurrentTestNumber {
            get;
            set;
        }
        static public Thread processPortThread {
            get;
            set;
        }

        static public Thread dumperThread {
            set;
            get;
        }

        public int SlisesCount {
            get;
            set;
        }

        public int DumpedCount {
            get;
            set;
        }

        public int PlottedCount {
            get;
            set;
        }

        public int ChannelsCount {
            get;
            set;
        }

        static public int ShiftSize {
            get;
            set;
        }

        static public double MissValuesPersentage {
            get;
            set;
        }

        public static DataStream Controller {
            get;
            set;
        }

        public static string PortToConnect {
            get;
            set;
        }

        public int FrequencyMaxValue {
            get;
            set;
        }

        static public int PointsPerXAxis {
            get;
            set;
        }

        static public bool NeedToStop {
            get;
            set;
        }

        public Configuration() {
            ShiftSize = 7;
            NeedToStop = true;
            PointsPerXAxis = 500;
            FrequencyMaxValue = 5000;
            MissValuesPersentage = 0.5;
            ChannelsCount = 4;
            DumpedCount = 0;
            PlottedCount = 0;
            SlisesCount = 0;
            CurrentTestNumber = -1;
        }

        public static int SpeedUpThreshold {
            get {
                return 1000;
            }
        }
    }
}
