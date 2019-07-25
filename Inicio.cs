using System;
using System.Windows.Forms;
using System.IO;
namespace Sermed
{
    public partial class Inicio : Form
    {
        private Timer trm;
        private Timer tmrbar;

        public Inicio()
        {
            InitializeComponent();
        }
        private void Inicio_Load(object sender, EventArgs e)
        {
            string[] args = Environment.GetCommandLineArgs();
            label1.Text = new sermed.model.VersionClass().version;
            trm = new Timer();
            tmrbar = new Timer();
            trm.Interval = (int)TimeSpan.FromSeconds(2).TotalMilliseconds;
            tmrbar.Interval = (int)TimeSpan.FromMilliseconds(15).TotalMilliseconds;
            progressBar1.Value = 0;
            tmrbar.Tick += delegate
            {
                if (progressBar1.Value < 100)
                    progressBar1.Value += 1;
            };
            trm.Tick += delegate
            {
                this.Hide();
                Form1 objUI = new Form1();
                objUI.Closed += (s, arg) => this.Close();
                objUI.Show();
                tmrbar.Stop();
                tmrbar.Dispose();
                trm.Stop();
                trm.Dispose();
            };
            tmrbar.Start();
            trm.Start();
        }
    }
}