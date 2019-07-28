using System;
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
            using (Mutex mutex = new Mutex(false, appGuid))
            {
                if (!mutex.WaitOne(0, false))
                {
                    MessageBox.Show("El programa ya se está ejecutando", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                var config = new sermed.shared.Shared().GetConfig();
                string[] args = Environment.GetCommandLineArgs();
                try
                {
                    var argumento = args[1];
                    if (!String.IsNullOrEmpty(config.cmbPortName))
                        Application.Run(new Form2());
                    else
                    {
                        MessageBox.Show("Configure la aplicación antes de utilizarla", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        Application.Run(new Inicio());
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Config.:" + ex.Message);
                    if (!String.IsNullOrEmpty(config.cmbPortName))
                    {
                        if (MessageBox.Show("¿Deseas cambiar la configuración de la aplicación?", "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                            Application.Run(new Inicio());
                        else
                            return;
                    }
                    else
                        Application.Run(new Inicio());
                }
            }
        }
        private static string appGuid = "c0a76b5a-12ab-45c5-b9d9-d222fca52369";
    }
}
