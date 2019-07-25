using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO.Ports;
using System.IO;
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
        public SerialPort ComPort = new SerialPort();
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
            if (!String.IsNullOrEmpty(config.cmbReintento))
                cmbReintento.SelectedItem = config.cmbReintento;
            else
                cmbReintento.SelectedIndex = 1;
            if (!String.IsNullOrEmpty(config.cmbTimeout))
                cmbTimeout.SelectedItem = config.cmbTimeout;
            else
                cmbTimeout.SelectedIndex = 3;
            if (!String.IsNullOrEmpty(config.equipo))
                equipo.Text = config.equipo;
            else
                equipo.Text = "0";
        }
        public void updatePorts()
        {
            string[] ports = SerialPort.GetPortNames();
            foreach (string port in ports)
            {
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
            if (cmbPortName.SelectedIndex != -1 & cmbTimeout.SelectedIndex != -1 & cmbReintento.SelectedIndex != -1)
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
            if (cmbPortName.SelectedIndex != -1 & cmbTimeout.SelectedIndex != -1 & cmbReintento.SelectedIndex != -1)
            {
                if (!ComPort.IsOpen)
                {
                    connect();
                }
                var hexSexonds = Convert.ToString(long.Parse(cmbTimeout.SelectedItem.ToString()), 16);
                if (hexSexonds.Length == 1)
                    hexSexonds = "0" + hexSexonds;
                string command = new SensorClass().SetTimeout.Replace("[value]", hexSexonds.ToUpper());
                byte[] data = new Shared().HexStringToByteArray(command);
                byte[] cheked = new Shared().checksum(data);
                ComPort.Write(cheked, 0, cheked.Length);
                string userRoot = Environment.GetEnvironmentVariable("USERPROFILE");
                var path = Path.GetFullPath(userRoot + "\\config.txt");
                try
                {
                    StreamWriter sw = new StreamWriter(path, false, Encoding.ASCII);
                    var config = String.Empty;
                    config += cmbPortName.SelectedItem + "\r\n";
                    config += cmbTimeout.SelectedItem + "\r\n";
                    config += cmbReintento.SelectedItem + "\r\n";
                    config += "115200\r\n";
                    config += "None\r\n";
                    config += "8\r\n";
                    config += "1\r\n";
                    config += equipo.Text + "\r\n";
                    sw.Write(config);
                    sw.Close();
                    MessageBox.Show(this, "Configuración guardada", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    disconnect();
                    this.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(this, "Error al guardar configuración:", "", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    Console.WriteLine("Exception: " + ex.Message);
                }
                finally
                {
                    Console.WriteLine("Executing finally block.");
                }
            }
            else
                MessageBox.Show("Por favor seleccióne todos los parámetros requeridos.", "", MessageBoxButtons.OK, MessageBoxIcon.Stop);
        }
    }
}