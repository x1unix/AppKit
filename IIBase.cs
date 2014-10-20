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
    public static class IIBase
    {
        public static void throwError(string description, string error, string at, int code)
        {
            MessageBox.Show("Runtime Error\n\nAn unhandled error has occured in this application.\nContact to application developer and report about error.\n\nError Code: " + error + " [" + code.ToString() + "]\nDescription: " + description + "\n\nDetails:\n\n" + at, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        public static Dictionary<string, IntentInvoker> IntentInvokers = new Dictionary<string, IntentInvoker>();
        public static void Load()
        {
            IntentInvokers.Add("fr_splash", new fr_SplashInvoker());

            IntentInvokers.Add("mod_openfiledialog", new mod_OpenFileDialogInvoker());
            IntentInvokers.Add("mod_messagebox", new mod_MessageBoxInvoker());
            IntentInvokers.Add("env_gethostname", new env_GetHostNameInvoker());

            IntentInvokers.Add("env_expandvariables", new env_ExpandVariablesInvoker());
            IntentInvokers.Add("env_getosversion", new env_GetOSVersionInvoker());
            IntentInvokers.Add("env_getworkingset", new env_GetWorkingSetInvoker());
            IntentInvokers.Add("frm_closeframe", new frm_CloseFrameInvoker());
            IntentInvokers.Add("frm_hideframe", new frm_HideFrameInvoker());
            IntentInvokers.Add("frm_showframe", new frm_ShowFrameInvoker());
            IntentInvokers.Add("frm_loadframe", new frm_LoadFrameInvoker());

            IntentInvokers.Add("fm_direxists", new fm_DirExistsInvoker());
            IntentInvokers.Add("fm_dircreate", new fm_DirCreateInvoker());
            IntentInvokers.Add("fm_dirdelete", new fm_DirDeleteInvoker());
            IntentInvokers.Add("fm_getfiles", new fm_GetFilesInvoker());
            IntentInvokers.Add("fm_getdirectories", new fm_GetDirectoriesInvoker());
            IntentInvokers.Add("fm_getdirectoriesroot", new fm_GetDirectoriesRootInvoker());
            IntentInvokers.Add("cmdshell", new CmdShellInvoker());
            IntentInvokers.Add("fm_fileexists", new fm_FileExistsInvoker());

            IntentInvokers.Add("fm_movefile", new fm_MoveFileInvoker());
            IntentInvokers.Add("fm_copyfile", new fm_CopyFileInvoker());
            IntentInvokers.Add("fm_deletefile", new fm_DeleteFileInvoker());
            IntentInvokers.Add("fm_readfile", new fm_ReadFileInvoker());
            IntentInvokers.Add("fm_createfile", new fm_CreateFileInvoker());
            IntentInvokers.Add("fm_createtext", new fm_CreateTextInvoker());

        }
        
    }

    
}
