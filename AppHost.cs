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
           // canvas.DocumentTitleChanged += new System.EventHandler(this.retrieveQuery);
           // this.Controls.Add(canvas);
            canvas.Navigate(start);

            //Debugger.Show();
            //var geckoDomElement = canvas.Document.DocumentElement;
            //if (geckoDomElement is GeckoHtmlElement)
            //{
            //    element = (GeckoHtmlElement)geckoDomElement;
            //    Debugger.Update(element.InnerHtml);
            //}
            //canvas.ViewSource();            
            
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
                if (IIBase.IntentInvokers[intent.query.Host] != null)
                {
                    IIBase.IntentInvokers[intent.query.Host].AttachIntent(intent);
                    IIBase.IntentInvokers[intent.query.Host].InvokeVoid();

                    if (IIBase.IntentInvokers[intent.query.Host].stdout)
                    {
                        var script = canvas.Document.CreateElement("script");
                        script.TextContent = IIBase.IntentInvokers[intent.query.Host].InvokeResult;
                        canvas.Document.GetElementsByTagName("head").First().AppendChild(script);
                        System.Threading.Thread.Sleep(500);
                    }
                }
                else
                {
                    string argus = "";
                    var args = System.Web.HttpUtility.ParseQueryString(intent.query.Query);
                    for (int ie = 0; ie <= args.Count - 1; ie++) { argus = argus + args.GetKey(ie) + " = " + args[args.GetKey(ie)] + "\n"; }
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
    }
}
