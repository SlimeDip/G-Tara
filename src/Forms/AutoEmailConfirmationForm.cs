using G_Tara.Models;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace G_Tara
{
    public partial class AutoEmailConfirmationForm : Form
    {
        [DllImport("user32.dll")]
        private static extern bool ReleaseCapture();

        [DllImport("user32.dll")]
        private static extern IntPtr SendMessage(IntPtr hWnd, int msg, int wParam, int lParam);

        private const int WM_NCLBUTTONDOWN = 0x00A1;
        private const int HTCAPTION = 0x0002;

        private Panel pnlTitleBar;
        private Button btnWindowMinimize;
        private Button btnWindowClose;
        private Label lblWindowTitle;
        public AutoEmailConfirmationForm(Gala gala)
        {
            InitializeComponent();
            

            var sb = new StringBuilder();
            sb.AppendLine($"Event: {gala.Name}");
            sb.AppendLine($"Date: {gala.ScheduledDate:MMMM dd, yyyy}");
            sb.AppendLine($"Location: {gala.Location}");
            sb.AppendLine();
            sb.AppendLine("Plan Details:");
            sb.AppendLine(gala.Plan);
            sb.AppendLine();
            sb.AppendLine("See you there!");
            
            txtPlanDetails.Text = sb.ToString();
            txtParticipants.Text = string.Join(Environment.NewLine, gala.Participants.Select(p => p.Email));

            var host = gala.Participants.FirstOrDefault(p => p.Name == gala.HostName);
            txtHostEmail.Text = host?.Email ?? "N/A";

            CreateCustomTitleBar();
            ApplyRoundedFormRegion();
            ApplyGaraTheme();
            this.Load += (s, e) => LayoutWindowButtons();
            this.Resize += (s, e) => LayoutWindowButtons();
        }

        private void CreateCustomTitleBar()
        {
            Color titleBarColor = Color.FromArgb(241, 206, 211);
            var scale = FormUtilities.GetDpiScale(this);
            
            pnlTitleBar = new Panel
            {
                Dock = DockStyle.Top,
                Height = FormUtilities.Scale(34, scale),
                BackColor = titleBarColor
            };
            pnlTitleBar.MouseDown += OnTitleBarMouseDown;

            lblWindowTitle = new Label
            {
                Text = "Auto Email Confirmation",
                AutoSize = true,
                Location = FormUtilities.ScalePoint(new Point(12, 8), scale),
                Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold),
                ForeColor = Color.FromArgb(80, 40, 45),
                BackColor = Color.Transparent
            };
            lblWindowTitle.MouseDown += OnTitleBarMouseDown;

            btnWindowMinimize = new Button
            {
                Text = "♡",
                Size = FormUtilities.ScaleSize(new Size(30, 28), scale),
                Location = new Point(this.ClientSize.Width - FormUtilities.Scale(102, scale), FormUtilities.Scale(2, scale)),
                Anchor = AnchorStyles.Top | AnchorStyles.Right,
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.Transparent,
                ForeColor = Color.FromArgb(117, 71, 76),
                Font = new Font("Segoe UI Symbol", 12F, FontStyle.Regular),
                Cursor = Cursors.Hand
            };
            btnWindowMinimize.FlatAppearance.BorderSize = 0;
            btnWindowMinimize.FlatAppearance.MouseOverBackColor = Color.FromArgb(235, 197, 203);
            btnWindowMinimize.FlatAppearance.MouseDownBackColor = Color.FromArgb(223, 181, 188);
            btnWindowMinimize.Click += (s, e) => this.WindowState = FormWindowState.Minimized;

            btnWindowClose = new Button
            {
                Text = "♥",
                Size = FormUtilities.ScaleSize(new Size(30, 28), scale),
                Location = new Point(this.ClientSize.Width - FormUtilities.Scale(44, scale), FormUtilities.Scale(2, scale)),
                Anchor = AnchorStyles.Top | AnchorStyles.Right,
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.Transparent,
                ForeColor = Color.FromArgb(117, 71, 76),
                Font = new Font("Segoe UI Symbol", 12F, FontStyle.Regular),
                Cursor = Cursors.Hand
            };
            btnWindowClose.FlatAppearance.BorderSize = 0;
            btnWindowClose.FlatAppearance.MouseOverBackColor = Color.FromArgb(235, 197, 203);
            btnWindowClose.FlatAppearance.MouseDownBackColor = Color.FromArgb(223, 181, 188);
            btnWindowClose.Click += (s, e) => this.Close();

            pnlTitleBar.Controls.Add(lblWindowTitle);
            pnlTitleBar.Controls.Add(btnWindowMinimize);
            pnlTitleBar.Controls.Add(btnWindowClose);
            
            this.Controls.Add(pnlTitleBar);
        }

        private void ApplyRoundedFormRegion()
        {
            if (this.Width <= 0 || this.Height <= 0) return;
            using var path = CreateRoundedRectPath(new Rectangle(0, 0, this.Width - 1, this.Height - 1), 20);
            this.Region = new Region(path);
        }

        private void OnTitleBarMouseDown(object? sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;
            ReleaseCapture();
            SendMessage(this.Handle, WM_NCLBUTTONDOWN, HTCAPTION, 0);
        }

        private void LayoutWindowButtons()
        {
            if (btnWindowMinimize == null || btnWindowClose == null || pnlTitleBar == null) return;
            var scale = FormUtilities.GetDpiScale(this);
            btnWindowClose.Location = new Point(pnlTitleBar.Width - btnWindowClose.Width - FormUtilities.Scale(8, scale), FormUtilities.Scale(2, scale));
            btnWindowMinimize.Location = new Point(btnWindowClose.Left - btnWindowMinimize.Width - FormUtilities.Scale(4, scale), FormUtilities.Scale(2, scale));
        }

        private void AutoEmailConfirmationForm_SizeChanged(object? sender, EventArgs e)
        {
            ApplyRoundedFormRegion();
        }

        private void ApplyGaraTheme()
        {
            Color btnBack = Color.FromArgb(248, 230, 231);
            Color btnBorder = Color.FromArgb(210, 170, 175);
            Color btnHover = Color.FromArgb(235, 190, 195);
            Color btnDown = Color.FromArgb(200, 150, 155);

            ApplyRoundedButtonStyle(btnConfirm, btnBack, btnBorder, btnHover, btnDown, null);
            ApplyRoundedButtonStyle(btnCancel, btnBack, btnBorder, btnHover, btnDown, null);
        }

        public bool IsConfirmed => DialogResult == DialogResult.OK;

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
        private static void ApplyRoundedButtonStyle(Button btn, Color back, Color border, Color hover, Color down, System.Media.SoundPlayer hoverSound)
        {
            if (btn.Tag is RoundedButtonStyleState) return; 

    var state = new RoundedButtonStyleState { Back = back, Border = border, Hover = hover, Down = down };
            btn.Tag = state;
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;

            btn.Region = new Region(CreateRoundedRectPath(new Rectangle(0, 0, btn.Width, btn.Height), 12));

            state.AnimationTimer = new System.Windows.Forms.Timer { Interval = 15 };
            state.AnimationTimer.Tick += (s, e) => {
                var delta = state.HoverTarget - state.HoverProgress;
                if (Math.Abs(delta) < 0.01f) { state.HoverProgress = state.HoverTarget; state.AnimationTimer.Stop(); }
                else { state.HoverProgress += delta * 0.22f; }
                btn.Invalidate();
            };

            btn.MouseEnter += (s, e) => { state.IsHover = true; state.HoverTarget = 1f; state.AnimationTimer.Start(); };
            btn.MouseLeave += (s, e) => { state.IsHover = false; state.HoverTarget = 0f; state.AnimationTimer.Start(); };
            btn.MouseDown += (s, e) => { state.IsDown = true; btn.Invalidate(); };
            btn.MouseUp += (s, e) => { state.IsDown = false; btn.Invalidate(); };

            btn.Paint += (s, e) => {
                e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                Color current = state.IsDown ? state.Down : Interpolate(state.Back, state.Hover, state.HoverProgress);
                using (var path = CreateRoundedRectPath(new Rectangle(0, 0, btn.Width - 1, btn.Height - 1), 12))
                {
                    using (var brush = new SolidBrush(current)) e.Graphics.FillPath(brush, path);
                    using (var pen = new Pen(state.Border, 1f)) e.Graphics.DrawPath(pen, path);
                }
                TextRenderer.DrawText(e.Graphics, btn.Text, btn.Font, btn.ClientRectangle, btn.ForeColor, TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
            };
        }

        private static Color Interpolate(Color b, Color t, float p) =>
            Color.FromArgb((int)(b.R + (t.R - b.R) * p), (int)(b.G + (t.G - b.G) * p), (int)(b.B + (t.B - b.B) * p)); 

        private static System.Drawing.Drawing2D.GraphicsPath CreateRoundedRectPath(Rectangle r, int rad)
        {
            var p = new System.Drawing.Drawing2D.GraphicsPath();
            int d = rad * 2;
            p.AddArc(r.X, r.Y, d, d, 180, 90);
            p.AddArc(r.Right - d, r.Y, d, d, 270, 90);
            p.AddArc(r.Right - d, r.Bottom - d, d, d, 0, 90);
            p.AddArc(r.X, r.Bottom - d, d, d, 90, 90);
            p.CloseFigure();
            return p;
        }

        public class RoundedButtonStyleState
        {
            public Color Back, Border, Hover, Down;
            public float HoverProgress, HoverTarget;
            public bool IsHover, IsDown;
            public System.Windows.Forms.Timer AnimationTimer;
        }
    }
}
