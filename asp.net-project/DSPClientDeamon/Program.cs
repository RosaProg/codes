using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.Windows.Forms;
using DSPClientDeamon.Properties;
using System.IO;
using System.Diagnostics;
using System.Windows.Automation;

namespace DSPClientDeamon
{
    public interface ILogic
    {
        void Print();
    }

    public class Logic : ILogic
    {
        public int Print()
        {
            int uno=0;
            string url2;

            System.Diagnostics.Process[] procesos = System.Diagnostics.Process.GetProcesses();
            foreach (System.Diagnostics.Process proceso in procesos)
            {
                Process[] Procesos_Navegador = Process.GetProcessesByName("iexplore");
                foreach (Process Navegador in Procesos_Navegador)
                {
                    if (Navegador.MainWindowHandle == IntPtr.Zero)
                    {
                        continue;
                    }

                    AutomationElement Elemento = AutomationElement.FromHandle(Navegador.MainWindowHandle);
                    AutomationElement Elemento_Editable = Elemento.FindFirst(TreeScope.Subtree, new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.Edit));

                    if (Elemento_Editable != null)
                    {
                        AutomationPattern[] Patterns = Elemento_Editable.GetSupportedPatterns();

                        if (Patterns.Length > 0)
                        {
                            foreach (var Urls in Patterns)
                            {
                                try
                                {
                                    ValuePattern Url = (ValuePattern)Elemento_Editable.GetCurrentPattern(Urls);
                                    string url = Url.Current.Value.ToString();
                                    url2 = Url.Current.Value.ToString();
                                    if (uno == 0)
                                    {
                                        string[] parameters_url = url.Split('&');

                                        string[] a = parameters_url[1].Split('=');
                                        string[] b = parameters_url[2].Split('=');
                                        string tipo = a[1];
                                        string id = b[1];
                                        foreach (string keyphrase in parameters_url)
                                        {
                                            // Armar el nombre del txt y escribir la data
                                            string[] key_value = keyphrase.Split('=');

                                            // Escribir el txt
                                            FileStream stream = new FileStream("c:\\temp\\archivo.txt", FileMode.OpenOrCreate, FileAccess.Write);
                                            StreamWriter writer = new StreamWriter(stream);
                                            writer.WriteLine("First Line");
                                            writer.Close();
                                            
                                        }

                                        //Console.WriteLine(url);
                                    }
                                }
                                catch (Exception excepcion)
                                {
                                    // Escribir la excepcion en el txt
                                    FileStream stream = new FileStream("c:\\temp\\Error.txt", FileMode.OpenOrCreate, FileAccess.Write);
                                    StreamWriter writer = new StreamWriter(stream);
                                    writer.WriteLine(excepcion.ToString());
                                    writer.Close();
                                }
                            }

                        }

                    }
                }
            }
            return 0;
        }
    }

    static class Program
    {
        [STAThread]
        static void Main()
        {
            // Visual Styles - Context
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new ApplicationStartUp());
        }
    }

    public class ApplicationStartUp : Form
    {
        private NotifyIcon trayIcon;
        private ContextMenu trayMenu;

        

        private void InitializeComponent()
        {
            // Visual Style - Context
            trayMenu = new ContextMenu();
            trayMenu.MenuItems.Add("Show", OnShow);
            trayMenu.MenuItems.Add("About...", OnAbout);
            trayMenu.MenuItems.Add("Exit", OnExit);
            trayIcon = new NotifyIcon();
            trayIcon.Text = Resources.TrayIcon;
            trayIcon.Icon = new Icon(global::DSPClientDeamon.Properties.Resources.IntegratedServer, 40, 40);
            trayIcon.ContextMenu = trayMenu;
            trayIcon.Visible = true;
        }

        public ApplicationStartUp()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            Visible = false;
            ShowInTaskbar = false;
            base.OnLoad(e);
        }

        private void OnShow(object sender, EventArgs e)
        {
            Form1 mo = new Form1();
            mo.Show();
        }

        private void OnAbout(object sender, EventArgs e)
        {
            About a = new About();
            a.Show();
        }

        private void OnExit(object sender, EventArgs e)
        {
            if (MessageBox.Show("Desea salir de esta aplicacion?","Exit",MessageBoxButtons.OKCancel)==DialogResult.OK)
            {
                trayIcon.Dispose();
                Application.Exit();
            }
        }

        protected override void Dispose(bool isDisposing)
        {
            if (isDisposing)
            {
                trayIcon.Dispose();
            }
            base.Dispose(isDisposing);
        }

    }
}
