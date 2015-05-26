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
using System.Collections.Concurrent;
using System.IO.Ports;
using System.Windows.Forms.DataVisualization.Charting;

//1000 точек в секунду на 4 канала, 100 мс
namespace ArduinoPlotter {
    public partial class MainForm : Form {

        Configuration config;
        Logger logger;
        GestureData data = new GestureData();
        Chart currentChart;
        DateTime beginTime;
        Measurement[] plotData;
        List<Chart> charts = new List<Chart>();
        Random rand = new Random();

        public MainForm() {
            config = new Configuration();
            logger = new Logger();
            plotData = new Measurement[Configuration.ShiftSize];
            Configuration.PortToConnect = "none";
            dumpFileQueue = new ConcurrentQueue<Measurement>();
            plotQueue = new ConcurrentQueue<Measurement>();
            InitializeComponent();
            initPorts();
            initDataPrinter();
            initSeries(basicChart);
            //initSeries(filterChart);
            currentChart = basicChart;
            beginTime = DateTime.Now;
        }

        void initSeries(Chart chart) {
            for (int i = 0; i < config.ChannelsCount; i++) {
                Series serie = new Series(i.ToString(), config.FrequencyMaxValue);
                serie.ChartType = SeriesChartType.FastLine;
                serie.XValueType = ChartValueType.UInt32;
                chart.Series.Add(serie);
                for (int j = 0; j < Configuration.PointsPerXAxis; j++) {
                    serie.Points.AddXY(j, 1000 * i + j / 2);
                }
            }
            charts.Add(chart);
        }

        void initDataPrinter() {
            for (int i = 0; i < 3; i++) {
                listBox1.Items.Add(0);
            }
            for (int i = 0; i < Constants.Tests.Length; i++) {
                listBox2.Items.Add(0);
                listBox2.Items[i] = Constants.Tests[i];
            }
        }

        void initPorts() {
            var ports = SerialPort.GetPortNames();
            comboBox1.DataSource = ports;
            Configuration.PortToConnect = comboBox1.Text;
        }

        private void startButton_Click(object sender, EventArgs e) {
            Configuration.NeedToStop = false;
            if (Configuration.PortToConnect == "none") {
                DialogResult result = MessageBox.Show("Choose port to connect!",
                    "Notification",
                    MessageBoxButtons.OK);
            } else {
                if (Configuration.Controller == null) {
                    Configuration.Controller = new ArduinoWrapper(Configuration.PortToConnect);
                }
                while (!Configuration.Controller.TryToConnect()) {
                    DialogResult result = MessageBox.Show("Can't connect to arduino. Abort?",
                        "Notification",
                        MessageBoxButtons.YesNo);
                    if (result == DialogResult.Yes) {
                        return;
                    }
                }
                startButton.Enabled = false;
                Configuration.processPortThread = new Thread(this.processPort);
                Configuration.dumperThread = new Thread(this.dumper);
                Configuration.processPortThread.Start();
                Configuration.dumperThread.Start();
            }
        }

        private void processPort() {
            Measurement slice;
            while (!Configuration.NeedToStop) {
                slice = Configuration.Controller.NextSlice();
                dumpFileQueue.Enqueue(slice);
                config.SlisesCount += 1;
                plotQueue.Enqueue(slice);
            }
            Configuration.Controller.BreakConnection();
        }

        private void stopButton_Click(object sender, EventArgs e) {
            Configuration.NeedToStop = true;
            if (Configuration.processPortThread != null) {
                Configuration.processPortThread.Join();
            }
            if (Configuration.dumperThread != null) {
                Configuration.dumperThread.Join();
            }
            startButton.Enabled = true;
        }

        private void timer1_Tick(object sender, EventArgs e) {
            if (!Configuration.NeedToStop) {
                listBox1.Items[0] = config.DumpedCount;
                listBox1.Items[1] = config.SlisesCount;
                listBox1.Items[2] = config.PlottedCount;
                if (currentChart != null) {
                    UpdateChart(currentChart);
                }
            }
        }

