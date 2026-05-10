using G_Tara.Models;
using G_Tara.Services;
using System.Drawing.Drawing2D;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace G_Tara
{
    public partial class ParticipantsManagementForm : Form
    {
        [DllImport("user32.dll")]
        private static extern bool ReleaseCapture();

        [DllImport("user32.dll")]
        private static extern IntPtr SendMessage(IntPtr hWnd, int msg, int wParam, int lParam);

        private const int WM_NCLBUTTONDOWN = 0x00A1;
        private const int HTCAPTION = 0x0002;

        private Participant? _selectedParticipant = null;
        private Panel? _selectedCardPanel = null;
        private Button btnFooterEdit;
        private Button btnFooterRemove;
        private Button btnFooterAdd;
        private Button btnFooterClose;
        private readonly ParticipantsDataService _dataService;
        private List<Participant> _participants;
        private readonly System.Media.SoundPlayer _hoverSound = new System.Media.SoundPlayer();

        private FlowLayoutPanel cardContainer;
        private TextBox txtSearch;
        private Button btnAdd;
        private Panel pnlTitleBar;
        private Button btnWindowMinimize;
        private Button btnWindowClose;
        private Label lblWindowTitle;

        public ParticipantsManagementForm(ParticipantsDataService dataService)
        {
            _dataService = dataService;
            _participants = new List<Participant>();
            LoadSounds();
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            Color galaPinkLight = Color.FromArgb(242, 215, 217);
            Color galaWhite = Color.FromArgb(255, 248, 248);
            Color galaText = Color.FromArgb(100, 60, 65);
            Color galaDeepPink = Color.FromArgb(189, 126, 131);
            Color darkPinkText = Color.FromArgb(120, 40, 50);
            Color searchPink = Color.FromArgb(248, 225, 228);


            this.Text = "Manage Participants";
            this.Size = new Size(900, 650);
            this.StartPosition = FormStartPosition.CenterParent;
            this.BackColor = galaPinkLight;
            this.Font = new Font("Segoe UI Semibold", 9.5F);
            this.ForeColor = galaText;
            this.FormBorderStyle = FormBorderStyle.None;
            this.SizeChanged += (s, e) => ApplyRoundedFormRegion();
            ApplyRoundedFormRegion();

            pnlTitleBar = new Panel
            {
                Dock = DockStyle.Top,
                Height = 34,
                BackColor = Color.FromArgb(241, 206, 211)
            };
            pnlTitleBar.MouseDown += OnTitleBarMouseDown;

            lblWindowTitle = new Label
            {
                Text = "Manage Participants",
                AutoSize = true,
                Location = new Point(12, 8),
                Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold),
                ForeColor = Color.FromArgb(80, 40, 45),
                BackColor = Color.Transparent
            };
            lblWindowTitle.MouseDown += OnTitleBarMouseDown;

            btnWindowMinimize = new Button
            {
                Text = "♡",
                Size = new Size(30, 28),
                Location = new Point(this.ClientSize.Width - 102, 2),
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
                Size = new Size(30, 28),
                Location = new Point(this.ClientSize.Width - 34, 2),
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

            LayoutWindowButtons();

            Panel topPanel = new Panel { Dock = DockStyle.Top, Height = 96, BackColor = Color.Transparent };
            Panel bottomPanel = new Panel { Dock = DockStyle.Bottom, Height = 100, BackColor = galaDeepPink, Padding = new Padding(0, 18, 0, 18) };

            TableLayoutPanel centerTable = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 1, RowCount = 1 };
            centerTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50f));
            centerTable.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 500f));
            centerTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50f));

            Panel searchContainer = new Panel { Size = new Size(500, 50), BackColor = searchPink, Anchor = AnchorStyles.None, Padding = new Padding(18, 10, 18, 10) };
            TableLayoutPanel searchLayout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 1,
                BackColor = Color.Transparent
            };
            searchLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 34f));
            searchLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));
            Label lblSearchIcon = new Label
            {
                Text = "🔍",
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleCenter,
                AutoSize = false,
                Font = new Font("Segoe UI", 12),
                BackColor = Color.Transparent
            };
            txtSearch = new TextBox
            {
                Dock = DockStyle.Fill,
                BorderStyle = BorderStyle.None,
                Font = new Font("Segoe UI", 12),
                PlaceholderText = "Search participants...",
                BackColor = searchPink
            };
            searchLayout.Controls.Add(lblSearchIcon, 0, 0);
            searchLayout.Controls.Add(txtSearch, 1, 0);
            searchContainer.Controls.Add(searchLayout);
            searchContainer.Region = new Region(CreateRoundedRectPath(new Rectangle(0, 0, 500, 50), 25));

            centerTable.Controls.Add(searchContainer, 1, 0);
            topPanel.Controls.Add(centerTable);

            cardContainer = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                Padding = new Padding(30, 10, 30, 40),
                BackColor = Color.Transparent,
                WrapContents = true
            };

            TableLayoutPanel footerLayout = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 3, RowCount = 1 };
            footerLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50f));
            footerLayout.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            footerLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50f));

            FlowLayoutPanel footerButtons = new FlowLayoutPanel
            {
                AutoSize = true,
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = false,
                Anchor = AnchorStyles.None,
                Margin = new Padding(0),
                Padding = new Padding(0)
            };

            btnFooterAdd = new Button { Text = "👤 Add Participant", Size = new Size(180, 46), Cursor = Cursors.Hand, Margin = new Padding(8, 0, 8, 0) };
            btnFooterEdit = new Button { Text = "✎ Edit Details", Size = new Size(170, 46), Cursor = Cursors.Hand, Enabled = false, Margin = new Padding(8, 0, 8, 0) };
            btnFooterRemove = new Button { Text = "🗑 Remove Selected", Size = new Size(190, 46), Cursor = Cursors.Hand, Enabled = false, Margin = new Padding(8, 0, 8, 0) };
            btnFooterClose = new Button { Text = "× Close", Size = new Size(120, 46), Cursor = Cursors.Hand, Margin = new Padding(8, 0, 8, 0) };

            footerButtons.Controls.Add(btnFooterAdd);
            footerButtons.Controls.Add(btnFooterEdit);
            footerButtons.Controls.Add(btnFooterRemove);
            footerButtons.Controls.Add(btnFooterClose);

            footerLayout.Controls.Add(footerButtons, 1, 0);
            bottomPanel.Controls.Add(footerLayout);

            btnFooterAdd.Click += (s, e) => AddParticipant();
            btnFooterEdit.Click += (s, e) => { if (_selectedParticipant != null) EditSpecificParticipant(_selectedParticipant); };
            btnFooterRemove.Click += (s, e) => { if (_selectedParticipant != null) RemoveSpecificParticipant(_selectedParticipant); };
            btnFooterClose.Click += (s, e) => this.Close();
            txtSearch.TextChanged += (s, e) => RefreshGrid(txtSearch.Text);

            ApplyPillButtonStyle(btnFooterAdd, galaWhite, darkPinkText);
            ApplyPillButtonStyle(btnFooterEdit, galaWhite, darkPinkText);
            ApplyPillButtonStyle(btnFooterRemove, galaWhite, darkPinkText);
            ApplyPillButtonStyle(btnFooterClose, galaWhite, darkPinkText);

            this.Controls.Add(cardContainer);
            this.Controls.Add(bottomPanel);
            this.Controls.Add(topPanel);
            this.Controls.Add(pnlTitleBar);

            LayoutWindowButtons();

            ApplyGaraTheme();
            this.Load += ParticipantsManagementForm_Load;
            this.Resize += (s, e) => LayoutWindowButtons();
        }

        private void ParticipantsManagementForm_Load(object? sender, EventArgs e)
        {
            LayoutWindowButtons();
            if (!DesignMode)
            {
                LoadParticipants();
            }
        }

        private void LoadParticipants()
        {
            _participants = _dataService.LoadParticipants();
            RefreshGrid();
        }

        private void RefreshGrid(string? searchTerm = null)
        {
            cardContainer.Controls.Clear();

            var displayList = string.IsNullOrWhiteSpace(searchTerm)
                ? _participants
                : _participants.Where(p => p.Name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)).ToList();

            foreach (var participant in displayList)
            {
                cardContainer.Controls.Add(CreateParticipantCard(participant));
            }
        }

        private void AddParticipant()
        {
            using var dialog = new ParticipantInputDialog();
            if (dialog.ShowDialog(this) == DialogResult.OK)
            {
                string savedImagePath = SaveParticipantImage(dialog.ParticipantImagePath, dialog.ParticipantName);

                var participant = new Participant
                {
                    Name = dialog.ParticipantName,
                    Email = dialog.ParticipantEmail,
                    AvailableDates = dialog.ParticipantAvailableDates,
                    ImagePath = savedImagePath
                };

                _dataService.SaveParticipant(participant);
                LoadParticipants();
            }
        }

        private void OnSearch(object sender, EventArgs e)
        {
            RefreshGrid(txtSearch.Text.Trim());
        }

        private void ApplyPillButtonStyle(Button btn, Color backColor, Color textColor)
        {
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 2;
            btn.FlatAppearance.BorderColor = Color.FromArgb(175, 118, 126);
            btn.BackColor = backColor;
            btn.ForeColor = textColor;
            btn.Font = new Font("Segoe UI", 11, FontStyle.Bold);

            void ApplyRegion()
            {
                if (btn.Width <= 0 || btn.Height <= 0) return;
                int radius = Math.Min(btn.Height / 2, 22);
                btn.Region = new Region(CreateRoundedRectPath(new Rectangle(0, 0, btn.Width, btn.Height), radius));
            }
            ApplyRegion();
            btn.Resize += (s, e) => ApplyRegion();
            btn.MouseEnter += (s, e) =>
            {
                btn.BackColor = Color.FromArgb(240, 240, 240);
                TryPlayHoverSound();
            };
            btn.MouseLeave += (s, e) => btn.BackColor = backColor;
        }

        private Panel CreateParticipantCard(Participant p)
        {
            Color cardBack = Color.FromArgb(255, 248, 248);
            Panel card = new Panel { Size = new Size(360, 174), Margin = new Padding(15), BackColor = cardBack, Padding = new Padding(12) };
            TableLayoutPanel mainLayout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 1,
                Padding = new Padding(10),
                Margin = new Padding(0),
                BackColor = cardBack
            };
            mainLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 130f));
            mainLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));
            CircleProfile picCircle = new CircleProfile
            {
                Size = new Size(110, 110),
                Anchor = AnchorStyles.None,
                Image = (string.IsNullOrEmpty(p.ImagePath) || !File.Exists(p.ImagePath)) ? null : Image.FromFile(p.ImagePath),
                BorderColorStart = Color.FromArgb(212, 175, 55),
                BorderColorEnd = Color.FromArgb(255, 235, 150)
            };
            FlowLayoutPanel textFlow = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                Padding = new Padding(10, 10, 0, 0),
                BackColor = cardBack
            };
            Label lblName = new Label { Text = $"Name: {p.Name}", Font = new Font("Segoe UI", 12, FontStyle.Bold), ForeColor = Color.FromArgb(120, 40, 50), AutoSize = true, BackColor = cardBack };
            Label lblEmail = new Label { Text = $"Email: {p.Email}", Font = new Font("Segoe UI", 10), ForeColor = Color.FromArgb(80, 40, 45), AutoSize = true, BackColor = cardBack };
            string availabilityText = "Dates: No dates set";
            if (p.AvailableDates?.Any() == true)
            {
                var sorted = p.AvailableDates.OrderBy(d => d).ToList();
                availabilityText = "Dates: " + string.Join(", ", sorted.Select(d => d.ToString("MMM dd")));
            }
            Label lblDates = new Label
            {
                Text = availabilityText,
                Font = new Font("Segoe UI", 10),
                ForeColor = Color.FromArgb(80, 40, 45),
                BackColor = cardBack,
                AutoSize = false,
                Size = new Size(190, 36),
                AutoEllipsis = true
            };
            Label lblId = new Label
            {
                Text = $"ID: {p.Id}",
                Font = new Font("Segoe UI", 8),
                ForeColor = Color.Gray,
                BackColor = cardBack,
                AutoSize = false,
                Size = new Size(190, 28),
                AutoEllipsis = true,
                Margin = new Padding(0, 5, 0, 0)
            };
            textFlow.Controls.AddRange(new Control[] { lblName, lblEmail, lblDates, lblId });
            mainLayout.Controls.Add(picCircle, 0, 0);
            mainLayout.Controls.Add(textFlow, 1, 0);
            card.Controls.Add(mainLayout);
            void ApplyCardRegion()
            {
                if (card.Width <= 0 || card.Height <= 0) return;
                using var regionPath = CreateRoundedRectPath(new Rectangle(0, 0, card.Width - 1, card.Height - 1), 20);
                card.Region = new Region(regionPath);
            }
            ApplyCardRegion();
            card.Resize += (s, e) => ApplyCardRegion();
            card.Paint += (s, e) => {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                Rectangle borderRect = new Rectangle(0, 0, card.Width - 1, card.Height - 1);
                using var path = CreateRoundedRectPath(borderRect, 20);
                using (var fillBrush = new SolidBrush(cardBack))
                {
                    e.Graphics.FillPath(fillBrush, path);
                }
                bool isSelected = _selectedParticipant?.Id == p.Id;
                Color borderColor = isSelected ? Color.FromArgb(128, 35, 45) : Color.FromArgb(172, 112, 121);
                float borderWidth = isSelected ? 3.8f : 2.2f;
                using var pen = new Pen(borderColor, borderWidth);
                pen.Alignment = PenAlignment.Inset;
                e.Graphics.DrawPath(pen, path);
            };
            void OnCardSelected(object? s, EventArgs e)
            {
                Panel? previouslySelected = _selectedCardPanel;
                _selectedParticipant = p;
                _selectedCardPanel = card;
                btnFooterEdit.Enabled = true;
                btnFooterRemove.Enabled = true;

                previouslySelected?.Invalidate();
                card.Invalidate();
            }
            card.Click += OnCardSelected;
            foreach (Control c in textFlow.Controls) c.Click += OnCardSelected;
            foreach (Control c in mainLayout.Controls) c.Click += OnCardSelected;
            return card;
        }

        private void EditSpecificParticipant(Participant p)
        {
            using var dialog = new ParticipantInputDialog
            {
                ParticipantName = p.Name,
                ParticipantEmail = p.Email,
                ParticipantAvailableDates = p.AvailableDates
            };

            if (dialog.ShowDialog(this) == DialogResult.OK)
            {
                p.Name = dialog.ParticipantName;
                p.Email = dialog.ParticipantEmail;
                p.AvailableDates = dialog.ParticipantAvailableDates;
                p.ImagePath = SaveParticipantImage(dialog.ParticipantImagePath, p.Name);
                _dataService.SaveParticipant(p);
                LoadParticipants();
            }
        }

        private void OnAddParticipant(object sender, EventArgs e)
        {
            using var dialog = new ParticipantInputDialog();
            if (dialog.ShowDialog(this) == DialogResult.OK)
            {
                string savedImagePath = SaveParticipantImage(dialog.ParticipantImagePath, dialog.ParticipantName);

                var participant = new Participant
                {
                    Name = dialog.ParticipantName,
                    Email = dialog.ParticipantEmail,
                    AvailableDates = dialog.ParticipantAvailableDates,
                    ImagePath = savedImagePath
                };
                _dataService.SaveParticipant(participant);
                LoadParticipants();
            }
        }

        private string SaveParticipantImage(string sourcePath, string participantName)
        {
            if (string.IsNullOrEmpty(sourcePath) || !File.Exists(sourcePath)) return string.Empty;

            try
            {
                string folderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ParticipantPhotos");
                if (!Directory.Exists(folderPath)) Directory.CreateDirectory(folderPath);

                // Create unique filename
                string extension = Path.GetExtension(sourcePath);
                string fileName = $"{participantName}_{Guid.NewGuid()}{extension}";
                string destPath = Path.Combine(folderPath, fileName);

                File.Copy(sourcePath, destPath, true);
                return destPath;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error saving image: " + ex.Message);
                return string.Empty;
            }
        }


        private void RemoveSpecificParticipant(Participant p)
        {
            if (MessageBox.Show($"Are you sure you want to remove {p.Name} (ID: {p.Id})?",
                "Confirm Deletion", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                _dataService.DeleteParticipant(p.Id);

                LoadParticipants();
                txtSearch.Clear();
            }
        }


        private void ApplyGaraTheme()
        {
            Color btnBack = Color.FromArgb(248, 230, 231);
            Color btnBorder = Color.FromArgb(210, 170, 175);
            Color btnHover = Color.FromArgb(235, 190, 195);
            Color btnDown = Color.FromArgb(200, 150, 155);

            ApplyRoundedButtonStyle(btnFooterAdd, btnBack, btnBorder, btnHover, btnDown, null);
            ApplyRoundedButtonStyle(btnFooterEdit, btnBack, btnBorder, btnHover, btnDown, null);
            ApplyRoundedButtonStyle(btnFooterRemove, btnBack, btnBorder, btnHover, btnDown, null);
            ApplyRoundedButtonStyle(btnFooterClose, btnBack, btnBorder, btnHover, btnDown, null);
        }

        private static void ApplyRoundedButtonStyle(Button btn, Color back, Color border, Color hover, Color down, System.Media.SoundPlayer? hoverSound)
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

                Color current = state.IsDown ? state.Down : Interpolate(state.Back, state.Hover, state.HoverProgress);

                using (var path = CreateRoundedRectPath(new Rectangle(0, 0, btn.Width - 1, btn.Height - 1), 12))
                {
                    using (var brush = new SolidBrush(current)) e.Graphics.FillPath(brush, path);
                    using (var pen = new Pen(state.Border, 1f)) e.Graphics.DrawPath(pen, path);
                }

                TextRenderer.DrawText(e.Graphics, btn.Text, btn.Font, btn.ClientRectangle, Color.FromArgb(64, 64, 64),
                    TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
            };
        }

        private static Color Interpolate(Color b, Color t, float p) =>
            Color.FromArgb((int)(b.R + (t.R - b.R) * p), (int)(b.G + (t.G - b.G) * p), (int)(b.B + (t.B - b.B) * p));

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
                
            }
        }

        private void TryPlayHoverSound()
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(_hoverSound.SoundLocation))
                {
                    _hoverSound.Play();
                }
            }
            catch
            {
                // Ignore playback errors.
            }
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
            btnWindowClose.Location = new Point(pnlTitleBar.Width - btnWindowClose.Width - 8, 2);
            btnWindowMinimize.Location = new Point(btnWindowClose.Left - btnWindowMinimize.Width - 8, 2);
        }

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
            public System.Windows.Forms.Timer AnimationTimer = new();
        }
    }

    [DefaultProperty("Image")]
    public class CircleProfile : Control
    {
        private Image? _image;
        private Color _borderColorStart = Color.Gold;
        private Color _borderColorEnd = Color.White;

        [Category("Appearance")]
        [Description("The profile image to display.")]
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public Image? Image
        {
            get => _image;
            set { _image = value; Invalidate(); }
        }

        [Category("Appearance")]
        [Description("The starting color of the gold gradient border.")]
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public Color BorderColorStart
        {
            get => _borderColorStart;
            set { _borderColorStart = value; Invalidate(); }
        }

        [Category("Appearance")]
        [Description("The ending color of the gold gradient border.")]
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public Color BorderColorEnd
        {
            get => _borderColorEnd;
            set { _borderColorEnd = value; Invalidate(); }
        }

        public CircleProfile()
        {
            this.DoubleBuffered = true;
            this.Size = new Size(120, 120);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            Rectangle rect = new Rectangle(5, 5, Width - 11, Height - 11);

            using (var path = new GraphicsPath())
            {
                path.AddEllipse(rect);
                this.Region = new Region(path);

                e.Graphics.FillEllipse(Brushes.White, rect);

                if (Image != null)
                {
                    e.Graphics.SetClip(path);
                    e.Graphics.DrawImage(Image, rect);
                    e.Graphics.ResetClip();
                }
                else
                {
                    e.Graphics.FillEllipse(Brushes.LightGray, rect);
                }

                using (var brush = new LinearGradientBrush(rect, BorderColorStart, BorderColorEnd, 45f))
                using (var pen = new Pen(brush, 4f))
                {
                    e.Graphics.DrawEllipse(pen, rect);
                }
            }
        }
    }
}