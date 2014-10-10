using System;
namespace CardWrapper
{
    partial class MagTekUserControl
    {
        public delegate void WrapperDataRecievedDelegate(object sender, EventArgs e);
        public event WrapperDataRecievedDelegate WrapperDataRecieved;
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MagTekUserControl));
            this.axUSBHID1 = new AxctlUSBHID.AxUSBHID();
            ((System.ComponentModel.ISupportInitialize)(this.axUSBHID1)).BeginInit();
            this.SuspendLayout();
            // 
            // axUSBHID1
            // 
            this.axUSBHID1.Enabled = true;
            this.axUSBHID1.Location = new System.Drawing.Point(41, 37);
            this.axUSBHID1.Name = "axUSBHID1";
            this.axUSBHID1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axUSBHID1.OcxState")));
            this.axUSBHID1.Size = new System.Drawing.Size(70, 68);
            this.axUSBHID1.TabIndex = 0;
            // 
            // MagTekUserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.axUSBHID1);
            this.Name = "MagTekUserControl";
            ((System.ComponentModel.ISupportInitialize)(this.axUSBHID1)).EndInit();
            this.ResumeLayout(false);

            this.axUSBHID1.CardDataChanged += new System.EventHandler(this.axUSBHID1_CardDataChanged);
        }
        public void axUSBHID1_CardDataChanged(object sender, EventArgs e)
        {
            if (WrapperDataRecieved != null)
            {
                WrapperDataRecieved(sender, e);
            }
            int i = 1;
        }

        #endregion

        private AxctlUSBHID.AxUSBHID axUSBHID1;

    }
}
