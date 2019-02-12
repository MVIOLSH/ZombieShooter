using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ZombieShooter_new
{

    public struct IconInfo
    {
        public bool fIcon;
        public int xhotSpot;
        public int yHotSpot;
        public IntPtr hbmMask;
        public IntPtr hbmColor;
    }

    class CustomCursor
    {
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool GetIconInfo(IntPtr hIcon, ref IconInfo piconInfo);

        [DllImport("user32.dll")]
        public static extern IntPtr CreateIconIndirect(ref IconInfo icon);

        public static System.Windows.Forms.Cursor CreateCursor(Bitmap bmp, int xhotspot, int yhotspot)
        {
            IntPtr ptr = bmp.GetHicon();
            IconInfo tmp =new IconInfo();
            GetIconInfo(ptr, ref tmp);
            tmp.xhotSpot = xhotspot;
            tmp.yHotSpot = yhotspot;
            ptr = CreateIconIndirect(ref tmp);
            return new System.Windows.Forms.Cursor(ptr);
        }
        
    }
}
