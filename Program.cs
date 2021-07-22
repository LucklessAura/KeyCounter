using System;
using System.Diagnostics;
using System.Globalization;
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

        public static keyCounterMainFrame_frame MainFrame;
        public static readonly string AppDataPath = Path.Join(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "KeyCounter");
        public static readonly string CommonAppsData = Path.Join(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "KeyCounter");
        

        [STAThread]
        static void Main()
        {
            using (Mutex mutex = new Mutex(true, "KeyCounter", out bool createNew))
            {
                
                if (createNew)
                {
                    if (!Directory.Exists(AppDataPath))
                    {
                        Directory.CreateDirectory(AppDataPath);
                    }
                    
                    Application.EnableVisualStyles();
                    Application.SetHighDpiMode(Application.HighDpiMode);
                    Application.SetCompatibleTextRenderingDefault(false);
                    Application.ThreadException += Application_ThreadException;
                    Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
                    AppDomain.CurrentDomain.UnhandledException += Application_UnhandledException;
                    
                     // while the other 2 handlers catch almost all exceptions this is the only way of getting the ones when the keyCounterMainFrame_frame fails to initialize
                     try
                     {
                         MainFrame = new keyCounterMainFrame_frame();
                         Application.Run(MainFrame);
                     }
                     catch (Exception e)
                     {
                         //not stopping this might lead to even more errors
                         KeyboardHookClass.DeleteKeyboardHook();
                         MouseHookClass.DeleteMouseHook();
                         GamepadHookClass.DestroyTimer();

                         if (e is ChainingException exception)
                         {
                             using (StreamWriter sw = File.AppendText(AppDataPath + "\\logs.log"))
                             {
                                 sw.WriteLine(DateTime.Now.ToString(CultureInfo.InvariantCulture));
                                 foreach (var cause in exception.GetExceptionChain())
                                 {
                                     sw.WriteLine(cause);
                                 }
                                 sw.WriteLine("\n~~~~~~~~~~~~~~~~~~~\n~~~~~~~~~~~~~~~~~~~\n");
                             }
                         }
                         else
                         {
                             using (StreamWriter sw = File.AppendText(AppDataPath + "\\logs.log"))
                             {
                                 var ex = e;
                                 sw.WriteLine(DateTime.Now.ToString(CultureInfo.InvariantCulture));
                                 if (ex != null) sw.WriteLine(ex.Message);
                                 sw.WriteLine("\n~~~~~~~~~~~~~~~~~~~\n~~~~~~~~~~~~~~~~~~~\n");
                             }
                         }
                         
                         MessageBox.Show("An exception occurred, for more info see the logs.", "ERROR", MessageBoxButtons.OK,
                             MessageBoxIcon.Error);
                         Application.Exit();
                     }
                    
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

        static void Application_ThreadException(object sender, ThreadExceptionEventArgs e)
        {
            //not stopping this might lead to even more errors
            KeyboardHookClass.DeleteKeyboardHook();
            MouseHookClass.DeleteMouseHook();
            GamepadHookClass.DestroyTimer();

            if (e.Exception is ChainingException exception)
            {
                using (StreamWriter sw = File.AppendText(AppDataPath + "\\logs.log"))
                {
                    sw.WriteLine(DateTime.Now.ToString(CultureInfo.InvariantCulture));
                    foreach (var cause in exception.GetExceptionChain())
                    {
                        sw.WriteLine(cause);
                    }
                    sw.WriteLine("\n~~~~~~~~~~~~~~~~~~~\n~~~~~~~~~~~~~~~~~~~\n");
                }
            }
            else
            {
                using (StreamWriter sw = File.AppendText(AppDataPath + "\\logs.log"))
                {
                    sw.WriteLine(DateTime.Now.ToString(CultureInfo.InvariantCulture));
                    sw.WriteLine(e.Exception.Message);
                    sw.WriteLine("\n~~~~~~~~~~~~~~~~~~~\n~~~~~~~~~~~~~~~~~~~\n");
                }
            }

            MessageBox.Show("An exception occurred, for more info see the logs.", "ERROR", MessageBoxButtons.OK,
                MessageBoxIcon.Error);
            Application.Exit();
        }


        static void Application_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            //not stopping this might lead to even more errors
            KeyboardHookClass.DeleteKeyboardHook();
            MouseHookClass.DeleteMouseHook();
            GamepadHookClass.DestroyTimer();

            if (e.ExceptionObject is ChainingException exception)
            {
                using (StreamWriter sw = File.AppendText(AppDataPath + "\\logs.log"))
                {
                    sw.WriteLine(DateTime.Now.ToString(CultureInfo.InvariantCulture));
                    foreach (var cause in exception.GetExceptionChain())
                    {
                        sw.WriteLine(cause);
                    }
                    sw.WriteLine("\n~~~~~~~~~~~~~~~~~~~\n~~~~~~~~~~~~~~~~~~~\n");
                }
            }
            else
            {
                using (StreamWriter sw = File.AppendText(AppDataPath + "\\logs.log"))
                {
                    var ex = e.ExceptionObject as Exception;
                    sw.WriteLine(DateTime.Now.ToString(CultureInfo.InvariantCulture));
                    if (ex != null) sw.WriteLine(ex.Message);
                    sw.WriteLine("\n~~~~~~~~~~~~~~~~~~~\n~~~~~~~~~~~~~~~~~~~\n");
                }
            }

            MessageBox.Show("An exception occurred, for more info see the logs.", "ERROR", MessageBoxButtons.OK,
                MessageBoxIcon.Error);
            Application.Exit();
        }

    }
}
