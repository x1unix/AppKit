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

    //Framework
    #region "fr_Splash"
    public class fr_SplashInvoker : IntentInvoker
    {
        public override void InvokeVoid()
        {
            wellcome w = new wellcome();
            w.Show();
        }
    }
    #endregion
    //Modal Windows
    #region "mod_OpenFileDialog"
    public class mod_OpenFileDialogInvoker : IntentInvoker
    {
        public override void InvokeVoid()
        {
            io = "";
            string ofd = "";
            string ofd_filter = "";
            if (args["title"] != "undefined")
            {
                ofd = args["title"];
            }
            if (args["filter"] != "undefined")
            {
                ofd_filter = args["filter"];
            }
            OpenFileDialog openfiledialog = new OpenFileDialog();
            openfiledialog.Title = ofd;
            openfiledialog.Filter = ofd_filter;
            openfiledialog.ShowDialog();
            io = "'" + openfiledialog.FileName + "'";

            io = io.Replace("\\", "\\\\");
            if (stdout == true)
            {
                InvokeResult = IResultConverter.JSString(var_out, openfiledialog.FileName);
            }
        }
    }
    #endregion
    #region "mod_MessageBox"
    public class mod_MessageBoxInvoker : IntentInvoker
    {
        public override void InvokeVoid()
        {
            MessageBoxIcon ico;
            switch (args["icon"])
            {
                case "0":
                    ico = MessageBoxIcon.Asterisk;
                    break;
                case "1":
                    ico = MessageBoxIcon.Error;
                    break;
                case "2":
                    ico = MessageBoxIcon.Exclamation;
                    break;
                case "3":
                    ico = MessageBoxIcon.Hand;
                    break;
                case "4":
                    ico = MessageBoxIcon.Information;
                    break;
                case "5":
                    ico = MessageBoxIcon.Question;
                    break;
                case "6":
                    ico = MessageBoxIcon.Stop;
                    break;
                case "7":
                    ico = MessageBoxIcon.Warning;
                    break;
                default:
                    ico = MessageBoxIcon.None;
                    break;
            }
            MessageBox.Show(args["content"], args["title"], MessageBoxButtons.OK, ico);
        }
    }
    #endregion

    //Environment
    #region "env_GetHostNameInvoker"
    public class env_GetHostNameInvoker : IntentInvoker
    {
        public override void InvokeVoid()
        {
            io = "";
            InvokeResult = IResultConverter.JSString(var_out, Environment.MachineName);
        }
    }
    #endregion

    #region "env_ExpandVariablesInvoker"
    public class env_ExpandVariablesInvoker : IntentInvoker
    {
        public override void InvokeVoid()
        {
            io = "";
            InvokeResult = IResultConverter.JSString(var_out, Environment.ExpandEnvironmentVariables(args["var"]));
        }
    }
    #endregion

    #region "env_GetOSVersionInvoker"
    public class env_GetOSVersionInvoker : IntentInvoker
    {
        public override void InvokeVoid()
        {
            io = "";
            InvokeResult = IResultConverter.JSString(var_out, Environment.OSVersion.VersionString);
        }
    }
    #endregion

    #region "env_GetWorkingSetInvoker"
    public class env_GetWorkingSetInvoker : IntentInvoker
    {
        public override void InvokeVoid()
        {
            io = "";
            InvokeResult = IResultConverter.JSLong(var_out, Environment.WorkingSet);
        }
    }
    #endregion


    //Windows
    #region "frm_CloseFrameInvoker"
    public class frm_CloseFrameInvoker : IntentInvoker
    {
        public override void InvokeVoid()
        {
            string fn = args["name"];
            if (Runtime.frames[fn] != null) { Runtime.frames[fn].Close(); }
        }
    }
    #endregion

    #region "frm_HideFrameInvoker"
    public class frm_HideFrameInvoker : IntentInvoker
    {
        public override void InvokeVoid()
        {
            string fn = args["name"];
            if (Runtime.frames[fn] != null) { Runtime.frames[fn].Hide(); }
        }
    }
    #endregion

    #region "frm_ShowFrameInvoker"
    public class frm_ShowFrameInvoker : IntentInvoker
    {
        public override void InvokeVoid()
        {
            string fn = args["name"];
            if (Runtime.frames[fn] != null) { Runtime.frames[fn].Show(); }
        }
    }
    #endregion

    #region "frm_LoadFrameInvoker"
    public class frm_LoadFrameInvoker : IntentInvoker
    {
        public override void InvokeVoid()
        {
            AppHost a = new AppHost();
            a.Text = args["title"];
            string source = Common.replaceConstant(args["src"]);
            source = source.Replace("res:", Common.work_path + "\\");
            source = Common.replaceConstant(source);
            source = Common.ConvertToURL(source);

            a.start = source;
            a.Height = Convert.ToInt32(args["height"]);
            a.Width = Convert.ToInt32(args["width"]);
            double d = Convert.ToDouble(args["opacity"]);
            d = d / 100;
            a.Opacity = d;
            a.MinimizeBox = Convert.ToBoolean(args["minbox"]);
            a.MaximizeBox = Convert.ToBoolean(args["maxbox"]);
            a.ControlBox = Convert.ToBoolean(args["control"]);
            a.ShowInTaskbar = Convert.ToBoolean(args["showintaskbar"]);
            Runtime.frames.Add(args["name"], a);
            Runtime.frames[args["name"]].Show();
        }
    }
    #endregion

    // Directory management
    #region "fm_DirExistsInvoker"
    public class fm_DirExistsInvoker : IntentInvoker
    {
        public override void InvokeVoid()
        {
            io = "";
            InvokeResult = IResultConverter.JSBool(var_out, Directory.Exists(args["path"]));
        }
    }
    #endregion

    #region "fm_DirCreateInvoker"
    public class fm_DirCreateInvoker : IntentInvoker
    {
        public override void InvokeVoid()
        {
            io = "";
            try
            {
                Directory.CreateDirectory(args["path"]);
                InvokeResult = IResultConverter.JSBool(var_out, true);
            }
            catch (Exception)
            { InvokeResult = IResultConverter.JSBool(var_out, false); }
        }
    }
    #endregion

    #region "fm_DirDeleteInvoker"
    public class fm_DirDeleteInvoker : IntentInvoker
    {
        public override void InvokeVoid()
        {
            io = "";
            try
            {
                Directory.Delete(args["path"]);
                InvokeResult = IResultConverter.JSBool(var_out, true);
            }
            catch (Exception)
            { InvokeResult = IResultConverter.JSBool(var_out, false); }
        }
    }
    #endregion

    #region "fm_GetFilesInvoker"
    public class fm_GetFilesInvoker : IntentInvoker
    {
        public override void InvokeVoid()
        {
            io = "";
            try
            {
                if (stdout == true)
                {
                    InvokeResult = IResultConverter.JSArray(var_out, Directory.GetFiles(args["path"]), true);
                }
            }
            catch (Exception) { InvokeResult = IResultConverter.JSBool(var_out, false); }
        }
    }
    #endregion

    #region "fm_GetDirectoriesInvoker"
    public class fm_GetDirectoriesInvoker : IntentInvoker
    {
        public override void InvokeVoid()
        {
            io = "";
            try
            {
                if (stdout == true)
                {
                    InvokeResult = IResultConverter.JSArray(var_out, Directory.GetDirectories(args["path"]), true);
                }
            }
            catch (Exception) { InvokeResult = IResultConverter.JSBool(var_out, false); }
        }
    }
    #endregion

    #region "fm_GetDirectoriesRootInvoker"
    public class fm_GetDirectoriesRootInvoker : IntentInvoker
    {
        public override void InvokeVoid()
        {
            io = "";
            try
            {
                if (stdout == true)
                {
                    InvokeResult = IResultConverter.JSArray(var_out, Directory.GetLogicalDrives(), true);
                }
            }
            catch (Exception)
            {
                InvokeResult = IResultConverter.JSBool(var_out, false);
            }
        }
    }
    #endregion

    //Shell
    #region "CmdShellInvoker"
    public class CmdShellInvoker : IntentInvoker
    {
        public override void InvokeVoid()
        {
            io = "";
            Process.Start(args["command"]);
        }
    }
    #endregion

    //File management
    #region "fm_FileExistsInvoker"
    public class fm_FileExistsInvoker : IntentInvoker
    {
        public override void InvokeVoid()
        {
            string file = args["filename"];
            InvokeResult = IResultConverter.JSBool(var_out, File.Exists(file));
        }
    }
    #endregion

    #region "fm_MoveFileInvoker"
    public class fm_MoveFileInvoker : IntentInvoker
    {
        public override void InvokeVoid()
        {
            io = "";
            string file_for_move = args["filename"];
            string pp = Path.GetFileName(file_for_move);
            string mv_Destination = args["destination"];
            if (File.Exists(file_for_move))
            {
                if (File.Exists(mv_Destination) || File.Exists(mv_Destination + "\\" + pp))
                {
                    io = "false";
                }
                else
                {
                    File.Move(file_for_move, mv_Destination);
                    io = "true";
                }
            }
            else
            {
                io = "false";
            }
            if (stdout == true)
            {
                InvokeResult = IResultConverter.JSBool(var_out, Convert.ToBoolean(io));
            }
        }
    }
    #endregion

    #region "fm_CopyFileInvoker"
    public class fm_CopyFileInvoker : IntentInvoker
    {
        public override void InvokeVoid()
        {
            io = "";
            string file_for_copy = args["filename"];
            string cp_Destination = args["destination"];
            if (File.Exists(file_for_copy))
            {
                string p = Path.GetFileName(file_for_copy);
                if (File.Exists(cp_Destination) || File.Exists(cp_Destination + "\\" + p))
                {
                    io = "false";
                }
                else
                {
                    File.Move(file_for_copy, cp_Destination);
                    io = "true";
                }
            }
            else
            {
                io = "false";
            }
            if (stdout == true)
            {
                InvokeResult = IResultConverter.JSBool(var_out, Convert.ToBoolean(io));
            }
        }
    }
    #endregion

    #region "fm_DeleteFileInvoker"
    public class fm_DeleteFileInvoker : IntentInvoker
    {
        public override void InvokeVoid()
        {
            io = "";
            string file = args["filename"];
            if (File.Exists(file))
            {
                io = "true";
                try
                {
                    File.Delete(file);
                }
                catch (Exception)
                {
                    io = "false";
                }
            }
            else
            {
                io = "false";
            }
            if (stdout == true)
            {
                InvokeResult = IResultConverter.JSBool(var_out, Convert.ToBoolean(io));
            }
        }
    }
    #endregion

    #region "fm_ReadFileInvoker"
    public class fm_ReadFileInvoker : IntentInvoker
    {
        public override void InvokeVoid()
        {
            io = "";
            if (File.Exists(args["filename"]))
            {
                io = "'" + File.ReadAllText(args["filename"]) + "'";
            }
            else
            {
                io = "false";
            }
            if (stdout == true)
            {
                InvokeResult = IResultConverter.JSBool(var_out, File.Exists(args["filename"]));
            }
        }
    }
    #endregion

    #region "fm_CreateFileInvoker"
    public class fm_CreateFileInvoker : IntentInvoker
    {
        //public CreateFileinvoker() : base() { }

        public override void InvokeVoid()
        {
            io = "true";
            try { File.Create(args["filename"]); }
            catch (UnauthorizedAccessException)
            {
                throwError("The caller does not have the required permission or specified a file that is read-only", "Cannot create file (" + args["filename"] + ")", "UnauthorizedAccessException (" + args["filename"] + ")\nat System.IO.File.Create()\nat System.Query()", 000036);
                io = "false";
            }

            catch (DirectoryNotFoundException)
            {
                throwError("The specified path is invalid (for example, it is on an unmapped drive).", "Cannot create file (" + args["filename"] + ")", "DirectoryNotFoundException (" + args["filename"] + ")\nat System.IO.File.Create()\nat System.Query()", 000036);
                io = "false";
            }
            catch (PathTooLongException)
            {
                throwError("The specified path, file name, or both exceed the system-defined maximum length.", "Cannot create file (" + args["filename"] + ")", "PathTooLongException (" + args["filename"] + ")\nat System.IO.File.Create()\nat System.Query()", 000036);
                io = "false";
            }
            catch (ArgumentNullException)
            {
                throwError("path is null.", "Cannot create file (" + args["filename"] + ")", "ArgumentNullException (" + args["filename"] + ")\nat System.IO.File.Create()\nat System.Query()", 000036);
                io = "false";
            }
            catch (NotSupportedException)
            {
                throwError("path is in an invalid format.", "Cannot create file (" + args["filename"] + ")", "NotSupportedException (" + args["filename"] + ")\nat System.IO.File.Create()\nat System.Query()", 000036);
                io = "false";
            }
            catch (IOException)
            {
                throwError("An I/O error occurred while creating the file.", "Cannot create file (" + args["filename"] + ")", "IOException (" + args["filename"] + ")\nat System.IO.File.Create()\nat System.Query()", 000036);
                io = "false";
            }
            catch (ArgumentException)
            {
                throwError("path is a zero-length string, contains only white space, or contains one or more invalid characters as defined by InvalidPathChars.", "Cannot create file (" + args["filename"] + ")", "ArgumentException (" + args["filename"] + ")\nat System.IO.File.Create()\nat System.Query()", 000036);
                io = "false";
            }
            //throwError("The caller does not have the required permission or specified a file that is read-only", "Cannot create file (" + args["filename"] + ")", "The caller does not have the required permission (" + args["filename"] + ")\nat System.IO.File.Create()\nat System.Query()", 000036);

            if (stdout == true)
            {
                InvokeResult = IResultConverter.JSBool(var_out, Convert.ToBoolean(io));
            }
        }
    }
    #endregion

    #region "fm_CreateTextInvoker"
    public class fm_CreateTextInvoker : IntentInvoker
    {

        public override void InvokeVoid()
        {
            io = "true";
            try { File.CreateText(args["filename"]); }
            catch (UnauthorizedAccessException)
            {
                throwError("The caller does not have the required permission or specified a file that is read-only", "Cannot create file (" + args["filename"] + ")", "UnauthorizedAccessException (" + args["filename"] + ")\nat System.IO.File.Create()\nat System.Query()", 000036);
                io = "false";
            }
            catch (DirectoryNotFoundException)
            {
                throwError("The specified path is invalid (for example, it is on an unmapped drive).", "Cannot create file (" + args["filename"] + ")", "DirectoryNotFoundException (" + args["filename"] + ")\nat System.IO.File.Create()\nat System.Query()", 000036);
                io = "false";
            }
            catch (PathTooLongException)
            {
                throwError("The specified path, file name, or both exceed the system-defined maximum length.", "Cannot create file (" + args["filename"] + ")", "PathTooLongException (" + args["filename"] + ")\nat System.IO.File.Create()\nat System.Query()", 000036);
                io = "false";
            }
            catch (ArgumentNullException)
            {
                throwError("path is null.", "Cannot create file (" + args["filename"] + ")", "ArgumentNullException (" + args["filename"] + ")\nat System.IO.File.Create()\nat System.Query()", 000036);
                io = "false";
            }
            catch (NotSupportedException)
            {
                throwError("path is in an invalid format.", "Cannot create file (" + args["filename"] + ")", "NotSupportedException (" + args["filename"] + ")\nat System.IO.File.Create()\nat System.Query()", 000036);
                io = "false";
            }

            catch (ArgumentException)
            {
                throwError("path is a zero-length string, contains only white space, or contains one or more invalid characters as defined by InvalidPathChars.", "Cannot create file (" + args["filename"] + ")", "ArgumentException (" + args["filename"] + ")\nat System.IO.File.Create()\nat System.Query()", 000036);
                io = "false";
            }

            if (stdout == true)
            {
                InvokeResult = IResultConverter.JSString(var_out, io);
            }

        }
    }
    #endregion
}
