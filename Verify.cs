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
                var sendData = new DataClass();
                sendData.accion = "validar";
                sendData.p_ci = urlparams[0];
                sendData.p_id_maquina = "1";
                sendData.version = new VersionClass().version;
                sendData.type = urlparams[1]; 
                var config = new Shared().GetConfig();
                WebRequest request = WebRequest.Create("http://visa.sermed.info:8081/WSHuella/ws/procesos/huellas");
                request.Method = "POST";
                string postData = JsonConvert.SerializeObject(sendData);
                byte[] byteArray = Encoding.UTF8.GetBytes(postData);
                request.ContentType = "text/plain";
                request.ContentLength = byteArray.Length;
                Stream dataStream = request.GetRequestStream();
                dataStream.Write(byteArray, 0, byteArray.Length);
                dataStream.Close();
                WebResponse response = request.GetResponse();
                Console.WriteLine(((HttpWebResponse)response).StatusDescription);
                dataStream = response.GetResponseStream();
                StreamReader reader = new StreamReader(dataStream);
                string responseFromServer = reader.ReadToEnd();
                Console.WriteLine(responseFromServer);
                label1.Text = responseFromServer;
                reader.Close();
                dataStream.Close();
                response.Close();
                var datos = JsonConvert.DeserializeObject(responseFromServer);
                byte[] data = new Shared().HexStringToByteArray(new SensorClass().DeleteAll);
                ComPort.Write(data, 0, data.Length);
                data = new Shared().HexStringToByteArray(new SensorClass().DeleteAll);
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
