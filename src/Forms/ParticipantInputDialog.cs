using System;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace G_Tara
{
    public partial class ParticipantInputDialog : Form
    {
        [DllImport("user32.dll")]
        private static extern bool ReleaseCapture();

        [DllImport("user32.dll")]
        private static extern IntPtr SendMessage(IntPtr hWnd, int msg, int wParam, int lParam);

        private const int WM_NCLBUTTONDOWN = 0x00A1;
        private const int HTCAPTION = 0x0002;

        private PictureBox picProfile;
        private Button btnUploadPhoto;
        private Label lblName;
        private TextBox txtParticipantName;
        private Label lblEmail;
        private TextBox txtParticipantEmail;
        private Label lblAvailableDates;
        private TextBox txtAvailableDates;
        private Button btnPickDates;
        private Button btnClearDates;
        private Button btnOK;
        private Button btnCancel;
        private Panel pnlPhotoFrame;
        private Label lblNoImage;
        private Panel pnlFooterStrip;
        private Label lblFooterTitle;
        private Panel pnlTitleBar;
        private Button btnWindowMinimize;
        private Button btnWindowMaximize;
        private Button btnWindowClose;
        private Label lblDialogTitle;
        private Image? _goldButtonTexture;
        private readonly System.Media.SoundPlayer _hoverSound = new System.Media.SoundPlayer();
        private List<DateTime> _selectedDates = new();
        private string _selectedImagePath = string.Empty;
        public string ParticipantImagePath => _selectedImagePath;

        public ParticipantInputDialog()
        {
            InitializeComponent();
            WireUiEvents();
            ApplyGaraTheme();
        }

        private void InitializeComponent()
        {
            lblName = new Label();
            txtParticipantName = new TextBox();
            lblEmail = new Label();
            txtParticipantEmail = new TextBox();
            lblAvailableDates = new Label();
            txtAvailableDates = new TextBox();
            btnPickDates = new Button();
            btnClearDates = new Button();
            picProfile = new PictureBox();
            btnUploadPhoto = new Button();
            btnOK = new Button();
            btnCancel = new Button();
            pnlPhotoFrame = new Panel();
            lblNoImage = new Label();
            pnlFooterStrip = new Panel();
            lblFooterTitle = new Label();
            pnlTitleBar = new Panel();
            btnWindowMinimize = new Button();
            btnWindowMaximize = new Button();
            btnWindowClose = new Button();
            lblDialogTitle = new Label();
            ((ISupportInitialize)picProfile).BeginInit();
            SuspendLayout();
            // 
            // pnlTitleBar
            // 
            pnlTitleBar.BackColor = Color.FromArgb(241, 206, 211);
            pnlTitleBar.Dock = DockStyle.Top;
            pnlTitleBar.Location = new Point(0, 0);
            pnlTitleBar.Name = "pnlTitleBar";
            pnlTitleBar.Size = new Size(704, 30);
            pnlTitleBar.TabIndex = 30;
            pnlTitleBar.MouseDown += OnDialogMouseDown;
            // 
            // lblDialogTitle
            // 
            lblDialogTitle.AutoSize = true;
            lblDialogTitle.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold);
            lblDialogTitle.ForeColor = Color.FromArgb(80, 40, 45);
            lblDialogTitle.Location = new Point(12, 6);
            lblDialogTitle.Name = "lblDialogTitle";
            lblDialogTitle.Size = new Size(97, 19);
            lblDialogTitle.TabIndex = 31;
            lblDialogTitle.Text = "Add Participant";
            lblDialogTitle.MouseDown += OnDialogMouseDown;
            // 
            // btnWindowMinimize
            // 
            btnWindowMinimize.FlatAppearance.BorderSize = 0;
            btnWindowMinimize.FlatStyle = FlatStyle.Flat;
            btnWindowMinimize.Font = new Font("Segoe UI Symbol", 12F, FontStyle.Regular);
            btnWindowMinimize.ForeColor = Color.FromArgb(117, 71, 76);
            btnWindowMinimize.Location = new Point(624, 1);
            btnWindowMinimize.Name = "btnWindowMinimize";
            btnWindowMinimize.Size = new Size(30, 28);
            btnWindowMinimize.TabIndex = 32;
            btnWindowMinimize.Text = "♡";
            btnWindowMinimize.UseVisualStyleBackColor = true;
            btnWindowMinimize.Click += OnWindowMinimizeClick;
            // 
            // btnWindowClose
            // 
            btnWindowClose.FlatAppearance.BorderSize = 0;
            btnWindowClose.FlatStyle = FlatStyle.Flat;
            btnWindowClose.Font = new Font("Segoe UI Symbol", 12F, FontStyle.Regular);
            btnWindowClose.ForeColor = Color.FromArgb(117, 71, 76);
            btnWindowClose.Location = new Point(664, 1);
            btnWindowClose.Name = "btnWindowClose";
            btnWindowClose.Size = new Size(30, 28);
            btnWindowClose.TabIndex = 33;
            btnWindowClose.Text = "♥";
            btnWindowClose.UseVisualStyleBackColor = true;
            btnWindowClose.Click += OnWindowCloseClick;
            // 
            // lblName
            // 
            lblName.AutoSize = true;
            lblName.Location = new Point(34, 38);
            lblName.Name = "lblName";
            lblName.Size = new Size(49, 19);
            lblName.TabIndex = 10;
            lblName.Text = "Name:";
            // 
            // txtParticipantName
            // 
            txtParticipantName.BackColor = Color.FromArgb(255, 248, 248);
            txtParticipantName.Location = new Point(174, 32);
            txtParticipantName.Name = "txtParticipantName";
            txtParticipantName.Size = new Size(265, 25);
            txtParticipantName.TabIndex = 9;
            // 
            // lblEmail
            // 
            lblEmail.AutoSize = true;
            lblEmail.Location = new Point(34, 86);
            lblEmail.Name = "lblEmail";
            lblEmail.Size = new Size(99, 19);
            lblEmail.TabIndex = 8;
            lblEmail.Text = "Email Address:";
            // 
            // txtParticipantEmail
            // 
            txtParticipantEmail.BackColor = Color.FromArgb(255, 248, 248);
            txtParticipantEmail.Location = new Point(174, 82);
            txtParticipantEmail.Name = "txtParticipantEmail";
            txtParticipantEmail.Size = new Size(265, 25);
            txtParticipantEmail.TabIndex = 7;
            // 
            // lblAvailableDates
            // 
            lblAvailableDates.AutoSize = true;
            lblAvailableDates.Location = new Point(34, 134);
            lblAvailableDates.Name = "lblAvailableDates";
            lblAvailableDates.Size = new Size(82, 19);
            lblAvailableDates.TabIndex = 6;
            lblAvailableDates.Text = "Availability:";
            // 
            // txtAvailableDates
            // 
            txtAvailableDates.BackColor = Color.FromArgb(255, 248, 248);
            txtAvailableDates.Location = new Point(174, 130);
            txtAvailableDates.Multiline = true;
            txtAvailableDates.Name = "txtAvailableDates";
            txtAvailableDates.ReadOnly = true;
            txtAvailableDates.Size = new Size(265, 70);
            txtAvailableDates.TabIndex = 5;
            // 
            // btnPickDates
            // 
            btnPickDates.Location = new Point(174, 212);
            btnPickDates.Name = "btnPickDates";
            btnPickDates.Size = new Size(124, 36);
            btnPickDates.TabIndex = 4;
            btnPickDates.Text = "Select Dates";
            // 
            // btnClearDates
            // 
            btnClearDates.Location = new Point(315, 212);
            btnClearDates.Name = "btnClearDates";
            btnClearDates.Size = new Size(124, 36);
            btnClearDates.TabIndex = 3;
            btnClearDates.Text = "Clear All";
            // 
            // pnlPhotoFrame
            // 
            pnlPhotoFrame.BackColor = Color.Transparent;
            pnlPhotoFrame.Location = new Point(488, 24);
            pnlPhotoFrame.Name = "pnlPhotoFrame";
            pnlPhotoFrame.Size = new Size(185, 185);
            pnlPhotoFrame.TabIndex = 20;
            pnlPhotoFrame.Paint += OnPhotoFramePaint;
            // 
            // picProfile
            // 
            picProfile.BackColor = Color.FromArgb(255, 248, 248);
            picProfile.BorderStyle = BorderStyle.None;
            picProfile.Location = new Point(14, 14);
            picProfile.Name = "picProfile";
            picProfile.Size = new Size(157, 157);
            picProfile.SizeMode = PictureBoxSizeMode.Zoom;
            picProfile.TabIndex = 1;
            picProfile.TabStop = false;
            // 
            // lblNoImage
            // 
            lblNoImage.BackColor = Color.Transparent;
            lblNoImage.Font = new Font("Segoe UI Semibold", 9.5F, FontStyle.Bold);
            lblNoImage.ForeColor = Color.FromArgb(110, 75, 78);
            lblNoImage.Location = new Point(28, 116);
            lblNoImage.Name = "lblNoImage";
            lblNoImage.Size = new Size(130, 25);
            lblNoImage.TabIndex = 21;
            lblNoImage.Text = "No Image Uploaded";
            lblNoImage.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // btnUploadPhoto
            // 
            btnUploadPhoto.Location = new Point(488, 216);
            btnUploadPhoto.Name = "btnUploadPhoto";
            btnUploadPhoto.Size = new Size(185, 42);
            btnUploadPhoto.TabIndex = 2;
            btnUploadPhoto.Text = "Upload Photo";
            btnUploadPhoto.Click += OnUploadPhotoClick;
            // 
            // pnlFooterStrip
            // 
            pnlFooterStrip.BackColor = Color.Transparent;
            pnlFooterStrip.Location = new Point(204, 356);
            pnlFooterStrip.Name = "pnlFooterStrip";
            pnlFooterStrip.Size = new Size(472, 52);
            pnlFooterStrip.TabIndex = 22;
            pnlFooterStrip.Paint += OnFooterStripPaint;
            // 
            // lblFooterTitle
            // 
            lblFooterTitle.AutoSize = false;
            lblFooterTitle.BackColor = Color.Transparent;
            lblFooterTitle.Font = new Font("Segoe UI Semibold", 12.5F, FontStyle.Bold);
            lblFooterTitle.ForeColor = Color.FromArgb(70, 46, 50);
            lblFooterTitle.Location = new Point(30, 10);
            lblFooterTitle.Name = "lblFooterTitle";
            lblFooterTitle.Size = new Size(150, 30);
            lblFooterTitle.TabIndex = 23;
            lblFooterTitle.Text = "Save & Confirm";
            lblFooterTitle.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // btnOK
            // 
            btnOK.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnOK.Location = new Point(388, 364);
            btnOK.Name = "btnOK";
            btnOK.Size = new Size(136, 36);
            btnOK.TabIndex = 0;
            btnOK.Text = "Confirm";
            // 
            // btnCancel
            // 
            btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnCancel.Location = new Point(535, 364);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(126, 36);
            btnCancel.TabIndex = 1;
            btnCancel.Text = "Cancel";
            // 
            // ParticipantInputDialog
            // 
            AcceptButton = btnOK;
            BackColor = Color.FromArgb(248, 235, 237);
            CancelButton = btnCancel;
            ClientSize = new Size(704, 450);
            Controls.Add(pnlTitleBar);
            Controls.Add(pnlFooterStrip);
            Controls.Add(lblFooterTitle);
            Controls.Add(btnCancel);
            Controls.Add(btnOK);
            Controls.Add(pnlPhotoFrame);
            Controls.Add(btnUploadPhoto);
            Controls.Add(btnClearDates);
            Controls.Add(btnPickDates);
            Controls.Add(txtAvailableDates);
            Controls.Add(lblAvailableDates);
            Controls.Add(txtParticipantEmail);
            Controls.Add(lblEmail);
            Controls.Add(txtParticipantName);
            Controls.Add(lblName);
            Font = new Font("Segoe UI Semibold", 10F);
            ForeColor = Color.FromArgb(100, 60, 65);
            FormBorderStyle = FormBorderStyle.None;
            MaximizeBox = false;
            Name = "ParticipantInputDialog";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Add Participant";
            ((ISupportInitialize)picProfile).EndInit();
            ResumeLayout(false);
            PerformLayout();

            pnlPhotoFrame.Controls.Add(picProfile);
            pnlPhotoFrame.Controls.Add(lblNoImage);
            pnlFooterStrip.Controls.Add(lblFooterTitle);
            pnlTitleBar.Controls.Add(lblDialogTitle);
            pnlTitleBar.Controls.Add(btnWindowMinimize);
            pnlTitleBar.Controls.Add(btnWindowClose);
            btnOK.BringToFront();
            btnCancel.BringToFront();
            LoadDialogAssets();
            LoadSounds();
            ApplyRoundedFormRegion();
            CenterDialogContentVertically();
            this.SizeChanged += (s, e) => ApplyRoundedFormRegion();
            this.SizeChanged += (s, e) => CenterDialogContentVertically();
            this.MouseDown += OnDialogMouseDown;
        }

        [System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Hidden)]
        public string ParticipantName
        {
            get => txtParticipantName.Text.Trim();
            set => txtParticipantName.Text = value;
        }

        [System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Hidden)]
        public string ParticipantEmail
        {
            get => txtParticipantEmail.Text.Trim();
            set => txtParticipantEmail.Text = value;
        }

        [System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Hidden)]
        public List<DateTime> ParticipantAvailableDates
        {
            get => new List<DateTime>(_selectedDates);
            set
            {
                _selectedDates = value?.Select(d => d.Date).Distinct().OrderBy(d => d).ToList() ?? new List<DateTime>();
                UpdateAvailableDatesDisplay();
            }
        }

        private void WireUiEvents()
        {
            btnOK.Click -= OnOKClick;
            btnOK.Click += OnOKClick;

            btnCancel.Click -= OnCancelClick;
            btnCancel.Click += OnCancelClick;

            btnPickDates.Click -= OnPickDatesClick;
            btnPickDates.Click += OnPickDatesClick;

            btnClearDates.Click -= OnClearDatesClick;
            btnClearDates.Click += OnClearDatesClick;
        }

        private void OnOKClick(object? sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtParticipantName.Text))
            {
                MessageBox.Show("Please enter the participant name.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(txtParticipantEmail.Text))
            {
                MessageBox.Show("Please enter the participant email.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (_selectedDates.Count == 0)
            {
                MessageBox.Show("Please pick at least one available date.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult = DialogResult.OK;
            Close();
        }

        private void OnCancelClick(object? sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void ParticipantInputDialog_Load(object sender, EventArgs e)
        {
            UpdateAvailableDatesDisplay();
        }

        private void OnPickDatesClick(object? sender, EventArgs e)
        {
            using var picker = new CalendarPickerForm(initialDates: _selectedDates, multiSelect: true);
            if (picker.ShowDialog(this) != DialogResult.OK)
            {
                return;
            }

            ParticipantAvailableDates = picker.SelectedDates;
        }

        private void OnClearDatesClick(object? sender, EventArgs e)
        {
            ParticipantAvailableDates = new List<DateTime>();
        }

        private void UpdateAvailableDatesDisplay()
        {
            if (_selectedDates.Count == 0)
            {
                txtAvailableDates.Text = "Pending";
                return;
            }
            txtAvailableDates.Text = string.Join(
                Environment.NewLine,
                _selectedDates
                    .OrderBy(d => d)
                    .Select(d => d.ToString("ddd, MMM dd, yyyy")));
        }
        private void ApplyGaraTheme()
        {
            Color goldBack = Color.FromArgb(212, 181, 133);
            Color goldBorder = Color.FromArgb(171, 138, 84);
            Color goldHover = Color.FromArgb(225, 196, 150);
            Color goldDown = Color.FromArgb(191, 160, 112);

            Color mauveBack = Color.FromArgb(197, 156, 160);
            Color mauveBorder = Color.FromArgb(167, 125, 130);
            Color mauveHover = Color.FromArgb(209, 171, 176);
            Color mauveDown = Color.FromArgb(178, 136, 142);

            Color lightBack = Color.FromArgb(252, 241, 243);
            Color lightBorder = Color.FromArgb(209, 167, 172);
            Color lightHover = Color.FromArgb(247, 229, 232);
            Color lightDown = Color.FromArgb(235, 210, 214);

            ApplyRoundedButtonStyle(btnPickDates, goldBack, goldBorder, goldHover, goldDown, _hoverSound, _goldButtonTexture);
            ApplyRoundedButtonStyle(btnClearDates, goldBack, goldBorder, goldHover, goldDown, _hoverSound, _goldButtonTexture);
            ApplyRoundedButtonStyle(btnUploadPhoto, mauveBack, mauveBorder, mauveHover, mauveDown, _hoverSound);
            ApplyRoundedButtonStyle(btnOK, mauveBack, mauveBorder, mauveHover, mauveDown, _hoverSound);
            ApplyRoundedButtonStyle(btnCancel, lightBack, lightBorder, lightHover, lightDown, _hoverSound);

            btnUploadPhoto.Text = "📷  Upload Photo";
            btnOK.ForeColor = Color.WhiteSmoke;
            btnCancel.ForeColor = Color.FromArgb(112, 73, 77);
            txtAvailableDates.Text = _selectedDates.Count == 0 ? "Dates Selected" : txtAvailableDates.Text;
        }

        private void OnUploadPhotoClick(object sender, EventArgs e)
        {
            using OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.gif";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                _selectedImagePath = ofd.FileName;
                picProfile.Image = Image.FromFile(_selectedImagePath);
                lblNoImage.Visible = false;
            }
        }

        private void LoadDialogAssets()
        {
            try
            {
                string assetsFolder = Path.Combine(AppContext.BaseDirectory, "Assets");
                string noImagePath = Path.Combine(assetsFolder, "No Image.png");
                if (File.Exists(noImagePath))
                {
                    picProfile.Image = Image.FromFile(noImagePath);
                    lblNoImage.Visible = false;
                }
                string goldTexturePath = Path.Combine(assetsFolder, "Gold.png");
                if (File.Exists(goldTexturePath))
                {
                    _goldButtonTexture = Image.FromFile(goldTexturePath);
                }
            }
            catch
            {
                // Keep text placeholder when asset loading fails.
            }
        }

        private void LoadSounds()
        {
            try
            {
                string[] candidates =
                {
                    Path.Combine(AppContext.BaseDirectory, "Assets", "hover.wav"),
                    Path.Combine(AppContext.BaseDirectory, "assets", "hover.wav")
                };
                foreach (string soundPath in candidates)
                {
                    if (!File.Exists(soundPath)) continue;
                    _hoverSound.SoundLocation = soundPath;
                    _hoverSound.Load();
                    break;
                }
            }
            catch
            {
                // Ignore sound load failures and keep UI working.
            }
        }

        private void ApplyRoundedFormRegion()
        {
            if (Width <= 0 || Height <= 0) return;
            using var path = CreateRoundedRectPath(new Rectangle(0, 0, Width - 1, Height - 1), 20);
            this.Region = new Region(path);
        }

        private void OnDialogMouseDown(object? sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;
            ReleaseCapture();
            SendMessage(this.Handle, WM_NCLBUTTONDOWN, HTCAPTION, 0);
        }

        private void OnWindowMinimizeClick(object? sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void OnWindowCloseClick(object? sender, EventArgs e)
        {
            this.Close();
        }

        private void CenterDialogContentVertically()
        {
            Control[] contentControls =
            {
                lblName, txtParticipantName, lblEmail, txtParticipantEmail, lblAvailableDates, txtAvailableDates,
                btnPickDates, btnClearDates, pnlPhotoFrame, btnUploadPhoto, pnlFooterStrip, btnOK, btnCancel
            };

            int areaTop = pnlTitleBar.Bottom + 6;
            int areaBottom = ClientSize.Height - 8;
            int areaHeight = areaBottom - areaTop;
            if (areaHeight <= 0) return;

            int currentTop = contentControls.Min(c => c.Top);
            int currentBottom = contentControls.Max(c => c.Bottom);
            int contentHeight = currentBottom - currentTop;
            if (contentHeight <= 0) return;

            int targetTop = areaTop + Math.Max(0, (areaHeight - contentHeight) / 2);
            int deltaY = targetTop - currentTop;
            if (deltaY == 0) return;

            foreach (Control c in contentControls)
            {
                c.Top += deltaY;
            }

            // Keep footer label anchored inside strip after reflow.
            lblFooterTitle.Location = new Point(30, 10);
        }

        private void OnPhotoFramePaint(object? sender, PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            var rect = new Rectangle(1, 1, pnlPhotoFrame.Width - 3, pnlPhotoFrame.Height - 3);
            using var path = CreateRoundedRectPath(rect, 14);
            using var fillBrush = new SolidBrush(Color.FromArgb(255, 248, 248));
            e.Graphics.FillPath(fillBrush, path);
            using var pen = new Pen(Color.FromArgb(187, 160, 108), 3f);
            e.Graphics.DrawPath(pen, path);
        }

        private void OnFooterStripPaint(object? sender, PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            var rect = new Rectangle(1, 1, pnlFooterStrip.Width - 3, pnlFooterStrip.Height - 3);
            using var path = CreateRoundedRectPath(rect, 24);
            using var fillBrush = new SolidBrush(Color.FromArgb(247, 224, 227));
            e.Graphics.FillPath(fillBrush, path);
            using var pen = new Pen(Color.FromArgb(104, 110, 118), 2f);
            e.Graphics.DrawPath(pen, path);
        }

        private static void ApplyRoundedButtonStyle(Button btn, Color back, Color border, Color hover, Color down, System.Media.SoundPlayer? hoverSound, Image? textureImage = null)
        {
            if (btn.Tag is RoundedButtonStyleState) return;

            var state = new RoundedButtonStyleState { Back = back, Border = border, Hover = hover, Down = down, TextureImage = textureImage };
            btn.Tag = state;

            // 1. IMPROVED CONTROL SETTINGS
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
            btn.BackColor = Color.Transparent;
            // This helps prevent "flicker" and default backgrounds showing through
            // Replace btn.SetStyle(...) with this:
            // Use System.Reflection for BindingFlags
            typeof(Control).GetMethod("SetStyle", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                ?.Invoke(btn, new object[] { ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint, true });

            void ApplyRegion()
            {
                if (btn.Width <= 0 || btn.Height <= 0) return;
                using var regionPath = CreateRoundedRectPath(new Rectangle(0, 0, btn.Width - 1, btn.Height - 1), 12);
                btn.Region = new Region(regionPath);
            }
            ApplyRegion();
            btn.Resize += (s, e) => ApplyRegion();

            state.AnimationTimer = new System.Windows.Forms.Timer { Interval = 15 };
            state.AnimationTimer.Tick += (s, e) => {
                var delta = state.HoverTarget - state.HoverProgress;
                if (Math.Abs(delta) < 0.01f)
                {
                    state.HoverProgress = state.HoverTarget;
                    state.AnimationTimer.Stop();
                }
                else
                {
                    state.HoverProgress += delta * 0.22f;
                }
                btn.Invalidate();
            };

            btn.MouseEnter += (s, e) =>
            {
                state.IsHover = true;
                state.HoverTarget = 1f;
                state.AnimationTimer.Start();
                try
                {
                    if (hoverSound != null && !string.IsNullOrWhiteSpace(hoverSound.SoundLocation))
                    {
                        hoverSound.Play();
                    }
                }
                catch
                {
                    // Ignore hover sound playback errors.
                }
            };
            btn.MouseLeave += (s, e) => { state.IsHover = false; state.HoverTarget = 0f; state.AnimationTimer.Start(); };
            btn.MouseDown += (s, e) => { state.IsDown = true; btn.Invalidate(); };
            btn.MouseUp += (s, e) => { state.IsDown = false; btn.Invalidate(); };

            btn.Paint += (s, e) => {
                e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;


                Rectangle rect = new Rectangle(0, 0, btn.Width - 1, btn.Height - 1);
                Color current = state.IsDown ? state.Down : Interpolate(state.Back, state.Hover, state.HoverProgress);

                using (var path = CreateRoundedRectPath(rect, 12))
                {
                    // Draw the background of the button
                    using (var brush = new SolidBrush(current))
                        e.Graphics.FillPath(brush, path);
                    if (state.TextureImage != null)
                    {
                        using var textureBrush = new TextureBrush(state.TextureImage);
                        e.Graphics.FillPath(textureBrush, path);
                    }

                    // Draw the border
                    using (var pen = new Pen(state.Border, 1.5f))
                        e.Graphics.DrawPath(pen, path);
                }

                // Draw the text (This uses the .Text property we just set)
                TextRenderer.DrawText(e.Graphics, btn.Text, btn.Font, btn.ClientRectangle,
                    Color.FromArgb(80, 40, 45),
                    TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis);
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

        // State class to track independent animation progress for each button
        public class RoundedButtonStyleState
        {
            public Color Back, Border, Hover, Down;
            public float HoverProgress, HoverTarget;
            public bool IsHover, IsDown;
            public System.Windows.Forms.Timer AnimationTimer = new();
            public Image? TextureImage;
        }
    }
}