using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Threading;
using System.IO;

namespace Sermed
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            using (Mutex mutex = new Mutex(false,  appGuid))
            {
                if (!mutex.WaitOne(0, false))
                {
                    MessageBox.Show("El programa ya está en ejecución");
                    return;
                }



                string[] args = Environment.GetCommandLineArgs();
                String line;
                try
                {
                    var argumento = args[1];
                    string userRoot = Environment.GetEnvironmentVariable("USERPROFILE");
                    var path = Path.GetFullPath(userRoot + "\\config.txt");
                    StreamReader sr = new StreamReader(path);
                    line = sr.ReadLine();
                    while (line != null)
                        line = sr.ReadLine();
                    sr.Close();
                    Application.Run(new Form2());
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Err.:" + ex.Message);
                    Application.Run(new Inicio());
                }

            }
        }
        private static string appGuid = "c0a76b5a-12ab-45c5-b9d9-d222fca52369";
    }
}
