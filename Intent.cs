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
    /// <summary>
    /// Intent invoker and handler
    /// </summary>
    public class IntentInvoker
    {
        string _IntentId;
        public string IntentId { get { return _IntentId; } set { _IntentId = value; } }

        System.Collections.Specialized.NameValueCollection _args;
        public System.Collections.Specialized.NameValueCollection args { get { return _args; } set { _args = value; } }

        Uri _query;
        public Uri query { get { return _query; } set { _query = value; } }


        bool _stdout = false;
        public bool stdout { get { return _stdout; } set { _stdout = value; } }

        string _var_out;
        public string var_out { get { return _var_out; } set { _var_out = value; } }

        string _io = "undefined";
        public bool ReturnValue { get { return stdout; } set { stdout = value; } }

        public string InvokeResult { get { return _io; } set { _io = value; } }
        public string io { get { return _io; } set { _io = value; } }
        public void AttachIntent(Intent intent)
        {
            this.query = intent.query;
            this.args = intent.args;
            this.IntentId = intent.IntentId;

            if (args["stdout"] != null)
            {
                if (args["stdout"] == "undefined" || args["stdout"] == "null")
                { this.stdout = false; }
                else
                {
                    this.stdout = true;
                    this.var_out = args["stdout"];
                }
            }
        }
        //public IntentInvoker()
        //{
            
        //}

        public void throwError(string description, string error, string at, int code)
        {
            MessageBox.Show("Runtime Error\n\nAn unhandled error has occured in this application.\nContact to application developer and report about error.\n\nError Code: " + error + " [" + code.ToString() + "]\nDescription: " + description + "\n\nDetails:\n\n" + at, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        /// <summary>
        /// Calls invoked function
        /// </summary>
        public virtual void InvokeVoid()
        {

        }
        
        
    }
    public class Intent{
        public string IntentId;
        public bool isValid;
        public System.Collections.Specialized.NameValueCollection args;
        public Uri query;
        public Intent(string intentQuery) {
            query = new Uri(intentQuery);
            args = System.Web.HttpUtility.ParseQueryString(query.Query);
            IntentId = query.Host;
            if (query.Scheme == "intent" || query.Scheme == "query") { isValid = true; } else { isValid = false; }
        }
    }
    /// <summary>
    /// Native code to JS converter
    /// </summary>
    public static class IResultConverter
    {
        /// <summary>
        /// Convert String to JavaScript variable
        /// </summary>
        /// <param name="JS_Var">Target</param>
        /// <param name="Value">Value</param>
        /// <returns>Javacript variable with string</returns>
        public static string JSString(string JS_Var, string Value) { return "var " + JS_Var + " = \"" + Value + "\";"; }
        /// <summary>
        /// Convert data to JavaScript bool variable
        /// </summary>
        /// <param name="JS_Var">Variable</param>
        /// <param name="Value">Value</param>
        /// <returns>Javascript bool variable</returns>
        public static string JSBool(string JS_Var, bool Value) { return "var " + JS_Var + " = " + Convert.ToString(Value) + ";"; }
        public static string JSInteger(string JS_Var, int Value) { return "var " + JS_Var + " = " + Convert.ToString(Value) + ";"; }
        public static string JSLong(string JS_Var, long Value) { return "var " + JS_Var + " = " + Convert.ToString(Value) + ";"; }

        
        public static string JSArray(string JS_Var, string[] arr, bool pathFilter = true)
        {
            int to = arr.Length - 1;
            string _return = "[";
            for (int x = 0; x <= to; x++)
            {
                string comma = "";
                if (x != to)
                {
                    comma = ",";
                }
                string c = arr[x];
                if (pathFilter) { c = arr[x].Replace("\\", "\\\\"); }
                _return = _return + "\"" + c + "\"" + comma;
            }
            _return = _return + "]";
            _return = "var " + JS_Var + " = " + _return+";";
            return _return;
        }

    }
}
