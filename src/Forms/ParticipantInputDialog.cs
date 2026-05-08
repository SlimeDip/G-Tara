using System;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;

namespace G_Tara
{
    public partial class ParticipantInputDialog : Form
    {
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
            ((ISupportInitialize)picProfile).BeginInit();
            SuspendLayout();
            // 
            // lblName
            // 
            lblName.AutoSize = true;
            lblName.Location = new Point(30, 40);
            lblName.Name = "lblName";
            lblName.Size = new Size(49, 19);
            lblName.TabIndex = 10;
            lblName.Text = "Name:";
            // 
            // txtParticipantName
            // 
            txtParticipantName.BackColor = Color.FromArgb(255, 248, 248);
            txtParticipantName.Location = new Point(160, 38);
            txtParticipantName.Name = "txtParticipantName";
            txtParticipantName.Size = new Size(250, 25);
            txtParticipantName.TabIndex = 9;
            // 
            // lblEmail
            // 
            lblEmail.AutoSize = true;
            lblEmail.Location = new Point(30, 90);
            lblEmail.Name = "lblEmail";
            lblEmail.Size = new Size(99, 19);
            lblEmail.TabIndex = 8;
            lblEmail.Text = "Email Address:";
            // 
            // txtParticipantEmail
            // 
            txtParticipantEmail.BackColor = Color.FromArgb(255, 248, 248);
            txtParticipantEmail.Location = new Point(160, 88);
            txtParticipantEmail.Name = "txtParticipantEmail";
            txtParticipantEmail.Size = new Size(250, 25);
            txtParticipantEmail.TabIndex = 7;
            // 
            // lblAvailableDates
            // 
            lblAvailableDates.AutoSize = true;
            lblAvailableDates.Location = new Point(30, 140);
            lblAvailableDates.Name = "lblAvailableDates";
            lblAvailableDates.Size = new Size(82, 19);
            lblAvailableDates.TabIndex = 6;
            lblAvailableDates.Text = "Availability:";
            // 
            // txtAvailableDates
            // 
            txtAvailableDates.BackColor = Color.FromArgb(255, 248, 248);
            txtAvailableDates.Location = new Point(160, 138);
            txtAvailableDates.Multiline = true;
            txtAvailableDates.Name = "txtAvailableDates";
            txtAvailableDates.ReadOnly = true;
            txtAvailableDates.Size = new Size(250, 70);
            txtAvailableDates.TabIndex = 5;
            // 
            // btnPickDates
            // 
            btnPickDates.Location = new Point(160, 220);
            btnPickDates.Name = "btnPickDates";
            btnPickDates.Size = new Size(120, 35);
            btnPickDates.TabIndex = 4;
            btnPickDates.Text = "Select Dates";
            // 
            // btnClearDates
            // 
            btnClearDates.Location = new Point(290, 220);
            btnClearDates.Name = "btnClearDates";
            btnClearDates.Size = new Size(120, 35);
            btnClearDates.TabIndex = 3;
            btnClearDates.Text = "Clear All";
            // 
            // picProfile
            // 
            picProfile.BackColor = Color.FromArgb(255, 248, 248);
            picProfile.BorderStyle = BorderStyle.FixedSingle;
            picProfile.Location = new Point(460, 40);
            picProfile.Name = "picProfile";
            picProfile.Size = new Size(200, 200);
            picProfile.SizeMode = PictureBoxSizeMode.Zoom;
            picProfile.TabIndex = 1;
            picProfile.TabStop = false;
            // 
            // btnUploadPhoto
            // 
            btnUploadPhoto.Location = new Point(460, 250);
            btnUploadPhoto.Name = "btnUploadPhoto";
            btnUploadPhoto.Size = new Size(200, 40);
            btnUploadPhoto.TabIndex = 2;
            btnUploadPhoto.Text = "📷 Upload Photo";
            btnUploadPhoto.Click += OnUploadPhotoClick;
            // 
            // btnOK
            // 
            btnOK.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnOK.Location = new Point(462, 356);
            btnOK.Name = "btnOK";
            btnOK.Size = new Size(110, 34);
            btnOK.TabIndex = 0;
            btnOK.Text = "Confirm";
            // 
            // btnCancel
            // 
            btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnCancel.Location = new Point(584, 356);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(110, 34);
            btnCancel.TabIndex = 1;
            btnCancel.Text = "Cancel";
            // 
            // ParticipantInputDialog
            // 
            AcceptButton = btnOK;
            BackColor = Color.FromArgb(242, 215, 217);
            CancelButton = btnCancel;
            ClientSize = new Size(704, 411);
            Controls.Add(btnCancel);
            Controls.Add(btnOK);
            Controls.Add(picProfile);
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
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            Name = "ParticipantInputDialog";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Add Participant";
            ((ISupportInitialize)picProfile).EndInit();
            ResumeLayout(false);
            PerformLayout();
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
            // G-Tara Signature Button Palette
            Color btnBack = Color.FromArgb(248, 230, 231);
            Color btnBorder = Color.FromArgb(210, 170, 175);
            Color btnHover = Color.FromArgb(235, 190, 195);
            Color btnDown = Color.FromArgb(200, 150, 155);

            // Apply the rounded style and animation to all buttons
            ApplyRoundedButtonStyle(btnPickDates, btnBack, btnBorder, btnHover, btnDown, null);
            ApplyRoundedButtonStyle(btnClearDates, btnBack, btnBorder, btnHover, btnDown, null);
            ApplyRoundedButtonStyle(btnOK, btnBack, btnBorder, btnHover, btnDown, null);
            ApplyRoundedButtonStyle(btnCancel, btnBack, btnBorder, btnHover, btnDown, null);

            if (btnUploadPhoto != null)
            {
                ApplyRoundedButtonStyle(btnUploadPhoto, btnBack, btnBorder, btnHover, btnDown, null);
            }
        }

        private void OnUploadPhotoClick(object sender, EventArgs e)
        {
            using OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.gif";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                _selectedImagePath = ofd.FileName;
                picProfile.Image = Image.FromFile(_selectedImagePath);
            }
        }

        private static void ApplyRoundedButtonStyle(Button btn, Color back, Color border, Color hover, Color down, System.Media.SoundPlayer? hoverSound)
        {
            if (btn.Tag is RoundedButtonStyleState) return;

            var state = new RoundedButtonStyleState { Back = back, Border = border, Hover = hover, Down = down };
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

            // 2. REMOVE THE REGION ASSIGNMENT 
            // (Manual painting with SmoothingMode.AntiAlias handles the "rounded" look better)
            btn.Region = null;

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

            btn.MouseEnter += (s, e) => { state.IsHover = true; state.HoverTarget = 1f; state.AnimationTimer.Start(); };
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
        }
    }
}