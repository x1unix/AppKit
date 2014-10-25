using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.IO;
using System.Windows.Forms;
using System.Drawing;


namespace WebAppKit
{
    
    static class Program
    {
        public static wellcome WellcomeScreen = new wellcome();
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
            
            Application.EnableVisualStyles();
            //Application.SetCompatibleTextRenderingDefault(false);   
           // Application.Run(WellcomeScreen);

            string[] args = Environment.GetCommandLineArgs();
            if (args.Length > 1)
            {
                if (args.Length > 2)
                {
                    for (int x = 1; x < args.Length; x++)
                    {
                        Common.Arguments.Add(args[x]);
                    }
                }

                Midlet midlet = new Midlet(args[1]);
                
                Common.current_midlet = args[1];
                Common.work_path = midlet.Workpath;
                Common.DebugLevel = midlet.DebugLevel;
                if (Common.DebugLevel > 0 && Debugger.isInitialised == false)
                {
                    Debugger.Show(); 
                    Debugger.AddEvent(Common.current_midlet, "Loaded manifest successfully!");
                    Debugger.AddEvent("VM", "Work path set to: '" + Common.replaceConstant(Common.work_path) + "'");
                }
                
                IIBase.Load();
                //midlet.frame.Show();
               // Common.MainFrame = midlet.frame;
                Application.Run(midlet.frame);
                //WellcomeScreen.Hide();

            }
            else
            {
                Application.Exit();
            }
           
        }
    }

    #region "Common"
    public static class Common
    {
        public static List<string> Arguments = new List<string>();
        public static Gecko.GeckoWebBrowser MainFrame;
        public static int DebugLevel = 0;
        public static string ConvertToURL(string path)
        {
            string o = path.Replace("\\", "/");
            if (o.Contains("://"))
            {

            }
            else
            {
                o = "file:///" + o;
            }
            return o;
        }
        public static string current_midlet = "";
        public static string work_path = "";
        public static string replaceConstant(string str)
        {
            string menupath = "";
            if (Directory.Exists(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + "\\Microsoft\\Windows\\Start Menu\\Programs"))
            {
                menupath = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + "\\Microsoft\\Windows\\Start Menu\\Programs";
            }
            else
            {
                menupath = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + "\\Start Menu\\Programs";
            }

            str = str.Replace("$cd", Path.GetDirectoryName(current_midlet));
            str = str.Replace("$start_menu", menupath);
            str = str.Replace("$appdir", Application.StartupPath);
            str = str.Replace("$programfiles", Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles));
            str = str.Replace("$appdata", Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData));
            str = str.Replace("$documents", Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments));
            str = str.Replace("$home", Environment.ExpandEnvironmentVariables("%HOMEDRIVE%%HOMEPATH%"));
            str = str.Replace("$n", "\n");
            str = str.Replace("$desktop", Environment.GetFolderPath(Environment.SpecialFolder.Desktop));
            str = str.Replace("$desk_dir", Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory));
            str = str.Replace("$username", Environment.UserName);
            str = str.Replace("$os_version", Environment.OSVersion.VersionString);
            return str;

        }
    }
  
    #endregion



    public class Midlet
    {

        public Midlet(string xPath)
        {
            Location = xPath;
            if (File.Exists(xPath))
            {
                Manifest.Load(xPath);
                Common.current_midlet = xPath;
                readManifest();
            }
            else
            {
                  MessageBox.Show("Runtime Error:\n\nCannot load application. File not found:\n\n'" + xPath + "'", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                  Application.Exit();
            }
        }
        public int DebugLevel { get; set; }
        public string Name = "";
        public string Version = "";
        public string Vendor = "";
        public string Workpath = "";

        public AppHost frame = new AppHost();
        public string Location { get; set; }
        private XmlDocument Manifest = new XmlDocument();

        private void setVal(XmlAttribute at, string to)
        {
            if (at != null)
            {
                to = at.Value;
            }
        }
        private void setbool(XmlAttribute attrib, bool query)
        {
            if (attrib != null)
            {
                query = Convert.ToBoolean(attrib.Value);
            }
        }
        private string c(string query)
        {
            return Common.replaceConstant(query);
        }
        private void alert(string s)
        {
            MessageBox.Show(s);
        }
        private void readManifest()
        {
            frame.primary = true;
            XmlNode header = Manifest.SelectSingleNode("manifest").SelectSingleNode("application");
            XmlAttributeCollection attr = header.Attributes;
            setVal(attr["name"], Name);
            setVal(attr["version"], Version);
            setVal(attr["vendor"], Vendor);
            if (attr["workpath"] != null)
            {
                Workpath = attr["workpath"].Value;
                Common.work_path = Workpath;
            }
            if (attr["debuglevel"] != null)
            {
                DebugLevel = Convert.ToInt32(attr["debuglevel"].Value);
            }

            XmlNode xframe = header.SelectSingleNode("frame");
            XmlAttributeCollection f = xframe.Attributes;

            if (f["height"] != null)
            {
                frame.Height = Convert.ToInt32(f["height"].Value);
            }
            if (f["width"] != null)
            {
                frame.Width = Convert.ToInt32(f["width"].Value);
            }
            if (f["title"] != null)
            {
                frame.Text = c(f["title"].Value);
            }

            if (f["src"] != null)
            {
                string source = Common.replaceConstant(f["src"].Value);
                source = source.Replace("res:", attr["workpath"].Value + "\\");
                source = Common.replaceConstant(source);
                source = Common.ConvertToURL(source);
                frame.start = source;
                frame.isMain = true;
            }
            if (f["maximisebox"] != null)
            {
                frame.MaximizeBox = Convert.ToBoolean(f["maximisebox"].Value);
               // alert("maximisebox was set to: " + f["maximisebox"].Value);
            }
            if (f["minimisebox"] != null)
            {
               //  alert("MinimizeBox was set to: " + f["minimisebox"].Value);
            }
            if (f["controlbox"] != null)
            {
                frame.ControlBox = Convert.ToBoolean(f["controlbox"].Value);
            }
            if (f["icon"] != null)
            {
                string val = Common.replaceConstant(f["icon"].Value);
                val = val.Replace("res:", attr["workpath"].Value + "\\");
                val = Common.replaceConstant(val);
                val = val.Replace("/", "\\");
                if (File.Exists(val))
                {
                    Icon icon = Icon.ExtractAssociatedIcon(val);
                    frame.Icon = icon;
                    //alert("Icon found: " + val);
                }
                else
                {
                  //  alert("Icon NOT found: " + val);
                }
            }
            if (f["type"] != null)
            {
                switch (f["type"].Value.ToLower())
                {
                    case "window:fixed":
                        frame.FormBorderStyle = FormBorderStyle.FixedSingle;
                        break;
                    case "window:fixed-dialog":
                        frame.FormBorderStyle = FormBorderStyle.FixedDialog;
                        break;
                    case "window:fixed-3d":
                        frame.FormBorderStyle = FormBorderStyle.Fixed3D;
                        break;
                    case "window:fixed-tool":
                        frame.FormBorderStyle = FormBorderStyle.FixedToolWindow;
                        break;
                    case "none":
                        frame.FormBorderStyle = FormBorderStyle.None;
                        break;
                    case "window:sizable-tool":
                        frame.FormBorderStyle = FormBorderStyle.SizableToolWindow;
                        break;
                    case "window:sizable":
                        frame.FormBorderStyle = FormBorderStyle.Sizable;
                        break;
                    case "window:fullscreen":
                        frame.FormBorderStyle = FormBorderStyle.None;
                        frame.WindowState = FormWindowState.Maximized;
                        break;
                    default:
                        frame.FormBorderStyle = FormBorderStyle.Sizable;
                        break;
                }
            }

        }
    }
}
