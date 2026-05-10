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

        public static Font ScaleFont(Font font, float scale)
        {
            return new Font(font.FontFamily, font.Size * scale, font.Style);
        }

        public static Rectangle ScaleRectangle(Rectangle rect, float scale)
        {
            return new Rectangle(Scale(rect.X, scale), Scale(rect.Y, scale), Scale(rect.Width, scale), Scale(rect.Height, scale));
        }

        public static Padding ScalePadding(Padding padding, float scale)
        {
            return new Padding(Scale(padding.Left, scale), Scale(padding.Top, scale), Scale(padding.Right, scale), Scale(padding.Bottom, scale));
        }
    }
}
