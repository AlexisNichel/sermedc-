using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace sermed
{
    public partial class Help : Form
    {
        public Help()
        {
            InitializeComponent();
        }
        private Timer step;
        private void Form3_Load(object sender, EventArgs e)
        {
            step = new Timer();
            step.Interval = (int)TimeSpan.FromMilliseconds(2000).TotalMilliseconds;
            step.Tick += delegate
            {
                label1.Visible = true;
                string[] ports = SerialPort.GetPortNames();
                if (ports.Length > 0)
                {
                    step.Stop();
                    step.Dispose();
                    this.Close();
                }
            };
            step.Start();
        }
        private void Cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
