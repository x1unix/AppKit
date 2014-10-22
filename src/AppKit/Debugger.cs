using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WebAppKit
{
    public static class Debugger
    {
        public static bool isInitialised = false;
        public static DebugConsole DebugInterface = new DebugConsole();
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
        public static void AddEvent(string source, string event_desc){
            EventsLog.Text = "\n" + EventsLog.Text + source + ":: " + event_desc + ";" + Environment.NewLine + Environment.NewLine;
        }
        public static void AddError(string source, uint code, string message, uint line, uint chr)
        {
            ErrorsList.Text = ErrorsList.Text + source + " [" + code.ToString() + "]:: " + message + " at line " + line.ToString() + ", char " + chr.ToString() + ";\n" + Environment.NewLine + Environment.NewLine;
        }

        #region "Debugger constructor"
        public static System.Windows.Forms.TabControl Tabs;
        public static System.Windows.Forms.TabPage errTab;
        public static System.Windows.Forms.TabPage jsConsole;
        public static System.Windows.Forms.TextBox ErrorsList;
        public static System.Windows.Forms.TabPage logTab;
        public static System.Windows.Forms.TextBox EventsLog;

        public static void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DebugConsole));
            Tabs = new System.Windows.Forms.TabControl();
            errTab = new System.Windows.Forms.TabPage();
            jsConsole = new System.Windows.Forms.TabPage();
            logTab = new System.Windows.Forms.TabPage();
            ErrorsList = new System.Windows.Forms.TextBox();
            EventsLog = new System.Windows.Forms.TextBox();
            Tabs.SuspendLayout();
            errTab.SuspendLayout();
            logTab.SuspendLayout();
            
            // 
            // Tabs
            // 
            Tabs.Controls.Add(errTab);
            Tabs.Controls.Add(jsConsole);
            Tabs.Controls.Add(logTab);
            Tabs.Dock = System.Windows.Forms.DockStyle.Fill;
            Tabs.Location = new System.Drawing.Point(0, 0);
            Tabs.Name = "Tabs";
            Tabs.SelectedIndex = 0;
            Tabs.Size = new System.Drawing.Size(415, 453);
            Tabs.TabIndex = 0;
            // 
            // errTab
            // 
            errTab.Controls.Add(ErrorsList);
            errTab.Location = new System.Drawing.Point(4, 22);
            errTab.Name = "errTab";
            errTab.Padding = new System.Windows.Forms.Padding(3);
            errTab.Size = new System.Drawing.Size(407, 427);
            errTab.TabIndex = 0;
            errTab.Text = "Error List";
            errTab.UseVisualStyleBackColor = true;
            // 
            // jsConsole
            // 
            jsConsole.Location = new System.Drawing.Point(4, 22);
            jsConsole.Name = "jsConsole";
            jsConsole.Padding = new System.Windows.Forms.Padding(3);
            jsConsole.Size = new System.Drawing.Size(407, 427);
            jsConsole.TabIndex = 1;
            jsConsole.Text = "Console";
            jsConsole.UseVisualStyleBackColor = true;
            // 
            // logTab
            // 
            logTab.Controls.Add(EventsLog);
            logTab.Location = new System.Drawing.Point(4, 22);
            logTab.Name = "logTab";
            logTab.Size = new System.Drawing.Size(407, 427);
            logTab.TabIndex = 2;
            logTab.Text = "Events Log";
            logTab.UseVisualStyleBackColor = true;
            // 
            // ErrorsList
            // 
            ErrorsList.Dock = System.Windows.Forms.DockStyle.Fill;
            ErrorsList.HideSelection = false;
            ErrorsList.Location = new System.Drawing.Point(3, 3);
            ErrorsList.Multiline = true;
            ErrorsList.Name = "ErrorsList";
            ErrorsList.ReadOnly = true;
            ErrorsList.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            ErrorsList.Size = new System.Drawing.Size(401, 421);
            ErrorsList.TabIndex = 0;
            //EventsLog
            EventsLog.Dock = System.Windows.Forms.DockStyle.Fill;
            EventsLog.HideSelection = false;
            EventsLog.Location = new System.Drawing.Point(3, 3);
            EventsLog.Multiline = true;
            EventsLog.Name = "EventsLog";
            EventsLog.ReadOnly = true;
            EventsLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            EventsLog.Size = new System.Drawing.Size(401, 421);
            EventsLog.TabIndex = 0;

        }
        #endregion
    }
}
