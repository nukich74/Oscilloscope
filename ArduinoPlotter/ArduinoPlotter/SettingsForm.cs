using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ArduinoPlotter {
    public partial class SettingsForm : Form {
        public SettingsForm() {
            InitializeComponent();
            this.shiftBar.Maximum = Configuration.PointsPerXAxis / 20; //last value=5
            this.shiftBar.Value = Configuration.ShiftSize;
            this.missPointBar.Value = (int)(Configuration.MissValuesPersentage * 100);
        }

        private void missPointBar_Scroll(object sender, System.EventArgs e) {
            //      Configuration.MissValuesPersentage = ((TrackBar)sender).Value / 100.0;
        }

        private void shiftBar_Scroll(object sender, EventArgs e) {
            Configuration.ShiftSize = ((TrackBar)sender).Value;
        }

        private void RButton_CheckedChanged(object sender, EventArgs e) {
            Configuration.NeedToStop = true;
            while (Configuration.dumperThread.IsAlive || Configuration.processPortThread.IsAlive) {
                Thread.Sleep(100);
            }
            if (arduinoRButton.Checked == true) {
                if (Configuration.Controller != null && Configuration.Controller.IsConnected()) {
                    Configuration.Controller.BreakConnection();
                }
                Configuration.Controller = new ArduinoWrapper(Configuration.PortToConnect);
            } else if (fileRButton.Checked == true) {
                if (Configuration.Controller != null && Configuration.Controller.IsConnected()) {
                    Configuration.Controller.BreakConnection();
                }
                if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK) {
                    Configuration.Controller = new FileWrapper(openFileDialog1.FileName);
                }
            } else {
                Debug.Assert(false);
            }
        } 

    }
}
