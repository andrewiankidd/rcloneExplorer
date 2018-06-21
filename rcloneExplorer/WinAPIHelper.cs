using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace rcloneExplorer
{
    class WinAPIHelper
    {
        public delegate bool Win32Callback(IntPtr hwnd, IntPtr lParam);

        [DllImport("user32.Dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool EnumChildWindows(IntPtr parentHandle, Win32Callback callback, IntPtr lParam);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool MoveWindow(IntPtr hWnd, int X, int Y, int nWidth, int nHeight, bool bRepaint);

        [DllImport("user32.dll")]
        public static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndNewParent);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);

        [DllImport("user32.dll", SetLastError = true)]
        static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint processId);

        [DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll", EntryPoint = "FindWindow", SetLastError = true)]
        static extern IntPtr FindWindowByCaption(IntPtr ZeroOnly, string lpWindowName);

        // A Win32 constant
        const int WM_SETTEXT = 0x000C;
        const int WM_KEYDOWN = 0x0100;
        const int VK_RETURN = 0x0D;

        // An overload of the SendMessage function, this time taking in a string as the lParam.
        [DllImport("User32.dll")]
        public static extern Int32 SendMessage(int hWnd, int Msg, int wParam, string lParam);

        [DllImport("User32.Dll")]
        public static extern Int32 PostMessage(int hWnd, int msg, int wParam, int lParam);

        public static IntPtr FindWindow(IntPtr parentHandle, Predicate<IntPtr> target)
        {
            var result = IntPtr.Zero;
            if (parentHandle == IntPtr.Zero)
                parentHandle = Process.GetCurrentProcess().MainWindowHandle;
            EnumChildWindows(parentHandle, (hwnd, param) => {
                if (target(hwnd))
                {
                    result = hwnd;
                    return false;
                }
                return true;
            }, IntPtr.Zero);
            return result;
        }

        public static string GetWindowText(IntPtr ptr)
        {
            StringBuilder sb = new StringBuilder(256);
            GetWindowText(ptr, sb, 256);
            return sb.ToString();
        }

        public static IntPtr getWindow(string windowName, int pid)
        {
            IntPtr hWnd = new IntPtr(0);
            while (hWnd == new IntPtr(0) && (Process.GetProcesses().Any(x => x.Id == pid)))
            {
                System.Threading.Thread.Sleep(500);
                hWnd = FindWindow(null, windowName);
            }

            return hWnd;
        }

        public static void SendInput(IntPtr hwnd, string text)
        {
            SendMessage((int)hwnd, WM_SETTEXT, 0, text);
        }

        public static void SendInput(IntPtr hwnd, System.Windows.Forms.Keys key)
        {
            PostMessage((int)hwnd, WM_KEYDOWN, 0x20, 1);
        }

        public static int GetWindowPID(IntPtr hWnd)
        {
            uint x = 0;
            GetWindowThreadProcessId(hWnd, out x);
            return (int)x;
        }
    }
}
