using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace WebAppKit
{
    public static class Debugger
    {
        public static bool isInitialised = false;
        public static DebugConsole DebugInterface = new DebugConsole();
        public static AppHost currentHost;
        public static Gecko.GeckoWebBrowser target;
        public static Gecko.GeckoWebBrowser MainFrame;
        public static void Show()
        {
            if (isInitialised == false)
            {
                DebugInterface.Show();
                InitializeComponent();
                DebugInterface.Controls.Add(Tabs);
                isInitialised = true;
            }
        }
        public static void GetSource(Gecko.GeckoWebBrowser w)
        {
            PgSource.Text = "";
            if (!string.IsNullOrEmpty(w.Document.GetElementsByTagName("html")[0].InnerHtml))
                PgSource.Text = w.Document.GetElementsByTagName("html")[0].InnerHtml;
            PgSource.Text = "<html>" + Environment.NewLine + PgSource.Text.Replace("\n", Environment.NewLine) + Environment.NewLine+"</html>";


        }
        public static void AddEvent(string source, string event_desc){
            EventsLog.Text = "\n" + EventsLog.Text + source + ":: " + event_desc + ";" + Environment.NewLine + Environment.NewLine;
        }
        public static void AddError(string source, uint code, string message, uint line, uint chr)
        {
            System.Media.SystemSounds.Asterisk.Play();
            ErrorsList.Text = ErrorsList.Text + source + " [" + code.ToString() + "]:: " + message + " at line " + line.ToString() + ", char " + chr.ToString() + ";\n" + Environment.NewLine + Environment.NewLine;
        }

        #region "Debugger constructor"
        public static System.Windows.Forms.TabControl Tabs;
        public static System.Windows.Forms.TabPage errTab;
        public static System.Windows.Forms.TabPage jsConsole;
        public static System.Windows.Forms.TextBox ErrorsList;
        public static System.Windows.Forms.TextBox PgSource;
        public static TextBox JSConsole;
        public static TextBox JSInput = new TextBox();
        public static System.Windows.Forms.TabPage logTab;
        public static System.Windows.Forms.TabPage sourceTab;
        public static System.Windows.Forms.TextBox EventsLog;

        private static Font InputFont
        {
            get
            {
                string fontName = "Consolas";
                float fontSize = 12;
                Font fontTester = new Font(fontName, fontSize, FontStyle.Regular, GraphicsUnit.Pixel);
                if (fontTester.Name == fontName)
                {
                    // Font exists
                    return fontTester;
                }
                else
                {
                    // Font doesn't exist
                    return new Font("Courier New", fontSize, FontStyle.Regular, GraphicsUnit.Pixel);
                }
            }
        }
        private static void tab_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (Tabs.SelectedIndex)
            {
                case 1:
                    if (target == null)
                    {
                        target = Common.MainFrame;
                    }
                    GetSource(target);
                    break;
            }
        }
        
        private static void dbgconsole_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) { JSConsole.Text = JSConsole.Text + JSInput.Text + Environment.NewLine+currentHost.InvokeScriptMethod(JSInput.Text) + Environment.NewLine; JSInput.Text = "// Type code here and press 'Enter'"; }
        }
        public static void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DebugConsole));
            Tabs = new System.Windows.Forms.TabControl();
            errTab = new System.Windows.Forms.TabPage();
            jsConsole = new System.Windows.Forms.TabPage();
            logTab = new System.Windows.Forms.TabPage();
            PgSource = new System.Windows.Forms.TextBox();
            sourceTab = new System.Windows.Forms.TabPage();
            ErrorsList = new System.Windows.Forms.TextBox();
            EventsLog = new System.Windows.Forms.TextBox();
            JSConsole = new TextBox();
            /*Tabs.SuspendLayout();
            errTab.SuspendLayout();
            logTab.SuspendLayout();
            */
            
            
            // 
            // Tabs
            // 
            Tabs.Controls.Add(errTab);
            Tabs.Controls.Add(sourceTab);
            Tabs.Controls.Add(jsConsole);
            Tabs.Controls.Add(logTab);
            Tabs.Dock = System.Windows.Forms.DockStyle.Fill;
            Tabs.Location = new System.Drawing.Point(0, 0);
            Tabs.Name = "Tabs";
            Tabs.SelectedIndex = 0;
            Tabs.Size = new System.Drawing.Size(415, 453);
            Tabs.TabIndex = 0;

            //
            // JSInput
            //

            JSInput.Font = InputFont;
            JSInput.BackColor = System.Drawing.Color.FromArgb(39, 40, 44);
            //JSInput.BackColor = System.Drawing.Color.FromArgb(255, 40, 255);
            JSInput.ForeColor = System.Drawing.Color.FromArgb(230, 219, 116);
            JSInput.Dock = System.Windows.Forms.DockStyle.None;
            JSInput.Height = 48;
            JSInput.HideSelection = false;
            JSInput.Location = new System.Drawing.Point(3, 3);
            JSInput.Dock = DockStyle.Bottom;
            JSInput.Multiline = false;
            JSInput.Name = "JSInput";
            JSInput.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            JSInput.Size = new System.Drawing.Size(401, 421);
            JSInput.KeyDown += new System.Windows.Forms.KeyEventHandler(dbgconsole_KeyDown);
            JSInput.Text = "// Type code here and press 'Enter'";

            //
            // JSConsole
            //

            JSConsole.Font = InputFont;
            JSConsole.BackColor = System.Drawing.Color.FromArgb(39, 40, 44);
            JSConsole.ForeColor = System.Drawing.Color.FromArgb(230, 219, 116);
            JSConsole.Dock = System.Windows.Forms.DockStyle.Fill;
            JSConsole.HideSelection = false;
            JSConsole.Location = new System.Drawing.Point(3, 3);
            JSConsole.Multiline = true;
            JSConsole.ReadOnly = true;
            JSConsole.Name = "JSConsole";
            JSConsole.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            JSConsole.Size = new System.Drawing.Size(401, 421);
            //JSConsole.KeyDown += new System.Windows.Forms.KeyEventHandler(dbgconsole_KeyDown);


            // 
            // sourceTab
            // 

            sourceTab.Controls.Add(PgSource);
            sourceTab.Location = new System.Drawing.Point(4, 22);
            sourceTab.Name = "sourceTab";
            sourceTab.Padding = new System.Windows.Forms.Padding(3);
            sourceTab.Size = new System.Drawing.Size(407, 427);
            sourceTab.TabIndex = 1;
            sourceTab.Text = "Source";
            sourceTab.UseVisualStyleBackColor = true;
            // 
            // errTab
            // 
            errTab.Controls.Add(ErrorsList);
            errTab.Location = new System.Drawing.Point(4, 22);
            errTab.Name = "errTab";
            errTab.Padding = new System.Windows.Forms.Padding(3);
            errTab.Size = new System.Drawing.Size(407, 427);
            errTab.TabIndex = 2;
            errTab.Text = "Error List";
            errTab.UseVisualStyleBackColor = true;
            // 
            // jsConsole
            // 
            jsConsole.Controls.Add(JSInput);
            jsConsole.Controls.Add(JSConsole);
            
            jsConsole.Location = new System.Drawing.Point(4, 22);
            jsConsole.Name = "jsConsole";
            jsConsole.Padding = new System.Windows.Forms.Padding(3);
            jsConsole.Size = new System.Drawing.Size(407, 427);
            //jsConsole.TabIndex = 3;
            jsConsole.Text = "Console";
            jsConsole.UseVisualStyleBackColor = true;
            // 
            // logTab
            // 
            logTab.Controls.Add(EventsLog);
            logTab.Location = new System.Drawing.Point(4, 22);
            logTab.Name = "logTab";
            logTab.Size = new System.Drawing.Size(407, 427);
            logTab.TabIndex = 4;
            logTab.Text = "Events Log";
            logTab.UseVisualStyleBackColor = true;
            // 
            // PgSource
            // 
            PgSource.Dock = System.Windows.Forms.DockStyle.Fill;
            PgSource.HideSelection = false;
            PgSource.Location = new System.Drawing.Point(3, 3);
            PgSource.Font = InputFont;
            PgSource.BackColor = System.Drawing.Color.FromArgb(39, 40, 44);
            PgSource.ForeColor = System.Drawing.Color.WhiteSmoke;
            PgSource.Multiline = true;
            PgSource.Name = "PgSource";
            PgSource.ReadOnly = true;
            PgSource.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            PgSource.Size = new System.Drawing.Size(401, 421);


            // 
            // ErrorsList
            // 
            ErrorsList.Dock = System.Windows.Forms.DockStyle.Fill;
            ErrorsList.HideSelection = false;
            ErrorsList.Location = new System.Drawing.Point(3, 3);
            ErrorsList.Multiline = true;
            ErrorsList.Name = "ErrorsList";
            ErrorsList.ReadOnly = true;
            ErrorsList.Font = InputFont;
            ErrorsList.BackColor = System.Drawing.Color.FromArgb(39, 40, 44);
            ErrorsList.ForeColor = System.Drawing.Color.Tomato;
            ErrorsList.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            ErrorsList.Size = new System.Drawing.Size(401, 421);
            ErrorsList.TabIndex = 0;



            //EventsLog
            EventsLog.Font = InputFont;
            EventsLog.BackColor = System.Drawing.Color.FromArgb(39, 40, 44);
            EventsLog.ForeColor = System.Drawing.Color.FromArgb(230, 219, 116);
            EventsLog.Dock = System.Windows.Forms.DockStyle.Fill;
            EventsLog.HideSelection = false;
            EventsLog.Location = new System.Drawing.Point(3, 3);
            EventsLog.Multiline = true;
            EventsLog.Name = "EventsLog";
            EventsLog.ReadOnly = true;
            EventsLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            EventsLog.Size = new System.Drawing.Size(401, 421);
            EventsLog.TabIndex = 0;
           
            Tabs.SelectedIndexChanged += new System.EventHandler(tab_SelectedIndexChanged);

            
        }
        #endregion
    }
}
