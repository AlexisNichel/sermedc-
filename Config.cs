using System;
using System.Windows.Forms;
using System.IO.Ports;
using sermed.model;
using Microsoft.Win32;
using sermed.shared;
namespace Sermed
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        private Timer step;
        public SerialPort ComPort = new SerialPort();
        public Boolean AlertPermit = true;
        private void Form1_Load(object sender, EventArgs e)
        {
            groupBox4.Enabled = false;
            var config = new Shared().GetConfig();
            updatePorts();
            if (!String.IsNullOrEmpty(config.cmbPortName))
                cmbPortName.SelectedItem = config.cmbPortName;
            else
            {
                if (cmbPortName.Items.Count > 0)
                    cmbPortName.SelectedIndex = 0;
            }
            if (!String.IsNullOrEmpty(config.cmbTimeout))
                cmbTimeout.SelectedItem = config.cmbTimeout;
            else
                cmbTimeout.SelectedIndex = 3;
            if (!String.IsNullOrEmpty(config.equipo))
            {
                equipo.Text = config.equipo;
                equipo.Enabled = false;
            }
            else
                equipo.Text = "0";
        }
        public void updatePorts()
        {
            string[] ports = SerialPort.GetPortNames();
            if (ports.Length == 0 && AlertPermit)
            {
                step = new Timer();
                step.Interval = (int)TimeSpan.FromMilliseconds(2000).TotalMilliseconds;
                step.Tick += delegate
                {
                    ports = SerialPort.GetPortNames();
                    if (ports.Length > 0)
                    {
                        step.Stop();
                        step.Dispose();
                        foreach (string port in ports)
                            cmbPortName.Items.Add(port);
                        cmbPortName.SelectedIndex = 0;

                    }
                };
                step.Start();
                AlertPermit = false;
                sermed.Help objUI = new sermed.Help();
                objUI.Show();
            }
            else
            {
                foreach (string port in ports)
                    cmbPortName.Items.Add(port);
            }
        }
        private void BtnLedOn(object sender, EventArgs e)
        {
            byte[] data = new Shared().HexStringToByteArray(new SensorClass().LedOn);
            ComPort.Write(data, 0, data.Length);
        }
        private void BtnLedOff(object sender, EventArgs e)
        {
            byte[] data = new Shared().HexStringToByteArray(new SensorClass().LedOff);
            ComPort.Write(data, 0, data.Length);
        }
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (ComPort.IsOpen) ComPort.Close();
        }
        public void connect()
        {
            bool error = false;
            if (cmbPortName.SelectedIndex != -1 & cmbTimeout.SelectedIndex != -1)
            {
                ComPort.PortName = cmbPortName.Text;
                ComPort.BaudRate = 115200;
                ComPort.Parity = (Parity)Enum.Parse(typeof(Parity), "None");
                ComPort.DataBits = 8;
                ComPort.StopBits = (StopBits)Enum.Parse(typeof(StopBits), "1");
                try
                {
                    ComPort.Open();
                }
                catch (UnauthorizedAccessException) { error = true; }
                catch (System.IO.IOException) { error = true; }
                catch (ArgumentException) { error = true; }
                if (error) MessageBox.Show(this, "No se pudo abrir el puerto COM. Lo más probable es que ya esté en uso o haya sido eliminado.", "Puerto no válido", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
            else
                MessageBox.Show("Por favor seleccióne todos los parámetros requeridos.", "Seleccionar puerto", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            if (ComPort.IsOpen)
            {
                btnConnect.Text = "Desconectar";
                groupBox3.Enabled = false;
                groupBox4.Enabled = true;
            }
        }
        public void disconnect()
        {
            ComPort.Close();
            btnConnect.Text = "Conectar";
            groupBox3.Enabled = true;
            groupBox4.Enabled = false;
        }
        private void btnConnect_Click(object sender, EventArgs e)
        {
            if (ComPort.IsOpen)
                disconnect();
            else
                connect();
        }
        private void BtnCancel_Click(object sender, EventArgs e)
        {
            if (ComPort.IsOpen) ComPort.Close();
            this.Close();
        }
        private void BtnSave(object sender, EventArgs e)
        {
            RegistryKey key = Registry.CurrentUser.OpenSubKey("sermed");
            key = Registry.CurrentUser.CreateSubKey("Software\\Classes\\" + "sermed");
            key.SetValue(string.Empty, "URL:sermed Protocol");
            key.SetValue("URL Protocol", string.Empty);
            key = key.CreateSubKey(@"shell\open\command");
            string applicationLocation = typeof(Program).Assembly.Location;
            key.SetValue("", "\"" + applicationLocation + "\" \"%1\"");
            key.Close();
            if (cmbPortName.SelectedIndex != -1 & cmbTimeout.SelectedIndex != -1)
            {
                if (!ComPort.IsOpen)
                    connect();
                var hexSexonds = Convert.ToString(long.Parse(cmbTimeout.SelectedItem.ToString()), 16);
                if (hexSexonds.Length == 1)
                    hexSexonds = "0" + hexSexonds;
                string command = new SensorClass().SetTimeout.Replace("[value]", hexSexonds.ToUpper());
                byte[] data = new Shared().HexStringToByteArray(command);
                byte[] cheked = new Shared().checksum(data);
                ComPort.Write(cheked, 0, cheked.Length);
                if (ComPort.IsOpen) ComPort.Close();
                try
                {
                    sermed.Properties.Settings.Default.Port = cmbPortName.SelectedItem.ToString();
                    sermed.Properties.Settings.Default.TimeOut = cmbTimeout.SelectedItem.ToString();
                    sermed.Properties.Settings.Default.Equipo = equipo.Text.ToString();
                    sermed.Properties.Settings.Default.Save();
                    MessageBox.Show(this, "Configuración guardada", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    disconnect();
                    this.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(this, "Error al guardar configuración:", "", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    Console.WriteLine("Exception: " + ex.Message);
                }
            }
            else
                MessageBox.Show("Por favor seleccióne todos los parámetros requeridos.", "", MessageBoxButtons.OK, MessageBoxIcon.Stop);
        }
    }
}