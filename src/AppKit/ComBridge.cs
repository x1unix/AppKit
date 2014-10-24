using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gecko;

namespace WebAppKit
{
    public static class ComBridge
    {
        #region "ComBridge"
        public static string InvokeScriptMethod(string script, GeckoWebBrowser canvas)
        {
            nsISupports thisPointer = (nsISupports)canvas.Document.GetHtmlElementById("Body").DomObject;
            string result;
            // Run some javascript without to read the HTML data
            using (var context = new AutoJSContext(canvas.Window.JSContext))
            {
                if (!context.EvaluateScript(script, thisPointer, out result))
                {
                    System.Windows.Forms.MessageBox.Show("Failed to execute Javascript");
                }
            }
            return result;
        }

        #endregion
    }
}
