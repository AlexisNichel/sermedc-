using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Threading;
namespace Sermed
    {
    static class Program
        {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
            {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            using (Mutex mutex = new Mutex(false,  appGuid))
            {
                if (!mutex.WaitOne(0, false))
                {
                    string[] args = Environment.GetCommandLineArgs();
                    try
                    {
                        Form2 objUI = new Form2();
                        objUI.Show();
                    //    return;
                        // MessageBox.Show("Argumento: " + args[1].Replace("fingerprint://", string.Empty));
                        //ir a la ventana onda tranqui
                    }
                    catch
                    {
                        MessageBox.Show("El programa ya está en ejecución");
                        return;
                    }
                }
                Application.Run(new Inicio());
            }
        }
        private static string appGuid = "c0a76b5a-12ab-45c5-b9d9-d693faa6e7b9";
    }
}
