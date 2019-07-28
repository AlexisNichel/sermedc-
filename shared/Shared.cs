using System;
using System.IO;
using System.Text;
namespace sermed.shared
{
    internal class Shared
    {
        public model.ConfigClass GetConfig()
        {
            string userRoot = System.Environment.GetEnvironmentVariable("USERPROFILE");
            var path = Path.GetFullPath(userRoot + "\\config.txt");
            var config = new model.ConfigClass();
            try
            {
                using (StreamReader sr = new StreamReader(path))
                {
                    config.cmbPortName = sr.ReadLine();
                    config.cmbTimeout = sr.ReadLine();
                    config.cmbReintento = sr.ReadLine();
                    config.cmbBaudRate = sr.ReadLine();
                    config.cmbParity = sr.ReadLine();
                    config.cmbDataBits = sr.ReadLine();
                    config.cmbStopBits = sr.ReadLine();
                    config.equipo = sr.ReadLine();
                    return config;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return config;
            }
        }
        public byte[] HexStringToByteArray(string s)
        {
            s = s.Replace(" ", "");
            byte[] buffer = new byte[s.Length / 2];
            for (int i = 0; i < s.Length; i += 2)
            {
                buffer[i / 2] = Convert.ToByte(s.Substring(i, 2), 16);
            }
            return buffer;
        }
        public string ByteArrayToHexString(byte[] data)
        {
            StringBuilder sb = new StringBuilder(data.Length * 3);
            foreach (byte b in data)
            {
                var pre = Convert.ToString(b, 16);
                if (pre.Length == 1)
                    pre = "0" + pre;
                sb.Append(pre);
            }
            return sb.ToString().ToUpper();
        }
        public byte[] checksum(byte[] command)
        {
            int sum = 0;
            for (var i = 0; i < command.Length; i++)
            {
                sum = sum + command[i];
            }
            string sumtext = Convert.ToString(sum, 16);
            var hexSum = sumtext.Length == 3 ? "0" + sumtext : sumtext;
            var chk = new Shared().HexStringToByteArray(hexSum.ToUpper());
            var chk1 = chk[0];
            chk[0] = chk[1];
            chk[1] = chk1;
            int length = command.Length + chk.Length;
            byte[] end = new byte[length];
            command.CopyTo(end, 0);
            chk.CopyTo(end, command.Length);
            return end;
        }
        public byte[] set_cmd()
        {
            byte[] command = new byte[24];
            for (var i = 0; i <= 23; i++)
            {
                command[i] = 0;
            }
            command[0] = 85;
            command[1] = 0xAA;
            return command;
        }
        public byte[] calcular_chk(byte[] command)
        {
            var sum = 0;
            for (var i = 0; i <= 21; i++)
            {
                sum = sum + command[i];
            }
            string sumtext = Convert.ToString(sum, 16);
            var hexSum = sumtext.Length == 3 ? "0" + sumtext : sumtext;
            var chk = new Shared().HexStringToByteArray(hexSum.ToUpper());
            command[22] = chk[1];
            command[23] = 0x01;
            return command;
        }
        public byte[] GetCleanFP(byte[] command)
        {
            byte[] final = new byte[498];
            for (var i = 0; i <= 497; i++)
            {
                final[i] = command[i + 34];
            }
            return final;
        }
        public byte[] ConcatByte(byte[] byte1, byte[] byte2)
        {
            int finalLength = byte1.Length + byte2.Length;
            byte[] final = new byte[finalLength];
            Buffer.BlockCopy(byte1, 0, final, 0, byte1.Length);
            Buffer.BlockCopy(byte2, 0, final, byte1.Length, byte2.Length);
            return final;
        }
        public static Int32 VerifyEnrollStep(byte[] result, int step)
        {
            Int32 status = 0;
            if (result[8] == 0x23)
            {
                status = 6;
                Console.WriteLine("Tiempo de espera agotado");
            }
            else if (result[8] == 0xF1 || result[8] == 0xF2 || result[8] == 0xF3)
            {
                switch (result[8])
                {
                    case 0xF1:
                        status = 1;
                        break;
                    case 0xF2:
                        status = 2;
                        break;
                    case 0xF3:
                        status = 3;
                        break;
                }
                Console.WriteLine("Ingresar huella");
            }
            else if (result[8] == 0xF4)
            {
                status = 0;
                Console.WriteLine("Dejar de pulsar el sensor");
            }
            else if (result[8] == 0x14)
            {
                status = 4;
                Console.WriteLine("Ya hay una huella en la posición");
            }
            else if (result[8] == 0x19)
            {
                status = 4;
                Console.WriteLine("La huella ya existe");
            }
            else if (result[4] == 0x06 && (result[8] == 0x01 || result[8] == 0x02))
            {
                status = 8;
                Console.WriteLine("Success");
            }
            else if (result[8] == 0x30)
            {
                status = 5;
                Console.WriteLine("Error en la marcación.");
            }
            else if (result[2] == 0x30 && result[3] == 0x01 && result[8] == 0x00)
            {
                status = 7;
                Console.WriteLine("Enrolamiento Cancelado.");
            }
            else if (result[8] == 0x21)
            {
                status = 5;
                Console.WriteLine("El sensor y/o la huella del paciente poseen manchas o impurezas. Limpie ambos e intente nuevamente.");
            }
            else
            {
                if (step == 1)
                    status = 3;
                else if (step == 2)
                    status = 6;
                else
                    status = 2;
                Console.WriteLine("Other command");
            }
            return status;
        }
        public static Int32 VerifyReadStep(byte[] result, int step)
        {
            Int32 status = 0;
            if (result[8] == 0x23)
            {
                status = 4;
                Console.WriteLine("Tiempo de espera agotado");
            }
            if (result[8] == 0xF4)
            {
                status = 0;
                Console.WriteLine("Dejar de pulsar el sensor");
            }
            else if ((result[6] == 0) && (result[7] == 0 && result[2] == 0x02 && result[3] == 0x01))
            {
                status = 6;
                Console.WriteLine("La huella se encuentra en el template " + result[8]);
            }
            else if (result[2] == 0x30 && result[3] == 0x01 && result[8] == 0x00)
            {
                status = 3;
                Console.WriteLine("Enrolamiento Cancelado.");
            }
            else if (result[2] == 0x02 && result[3] == 0x01 && result[8] == 0x12)
            {
                status = 5;
                Console.WriteLine("La huella no se encuentra registrada.");
            }
            else if (result[2] == 0x02 && result[3] == 0x01 && result[8] == 0x21)
            {
                status = 1;
                Console.WriteLine("Error de lectura");
            }
            else
            {
                status = 2;
                Console.WriteLine("Fatal error");
            }
            return status;
        }
    }
}
