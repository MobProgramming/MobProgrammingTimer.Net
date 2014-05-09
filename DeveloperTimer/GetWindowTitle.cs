using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace DeveloperTimer
{
    public interface IGetWindowTitle
    {
        string GetProcessExeName();
        string GetActiveWindowTitle();
    }

    public class GetWindowTitle : IGetWindowTitle
    {
        [DllImport("user32.dll")]
        static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        static extern int GetWindowText(IntPtr hWnd, StringBuilder text, int count);

        [DllImport("user32.dll")]
        static extern UInt32 GetWindowThreadProcessId(Int32 hWnd, out Int32 lpdwProcessId);


        private static Int32 GetWindowProcessID(Int32 hwnd)
        {
            Int32 pid = 1;
            GetWindowThreadProcessId(hwnd, out pid);
            return pid;
        }

        public static string GetProcessExeNameInternal()
        {
            try
            {
                int hwnd = 0;
                hwnd = (int)GetForegroundWindow();
                //string appProcessName = Process.GetProcessById((int)GetWindowProcessID(hwnd)).ProcessName;
                string appExePath = Process.GetProcessById((int)GetWindowProcessID(hwnd)).MainModule.FileName;
                string appExeName = appExePath.Substring(appExePath.LastIndexOf(@"\") + 1);
                return appExeName;
            }
            catch
            {
                return "Cant Track x64 Applications";
            }
        }

        public static string GetActiveWindowTitleInternal()
        {
            const int nChars = 256;
            IntPtr handle = IntPtr.Zero;
            StringBuilder Buff = new StringBuilder(nChars);
            handle = GetForegroundWindow();

            if (GetWindowText(handle, Buff, nChars) > 0)
            {
                return Buff.ToString();
            }
            return null;
        }

        public string GetProcessExeName()
        {
            return GetProcessExeNameInternal();
        }

        public string GetActiveWindowTitle()
        {
            return GetActiveWindowTitleInternal();
        }
    }
}
