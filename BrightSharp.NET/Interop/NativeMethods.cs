using BrightSharp.Interop.Structures;
using System;
using System.Runtime.InteropServices;

namespace BrightSharp.Interop
{
    internal static class NativeMethods
    {
        #region Monitor

        [DllImport("user32.dll", SetLastError = true)]
        internal static extern bool GetMonitorInfo(IntPtr hMonitor, MONITORINFO lpmi);

        [DllImport("user32.dll", SetLastError = true)]
        internal static extern IntPtr MonitorFromWindow(IntPtr handle, int flags);
        
        #endregion

    }
}
