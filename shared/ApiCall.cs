using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Web.Script.Serialization;
using System.Windows.Forms;
using sermed.model;
using Newtonsoft.Json;
using System.Net.Http;
using System.IO.Ports;
using System.Net;
using sermed.shared;

namespace sermed.shared
{
    internal class ApiCall
    {
        public static VerifyClass GetVerifyData(String document, String type, String path)
        {
            var config = new Shared().GetConfig();
            var sendData = new DataClass();
            sendData.accion = "validar";
            sendData.p_ci = document;
            sendData.p_id_maquina = config.equipo;
            sendData.version = new VersionClass().version;
            sendData.type = type;
            WebRequest request = WebRequest.Create(new UrlClass().url + path);
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
            reader.Close();
            dataStream.Close();
            response.Close();
            VerifyClass datos = JsonConvert.DeserializeObject<VerifyClass>(responseFromServer);
            return datos;
        }
    }
}
