using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Web.Script.Serialization;
using sermed.model;
using Newtonsoft.Json;
using System.Net;
namespace sermed.shared
{
    internal class ApiCall
    {
        public static VerifyClass SendFPReq(String action, String document, int dedo, String huella, String type)
        {
            var config = new Shared().GetConfig();
            var sendData = new DataClass();
            sendData.accion = action;
            sendData.p_ci = document;
            sendData.p_id_maquina = Convert.ToInt32(config.equipo);
            sendData.version = new VersionClass().version;
            sendData.p_nro_dedo1 = dedo;
            sendData.p_huella1 = huella;
            sendData.type = type;
            WebRequest request = WebRequest.Create(new UrlClass().url + "huellas");
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
