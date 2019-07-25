using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
namespace sermed.model
{
    class ConfigClass
    {
        public String cmbPortName { get; set; }
        public String cmbTimeout { get; set; }
        public String cmbReintento { get; set; }
        public String cmbBaudRate { get; set; }
        public String cmbParity { get; set; }
        public String cmbDataBits { get; set; }
        public String cmbStopBits { get; set; }
        public String equipo { get; set; }
    }
    class VersionClass
    {
        public String version = "Versión: 3.0.0";
    }
    class UrlClass
    {
        public String url = "http://visa.sermed.info:8081/WSHuella/ws/procesos/";
    }
    class DataClass
    {
        public String accion { get; set; }
        public String p_ci { get; set; }
        public String p_id_maquina { get; set; }
        public String type { get; set; }
        public String version { get; set; }
        
    }
    class VerifyClass
    {
        public String P_MENSAJE { get; set; }
        public Int32 P_NRO_DEDO1 { get; set; }
        public Int32 P_NRO_DEDO2 { get; set; }
        public String P_OK { get; set; }
    }
}