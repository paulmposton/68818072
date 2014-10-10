using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CardWrapper
{
    public partial class MagTekUserControl : UserControl
    {
        private AxctlUSBHID.AxUSBHID reader = new AxctlUSBHID.AxUSBHID();

        public MagTekUserControl()
        {
            InitializeComponent();
            this.axUSBHID1.PortOpen = true;

        }
    }
}
