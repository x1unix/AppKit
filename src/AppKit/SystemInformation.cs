using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Win32;

namespace WebAppKit
{
    public static class SystemInformation
    {
        public static string OSName
        {
            get
            {
                String subkey = @"SOFTWARE\Microsoft\Windows NT\CurrentVersion";
                RegistryKey key = Registry.LocalMachine;
                RegistryKey skey = key.OpenSubKey(subkey);
                return skey.GetValue("ProductName").ToString();
            }
        }
        public static string OSBulid
        {
            get
            {
                String subkey = @"SOFTWARE\Microsoft\Windows NT\CurrentVersion";
                RegistryKey key = Registry.LocalMachine;
                RegistryKey skey = key.OpenSubKey(subkey);
                return skey.GetValue("CurrentBuild").ToString();
            }
        }
    }
}
