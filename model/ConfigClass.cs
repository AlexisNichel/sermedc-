using System;
using System.Windows.Forms;
namespace sermed.model
{
    class ConfigClass
    {
        public String cmbPortName { get; set; }
        public String cmbTimeout { get; set; }
        public String equipo { get; set; }
    }
    class VersionClass
    {
        public String version = "Version " + Application.ProductVersion;
    }
    class UrlClass
    {
        public String url = Properties.Settings.Default.url;
    }
    class DataClass
    {
        public String accion { get; set; }
        public String p_ci { get; set; }
        public Int32 p_id_maquina { get; set; }
        public String type { get; set; }
        public String version { get; set; }
        public Int32 p_nro_dedo1 { get; set; }
        public String p_huella1 { get; set; }
    }
    class VerifyClass
    {
        public String P_NOMBRE { get; set; }
        public String P_MENSAJE { get; set; }
        public Int32 P_NRO_DEDO1 { get; set; }
        public Int32 P_NRO_DEDO2 { get; set; }
        public String P_OK { get; set; }
        public String P_HUELLA1 { get; set; }
    }
}