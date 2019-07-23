﻿using System;
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
                buffer[i / 2] = (byte)Convert.ToByte(s.Substring(i, 2), 16);
            return buffer;
        }
        public string ByteArrayToHexString(byte[] data)
        {
            StringBuilder sb = new StringBuilder(data.Length * 3);
            foreach (byte b in data)
                sb.Append(Convert.ToString(b, 16).PadLeft(2, '0').PadRight(3, ' '));
            return sb.ToString().ToUpper();
        }
    }
}