        private void UpdateChart(Chart chart) {
            Measurement value;

            if (Configuration.Controller == null || !Configuration.Controller.IsConnected()) {
                return;
            }

            int adaptiveShiftSize = 0;
            if (config.SlisesCount - config.PlottedCount > Configuration.SpeedUpThreshold) {
                adaptiveShiftSize = 10;
            }
            int shiftValue = Configuration.ShiftSize + adaptiveShiftSize;
            int size = chart.Series[0].Points.Count;

            if (plotData.Length != shiftValue) {
                plotData = new Measurement[shiftValue];
            }

            for (int j = 0; j < shiftValue; ) {
                bool deqResult = plotQueue.TryDequeue(out value);
                if (deqResult == false) {
                    continue;
                }
                plotData[j] = value;
                j++;
                config.PlottedCount += 1;
            }
            for (int i = 0; i < config.ChannelsCount; i++) {
                chart.Series[i].Points.SuspendUpdates();
                for (int p = 0; p < chart.Series[i].Points.Count - shiftValue; p++) {
                    chart.Series[i].Points[p].SetValueY(chart.Series[i].Points[p + shiftValue].YValues[0]);
                }
                for (int p = 0; p < shiftValue; p++) {
                    chart.Series[i].Points[size - shiftValue + p].SetValueY(700 * i + plotData[p][i]);
                }
                chart.Series[i].Points.ResumeUpdates();
            }
        }

        private void dumper() {
            Measurement item;
            while (!Configuration.NeedToStop) {
                if (!dumpFileQueue.TryDequeue(out item)) {
                    continue;
                } else {
                    data.AddMeasurement(item);
                    config.DumpedCount++;
                }
            }
        }

        private void comboBox1_RegionChanged(object sender, EventArgs e) {
            Configuration.PortToConnect = comboBox1.Text;
        }

        ConcurrentQueue<Measurement> dumpFileQueue;
        ConcurrentQueue<Measurement> plotQueue;

        private void settingsToolStripMenuItem_Click(object sender, EventArgs e) {
            //           if (config.NeedToStop) {
            SettingsForm settingsForm = new SettingsForm();
            settingsForm.Show();
            //         } else {
            //        DialogResult result = MessageBox.Show("You can't change params in process",
            //            "Notification",
            //            MessageBoxButtons.OK);
            //    }
        }

        private void refreshUI(Chart chart) {
            chart.Series.SuspendUpdates();
            for (int i = 0; i < config.ChannelsCount; i++) {
                while (chart.Series[i].Points.Count > 0) {
                    chart.Series[i].Points.RemoveAt(chart.Series[i].Points.Count - 1);
                }
            }
            chart.Series.ResumeUpdates();
            listBox1.Items.Clear();
            initSeries(currentChart);
            initDataPrinter();
            config.DumpedCount = 0;
            config.PlottedCount = 0;
            config.SlisesCount = 0;
            plotData = new Measurement[Configuration.ShiftSize];
        }

        private void chartCollection_SelectedIndexChanged(object sender, EventArgs e) {
            if (((TabControl)(sender)).SelectedIndex >= charts.Count) {
                currentChart = null;
            } else {
                currentChart = charts[((TabControl)(sender)).SelectedIndex];
            }
        }

        private void comboBox1_MouseClick(object sender, MouseEventArgs e) {
            initPorts();
        }

        private void saveBtn_Click(object sender, EventArgs e) {
            if (saveFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK) {
                logger.DumpToXmlFile(data, saveFileDialog1.FileName);
            } else {
                logger.DumpToXmlFile(data);
            }
        }

        private void beginTestBtn_Click(object sender, EventArgs e) {
            data = new GestureData();
            label6.ForeColor = Color.Black;
            label4.ForeColor = Color.Black;
            startButton.Enabled = false;
            stopButton.Enabled = false;
            if (Configuration.CurrentTestNumber < 0 || Configuration.CurrentTestNumber >= listBox2.Items.Count) {
                DialogResult result = MessageBox.Show("Choose test", "Attention!", MessageBoxButtons.OK);
            }
            label4.ForeColor = Color.Red;
            label4.Text = "5".ToString();
            timer2.Start();
        }

        private void listBox2_SelectedIndexChanged(object sender, EventArgs e) {
            Configuration.CurrentTestNumber = ((ListBox)sender).SelectedIndex;
        }

        private void timer2_Tick(object sender, EventArgs e) {
            label4.Text = (int.Parse(label4.Text) - 1).ToString();
            if (label4.Text == "0") {
                timer2.Stop();
                label6.Text = "10".ToString();
                startButton_Click(null, null);
                label6.ForeColor = Color.Red;
                timer3.Start();
            }
        }

        private void timer3_Tick(object sender, EventArgs e) {
            label6.Text = (int.Parse(label6.Text) - 1).ToString();
            if (label6.Text == "0") {
                timer3.Stop();
                stopButton_Click(null, null);
                saveBtn_Click(null, null);
                startButton.Enabled = true;
                stopButton.Enabled = true;
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e) {
            Configuration.PortToConnect = comboBox1.Text;
        }
    }
}