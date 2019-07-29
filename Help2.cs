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
    public partial class Help2 : Form
    {
        public Help2()
        {
            InitializeComponent();
        }
        private Timer step;
        private void Form3_Load(object sender, EventArgs e)
        {
            step = new Timer();
            var config = new shared.Shared().GetConfig();
            label3.Text = "Dispositivo no encontrado en el puerto " + config.cmbPortName;
            step.Interval = (int)TimeSpan.FromMilliseconds(2000).TotalMilliseconds;
            step.Tick += delegate
            {
                label1.Visible = true;
                pictureBox1.Visible = true;
                string[] ports = SerialPort.GetPortNames();
                if (ports.Length > 0)
                {
                    foreach (string port in ports)
                    {
                        if(port == config.cmbPortName)
                        {
                            step.Stop();
                            step.Dispose();
                            this.Close();
                        }
                    }
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
