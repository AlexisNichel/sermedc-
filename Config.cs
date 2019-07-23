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
            var config = new Shared().GetConfig();
            updatePorts();          
            cmbBaudRate.SelectedIndex = 7;
            cmbDataBits.SelectedIndex = 1;
            cmbStopBits.SelectedIndex = 0;
            cmbParity.SelectedIndex = 0;
            cmbReintento.SelectedIndex = 1;
            cmbTimeout.SelectedIndex = 3;
            groupBox4.Enabled = false;
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
            if (cmbPortName.SelectedIndex != -1 & cmbBaudRate.SelectedIndex != -1 & cmbParity.SelectedIndex != -1 & cmbDataBits.SelectedIndex != -1 & cmbStopBits.SelectedIndex != -1 & cmbTimeout.SelectedIndex != -1 & cmbReintento.SelectedIndex != -1)
             {
                ComPort.PortName = cmbPortName.Text;
                ComPort.BaudRate = int.Parse(cmbBaudRate.Text);      
                ComPort.Parity = (Parity)Enum.Parse(typeof(Parity), cmbParity.Text); 
                ComPort.DataBits = int.Parse(cmbDataBits.Text);      
                ComPort.StopBits = (StopBits)Enum.Parse(typeof(StopBits), cmbStopBits.Text);
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
                groupBox1.Enabled = false;
                groupBox3.Enabled = false;
                groupBox4.Enabled = true;
            }
        }
        public void disconnect()
        {
            ComPort.Close();
            btnConnect.Text = "Conectar";
            groupBox1.Enabled = true;
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
            if (cmbPortName.SelectedIndex != -1 & cmbBaudRate.SelectedIndex != -1 & cmbParity.SelectedIndex != -1 & cmbDataBits.SelectedIndex != -1 & cmbStopBits.SelectedIndex != -1 & cmbTimeout.SelectedIndex != -1 & cmbReintento.SelectedIndex != -1)
            {
                if (ComPort.IsOpen)
                {
                    disconnect();
                }
                string userRoot = System.Environment.GetEnvironmentVariable("USERPROFILE");
                var path = Path.GetFullPath(userRoot+"\\config.txt");
                try
                {
                    StreamWriter sw = new StreamWriter(path, false, Encoding.ASCII);
                    var config = String.Empty;
                    config += cmbPortName.SelectedItem + "\r\n";
                    config += cmbTimeout.SelectedItem + "\r\n";
                    config += cmbReintento.SelectedItem + "\r\n";
                    config += cmbBaudRate.SelectedItem + "\r\n";
                    config += cmbParity.SelectedItem + "\r\n";
                    config += cmbDataBits.SelectedItem + "\r\n";
                    config += cmbStopBits.SelectedItem + "\r\n";
                    config += equipo.Text + "\r\n";
                    sw.Write(config);
                    sw.Close();
                    this.Hide();
                    Form2 objUI = new Form2();
                    objUI.Closed += (s, args) => this.Close();
                    objUI.Show();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(this, "Error al guardar configuración:", "Error de acceso al filesystem", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    Console.WriteLine("Exception: " + ex.Message);
                }
                finally
                {
                    Console.WriteLine("Executing finally block.");
                }
            }
            else
                MessageBox.Show("Por favor seleccióne todos los parámetros requeridos.", "Seleccionar puerto", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
    }
}