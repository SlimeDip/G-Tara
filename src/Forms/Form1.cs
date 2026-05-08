using G_Tara.Models;
using G_Tara.Services;
using System.Runtime.InteropServices;

namespace G_Tara
{
    public partial class Form1 : Form
    {
        [DllImport("user32.dll")]
        private static extern bool ReleaseCapture();

        [DllImport("user32.dll")]
        private static extern IntPtr SendMessage(IntPtr hWnd, int msg, int wParam, int lParam);

        private const int WM_NCLBUTTONDOWN = 0x00A1;
        private const int HTCAPTION = 0x0002;

        private readonly System.Media.SoundPlayer _hoverSound = new System.Media.SoundPlayer();
        private readonly GalaDataService _dataService;
        private readonly ParticipantsDataService _participantsDataService;
        private readonly WeatherService _weatherService;
        private Host? _currentHost;
        private List<Participant> _participants;
        private List<Gala> _galas;
        private bool _dateSortAscending = true;

        [System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Hidden)]
        public List<Participant> Participants { get; set; } = new();

        public Form1()
        {
            InitializeComponent();
            LoadSounds();
            _dataService = new GalaDataService();
            _participantsDataService = new ParticipantsDataService();
            _weatherService = new WeatherService();
            _galas = new List<Gala>();
            _participants = new List<Participant>();

            ApplyTitleBarStyling();
            ApplyToolbarStyling();
            TryLoadToolbarIconsFromAssets();

            LayoutTitleBar();
            if (titleBar != null)
            {
                titleBar.SizeChanged += (_, _) => LayoutTitleBar();
            }
        }

        public Form1(Host? host) : this()
        {
            _currentHost = host;
        }

        public void Form1_Load(object sender, EventArgs e)
        {
            LoadGalas();
        }

