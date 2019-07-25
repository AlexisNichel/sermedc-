using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Web.Script.Serialization;
using System.Windows.Forms;
using sermed.model;
using Newtonsoft.Json;
using System.Net.Http;
using System.IO.Ports;
using System.IO;
using System.Net;
using sermed.shared;

namespace Sermed
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }
        private SerialPort ComPort = new SerialPort();  //Initialise ComPort Variable as SerialPort
        private void Form2_Load(object sender, EventArgs e)
        {
            string[] args = Environment.GetCommandLineArgs();
            try
            {
                var argument = args[1].Replace("sermed://", string.Empty);
                var urlparams = argument.Split('/');
                var datos = sermed.shared.ApiCall.GetVerifyData(urlparams[0], "visa", "huellas");
                label1.Text = JsonConvert.SerializeObject(datos);
                if (datos.P_OK == "NO")
                {

                }






                byte[] data = new Shared().HexStringToByteArray(new SensorClass().DeleteAll);
                ComPort.Write(data, 0, data.Length);
                data = new Shared().HexStringToByteArray(new SensorClass().LedOn);
                ComPort.Write(data, 0, data.Length);
            }
            catch
            {
                //  MessageBox.Show("Error al obtener parametros");
                //  return;
            }
        }
    }
}
