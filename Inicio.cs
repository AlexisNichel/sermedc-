using System;
using System.Windows.Forms;
using System.IO;
namespace Sermed
{
    public partial class Inicio : Form
    {
        private Timer tmr;
        private Timer tmrbar;
        public Inicio()
        {
            InitializeComponent();
        }
        private void Inicio_Load(object sender, EventArgs e)
        {
            label1.Text = new sermed.model.VersionClass().version;
            tmr = new Timer();
            tmrbar = new Timer();
            progressBar1.Value = 0;
            String line;
            tmrbar.Tick += delegate
            {
                try
                {
                    var path = Path.GetFullPath("config.txt");
                    StreamReader sr = new StreamReader(path);
                    line = sr.ReadLine();
                    while (line != null)
                    {
                        Console.WriteLine(line);
                        line = sr.ReadLine();
                    }
                    sr.Close();
                    Console.ReadLine();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Err.:"+ ex.Message);
                    this.Hide();
                    Form1 objUI = new Form1();
                    objUI.Closed += (s, args) => this.Close();
                    objUI.Show();
                    tmrbar.Stop();
                    tmrbar.Dispose();
                    tmr.Stop();
                    tmr.Dispose();
                }
                finally
                {
                    Console.WriteLine("Executing finally block.");
                }
                if (progressBar1.Value < 100)
                    progressBar1.Value += 1;
            };
            tmr.Tick += delegate {
                this.Hide();
                Form2 objUI = new Form2();
                objUI.Closed += (s, args) => this.Close();
                objUI.Show();
                tmrbar.Stop();
                tmrbar.Dispose();
                tmr.Stop();
                tmr.Dispose();
            };
            tmrbar.Interval = (int)TimeSpan.FromMilliseconds(10).TotalMilliseconds;
            tmr.Interval = (int)TimeSpan.FromSeconds(2).TotalMilliseconds;
            tmrbar.Start();
            tmr.Start();
        }
    }
}