        private void TitleBar_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left)
            {
                return;
            }

            ReleaseCapture();
            SendMessage(Handle, WM_NCLBUTTONDOWN, HTCAPTION, 0);
        }

        private void BtnWindowMinimize_Click(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Minimized;
        }

        private void BtnWindowClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void ApplyTitleBarStyling()
        {
            if (lblTitle != null)
            {
                lblTitle.AutoSize = true;
                lblTitle.Font = new Font("Segoe UI", 10.5F, FontStyle.Bold);
                lblTitle.ForeColor = Color.FromArgb(250, 247, 245);
            }

            StyleTitleBarButton(btnWindowMinimize, "♡");
            StyleTitleBarButton(btnWindowClose, "♥");
        }

        private static void StyleTitleBarButton(Button? btn, string text)
        {
            if (btn == null)
            {
                return;
            }

            btn.Text = text;
            btn.UseVisualStyleBackColor = false;
            btn.BackColor = Color.Transparent;
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
            btn.FlatAppearance.MouseOverBackColor = Color.FromArgb(224, 156, 166);
            btn.FlatAppearance.MouseDownBackColor = Color.FromArgb(210, 137, 147);
            btn.Font = new Font("Segoe UI Symbol", 12F, FontStyle.Regular);
            btn.TextAlign = ContentAlignment.MiddleCenter;
            btn.Padding = new Padding(0, 1, 0, 0);
            btn.Margin = new Padding(0);
        }

        private void LayoutTitleBar()
        {
            if (titleBar == null)
            {
                return;
            }
            // Position the logo first[cite: 2]
            if (picMiniLogo != null)
            {
                picMiniLogo.Location = new Point(12, (titleBar.Height - picMiniLogo.Height) / 2);
            }

            // Position the title text right after the logo[cite: 2]
            if (lblTitle != null && picMiniLogo != null)
            {
                lblTitle.Location = new Point(picMiniLogo.Right + 8, (titleBar.Height - lblTitle.Height) / 2);
            }

            if (btnWindowMinimize != null && btnWindowClose != null)
            {
                // Center the window buttons using the container padding (FlowLayoutPanel padding + button margin).
                var top = Math.Max(0, (titleBar.Height - btnWindowClose.Height) / 2);

                if (windowButtonsPanel != null)
                {
                    top = Math.Max(0, (titleBar.Height - 32) / 2);
                    windowButtonsPanel.Padding = new Padding(0, top, 8, 0);
                }

                btnWindowMinimize.Margin = new Padding(0, 0, 6, 0);
                btnWindowClose.Margin = new Padding(0);
            }
        }

        private void ApplyToolbarStyling()
        {
            var scale = DeviceDpi / 96f;

            if (mainPanel.RowStyles.Count > 0)
            {
                mainPanel.RowStyles[0].SizeType = SizeType.Absolute;
                mainPanel.RowStyles[0].Height = (int)Math.Round(140 * scale);
            }

            if (toolbarContainer != null)
            {
                toolbarContainer.Padding = new Padding(0);
                toolbarContainer.Margin = new Padding(0);
            }

            if (toolbarPanel != null)
            {
                toolbarPanel.Dock = DockStyle.None;
                toolbarPanel.Anchor = AnchorStyles.None;
                toolbarPanel.AutoSize = true;
                toolbarPanel.AutoSizeMode = AutoSizeMode.GrowAndShrink;
                toolbarPanel.WrapContents = false;
                toolbarPanel.Padding = new Padding(0);
                toolbarPanel.Margin = new Padding(0);
                toolbarPanel.BackColor = Color.Transparent;
            }

            var buttonBack = Color.FromArgb(241, 214, 221);
            var border = Color.FromArgb(186, 120, 132);
            var hover = Color.FromArgb(242, 226, 229);
            var down = Color.FromArgb(226, 199, 205);

            StyleToolbarButton(btnAdd, buttonBack, border, hover, down, _hoverSound);
            StyleToolbarButton(btnEdit, buttonBack, border, hover, down, _hoverSound);
            StyleToolbarButton(btnDelete, buttonBack, border, hover, down, _hoverSound);
            StyleToolbarButton(btnManageParticipants, buttonBack, border, hover, down, _hoverSound);
            StyleToolbarButton(btnAutoEmail, buttonBack, border, hover, down, _hoverSound);

            if (btnManageParticipants != null)
            {
                btnManageParticipants.Text = "Manage\r\nParticipants";
                btnManageParticipants.Font = new Font("Comic Sans MS", 8.5F, FontStyle.Bold);
            }

            ApplyListStyling();
        }

        private void ApplyListStyling()
        {
            if (listPanel != null)
            {
                listPanel.BackColor = Color.FromArgb(240, 220, 226);
                listPanel.Padding = new Padding(120, 0, 120, 80);
            }

            if (dgvGalas != null)
            {
                dgvGalas.Dock = DockStyle.Fill;
                dgvGalas.BackgroundColor = Color.FromArgb(248, 238, 241);
                dgvGalas.BorderStyle = BorderStyle.None;
                dgvGalas.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
                dgvGalas.GridColor = Color.FromArgb(210, 170, 178);

                dgvGalas.ScrollBars = ScrollBars.Both;
                dgvGalas.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
                dgvGalas.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;

                dgvGalas.EnableHeadersVisualStyles = false;
                dgvGalas.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(231, 199, 206);
                dgvGalas.ColumnHeadersDefaultCellStyle.ForeColor = Color.FromArgb(60, 45, 48);
                dgvGalas.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Regular);

                dgvGalas.DefaultCellStyle.BackColor = Color.FromArgb(252, 248, 249);
                dgvGalas.DefaultCellStyle.ForeColor = Color.FromArgb(60, 45, 48);
                dgvGalas.DefaultCellStyle.SelectionBackColor = Color.FromArgb(214, 147, 156);
                dgvGalas.DefaultCellStyle.SelectionForeColor = Color.White;
                dgvGalas.DefaultCellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Regular);

                dgvGalas.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(248, 238, 241);
                dgvGalas.RowHeadersVisible = true;
                dgvGalas.RowHeadersDefaultCellStyle.BackColor = Color.FromArgb(231, 199, 206);
                dgvGalas.RowHeadersDefaultCellStyle.ForeColor = Color.FromArgb(60, 45, 48);
                dgvGalas.RowHeadersDefaultCellStyle.SelectionBackColor = Color.FromArgb(214, 147, 156);
                dgvGalas.RowHeadersDefaultCellStyle.SelectionForeColor = Color.White;
                dgvGalas.RowHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
                dgvGalas.RowTemplate.Height = 28;

                dgvGalas.CellFormatting -= DgvGalas_CellFormatting;
                dgvGalas.CellFormatting += DgvGalas_CellFormatting;
            }
        }

        private void DgvGalas_CellFormatting(object? sender, DataGridViewCellFormattingEventArgs e)
        {
            if (sender is not DataGridView dgv || e.RowIndex < 0 || e.ColumnIndex < 0)
            {
                return;
            }

            // Force the entire selected row to use the theme selection color,
            // including the "Id" column that tends to keep system highlight colors.
            var rowSelected = dgv.Rows[e.RowIndex].Selected;
            if (!rowSelected)
            {
                return;
            }

            e.CellStyle.SelectionBackColor = dgv.DefaultCellStyle.SelectionBackColor;
            e.CellStyle.SelectionForeColor = dgv.DefaultCellStyle.SelectionForeColor;
        }

        private static void StyleToolbarButton(Button? btn, Color back, Color border, Color hover, Color down, System.Media.SoundPlayer hoverSound)
        {
            if (btn == null)
            {
                return;
            }

            var scale = btn.DeviceDpi / 96f;

            btn.UseVisualStyleBackColor = false;
            btn.BackColor = Color.Transparent;
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
            btn.FlatAppearance.MouseOverBackColor = Color.Transparent;
            btn.FlatAppearance.MouseDownBackColor = Color.Transparent;
            btn.Font = new Font("Comic Sans MS", 10F, FontStyle.Bold);
            btn.ForeColor = Color.FromArgb(60, 45, 48);
            btn.Size = new Size(
                (int)Math.Round(156 * scale),
                (int)Math.Round(92 * scale));
            btn.TextImageRelation = TextImageRelation.ImageAboveText;
            btn.ImageAlign = ContentAlignment.MiddleCenter;
            btn.TextAlign = ContentAlignment.BottomCenter;
            btn.Margin = new Padding(12, 0, 12, 0);

            ApplyRoundedButtonStyle(btn, back, border, hover, down, hoverSound);
        }

        private sealed class RoundedButtonStyleState
        {
            public required Color Back { get; init; }
            public required Color Border { get; init; }
            public required Color Hover { get; init; }
            public required Color Down { get; init; }
            public bool IsHover { get; set; }
            public bool IsDown { get; set; }

            public float HoverProgress { get; set; }
            public float HoverTarget { get; set; }
            public System.Windows.Forms.Timer? AnimationTimer { get; set; }
        }

        private static void ApplyRoundedButtonStyle(Button btn, Color back, Color border, Color hover, Color down, System.Media.SoundPlayer hoverSound)
        {
            if (btn.Tag is RoundedButtonStyleState)
            {
                return;
            }

            var state = new RoundedButtonStyleState
            {
                Back = back,
                Border = border,
                Hover = hover,
                Down = down
            };

            btn.Tag = state;

            // Prevent default theme drawing from fighting our custom paint.
            btn.UseCompatibleTextRendering = true;

            // Ensure Region is applied immediately.
            btn.Region?.Dispose();
            btn.Region = new Region(CreateRoundedRectPath(new Rectangle(0, 0, btn.Width, btn.Height), 16));

            state.AnimationTimer = new System.Windows.Forms.Timer { Interval = 15 };
            state.AnimationTimer.Tick += (_, _) =>
            {
                // Smoothly approach target.
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

            btn.MouseEnter += (_, _) =>
            {
                state.IsHover = true;
                state.HoverTarget = 1f;
                state.AnimationTimer?.Start();

                // Play the custom sound
                try
                {
                    // Only play if the sound was successfully loaded
                    if (hoverSound != null && !string.IsNullOrEmpty(hoverSound.SoundLocation))
                    {
                        hoverSound.Play();
                    }
                }
                catch
                {
                    // Fallback to a subtle system sound if the custom file fails
                    System.Media.SystemSounds.Asterisk.Play();
                }

                btn.Invalidate();
            };

            btn.MouseLeave += (_, _) =>
            {
                state.IsHover = false;
                state.IsDown = false;
                state.HoverTarget = 0f;
                state.AnimationTimer?.Start();
                btn.Invalidate();
            };

            btn.MouseDown += (_, e) =>
            {
                if (e.Button == MouseButtons.Left)
                {
                    state.IsDown = true;
                    btn.Invalidate();
                }
            };

            btn.MouseUp += (_, _) =>
            {
                state.IsDown = false;
                btn.Invalidate();
            };

            btn.SizeChanged += (_, _) =>
            {
                btn.Region?.Dispose();
                btn.Region = new Region(CreateRoundedRectPath(new Rectangle(0, 0, btn.Width, btn.Height), 16));
            };

            btn.Paint += (_, e) =>
            {
                if (btn.Tag is not RoundedButtonStyleState s)
                {
                    return;
                }

                e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

                var baseInset = 2;
                var inset = (int)Math.Round(baseInset * (1f - s.HoverProgress));
                if (s.IsDown) inset = Math.Max(inset, 2);

                var rect = new Rectangle(inset, inset, (btn.Width - 1) - (inset * 2), (btn.Height - 1) - (inset * 2));
                var fill = s.IsDown ? s.Down : (s.IsHover ? s.Hover : s.Back);

                using var path = CreateRoundedRectPath(rect, 16);
                using var brush = new SolidBrush(fill);

                // Soft shadow
                var shadowRect = new Rectangle(rect.X + 2, rect.Y + 3, rect.Width, rect.Height);
                using var shadowPath = CreateRoundedRectPath(shadowRect, 16);
                using var shadowBrush = new SolidBrush(Color.FromArgb(30, 0, 0, 0));
                e.Graphics.FillPath(shadowBrush, shadowPath);

                e.Graphics.FillPath(brush, path);
                using var borderPen = new Pen(s.Border, 1);
                e.Graphics.DrawPath(borderPen, path);

                // Layout Icon and Text
                var image = btn.Image;
                var text = btn.Text ?? string.Empty;
                var imageSize = 32;
                var spacing = 4; // Reduced spacing between icon and text

                // Calculate total content height to center it as a block
                var textSize = TextRenderer.MeasureText(text, btn.Font, new Size(rect.Width - 16, 100), TextFormatFlags.WordBreak);
                var totalContentHeight = (image != null ? imageSize + spacing : 0) + textSize.Height;

                // Starting Y position to center the icon+text block vertically
                var startY = rect.Y + (rect.Height - totalContentHeight) / 2;

                if (image != null)
                {
                    var imageRect = new Rectangle(
                        rect.X + (rect.Width - imageSize) / 2,
                        startY,
                        imageSize,
                        imageSize);
                    e.Graphics.DrawImage(image, imageRect);
                    startY = imageRect.Bottom + spacing;
                }

                if (!string.IsNullOrWhiteSpace(text))
                {
                    var textRect = new Rectangle(rect.X + 8, startY, rect.Width - 16, textSize.Height);
                    TextRenderer.DrawText(
                        e.Graphics,
                        text,
                        btn.Font,
                        textRect,
                        btn.ForeColor,
                        TextFormatFlags.HorizontalCenter | TextFormatFlags.Top | TextFormatFlags.WordBreak | TextFormatFlags.NoPadding);
                }
            };
        }

        private static System.Drawing.Drawing2D.GraphicsPath CreateRoundedRectPath(Rectangle rect, int radius)
        {
            var path = new System.Drawing.Drawing2D.GraphicsPath();
            var d = radius * 2;

            if (d > rect.Width)
            {
                d = rect.Width;
            }

            if (d > rect.Height)
            {
                d = rect.Height;
            }

            path.AddArc(rect.X, rect.Y, d, d, 180, 90);
            path.AddArc(rect.Right - d, rect.Y, d, d, 270, 90);
            path.AddArc(rect.Right - d, rect.Bottom - d, d, d, 0, 90);
            path.AddArc(rect.X, rect.Bottom - d, d, d, 90, 90);
            path.CloseFigure();
            return path;
        }

        private void TryLoadToolbarIconsFromAssets()
        {
            TrySetButtonIcon(btnAdd, "add.png", "add_gala.png", "calendar.png");
            TrySetButtonIcon(btnEdit, "edit.png");
            TrySetButtonIcon(btnDelete, "delete.png");
            TrySetButtonIcon(btnManageParticipants, "manage.png", "participants.png");
            TrySetButtonIcon(btnAutoEmail, "email.png", "auto_email.png", "mail.png");
        }

        private void LoadSounds()
        {
            try
            {
                // Assumes you have a file named 'hover.wav' in your Assets folder
                string soundPath = Path.Combine(AppContext.BaseDirectory, "Assets", "hover.wav");
                if (File.Exists(soundPath))
                {
                    _hoverSound.SoundLocation = soundPath;
                    _hoverSound.Load(); // Pre-load into memory for instant playback
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Could not load sound: {ex.Message}");
            }
        }

        private static void TrySetButtonIcon(Button? btn, params string[] fileNames)
        {
            if (btn == null)
            {
                return;
            }

            foreach (var fileName in fileNames)
            {
                try
                {
                    var path = Path.Combine(AppContext.BaseDirectory, "Assets", fileName);
                    if (!File.Exists(path))
                    {
                        System.Diagnostics.Debug.WriteLine($"Icon not found: {path}");
                        continue;
                    }

                    using var original = Image.FromFile(path);
                    btn.Image = new Bitmap(original, new Size(28, 28));
                    return;
                }
                catch
                {
                    // ignore icon load failures
                }
            }
        }

        private void LoadGalas()
        {
            _galas = _dataService.LoadGalas();
            RefreshGalasList();
        }

        private void RefreshGalasList()
        {
            dgvGalas.DataSource = null;
            dgvGalas.DataSource = _galas
                .OrderBy(g => g.ScheduledDate)
                .ToList();

            if (dgvGalas.Columns.Contains("colDate"))
            {
                dgvGalas.Columns["colDate"].DefaultCellStyle.Format = "yyyy-MM-dd";
            }
        }

        private void OnGridColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.ColumnIndex < 0)
            {
                return;
            }

            var column = dgvGalas.Columns[e.ColumnIndex];
            if (column == null || column.DataPropertyName != nameof(Gala.ScheduledDate))
            {
                return;
            }

            _dateSortAscending = !_dateSortAscending;
            var sorted = _dateSortAscending
                ? _galas.OrderBy(g => g.ScheduledDate).ToList()
                : _galas.OrderByDescending(g => g.ScheduledDate).ToList();

            dgvGalas.DataSource = null;
            dgvGalas.DataSource = sorted;
            dgvGalas.Columns["colDate"].DefaultCellStyle.Format = "yyyy-MM-dd";
        }

        private Gala? GetSelectedGala()
        {
            if (dgvGalas.SelectedRows.Count == 0)
            {
                return null;
            }

            var galaId = dgvGalas.SelectedRows[0].Cells["Id"].Value?.ToString();
            if (string.IsNullOrWhiteSpace(galaId))
            {
                return null;
            }

            return _galas.FirstOrDefault(g => g.Id == galaId);
        }

        private void OnAddClick(object sender, EventArgs e)
        {
            AddGala();
        }

        private void OnEditClick(object sender, EventArgs e)
        {
            var gala = GetSelectedGala();
            if (gala == null)
            {
                MessageBox.Show("Select a gala first.");
                return;
            }

            EditGala(gala);
        }

        private void OnDeleteClick(object sender, EventArgs e)
        {
            var gala = GetSelectedGala();
            if (gala == null)
            {
                MessageBox.Show("Select a gala first.");
                return;
            }

            DeleteGala(gala.Id);
        }

        private void OnGridCellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
            {
                return;
            }

            var galaId = dgvGalas.Rows[e.RowIndex].Cells["Id"].Value?.ToString();
            if (string.IsNullOrWhiteSpace(galaId))
            {
                return;
            }

            var gala = _galas.FirstOrDefault(g => g.Id == galaId);
            if (gala != null)
            {
                EditGala(gala);
            }
        }

        private void OnGridKeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Delete)
            {
                return;
            }

            var gala = GetSelectedGala();
            if (gala != null)
            {
                DeleteGala(gala.Id);
            }

            e.Handled = true;
        }

        private void AddGala()
        {
            var addForm = new AddEditGalaForm(null, _dataService, _participantsDataService);
            if (addForm.ShowDialog() == DialogResult.OK)
            {
                LoadGalas();
            }
        }

        private void EditGala(Gala gala)
        {
            var editForm = new AddEditGalaForm(gala, _dataService, _participantsDataService);
            if (editForm.ShowDialog() == DialogResult.OK)
            {
                LoadGalas();
            }
        }

        private void DeleteGala(string id)
        {
            if (MessageBox.Show("Are you sure you want to delete this gala?", "Confirm Delete", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                _dataService.DeleteGala(id);
                LoadGalas();
            }
        }

        private void OnManageParticipantsClick(object sender, EventArgs e)
        {
            using var participantsForm = new ParticipantsManagementForm(_participantsDataService, _currentHost);
            participantsForm.ShowDialog(this);
        }

        private async void btnAutoEmail_Click(object sender, EventArgs e) // Modified
        {
            var gala = GetSelectedGala();
            if (gala == null)
            {
                MessageBox.Show("Select a gala first.");
                return;
            }

            using (var confirmForm = new AutoEmailConfirmationForm(gala))
            {
                if (confirmForm.ShowDialog(this) == DialogResult.OK)
                {
                    await SendPlanEmailToParticipants(gala); //Modified: Waits to complete the process before proceeding
                }
            }
        }

        private async Task SendPlanEmailToParticipants(Gala gala)//Modified
        {
            var gmailParticipants = gala.Participants.Where(p => !string.IsNullOrWhiteSpace(p.Email) && p.Email.EndsWith("@gmail.com", StringComparison.OrdinalIgnoreCase)).ToList();
            if (!gmailParticipants.Any())
            {
                MessageBox.Show("No participants with Gmail addresses found.");
                return;
            }

            var hostName = !string.IsNullOrWhiteSpace(gala.HostName)
                ? gala.HostName
                : _currentHost?.Name ?? "Host";
            var hostEmail = _currentHost?.Email ?? string.Empty;

            string? smtpUser = Environment.GetEnvironmentVariable("GMAIL_USER");
            string? smtpPass = Environment.GetEnvironmentVariable("GMAIL_PASS");
            if (string.IsNullOrWhiteSpace(smtpUser) || string.IsNullOrWhiteSpace(smtpPass))
            {
                MessageBox.Show("GMAIL_USER or GMAIL_PASS environment variables not set.");
                return;
            }

            var smtp = new System.Net.Mail.SmtpClient("smtp.gmail.com", 587)
            {
                Credentials = new System.Net.NetworkCredential(smtpUser, smtpPass),
                EnableSsl = true
            };

            var weather = await _weatherService.GetWeatherAsync( //Get weather data
                gala.Latitude,
                gala.Longitude,
                gala.ScheduledDate
             );

            var tips = weather != null //Gives tips depending on the Weather conditions
                ? _weatherService.GetWeatherTips(weather)
                : new List<string> { "Weather data unavailable. Please check conditions manually." };

            var extraTips = BuildAdditionalTips(gala, weather);
            var allTips = tips
                .Concat(extraTips)
                .Where(t => !string.IsNullOrWhiteSpace(t))
                .Distinct()
                .ToList();

            if (allTips.Count == 0)
            {
                allTips.Add("Please check details and reach out to the host if anything changes.");
            }

            string tipSection = string.Join("\n- ", allTips);

            var weatherSummary = weather != null
                ? $"{weather.Description}, {weather.Temperature}°C, Humidity {weather.Humidity}%, Wind {weather.WindSpeed} km/h"
                : "Weather data unavailable.";


            foreach (var participant in gmailParticipants)
            {
                try
                {
                    var mail = new System.Net.Mail.MailMessage(smtpUser, participant.Email)
                    {
                        Subject = $"Gala Plan: {gala.Name}",
                        Body = $"Tara na, {participant.Name}!\n" +
                        $"Things are about to get exciting! Your upcoming Gala is just around the corner!\n" +
                        $"Here are the Gala Details:\n\n" +
                        $"GALA DETAILS\n" +
                        $"Name: {gala.Name}\n" +
                        $"Date: {gala.ScheduledDate:yyyy-MM-dd}\n" +
                        $"Location: {gala.Location}\n" +
                        $"Plan: {gala.Plan}\n\n" +
                        $"HOST\n" +
                        $"Organizer: {hostName}\n" +
                        (string.IsNullOrWhiteSpace(hostEmail) ? string.Empty : $"Contact: {hostEmail}\n") +
                        $"\n" +
                        $"WEATHER\n" +
                        $"Forecast: {weatherSummary}\n\n" +
                        $"RECOMMENDATIONS\n" +
                        $"- {tipSection}\n\n" +
                        $"If you have any questions or updates, please reach out to the host.\n" +
                        $"\nAno G? Tara!",
                    };
                    smtp.Send(mail);
                }
                catch (Exception ex)
                {
                    string errorDetails = $"Failed to send email to {participant.Email}:\n{ex.Message}\n\n{ex.StackTrace}";
                    MessageBox.Show(errorDetails, "SMTP Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Console.WriteLine(errorDetails);
                }
            }

            MessageBox.Show("Plan details sent to all participants' Gmail addresses.");
        }

        private List<string> BuildAdditionalTips(Gala gala, WeatherData? weather)
        {
            var tips = new List<string>();

            if (!string.IsNullOrWhiteSpace(gala.Location))
            {
                tips.Add($"Confirm the meeting point around {gala.Location}.");
            }

            if (gala.LocationItems != null && gala.LocationItems.Count > 0)
            {
                var suggested = gala.LocationItems
                    .Select(l => l.Name)
                    .Where(n => !string.IsNullOrWhiteSpace(n))
                    .Distinct()
                    .Take(3)
                    .ToList();

                if (suggested.Count > 0)
                {
                    tips.Add($"Suggested nearby spots: {string.Join(", ", suggested)}.");
                }
            }

            if (gala.Participants != null && gala.Participants.Count >= 8)
            {
                tips.Add("Consider coordinating transport or carpooling for a smoother arrival.");
            }

            if (gala.Participants == null || gala.Participants.Count == 0)
            {
                tips.Add("Invite participants so everyone receives the latest plan.");
            }

            if (weather != null && weather.Description.Contains("rain", StringComparison.OrdinalIgnoreCase))
            {
                tips.Add("Plan a covered meet-up spot in case of rain.");
            }

            if (weather != null && weather.Description.Contains("clear", StringComparison.OrdinalIgnoreCase))
            {
                tips.Add("If outdoors, bring sun protection and water.");
            }

            return tips;
        }

        private void dgvGalas_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
