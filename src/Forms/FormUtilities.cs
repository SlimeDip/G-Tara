using System.Drawing;
using System.Drawing.Drawing2D;

namespace G_Tara
{
    public static class FormUtilities
    {
        public static GraphicsPath CreateRoundedRectPath(Rectangle r, int rad)
        {
            var p = new GraphicsPath();
            int d = rad * 2;
            p.AddArc(r.X, r.Y, d, d, 180, 90);
            p.AddArc(r.Right - d, r.Y, d, d, 270, 90);
            p.AddArc(r.Right - d, r.Bottom - d, d, d, 0, 90);
            p.AddArc(r.X, r.Bottom - d, d, d, 90, 90);
            p.CloseFigure();
            return p;
        }

        public static float GetDpiScale(Control control)
        {
            return control.DeviceDpi / 96f;
        }

        public static int Scale(int value, float scale)
        {
            return (int)Math.Round(value * scale);
        }

        public static Size ScaleSize(Size size, float scale)
        {
            return new Size(Scale(size.Width, scale), Scale(size.Height, scale));
        }

        public static Point ScalePoint(Point point, float scale)
        {
            return new Point(Scale(point.X, scale), Scale(point.Y, scale));
        }
    }
}
