using BrightSharp.Interop;
using BrightSharp.Interop.Constants;
using BrightSharp.Interop.Structures;
using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;

namespace BrightSharp.Behaviors
{
    internal class MinMaxSize_Logic
    {
        private HwndSource hwndSource;
        private Window associatedObject;
        public MinMaxSize_Logic(Window associatedObject)
        {
            this.associatedObject = associatedObject;
        }
        public void OnAttached()
        {
            this.associatedObject.SourceInitialized += AssociatedObject_SourceInitialized;
        }

        private void AssociatedObject_SourceInitialized(object sender, EventArgs e)
        {
            IntPtr handle = new WindowInteropHelper(this.associatedObject).Handle;
            hwndSource = HwndSource.FromHwnd(handle);
            hwndSource.AddHook(WindowProc);
        }

        public void OnDetaching()
        {
            this.associatedObject.SourceInitialized -= AssociatedObject_SourceInitialized;
            if (hwndSource != null)
            {
                hwndSource.RemoveHook(WindowProc);
                hwndSource.Dispose();
            }
        }

        private IntPtr WindowProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            switch (msg)
            {
                case 0x0024:
                    WmGetMinMaxInfo(hwnd, lParam);
                    handled = true;
                    break;
            }
            return (IntPtr)0;
        }

        private void WmGetMinMaxInfo(IntPtr hwnd, IntPtr lParam)
        {
            var mmi = (MINMAXINFO)Marshal.PtrToStructure(lParam, typeof(MINMAXINFO));

            // Adjust the maximized size and position to fit the work area of the correct monitor
            IntPtr monitor = NativeMethods.MonitorFromWindow(hwnd, (int)MonitorFromWindowFlags.MONITOR_DEFAULTTONEAREST);

            if (monitor != IntPtr.Zero)
            {
                var monitorInfo = new MONITORINFO();
                NativeMethods.GetMonitorInfo(monitor, monitorInfo);
                RECT rcWorkArea = monitorInfo.rcWork;
                RECT rcMonitorArea = monitorInfo.rcMonitor;
                mmi.ptMaxPosition.x = Math.Abs(rcWorkArea.Left - rcMonitorArea.Left);
                mmi.ptMaxPosition.y = Math.Abs(rcWorkArea.Top - rcMonitorArea.Top);
                mmi.ptMaxSize.x = Math.Abs(rcWorkArea.Right - rcWorkArea.Left);
                mmi.ptMaxSize.y = Math.Abs(rcWorkArea.Bottom - rcWorkArea.Top);
                mmi.ptMinTrackSize.x = (int)associatedObject.MinWidth;
                mmi.ptMinTrackSize.y = (int)associatedObject.MinHeight;
            }

            Marshal.StructureToPtr(mmi, lParam, true);
        }

    }
}
