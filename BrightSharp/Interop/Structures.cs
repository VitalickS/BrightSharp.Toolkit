using System;
using System.Runtime.InteropServices;

namespace BrightSharp.Interop
{
    namespace Structures
    {

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        internal class MONITORINFO
        {
            public int cbSize = Marshal.SizeOf(typeof(MONITORINFO));

            public RECT rcMonitor;

            public RECT rcWork;

            public int dwFlags;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 0)]
        internal struct POINT
        {
            public int x;
            public int y;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 0)]
        internal struct RECT
        {
            public int Left;

            public int Top;

            public int Right;

            public int Bottom;

            public static readonly RECT Empty;

            public int Width
            {
                get {
                    return Math.Abs(Right - Left);
                } // Abs needed for BIDI OS
            }

            public int Height
            {
                get {
                    return Bottom - Top;
                }
            }

            public RECT(int left, int top, int right, int bottom)
            {
                Left = left;
                Top = top;
                Right = right;
                Bottom = bottom;
            }

            public RECT(RECT rcSrc)
            {
                Left = rcSrc.Left;
                Top = rcSrc.Top;
                Right = rcSrc.Right;
                Bottom = rcSrc.Bottom;
            }

            public bool IsEmpty
            {
                get {
                    // BUGBUG : On Bidi OS (hebrew arabic) left > right
                    return Left >= Right || Top >= Bottom;
                }
            }

            public override string ToString()
            {
                if (this == Empty)
                {
                    return "RECT {Empty}";
                }
                return "RECT { left : " + Left + " / top : " + Top + " / right : " + Right + " / bottom : " +
                       Bottom + " }";
            }

            public override bool Equals(object obj)
            {
                if (!(obj is RECT))
                {
                    return false;
                }
                return (this == (RECT)obj);
            }

            public override int GetHashCode()
            {
                return Left.GetHashCode() + Top.GetHashCode() + Right.GetHashCode() +
                       Bottom.GetHashCode();
            }

            public static bool operator ==(RECT rect1, RECT rect2)
            {
                return (rect1.Left == rect2.Left && rect1.Top == rect2.Top && rect1.Right == rect2.Right &&
                        rect1.Bottom == rect2.Bottom);
            }

            public static bool operator !=(RECT rect1, RECT rect2)
            {
                return !(rect1 == rect2);
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct MINMAXINFO
        {
            public POINT ptReserved;

            public POINT ptMaxSize;

            public POINT ptMaxPosition;

            public POINT ptMinTrackSize;

            public POINT ptMaxTrackSize;
        };
    }
}
