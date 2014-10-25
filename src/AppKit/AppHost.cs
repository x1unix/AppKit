using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Management;
using System.Windows.Forms;
using Gecko;
using System.IO;
using System.Web;
using System.Diagnostics;
namespace WebAppKit
{
    public partial class AppHost : Form
    {
        public AppHost()
        {
            InitializeComponent();
            
        }
        public GeckoHtmlElement element = null;
        public bool isMain = false;
        public string start = "";
        public bool primary = false;
        public string xLayout = "";
        private void Form1_Load(object sender, EventArgs e)
        {
            canvas.NoDefaultContextMenu = true;
            canvas.Navigate(start);
            if (Common.DebugLevel > 0)
            {
                if (Common.DebugLevel == 3 && Debugger.isInitialised == false)
                {
                    Debugger.Show();
                }
                Debugger.target = canvas;
                Debugger.currentHost = this;
                Debugger.AddEvent("Frame ['"+this.Text+"']","Frame initialised");
                Debugger.AddEvent("Frame ['" + this.Text + "']", "Frame navigated to '"+start+"'");
                this.canvas.JavascriptError += new Gecko.GeckoWebBrowser.JavascriptErrorEventHandler(this.canvas_JavascriptError);
                if (isMain) { Debugger.MainFrame = canvas;
                
                    
                    
                    /*InvokeScriptMethod(IResultConverter.JSArray("Framework.CommandLineArgs", Common.Arguments.ToArray()));*/ }
            }

   }

        private void AppHost_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (primary || isMain) { Application.Exit(); } else { Debugger.target = Debugger.MainFrame; }
        }

        private void retrieveQuery(object sender, EventArgs e)
        {
            Intent intent = new Intent(canvas.DocumentTitle);
           
            if (intent.isValid)
            {
                string argus = "";
                Debugger.AddEvent("Frame ['" + this.Text + "']", "Received Intent ('" + canvas.DocumentTitle + "')");
                if (IIBase.IntentInvokers[intent.query.Host] != null)
                {
                    ;
                    var args = System.Web.HttpUtility.ParseQueryString(intent.query.Query);
                    for (int ie = 0; ie <= args.Count - 1; ie++) { argus = argus + "["+args.GetKey(ie) + "] = " + args[args.GetKey(ie)] + "; \n"; }

                    Debugger.AddEvent("Frame ['" + this.Text + "']", "Invoke method '" + intent.query.Host + "', Args: { "+argus+" }");
                    try
                    {
                        IIBase.IntentInvokers[intent.query.Host].AttachIntent(intent);
                        IIBase.IntentInvokers[intent.query.Host].AttachIntent(intent);
                        IIBase.IntentInvokers[intent.query.Host].InvokeVoid();

                        if (IIBase.IntentInvokers[intent.query.Host].stdout)
                        {
                            InvokeScriptMethod(IIBase.IntentInvokers[intent.query.Host].InvokeResult);
                            


                        }
                    }
                    catch (NullReferenceException)
                    {
                        Debugger.AddEvent("Frame ['" + this.Text + "']", "Unknown Intent invoke ('" + canvas.DocumentTitle + "')");
                        IIBase.throwError("Requested method is not recognized", "Unknown Environment Query (" + intent.query.Host + ")", "BadQueryException (" + intent.query.Host + ")\nat System.Query()\nat " + Path.GetFileName(Application.ExecutablePath) + "\n\nQuery arguments:\n\n" + argus, 000103);
                    }
                    
                }
                else
                {
                    Debugger.AddEvent("Frame ['" + this.Text + "']", "Unknown Intent invoke ('" + canvas.DocumentTitle + "')");
                    IIBase.throwError("Requested method is not recognized", "Unknown Environment Query (" + intent.query.Host + ")", "BadQueryException (" + intent.query.Host + ")\nat System.Query()\nat " + Path.GetFileName(Application.ExecutablePath) + "\n\nQuery arguments:\n\n" + argus, 000103);
                }
                InvokeScriptMethod("document.title = '"+this.Text+"';");

            }
            else
            {
                this.Text = canvas.DocumentTitle;
                MessageBox.Show(canvas.DocumentTitle);
            }
        }

        #region "ComBridge"
        /// <summary>
        /// Invokes JavaScript and returns to document
        /// </summary>
        /// <param name="script">JavaScript Code</param>
        /// <returns></returns>
        public string InvokeScriptMethod(string script)
        {
            Debugger.AddEvent("RE->DOC", script);
            //nsISupports thisPointer = (nsISupports)canvas.Document.GetHtmlElementById("Body").DomObject;
            nsISupports thisPointer = (nsISupports)canvas.Window.DomWindow;
            string result;
            using (var context = new AutoJSContext(canvas.Window.JSContext))
            {
                if (!context.EvaluateScript(script, thisPointer, out result))
                {
                    Debugger.AddError("JSVM", 13, "Failed to invoke JavaScript ['"+script+"']", 0, 0);
                    //System.Windows.Forms.MessageBox.Show("Failed to execute Javascript");
                }
            }
            System.Threading.Thread.Sleep(500);
            return result;
        }
        public void AppendScript(string jscript)
        {
            //Debugger.AddEvent("RE->DOC", IIBase.IntentInvokers[intent.query.Host].InvokeResult);
            var script = canvas.Document.CreateElement("script");
            script.TextContent = jscript;
            canvas.Document.GetElementsByTagName("head").First().AppendChild(script);
            //System.Threading.Thread.Sleep(500);
        }
        #endregion
        private void canvas_JavascriptError(object sender, JavascriptErrorEventArgs e)
        {
            if (Debugger.isInitialised == false)
            {
                Debugger.Show();
            }
            Debugger.AddError(e.Filename, e.ErrorNumber, e.Message, e.Line, e.Pos);
            //IIBase.throwError("Unhandled JavaScript error",e.Message,"[filename: '"+e.Filename+"']; [line: '"+e.Line+"']; [pos: '"+e.Pos+"']",Convert.ToInt32(e.ErrorNumber));
           
        }

        private void canvas_DocumentCompleted(object sender, Gecko.Events.GeckoDocumentCompletedEventArgs e)
        {
            JSContainer jsapi = new JSContainer();
            // Append API and environment info
            jsapi.AddScript(Properties.Resources.AppKit_IO);
            jsapi.AddScript(Properties.Resources.AppKit);
            jsapi.AddScript(IResultConverter.JSString("Framework.Environment.Hostname", Environment.MachineName, false));
            jsapi.AddScript(IResultConverter.JSLong("Framework.Environment.WorkingSet", Environment.WorkingSet, false));
            jsapi.AddScript(IResultConverter.JSString("Framework.Environment.OSVersion", Environment.OSVersion.VersionString, false));
            jsapi.AddScript(IResultConverter.JSString("Framework.Environment.OS", SystemInformation.OSName, false));
            jsapi.AddScript(IResultConverter.JSArray("Framework.CommandLineArgs", Common.Arguments.ToArray(), true, false));
            jsapi.AddScript(IResultConverter.JSString("Framework.RuntimeVersion", Application.ProductVersion, false));
            InvokeScriptMethod(jsapi.Content);
            //System.Threading.Thread.Sleep(1000);
            InvokeScriptMethod("onCreate();");
           
        }

        private void canvas_Navigated(object sender, GeckoNavigatedEventArgs e)
        {

        }
    }
}
