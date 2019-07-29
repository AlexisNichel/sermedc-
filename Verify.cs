using System;
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
        private Timer stepTime;
        public Boolean AlertPermit = true;
        byte[] data = null;
        int dedo;
        int step = 0;
        string document = String.Empty;
        String origen = String.Empty;
        String huella = String.Empty;
        Boolean fingerSelectEnable = false;
        private void Form2_Load(object sender, EventArgs e)
        {
            CheckForIllegalCrossThreadCalls = false;
            dedo = 1;
            var config = new Shared().GetConfig();
            string[] args = Environment.GetCommandLineArgs();
            try
            {
                if (!ComPort.IsOpen)
                    connect(sender, e);
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
                    data = new Shared().HexStringToByteArray(new SensorClass().DeleteAll);
                    ComPort.Write(data, 0, data.Length);
                    var byteBuffer = bufferRead();
                    if (byteBuffer.Length > 1)
                    {
                        if (byteBuffer[6] == 0 && byteBuffer[7] == 0)
                        {
                            var argument = args[1].Replace("sermed://", string.Empty);
                            var urlparams = argument.Split('/');
                            document = urlparams[0];
                            origen = urlparams[1];
                            var datos = ApiCall.SendFPReq("VALIDAR", document, 0, "", "visa");

                            if ((datos.P_OK == "SI" || datos.P_OK == "1") && !String.IsNullOrEmpty(datos.P_HUELLA1))
                            {
                                huella = datos.P_HUELLA1;
                                btncancel.Visible = true;
                                btncancel2.Visible = false;
                                retry.Visible = false;
                                bartimeout.Visible = true;
                                fingerSelectEnable = false;
                                //Thread verifyxs
                                System.Threading.Thread t = new System.Threading.Thread(new System.Threading.ThreadStart(WriteFP));
                                t.Start();
                                tmrbar.Start();
                                //verify
                            }
                            else if (datos.P_OK == "NO" || datos.P_OK == "0" || String.IsNullOrEmpty(datos.P_HUELLA1))
                            {
                                fingerSelectEnable = true;
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
        private void btn_cancel_click(object sender, EventArgs e)
        {
            if (ComPort.IsOpen)
            {
                byte[] data = new Shared().HexStringToByteArray(new SensorClass().Cancel);
                ComPort.Write(data, 0, data.Length);
                ComPort.Close();
            }
            this.Close();
            //ir a página de origen
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
                retry.Invoke(new Action(() => retry.Visible = true));
            }
            else if (datos.P_OK == "NO" || datos.P_OK == "0")
            {
                label2.Text = "Error en el envío de huella a server";
            }
        }
        private void EnviarRegistro()
        {
            label2.Text = "Huella del beneficiario ha sido validada";
            bartimeout.Invoke(new Action(() => bartimeout.Visible = false));
            btncancel.Invoke(new Action(() => btncancel.Visible = false));
            btncancel2.Invoke(new Action(() => btncancel2.Visible = false));
            btnEnroll.Invoke(new Action(() => btnEnroll.Visible = false));
            retry.Invoke(new Action(() => retry.Visible = false));
            bartimeout.Invoke(new Action(() => bartimeout.Visible = false));
            tmrbar.Stop();
            tmrbar.Dispose();
            System.Threading.Thread.Sleep(2000);
            switch (origen)
            {
                case "cn":
                    System.Diagnostics.Process.Start("http://visa.sermed.info/auth/visacion/visacion/visaConsultaHv/" + document);
                    break;
                case "cu":
                    System.Diagnostics.Process.Start("http://visa.sermed.info/auth/visacion/visacion/visaConsultaUrgenciaHv/" + document);
                    break;
                case "sn":
                    System.Diagnostics.Process.Start("http://visa.sermed.info/auth/visacion/visacion/visaServicioHv/" + document);
                    break;
                case "su":
                    System.Diagnostics.Process.Start("http://visa.sermed.info/auth/visacion/visacion/visaServicioUrgenciaHv/" + document);
                    break;
                case "ve":
                    System.Diagnostics.Process.Start("http://visa.sermed.info/auth/index.php/visacion/visacion");
                    break;
            }
            this.Close();
        }
        public void ReadProcess()
        {
            Int32 status = 0;
            while (status == 0)
            {
                var byteBuffer = bufferRead();
                if (byteBuffer.Length > 1)
                {
                    status = Shared.VerifyReadStep(byteBuffer, step);
                }
                else
                {
                    status = 7;
                }
            }
            if (status == 6)
            {
                EnviarRegistro();
            }
            else
            {
                switch (status)
                {
                    case 7:
                        lblstep.Text = "Error, el equipo se desconectó";
                        break;
                    case 1:
                        lblstep.Text = "Error en la lectura de la huella";
                        break;
                    case 2:
                        lblstep.Text = "Error no identificado en lectura";
                        break;
                    case 3:
                        lblstep.Text = "Se canceló el proceso de lectura";
                        break;
                    case 4:
                        lblstep.Text = "Se agotó el tiempo de espera";
                        break;
                    case 5:
                        lblstep.Text = "Huella no registrada o incorrecta";
                        break;
                }
                lblstep.Invoke(new Action(() => lblstep.Visible = true));
                bartimeout.Value = 0;
                tmrbar.Stop();
                tmrbar.Dispose();
                if (status == 7)
                {
                    sermed.Help2 objUI = new sermed.Help2();
                    objUI.ShowDialog();
                }
                btncancel.Invoke(new Action(() => btncancel.Visible = false));
                btncancel2.Invoke(new Action(() => btncancel2.Visible = true));
                retry.Invoke(new Action(() => retry.Visible = true));
            }
        }
        private byte[] bufferRead()
        {
            byte[] byteBuffer = null;
            try
            {
                int intBuffer;
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
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                byteBuffer = new byte[1];
            }
            return byteBuffer;
        }
        private void Btncancel2_Click(object sender, EventArgs e)
        {
            btn_cancel_click(sender, e);
        }
        private void Dedo1_Click(object sender, EventArgs e)
        {
            if (fingerSelectEnable)
            {
                dedo1.Visible = false;
                huella1.Visible = true;
                dedo2.Visible = true;
                huella2.Visible = false;
                dedo = 1;
            }
        }
        private void Dedo2_Click(object sender, EventArgs e)
        {
            if (fingerSelectEnable)
            {
                huella1.Visible = false;
                dedo1.Visible = true;
                dedo2.Visible = false;
                huella2.Visible = true;
                dedo = 2;
            }
        }
        private void Retry_Click(object sender, EventArgs e)
        {
            lblstep.Invoke(new Action(() => lblstep.Visible = false));
            Form2_Load(sender, e);
        }
        public void WriteFP()
        {
            string command = new SensorClass().WriteStart;
            data = new Shared().HexStringToByteArray(command);
            ComPort.Write(data, 0, data.Length);
            byte[] byteBuffer = bufferRead();
            if (byteBuffer.Length > 1)
            {
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
        }
        public void connect(object sender, EventArgs e)
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
            if (error)
            {
                stepTime = new Timer();
                stepTime.Interval = (int)TimeSpan.FromMilliseconds(2000).TotalMilliseconds;
                stepTime.Tick += delegate
                {
                    var ports = SerialPort.GetPortNames();
                    if (ports.Length > 0)
                    {
                        foreach (string port in ports)
                        {
                            if (port == config.cmbPortName)
                            {
                                AlertPermit = true;
                                stepTime.Stop();
                                stepTime.Dispose();
                                Form2_Load(sender, e);
                            }
                        }
                    }
                };
                AlertPermit = false;
                stepTime.Start();
                sermed.Help2 objUI = new sermed.Help2();
                objUI.Show();
            }
        }
    }
}
