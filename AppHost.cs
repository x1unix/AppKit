using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
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

      //  public GeckoWebBrowser wb = new GeckoWebBrowser();
        public string start = "";
        public bool primary = false;
        public string xLayout = "";
        private void Form1_Load(object sender, EventArgs e)
        {
            //Gecko.Xpcom.Initialize();

           // canvas.Dock = DockStyle.Fill;
            canvas.NoDefaultContextMenu = true;

            canvas.Navigate(start);
            if (Common.DebugLevel > 0)
            {
                if (Common.DebugLevel == 3 && Debugger.isInitialised == false)
                {
                    Debugger.Show();
                }
                Debugger.AddEvent("Frame ['"+this.Text+"']","Frame initialised");
                Debugger.AddEvent("Frame ['" + this.Text + "']", "Frame navigated to '"+start+"'");
                this.canvas.JavascriptError += new Gecko.GeckoWebBrowser.JavascriptErrorEventHandler(this.canvas_JavascriptError);
            }
   }

        private void AppHost_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (primary) { Application.Exit(); }
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
                    IIBase.IntentInvokers[intent.query.Host].AttachIntent(intent);
                    IIBase.IntentInvokers[intent.query.Host].InvokeVoid();

                    if (IIBase.IntentInvokers[intent.query.Host].stdout)
                    {
                        Debugger.AddEvent("RE->DOC", IIBase.IntentInvokers[intent.query.Host].InvokeResult);
                        var script = canvas.Document.CreateElement("script");
                        script.TextContent = IIBase.IntentInvokers[intent.query.Host].InvokeResult;
                        canvas.Document.GetElementsByTagName("head").First().AppendChild(script);
                        System.Threading.Thread.Sleep(500);
                    }
                }
                else
                {
                    Debugger.AddEvent("Frame ['" + this.Text + "']", "Unknown Intent invoke ('" + canvas.DocumentTitle + "')");
                    IIBase.throwError("Requested method is not recognized", "Unknown Environment Query (" + intent.query.Host + ")", "BadQueryException (" + intent.query.Host + ")\nat System.Query()\nat " + Path.GetFileName(Application.ExecutablePath) + "\n\nQuery arguments:\n\n" + argus, 000103);
                }

            }
            else
            {
                this.Text = canvas.DocumentTitle;
            }
        }


        private void canvas_LocationChanged(object sender, EventArgs e)
        {
            
            
        }

        private void canvas_JavascriptError(object sender, JavascriptErrorEventArgs e)
        {
            if (Debugger.isInitialised == false)
            {
                Debugger.Show();
            }
            Debugger.AddError(e.Filename, e.ErrorNumber, e.Message, e.Line, e.Pos);
            //IIBase.throwError("Unhandled JavaScript error",e.Message,"[filename: '"+e.Filename+"']; [line: '"+e.Line+"']; [pos: '"+e.Pos+"']",Convert.ToInt32(e.ErrorNumber));
           
        }
    }
}
