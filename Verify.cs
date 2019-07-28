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
using System.IO.Ports;
using sermed.shared;
namespace Sermed
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }
        private SerialPort ComPort = new SerialPort(); 
        private Timer tmrbar;
        byte[] data = null;
        int dedo;
        int step = 0;
        string document = "";
        String queryType = "";
        String huella = "";
        private void Form2_Load(object sender, EventArgs e)
        {
            CheckForIllegalCrossThreadCalls = false;
            dedo = 1;
            var config = new Shared().GetConfig();
            string[] args = Environment.GetCommandLineArgs();
            try
            {
                if (!ComPort.IsOpen)
                    connect();
                if (ComPort.IsOpen)
                {
                    var time = config.cmbTimeout;
                    bartimeout.Value = 100;
                    tmrbar = new Timer();
                    double timeConfigInt = Convert.ToDouble(time);
                    double resTemp = timeConfigInt / 100;
                    var timeParse = resTemp * 1000;
                    tmrbar.Interval = (int)TimeSpan.FromMilliseconds(timeParse).TotalMilliseconds;
                    tmrbar.Tick += delegate
                    {
                        if (bartimeout.Value > 0)
                            bartimeout.Value -= 1;
                        else
                        {
                            data = new Shared().HexStringToByteArray(new SensorClass().Cancel);
                            // ComPort.Write(data, 0, data.Length);
                            this.Close();
                        }
                    };
                    //borrar (no importa la razon sea enrol o lectura, al final son la misma mierda)
                    data = new Shared().HexStringToByteArray(new SensorClass().DeleteAll);
                    ComPort.Write(data, 0, data.Length);
                    var byteBuffer = bufferRead();
                    if (byteBuffer[6] == 0 && byteBuffer[7] == 0)
                    {
                        args = new string[2];
                        args[0] = "as"; //estado
                        args[1] = "1125312030/ca";
                        var argument = args[1].Replace("sermed://", string.Empty);
                        var urlparams = argument.Split('/');
                        document = urlparams[0];
                        queryType = urlparams[1];
                        var datos = ApiCall.SendFPReq("VALIDAR", document, 0, "", "visa");

                        if ((datos.P_OK == "SI" || datos.P_OK == "1") && !String.IsNullOrEmpty(datos.P_HUELLA1))
                        {
                            huella = datos.P_HUELLA1;
                            btncancel.Visible = true;
                            bartimeout.Visible = true;
                           
                            //Thread verifyxs
                            System.Threading.Thread t = new System.Threading.Thread(new System.Threading.ThreadStart(WriteFP));
                            t.Start();
                            tmrbar.Start();

                            //verify
                        }
                        else if (datos.P_OK == "NO" || datos.P_OK == "0" || String.IsNullOrEmpty(datos.P_HUELLA1))
                        {
                            label2.Text = "Seleccione un dedo y pulsa enrolar";
                            btncancel2.Visible = true;
                            btnEnroll.Visible = true;
                            //enrol
                        }
                        else
                        {
                            //ir a web
                        }
                    }
                    else
                    {
                        //ir a una página que pregunte si quieres reintentar...
                    }
                    // System.Diagnostics.Process.Start("http://google.com");
                }
                else
                {
                    //ir a una página que pregunte si quieres reintentar...
                }
            }
            catch (Exception ex)
            {
                //hacer un disconect?
                this.Close();
                //ir a una página que pregunte si quieres reintentar...
                MessageBox.Show("Error: " + ex);
                return;
            }
        }
        public void WriteFP()
        {
            string command = new SensorClass().WriteStart;
            data = new Shared().HexStringToByteArray(command);
            ComPort.Write(data, 0, data.Length);
            byte[] byteBuffer = bufferRead();
            command = new SensorClass().FPBase.Replace("[value]", "1");
            data = new Shared().HexStringToByteArray(command);
            byte[] huellaByte = System.Convert.FromBase64String(huella);
            byte[] concatData = new Shared().ConcatByte(data, huellaByte);
            byte[] cheked = new Shared().checksum(concatData);
            ComPort.Write(cheked, 0, cheked.Length);
            byteBuffer = bufferRead();
            command = new SensorClass().Verify;
            data = new Shared().HexStringToByteArray(command);
            ComPort.Write(data, 0, data.Length);
            ReadProcess();
        }
        public void ReadProcess()
        {
            Int32 status = 0;
            /*Status determina 0=intermedio, 1 =Error(lectura), 2=Fatal error, 3=Cancelado, 4=No registrado, 5=Success */
            while (status == 0)
            {
                var byteBuffer = bufferRead();
                status = Shared.VerifyReadStep(byteBuffer, step);
            }
            switch (status)
            {
                case 1: //Error de lectura
                    lblstep.Text = "Error de lectura";
                    break;
                case 2: //error
                    lblstep.Text = "Fatal error";
                    break;
                case 3: //Cancelado
                    lblstep.Text = "Cancelado";
                    break;
                case 4: //timeour
                    lblstep.Text = "Espera agotada";
                    break;
                case 5: //no reg
                    lblstep.Text = "La huella no se encuentra registrada";
                    break;
                case 6: //Success
                    EnviarRegistro();
                    break;
            }
            
        }


        private void btn_cancel_click(object sender, EventArgs e)
        {
            if (ComPort.IsOpen)
            {
                byte[] data = new Shared().HexStringToByteArray(new SensorClass().Cancel);
                ComPort.Write(data, 0, data.Length);
                ComPort.Close();
            }
            this.Close();

            //ir a una página que pregunte si quieres reintentar...
        }
        private void Btncancel2_Click(object sender, EventArgs e)
        {
            btn_cancel_click(sender, e);
        }
        public void connect()
        {
            var config = new Shared().GetConfig();
            bool error = false;
            ComPort.PortName = config.cmbPortName;
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
            //definir si se mostrara el error
            if (error) MessageBox.Show(this, "No se pudo abrir el puerto COM. Desconecte el lector, conectelo nuevamente y vuelva a intentarlo.", "Error de puerto", MessageBoxButtons.OK, MessageBoxIcon.Stop);
        }
        private void Button3_Click(object sender, EventArgs e)
        {
            var command = new Shared().set_cmd();
            command[2] = 0x03;
            command[3] = 0x01;
            command[4] = 0x02;
            command[6] = 2;
            data = new Shared().calcular_chk(command);
            tmrbar.Start();
            ComPort.Write(data, 0, data.Length);
        }
        private void Dedo1_Click(object sender, EventArgs e)
        {
            dedo1.Visible = false;
            huella1.Visible = true;
            dedo2.Visible = true;
            huella2.Visible = false;
            dedo = 1;
        }
        private void Dedo2_Click(object sender, EventArgs e)
        {
            huella1.Visible = false;
            dedo1.Visible = true;
            dedo2.Visible = false;
            huella2.Visible = true;
            dedo = 2;
        }
        private void BtnEnroll_Click(object sender, EventArgs e)
        {
            //UI
            btnEnroll.Visible = false;
            btncancel.Visible = true;
            btncancel2.Visible = false;
            label2.Text = "Siga los pasos en pantalla para enrolar";
            pictureBox1.Visible = false;
            huella1.Visible = false;
            dedo1.Visible = false;
            dedo2.Visible = false;
            huella2.Visible = false;
            bartimeout.Visible = true;
            bartimeout.Value = 100;
            //command
            var command = new Shared().set_cmd();
            command[2] = 0x03;
            command[3] = 0x01;
            command[4] = 0x02;
            command[6] = 0x01;
            data = new Shared().calcular_chk(command);
            ComPort.Write(data, 0, data.Length);
            tmrbar.Start();
            step1picture.Visible = true;
            lblstep.Visible = true;
            //Thread enrol
            System.Threading.Thread t = new System.Threading.Thread(new System.Threading.ThreadStart(EnrolProcess));
            t.Start();
        }
        public void EnrolProcess()
        {
            Int32 status = 0;
            /*Status determina 0=intermedio, 1=Pedido(Primera), 2=Pedido(Segunda), 3=Pedido(Final), 4=Error (de existencia), 5 =Error(lectura), 6=Fatal error, 7=Cancelado, 8=Success */
            while (status <= 3)
            {
                var byteBuffer = bufferRead();
                status = Shared.VerifyEnrollStep(byteBuffer, step);
                if (status == 2)
                {
                    bartimeout.Value = 100;
                    flecha1.Invoke(new Action(() => flecha1.Visible = true));
                    step2picture.Invoke(new Action(() => step2picture.Visible = true));
                    lblstep.Text = "Coloque su dedo otra vez";
                    step = 1;
                }
                else if (status == 3)
                {
                    bartimeout.Value = 100;
                    flecha2.Invoke(new Action(() => flecha2.Visible = true));
                    step3picture.Invoke(new Action(() => step3picture.Visible = true));
                    lblstep.Text = "Coloque su dedo una última vez";
                    step = 2;
                }
            }
            switch (status)
            {
                case 4: //Error de existencia
                    lblstep.Text = "La huella ya existe";
                    break;
                case 5: //Error de lectura
                    lblstep.Text = "Error de lectura";
                    break;
                case 6: //timeour
                    lblstep.Text = "Espera agotada";
                    break;
                case 7: //Cancelado
                    lblstep.Text = "Cancelado";
                    break;
                case 8: //Success
                    EnviarHuella();
                    break;
            }
            bartimeout.Invoke(new Action(() => bartimeout.Visible = false));
            tmrbar.Stop();
            tmrbar.Dispose();
        }
        private void EnviarRegistro()
        {
            label2.Text = "Huella se ha validado correctamente";
            System.Diagnostics.Process.Start("http://visa.sermed.info/auth/visacion/visacion");
            bartimeout.Invoke(new Action(() => bartimeout.Visible = false));
            tmrbar.Stop();
            tmrbar.Dispose();
            this.Close();
        }
        private void EnviarHuella()
        {
            string command = new SensorClass().GetFP.Replace("[value]", "1");
            data = new Shared().HexStringToByteArray(command);
            byte[] cheked = new Shared().checksum(data);
            ComPort.Write(cheked, 0, cheked.Length);
            byte[] byteBuffer = bufferRead();
            byte[] huellaByte = new Shared().GetCleanFP(byteBuffer);
            String Base64 = Convert.ToBase64String(huellaByte);
            var datos = ApiCall.SendFPReq("INSERTAR", document, dedo, Base64, "enrol");
            if (datos.P_OK == "SI" || datos.P_OK == "1")
            {
                lblstep.Text = "Huella enrolada correctamente";
            }
            else if (datos.P_OK == "NO" || datos.P_OK == "0")
            {
                label2.Text = "Error";
            }
        }
        private byte[] bufferRead()
        {
            int intBuffer;
            byte[] byteBuffer = null;
            byte[] byteBuffer2 = null;
            var len = 0;
            while (len == 0)
            {
                intBuffer = ComPort.BytesToRead;
                byteBuffer = new byte[intBuffer];
                len = ComPort.Read(byteBuffer, 0, intBuffer);
                if (len == 24 || len == 48 || len == 534)
                    break;
                else
                    System.Threading.Thread.Sleep(100);
            }
            if (byteBuffer.Length == 48)
            {
                byteBuffer2 = new byte[24];
                for (var i = 0; i <= 23; i++)
                {
                    byteBuffer2[i] = byteBuffer2[i];
                }
                byteBuffer = byteBuffer2;
            }
            return byteBuffer;
        }
    }
}
