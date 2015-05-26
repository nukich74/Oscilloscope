namespace ArduinoPlotter {
    partial class SettingsForm {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.shiftBar = new System.Windows.Forms.TrackBar();
            this.label3 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.missPointBar = new System.Windows.Forms.TrackBar();
            this.dTypeGroupBox = new System.Windows.Forms.GroupBox();
            this.fileRButton = new System.Windows.Forms.RadioButton();
            this.arduinoRButton = new System.Windows.Forms.RadioButton();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            ((System.ComponentModel.ISupportInitialize)(this.shiftBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.missPointBar)).BeginInit();
            this.dTypeGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // shiftBar
            // 
            this.shiftBar.Location = new System.Drawing.Point(117, 65);
            this.shiftBar.Name = "shiftBar";
            this.shiftBar.Size = new System.Drawing.Size(104, 42);
            this.shiftBar.SmallChange = 2;
            this.shiftBar.TabIndex = 6;
            this.shiftBar.TickFrequency = 0;
            this.shiftBar.Scroll += new System.EventHandler(this.shiftBar_Scroll);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(43, 65);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(51, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Shift Size";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Enabled = false;
            this.label1.Location = new System.Drawing.Point(12, 28);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(106, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "MissPointPersentage";
            // 
            // missPointBar
            // 
            this.missPointBar.Enabled = false;
            this.missPointBar.Location = new System.Drawing.Point(117, 28);
            this.missPointBar.Maximum = 100;
            this.missPointBar.Name = "missPointBar";
            this.missPointBar.Size = new System.Drawing.Size(104, 42);
            this.missPointBar.TabIndex = 3;
            this.missPointBar.TickFrequency = 100;
            this.missPointBar.Scroll += new System.EventHandler(this.missPointBar_Scroll);
            // 
            // dTypeGroupBox
            // 
            this.dTypeGroupBox.Controls.Add(this.fileRButton);
            this.dTypeGroupBox.Controls.Add(this.arduinoRButton);
            this.dTypeGroupBox.Location = new System.Drawing.Point(15, 99);
            this.dTypeGroupBox.Name = "dTypeGroupBox";
            this.dTypeGroupBox.Size = new System.Drawing.Size(265, 50);
            this.dTypeGroupBox.TabIndex = 7;
            this.dTypeGroupBox.TabStop = false;
            this.dTypeGroupBox.Text = "DataStreamType";
            // 
            // fileRButton
            // 
            this.fileRButton.AutoSize = true;
            this.fileRButton.Location = new System.Drawing.Point(102, 20);
            this.fileRButton.Name = "fileRButton";
            this.fileRButton.Size = new System.Drawing.Size(41, 17);
            this.fileRButton.TabIndex = 1;
            this.fileRButton.TabStop = true;
            this.fileRButton.Text = "File";
            this.fileRButton.UseVisualStyleBackColor = true;
            // 
            // arduinoRButton
            // 
            this.arduinoRButton.AutoSize = true;
            this.arduinoRButton.Checked = true;
            this.arduinoRButton.Location = new System.Drawing.Point(17, 20);
            this.arduinoRButton.Name = "arduinoRButton";
            this.arduinoRButton.Size = new System.Drawing.Size(61, 17);
            this.arduinoRButton.TabIndex = 0;
            this.arduinoRButton.TabStop = true;
            this.arduinoRButton.Text = "Arduino";
            this.arduinoRButton.UseVisualStyleBackColor = true;
            this.arduinoRButton.CheckedChanged += new System.EventHandler(this.RButton_CheckedChanged);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // SettingsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(292, 273);
            this.Controls.Add(this.dTypeGroupBox);
            this.Controls.Add(this.shiftBar);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.missPointBar);
            this.Controls.Add(this.label1);
            this.Name = "SettingsForm";
            this.Text = "SettingsForm";
            ((System.ComponentModel.ISupportInitialize)(this.shiftBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.missPointBar)).EndInit();
            this.dTypeGroupBox.ResumeLayout(false);
            this.dTypeGroupBox.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TrackBar shiftBar;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TrackBar missPointBar;
        private System.Windows.Forms.GroupBox dTypeGroupBox;
        private System.Windows.Forms.RadioButton fileRButton;
        private System.Windows.Forms.RadioButton arduinoRButton;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
    }
}