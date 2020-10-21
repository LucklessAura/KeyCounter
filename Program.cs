using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace KeyCounter
{
    static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            /// <summary>
            /// Verify if there already exists an instance of this app, if it exists show a message else start the app.
            /// </summary>
            bool createNew = true;
            using (Mutex mutex = new Mutex(true,"KeyCounter",out createNew))
            {
                if (createNew)
                {
                    Application.SetHighDpiMode(HighDpiMode.SystemAware);
                    Application.EnableVisualStyles();
                    Application.SetCompatibleTextRenderingDefault(false);
                    Application.ThreadException += HandleExceptions;
                    AppDomain.CurrentDomain.UnhandledException += HandleExceptions;
                    Application.Run(new MainForm());       
                }
                else
                {
                    Process current = Process.GetCurrentProcess();
                    foreach (Process process in Process.GetProcessesByName(current.ProcessName))
                    {
                        if (process.Id != current.Id)
                        {
                            MessageBox.Show("There already is an instance of this app running.");
                            break;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Handler to write to a logs file all uncaught exceptions.
        /// </summary>
        private static void HandleExceptions(object sender, ThreadExceptionEventArgs e)
        {
            using (StreamWriter sw = File.AppendText(Path.GetDirectoryName(Application.ExecutablePath) + "\\" + "Logs.txt"))
            {
                sw.WriteLine(DateTime.Now);
                sw.WriteLine(e.Exception.Data + "\n");
                sw.WriteLine(e.Exception.Message + "\n");
                sw.WriteLine(e.Exception.InnerException + "\n");
                sw.WriteLine(e.Exception.StackTrace + "\n");
                sw.WriteLine("~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~\n");

            }
            MessageBox.Show("An exception occured, see log files for more info. The app wil now exit");
            Application.Exit();
        }

        /// <summary>
        /// Handler to write to a logs file all uncaught exceptions.
        /// </summary>
        private static void HandleExceptions(object sender, UnhandledExceptionEventArgs e)
        {
            using (StreamWriter sw = File.AppendText(Path.GetDirectoryName(Application.ExecutablePath) + "\\" + "Logs.txt"))
            {
                sw.WriteLine(DateTime.Now);
                sw.WriteLine(e.ExceptionObject.ToString() + "\n");
                sw.WriteLine("~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~\n");

            }
            MessageBox.Show("An exception occured, see log files for more info. The app wil now exit");
            Application.Exit();
        }
    }
}
