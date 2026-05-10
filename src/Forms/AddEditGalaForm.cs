using G_Tara.Models;
using G_Tara.Services;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing.Drawing2D;
using System.Xml.Linq;

namespace G_Tara
{
    public partial class AddEditGalaForm : Form
    {
        private Gala _gala;
        private readonly GalaDataService _dataService;
        private readonly WeatherService _weatherService;
        private readonly LocationService _locationService;
        private readonly ParticipantsDataService _participantsDataService;
        private readonly List<LocationItem> _selectedLocations;
        private List<Participant> _availableParticipants;
        private List<Participant> _selectedParticipants;
        private double _selectedLatitude;
        private double _selectedLongitude;
        private DateTime _selectedDate;
        private DateTime _rangeStart;
        private DateTime _rangeEnd;
        private readonly Label _lblCoordinates = new Label { AutoSize = true, Margin = new Padding(3, 7, 3, 3) };
        private FlowLayoutPanel? _participantCardsPanel;
        private PictureBox _picStatus = new PictureBox { SizeMode = PictureBoxSizeMode.Zoom, Size = new Size(26, 26) };
        private PictureBox _picLocation = new PictureBox { SizeMode = PictureBoxSizeMode.Zoom, Size = new Size(26, 26) };
        private PictureBox _picCategory = new PictureBox { SizeMode = PictureBoxSizeMode.Zoom, Size = new Size(26, 26) };
        private Panel? _categoryPill;

        public AddEditGalaForm(Gala? gala, GalaDataService dataService, ParticipantsDataService? participantsDataService = null)
        {
            InitializeComponent();
            ApplyGaraTheme();
            WireUiEvents();
            ApplyRoundedFormRegion();
            _gala = gala ?? new Gala();
            _dataService = dataService;
            _participantsDataService = participantsDataService ?? new ParticipantsDataService();
            _weatherService = new WeatherService();
            _locationService = new LocationService();
            _selectedLocations = new List<LocationItem>(_gala.LocationItems);
            _availableParticipants = new List<Participant>();
            _selectedParticipants = new List<Participant>(_gala.Participants);
        }

        private void CreateCustomTitleBar()
        {
            Color titleBarColor = Color.FromArgb(241, 206, 211);

            pnlTitleBar = new Panel
            {
                Dock = DockStyle.Top,
                Height = 34,
                BackColor = titleBarColor
            };
            pnlTitleBar.MouseDown += OnTitleBarMouseDown;

            lblWindowTitle = new Label
            {
                Text = "ADD/EDIT GALA DETAILS",
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
                Location = new Point(this.ClientSize.Width - 74, 2),
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
                Location = new Point(this.ClientSize.Width - 44, 2),
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
            pnlTitleBar.BringToFront();

            this.Resize += (sender, args) =>
            {
                if (btnWindowMinimize != null && btnWindowClose != null && pnlTitleBar != null)
                {
                    btnWindowClose.Location = new Point(pnlTitleBar.Width - btnWindowClose.Width - 8, 2);
                    btnWindowMinimize.Location = new Point(btnWindowClose.Left - btnWindowMinimize.Width - 8, 2);
                }
            };
        }

        private static GraphicsPath CreateRoundedRectPath(Rectangle rect, int radius)
        {
            var path = new GraphicsPath();
            int d = radius * 2;
            path.AddArc(rect.X, rect.Y, d, d, 180, 90);
            path.AddArc(rect.Right - d, rect.Y, d, d, 270, 90);
            path.AddArc(rect.Right - d, rect.Bottom - d, d, d, 0, 90);
            path.AddArc(rect.X, rect.Bottom - d, d, d, 90, 90);
            path.CloseFigure();
            return path;
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
            SendMessage(this.Handle, WM_NCLBUTTONDOWN, (IntPtr)HTCAPTION, IntPtr.Zero);
        }

        private void ApplyGaraTheme()
        {
            Color btnBack = Color.FromArgb(248, 230, 231);
            Color btnBorder = Color.FromArgb(214, 147, 156);
            Color btnHover = Color.FromArgb(235, 200, 210);
            Color btnDown = Color.FromArgb(214, 147, 156);

            System.Media.SoundPlayer? hoverSound = null;
            try
            {
                string hoverSoundPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "assets", "hover.wav");
                if (File.Exists(hoverSoundPath))
                    hoverSound = new System.Media.SoundPlayer(hoverSoundPath);
            }
            catch { }

            void ApplyToAll(Control.ControlCollection controls)
            {
                foreach (Control c in controls)
                {
                    if (c is Button btn)
                        ApplyRoundedButtonStyle(btn, btnBack, btnBorder, btnHover, btnDown, hoverSound!);
                    else if (c.HasChildren)
                        ApplyToAll(c.Controls);
                }
            }

            ApplyToAll(this.Controls);
        }

        private void LogWeatherCoordinates(string stage)
        {
            Debug.WriteLine($"[GetWeather:{stage}] lat={_selectedLatitude}, lon={_selectedLongitude}, location='{txtLocation.Text}'");
        }

        private void WireUiEvents()
        {
            btnPickMap.Click -= OnPickMapLocation;
            btnPickMap.Click += OnPickMapLocation;
            btnGetWeather.Click -= OnGetWeather;
            btnGetWeather.Click += OnGetWeather;
            btnRefreshPlaces.Click -= OnRefreshPlaces;
            btnRefreshPlaces.Click += OnRefreshPlaces;
            btnAdd.Click -= OnAddLocation;
            btnAdd.Click += OnAddLocation;
            btnRemove.Click -= OnRemoveLocation;
            btnRemove.Click += OnRemoveLocation;
            chkParticipants.ItemCheck -= OnParticipantToggled;
            chkParticipants.ItemCheck += OnParticipantToggled;
            btnSave.Click -= OnSave;
            btnSave.Click += OnSave;
            btnCancel.Click -= OnCancel;
            btnCancel.Click += OnCancel;
            btnPickRange.Click -= OnPickDateRange;
            btnPickRange.Click += OnPickDateRange;
            lstSearchResults.DoubleClick -= OnLocationDoubleClick;
            lstSearchResults.DoubleClick += OnLocationDoubleClick;
            lstSelectedLocations.DoubleClick -= OnLocationDoubleClick;
            lstSelectedLocations.DoubleClick += OnLocationDoubleClick;
        }

        private async void OnPickMapLocation(object? sender, EventArgs e)
        {
            var initialLat = _selectedLatitude;
            var initialLon = _selectedLongitude;

            if ((initialLat == 0 || initialLon == 0) && !string.IsNullOrWhiteSpace(txtLocation.Text))
            {
                var (lat, lon) = await _locationService.GeocodeLocationAsync(txtLocation.Text.Trim());
                if (lat != 0 || lon != 0)
                {
                    initialLat = lat;
                    initialLon = lon;
                }
            }

            if (initialLat == 0 || initialLon == 0)
            {
                initialLat = 14.5995;
                initialLon = 120.9842;
            }

            using var picker = new MapPickerForm(initialLat, initialLon);
            if (picker.ShowDialog(this) != DialogResult.OK)
                return;

            _selectedLatitude = picker.SelectedLatitude;
            _selectedLongitude = picker.SelectedLongitude;
            UpdateCoordinatesLabel();
            Debug.WriteLine($"[MapPin:selected] lat={_selectedLatitude}, lon={_selectedLongitude}");
            await UpdateWeatherSilentlyAsync();
        }

        private void AddEditGalaForm_Load(object sender, EventArgs e)
        {
            InitializeControls();
            this.SizeChanged += (s, _) => ApplyRoundedFormRegion();
        }

        private void InitializeControls()
        {
            ApplyTargetLayoutAndTheme();

            if (cmbCategory.Items.Count > 0 && cmbCategory.SelectedIndex < 0)
                cmbCategory.SelectedIndex = 0;

            _selectedLatitude = _gala.Latitude;
            _selectedLongitude = _gala.Longitude;

            if (lblName.Parent != null && !lblName.Parent.Controls.Contains(_lblCoordinates))
                lblName.Parent.Controls.Add(_lblCoordinates);

            UpdateCoordinatesLabel();

            txtName.Text = _gala.Name;
            var initialDate = _gala.ScheduledDate == default ? DateTime.Now : _gala.ScheduledDate;
            _selectedDate = initialDate.Date;
            _gala.ScheduledDate = _selectedDate;
            SetDateRange(
                _gala.RangeStartDate == default ? initialDate : _gala.RangeStartDate,
                _gala.RangeEndDate == default ? initialDate : _gala.RangeEndDate,
                refreshParticipants: false,
                updateWeather: false);
            txtLocation.Text = _gala.Location;
            if (cmbStatus.Items.Count > 0)
            {
                var index = cmbStatus.Items.IndexOf(_gala.Status);
                cmbStatus.SelectedIndex = index >= 0 ? index : 0;
            }
            txtPlan.Text = _gala.Plan;
            if (_gala.Weather != null)
            {
                lblWeather.Text = $"{_gala.Weather.Description}, {_gala.Weather.Temperature}°C, Humidity: {_gala.Weather.Humidity}%, Wind: {_gala.Weather.WindSpeed} km/h";
            }

            RefreshSelectedLocationsList();
            LoadParticipantsForGala();
            EnableParticipantControls();
            EnsureParticipantCardsSurface();

            WrapTextBoxWithRoundedPanel(txtName, Color.FromArgb(248, 232, 235), Color.FromArgb(214, 147, 156), 10);
            StylePillButtonWithDivider(btnPickRange);
            StyleDarkPillButton(btnPickMap);
        }

        private void EnableParticipantControls()
        {
            chkParticipants.Enabled = true;
        }

        private void UpdateCoordinatesLabel()
        {
            if (_selectedLatitude != 0 || _selectedLongitude != 0)
                _lblCoordinates.Text = $"Lat: {_selectedLatitude:F6}, Lon: {_selectedLongitude:F6}";
            else
                _lblCoordinates.Text = "No coordinates selected";
        }

        private async void OnSearchLocation(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtLocation.Text))
            {
                MessageBox.Show("Please enter a location first.");
                return;
            }

            var (lat, lon) = await _locationService.GeocodeLocationAsync(txtLocation.Text);
            if (lat == 0 && lon == 0)
            {
                MessageBox.Show("Location not found.");
                return;
            }

            _selectedLatitude = lat;
            _selectedLongitude = lon;
            UpdateCoordinatesLabel();
            await RefreshPlacesListAsync();
            await UpdateWeatherSilentlyAsync();
        }

        private async void OnRefreshPlaces(object? sender, EventArgs e)
        {
            await RefreshPlacesListAsync();
        }

        private async Task RefreshPlacesListAsync()
        {
            if (_selectedLatitude == 0 || _selectedLongitude == 0)
            {
                MessageBox.Show("Please pin a location on the map first.");
                return;
            }

            btnRefreshPlaces.Enabled = false;
            btnRefreshPlaces.Text = "Loading...";
            lstSearchResults.DataSource = null;
            lstSearchResults.Items.Clear();
            lstSearchResults.Items.Add("Loading locations...");

            var category = cmbCategory.SelectedItem?.ToString() ?? "Hotel";
            var locations = await _locationService.SearchLocationsAsync(_selectedLatitude, _selectedLongitude, category);

            lstSearchResults.Items.Clear();
            lstSearchResults.DisplayMember = "Name";
            lstSearchResults.ValueMember = "Id";
            lstSearchResults.DataSource = locations;

            btnRefreshPlaces.Enabled = true;
            btnRefreshPlaces.Text = "Refresh Locations";
        }

        private void OnPickDateRange(object? sender, EventArgs e)
        {
            var initialDates = BuildRangeDates(_rangeStart, _rangeEnd);
            using var picker = new CalendarPickerForm(initialDates: initialDates, multiSelect: true);
            if (picker.ShowDialog(this) != DialogResult.OK)
                return;

            if (picker.SelectedDates.Count > 0)
            {
                var start = picker.SelectedDates.Min().Date;
                var end = picker.SelectedDates.Max().Date;
                SetDateRange(start, end);
            }
        }

        private void SetDateRange(DateTime start, DateTime end, bool refreshParticipants = true, bool updateWeather = true)
        {
            if (end < start)
            {
                var temp = start;
                start = end;
                end = temp;
            }

            _rangeStart = start.Date;
            _rangeEnd = end.Date;
            _selectedDate = _rangeStart;
            _gala.RangeStartDate = _rangeStart;
            _gala.RangeEndDate = _rangeEnd;
            _gala.ScheduledDate = _rangeStart;
            txtDateRange.Text = _rangeStart == _rangeEnd
                ? _rangeStart.ToString("yyyy-MM-dd")
                : $"{_rangeStart:yyyy-MM-dd} to {_rangeEnd:yyyy-MM-dd}";

            if (refreshParticipants)
                RefreshParticipantsUI();

            if (updateWeather)
                _ = UpdateWeatherSilentlyAsync();
        }

        private static List<DateTime> BuildRangeDates(DateTime start, DateTime end)
        {
            var dates = new List<DateTime>();
            if (start == default || end == default)
                return dates;

            var cursor = start <= end ? start.Date : end.Date;
            var last = start <= end ? end.Date : start.Date;
            while (cursor <= last)
            {
                dates.Add(cursor);
                cursor = cursor.AddDays(1);
            }

            return dates;
        }

        private async Task UpdateWeatherSilentlyAsync()
        {
            if (_selectedLatitude == 0 || _selectedLongitude == 0) return;

            LogWeatherCoordinates("request_silent");
            lblWeather.Text = "Loading weather...";
            var weather = await _weatherService.GetWeatherAsync(_selectedLatitude, _selectedLongitude, _selectedDate);
            if (weather != null)
            {
                _gala.Weather = weather;
                lblWeather.Text = $"{weather.Description}, {weather.Temperature}°C, Humidity: {weather.Humidity}%, Wind: {weather.WindSpeed} km/h";
            }
            else
            {
                lblWeather.Text = "Weather data unavailable.";
            }
        }

        private async void OnGetWeather(object sender, EventArgs e)
        {
            LogWeatherCoordinates("clicked");

            if (_selectedLatitude == 0 || _selectedLongitude == 0)
            {
                if (string.IsNullOrWhiteSpace(txtLocation.Text))
                {
                    MessageBox.Show("Please pin a location on the map or search for one first.");
                    return;
                }

                var (lat, lon) = await _locationService.GeocodeLocationAsync(txtLocation.Text);
                if (lat == 0 && lon == 0)
                {
                    MessageBox.Show("Location not found.");
                    return;
                }

                _selectedLatitude = lat;
                _selectedLongitude = lon;
                UpdateCoordinatesLabel();
                LogWeatherCoordinates("geocoded");
            }

            LogWeatherCoordinates("request");
            lblWeather.Text = "Loading weather...";
            var weather = await _weatherService.GetWeatherAsync(_selectedLatitude, _selectedLongitude, _selectedDate);
            if (weather != null)
            {
                _gala.Weather = weather;
                lblWeather.Text = $"{weather.Description}, {weather.Temperature}°C, Humidity: {weather.Humidity}%, Wind: {weather.WindSpeed} km/h";
            }
            else
            {
                lblWeather.Text = "Weather data unavailable.";
                MessageBox.Show("Unable to retrieve weather data. The date may be out of range for the API or the API may not be reachable.", "Weather API Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void OnLocationDoubleClick(object? sender, EventArgs e)
        {
            if (sender is ListBox listBox && listBox.SelectedItem is LocationItem location)
            {
                if (location.Latitude != 0 && location.Longitude != 0)
                {
                    var lat = location.Latitude.ToString(System.Globalization.CultureInfo.InvariantCulture);
                    var lon = location.Longitude.ToString(System.Globalization.CultureInfo.InvariantCulture);
                    var url = $"https://www.google.com/maps/search/?api=1&query={lat},{lon}";
                    try
                    {
                        Process.Start(new ProcessStartInfo { FileName = url, UseShellExecute = true });
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Unable to open maps: {ex.Message}", "Map Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else if (location.Name != "Cant find anything" && !location.Name.StartsWith("Try again"))
                {
                    MessageBox.Show("Location coordinates not available.");
                }
            }
        }

        private void OnAddLocation(object sender, EventArgs e)
        {
            if (lstSearchResults.SelectedItem is LocationItem location)
            {
                if (!_selectedLocations.Any(l => l.Id == location.Id))
                {
                    _selectedLocations.Add(location);
                    RefreshSelectedLocationsList();
                }
            }
        }

        private void OnRemoveLocation(object sender, EventArgs e)
        {
            if (lstSelectedLocations.SelectedItem is LocationItem location)
            {
                _selectedLocations.Remove(location);
                RefreshSelectedLocationsList();
            }
        }

        private void RefreshSelectedLocationsList()
        {
            lstSelectedLocations.DataSource = null;
            lstSelectedLocations.DisplayMember = "Name";
            lstSelectedLocations.ValueMember = "Id";
            lstSelectedLocations.DataSource = new List<LocationItem>(_selectedLocations);
        }

        private void LoadParticipantsForGala()
        {
            _gala.ScheduledDate = _selectedDate;
            _availableParticipants = _participantsDataService.LoadParticipants();
            RefreshHostOptions();
            RefreshParticipantsUI();
        }

        private void RefreshHostOptions()
        {
            var selectedId = (cmbHost.SelectedItem as Participant)?.Id;

            cmbHost.DataSource = null;
            cmbHost.DisplayMember = "DisplayName";
            cmbHost.ValueMember = "Id";
            cmbHost.DataSource = new List<Participant>(_availableParticipants);

            Participant? toSelect = null;
            if (!string.IsNullOrWhiteSpace(_gala.HostName))
            {
                toSelect = _availableParticipants.FirstOrDefault(p =>
                    string.Equals(p.Name, _gala.HostName, StringComparison.OrdinalIgnoreCase));
            }

            if (toSelect == null && !string.IsNullOrWhiteSpace(selectedId))
                toSelect = _availableParticipants.FirstOrDefault(p => p.Id == selectedId);

            if (toSelect != null)
                cmbHost.SelectedItem = toSelect;
            else if (cmbHost.Items.Count > 0)
                cmbHost.SelectedIndex = 0;
        }

        private void RefreshParticipantsUI()
        {
            var availableForDate = _availableParticipants.ToList();

            chkParticipants.Items.Clear();
            foreach (var p in availableForDate)
            {
                bool isSelected = _selectedParticipants.Any(sp => sp.Id == p.Id);
                chkParticipants.Items.Add(p, isSelected);
            }
            chkParticipants.DisplayMember = "Name";
            RenderParticipantCards(availableForDate);
        }

        private void EnsureParticipantCardsSurface()
        {
            if (_participantCardsPanel != null)
                return;

            var parent = chkParticipants.Parent;
            if (parent == null)
                return;

            _participantCardsPanel = new FlowLayoutPanel
            {
                Name = "participantCardsPanel",
                Location = chkParticipants.Location,
                Size = chkParticipants.Size,
                AutoScroll = true,
                WrapContents = true,
                FlowDirection = FlowDirection.LeftToRight,
                Padding = new Padding(6),
                BackColor = Color.FromArgb(255, 248, 248)
            };

            parent.Controls.Add(_participantCardsPanel);
            _participantCardsPanel.BringToFront();
            chkParticipants.Visible = false;
        }

        private void RenderParticipantCards(List<Participant> participants)
        {
            EnsureParticipantCardsSurface();
            if (_participantCardsPanel == null)
                return;

            _participantCardsPanel.SuspendLayout();
            _participantCardsPanel.Controls.Clear();

            int cardWidth = 82 + 12;
            int availableWidth = _participantCardsPanel.Width - _participantCardsPanel.Padding.Horizontal;
            if (availableWidth > 0 && participants.Count > 0)
            {
                int cardsPerRow = Math.Max(1, availableWidth / cardWidth);
                int actualCardsInRow = Math.Min(participants.Count, cardsPerRow);
                int rowWidth = actualCardsInRow * cardWidth;
                int leftPadding = (_participantCardsPanel.Width - rowWidth) / 2;
                _participantCardsPanel.Padding = new Padding(Math.Max(6, leftPadding), 6, 6, 6);
            }

            foreach (var participant in participants)
            {
                bool isSelected = _selectedParticipants.Any(sp => sp.Id == participant.Id);
                _participantCardsPanel.Controls.Add(CreateParticipantCard(participant, isSelected));
            }

            _participantCardsPanel.ResumeLayout();
        }

        private Control CreateParticipantCard(Participant participant, bool isSelected)
        {
            bool isAvailable = participant.IsAvailableOn(_selectedDate);
            Color darkOrange = Color.DarkOrange;

            var card = new Panel
            {
                Size = new Size(82, 122),
                Margin = new Padding(6, 4, 6, 4),
                BackColor = Color.FromArgb(252, 241, 243),
                Cursor = Cursors.Hand
            };

            var avatar = new ParticipantAvatar
            {
                Size = new Size(52, 52),
                Location = new Point(15, 4),
                BorderColorStart = !isAvailable ? darkOrange : Color.FromArgb(214, 147, 156),
                BorderColorEnd = !isAvailable ? Color.Orange : Color.FromArgb(255, 220, 226),
                Image = !string.IsNullOrWhiteSpace(participant.ImagePath) && File.Exists(participant.ImagePath)
                    ? Image.FromFile(participant.ImagePath)
                    : null
            };

            var nameLabel = new Label
            {
                Text = participant.Name,
                Location = new Point(3, 58),
                Size = new Size(76, 30),
                TextAlign = ContentAlignment.TopCenter,
                Font = new Font("Segoe UI", 6.9F, FontStyle.Bold),
                ForeColor = Color.FromArgb(90, 50, 55),
                BackColor = Color.Transparent
            };

            var statusLabel = new Label
            {
                Text = !isAvailable ? "Unavailable" : (isSelected ? "Confirmed" : "Pending"),
                Location = new Point(15, 92),
                Size = new Size(60, 16),
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Segoe UI", 6.7F, FontStyle.Bold),
                ForeColor = !isAvailable ? darkOrange : (isSelected ? Color.FromArgb(54, 120, 70) : Color.FromArgb(165, 110, 55)),
                BackColor = Color.Transparent
            };

            var statusDot = new Panel
            {
                Size = new Size(8, 8),
                Location = new Point(7, 96),
                BackColor = !isAvailable ? darkOrange : (isSelected ? Color.FromArgb(74, 164, 95) : Color.FromArgb(224, 158, 87))
            };
            statusDot.Region = new Region(FormUtilities.CreateRoundedRectPath(new Rectangle(0, 0, statusDot.Width, statusDot.Height), 4));

            card.Controls.Add(avatar);
            card.Controls.Add(nameLabel);
            card.Controls.Add(statusLabel);
            card.Controls.Add(statusDot);

            void Toggle(object? s, EventArgs e)
            {
                if (isSelected)
                    _selectedParticipants.RemoveAll(p => p.Id == participant.Id);
                else
                {
                    if (!_selectedParticipants.Any(p => p.Id == participant.Id))
                        _selectedParticipants.Add(participant);
                }

                RefreshParticipantsUI();
            }

            card.Click += Toggle;
            avatar.Click += Toggle;
            nameLabel.Click += Toggle;
            statusLabel.Click += Toggle;
            statusDot.Click += Toggle;

            card.Paint += (s, e) =>
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                var borderRect = new Rectangle(1, 1, card.Width - 3, card.Height - 3);
                using var path = FormUtilities.CreateRoundedRectPath(borderRect, 14);
                using var fillBrush = new SolidBrush(Color.FromArgb(255, 248, 249));
                e.Graphics.FillPath(fillBrush, path);
                
                Color borderColor = !isAvailable ? darkOrange : (isSelected ? Color.FromArgb(104, 179, 123) : Color.FromArgb(214, 188, 191));
                float borderWidth = !isAvailable ? 1.5f : (isSelected ? 1.8f : 1.2f);
                
                using var pen = new Pen(borderColor, borderWidth);
                e.Graphics.DrawPath(pen, path);
            };

            return card;
        }

        private void ApplyTargetLayoutAndTheme()
        {
            Color bg = Color.FromArgb(243, 198, 208);
            Color card = Color.FromArgb(249, 230, 235);
            Color text = Color.FromArgb(94, 49, 58);
            Color deep = Color.FromArgb(197, 125, 143);

            BackColor = bg;
            Font = new Font("Segoe UI", 8.75F, FontStyle.Regular);
            ClientSize = new Size(920, 640);

            Panel? leftPanel = lblName.Parent as Panel;
            Panel? participantsPanel = lblParticipants.Parent as Panel;
            Panel? planPanel = lblPlan.Parent as Panel;
            Panel? rightPanel = participantsPanel?.Parent as Panel;

            if (container != null)
            {
                container.BackColor = bg;
                container.RowStyles[1].Height = 64;
            }

            if (mainPanel != null)
            {
                mainPanel.Padding = new Padding(14, 10, 14, 8);
                mainPanel.ColumnStyles[0].Width = 54f;
                mainPanel.ColumnStyles[1].Width = 46f;
            }

            if (leftPanel != null)
                leftPanel.BackColor = bg;

            if (rightPanel != null)
            {
                rightPanel.BackColor = bg;
                rightPanel.Padding = new Padding(10, 0, 0, 0);
            }

            if (picLogo != null) picLogo.Visible = false;

            lblName.Location = new Point(12, 10);
            lblName.Font = new Font("Segoe UI Semibold", 9.2F);
            lblName.ForeColor = text;
            txtName.Location = new Point(12, 30);
            txtName.Size = new Size(440, 28);
            txtName.BackColor = Color.FromArgb(248, 232, 235);

            // EVENT DATE row — horizontal: icon | label | [rounded textbox wrapper] | pill button
            picCalendar.Visible = true;
            picCalendar.Size = new Size(26, 26);
            picCalendar.Location = new Point(12, 80);
            picCalendar.SizeMode = PictureBoxSizeMode.Zoom;

            try
            {
                string iconPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "assets", "event.png");
                if (File.Exists(iconPath))
                    picCalendar.Image = Image.FromFile(iconPath);
            }
            catch { }

            lblDate.Location = new Point(46, 84);
            lblDate.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold);
            lblDate.ForeColor = text;
            lblDate.AutoSize = true;

            datePanel.Location = new Point(140, 74);
            datePanel.Size = new Size(300, 32);
            datePanel.BackColor = Color.Transparent;

            txtDateRange.Location = new Point(4, 5);
            txtDateRange.Size = new Size(158, 22);
            txtDateRange.BorderStyle = BorderStyle.None;
            txtDateRange.BackColor = Color.FromArgb(248, 232, 235);
            txtDateRange.TextAlign = HorizontalAlignment.Center;

            btnPickRange.Location = new Point(175, 1);
            btnPickRange.Size = new Size(124, 30);

            datePanel.Paint -= OnDatePanelPaint;
            datePanel.Paint += OnDatePanelPaint;

            // STATUS row — horizontal: icon | label | colored pill dropdown
            _picStatus.Location = new Point(12, 118);
            _picStatus.BackColor = Color.Transparent;
            try
            {
                string iconPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "assets", "status.png");
                if (File.Exists(iconPath))
                    _picStatus.Image = Image.FromFile(iconPath);
            }
            catch { }

            if (lblName.Parent != null && !lblName.Parent.Controls.Contains(_picStatus))
                lblName.Parent.Controls.Add(_picStatus);

            lblStatus.Location = new Point(44, 123);
            lblStatus.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold);
            lblStatus.ForeColor = text;
            lblStatus.AutoSize = true;

            cmbStatus.Visible = false;
            cmbStatus.Location = new Point(-500, -500);
            cmbStatus.Size = new Size(200, 24);

            BuildStatusPill(lblName.Parent, text);

            // LOCATION PIN row — horizontal: icon | label | rounded textbox | pill button
            _picLocation.Location = new Point(12, 172);
            _picLocation.BackColor = Color.Transparent;
            try
            {
                string iconPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "assets", "location.png");
                if (File.Exists(iconPath))
                    _picLocation.Image = Image.FromFile(iconPath);
            }
            catch { }

            if (lblName.Parent != null && !lblName.Parent.Controls.Contains(_picLocation))
                lblName.Parent.Controls.Add(_picLocation);

            lblLocation.Location = new Point(44, 177);
            lblLocation.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold);
            lblLocation.ForeColor = text;
            lblLocation.AutoSize = true;

            locationPanel.Location = new Point(140, 166);
            locationPanel.Size = new Size(320, 36);
            locationPanel.BackColor = Color.Transparent;

            txtLocation.Location = new Point(8, 7);
            txtLocation.Size = new Size(152, 22);
            txtLocation.BorderStyle = BorderStyle.None;
            txtLocation.Margin = new Padding(5, 10, 0, 0);
            txtLocation.BackColor = Color.FromArgb(248, 232, 235);
            txtLocation.Font = new Font("Segoe UI", 8.75F);

            btnPickMap.Location = new Point(196, 3);
            btnPickMap.Size = new Size(110, 30);
            btnPickMap.Margin = new Padding(13, 2, 0, 0);

            locationPanel.Paint -= OnLocationPanelPaint;
            locationPanel.Paint += OnLocationPanelPaint;

            _lblCoordinates.ForeColor = Color.FromArgb(44, 128, 76);
            _lblCoordinates.Font = new Font("Segoe UI", 8.2F, FontStyle.Bold);
            _lblCoordinates.Location = new Point(140, 206);

            // CATEGORY row — horizontal: icon | label | rounded pill showing selected category
            _picCategory.Location = new Point(12, 234);
            _picCategory.BackColor = Color.Transparent;
            try
            {
                string iconPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "assets", "category.png");
                if (File.Exists(iconPath))
                    _picCategory.Image = Image.FromFile(iconPath);
            }
            catch { }

            if (lblName.Parent != null && !lblName.Parent.Controls.Contains(_picCategory))
                lblName.Parent.Controls.Add(_picCategory);

            lblCategory.Location = new Point(44, 239);
            lblCategory.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold);
            lblCategory.ForeColor = text;
            lblCategory.AutoSize = true;

            cmbCategory.Visible = false;
            cmbCategory.Location = new Point(-500, -500);
            cmbCategory.Size = new Size(176, 24);

            BuildCategoryPill(lblName.Parent, text);

            // SEARCH & DISCOVER LOCATIONS card
            lblSearchResults.Text = "SEARCH & DISCOVER LOCATIONS";
            lblSearchResults.Location = new Point(12, 290);
            lblSearchResults.Font = new Font("Segoe UI Semibold", 8.5F, FontStyle.Bold);
            lblSearchResults.ForeColor = text;

            btnRefreshPlaces.Location = new Point(12, 315);
            btnRefreshPlaces.Size = new Size(140, 28);
            btnRefreshPlaces.Text = "Refresh Locations";

            lstSearchResults.Location = new Point(12, 350);
            lstSearchResults.Size = new Size(420, 90);
            lstSearchResults.BackColor = Color.FromArgb(255, 246, 248);
            lstSearchResults.BorderStyle = BorderStyle.FixedSingle;

            lblSelected.Location = new Point(12, 450);
            lblSelected.Font = new Font("Segoe UI Semibold", 8.6F, FontStyle.Bold);
            lblSelected.ForeColor = text;

            lstSelectedLocations.Location = new Point(12, 470);
            lstSelectedLocations.Size = new Size(420, 80);
            lstSelectedLocations.BackColor = Color.FromArgb(255, 246, 248);
            lstSelectedLocations.BorderStyle = BorderStyle.FixedSingle;

            int btnY = 560;
            int btnW = 120;
            int gap = 14;
            int totalBtnsWidth = btnW * 2 + gap;
            int btnStartX = (420 - totalBtnsWidth) / 2 + 12;

            btnAdd.Location = new Point(btnStartX, btnY);
            btnAdd.Size = new Size(btnW, 28);
            btnAdd.Text = "Add Location";

            btnRemove.Location = new Point(btnStartX + btnW + gap, btnY);
            btnRemove.Size = new Size(btnW, 28);
            btnRemove.Text = "Remove Location";

            lblWeatherTitle.Location = new Point(12, 600);
            lblWeatherTitle.Font = new Font("Segoe UI Semibold", 8.8F, FontStyle.Bold);
            lblWeatherTitle.ForeColor = text;

            weatherPanel.Location = new Point(12, 620);
            weatherPanel.Size = new Size(430, 34);
            weatherPanel.WrapContents = false;

            btnGetWeather.Size = new Size(120, 28);
            btnGetWeather.Margin = new Padding(0, 3, 8, 0);

            lblWeather.Font = new Font("Segoe UI", 8.2F);
            lblWeather.ForeColor = text;
            lblWeather.AutoSize = true;
            lblWeather.Margin = new Padding(0, 8, 0, 0);

            lblHost.Location = new Point(130, 12);
            lblHost.Font = new Font("Segoe UI Semibold", 8.9F);
            lblHost.ForeColor = text;
            BuildHostPill(participantsPanel, text);

            if (participantsPanel != null)
            {
                participantsPanel.BackColor = card;
                participantsPanel.Size = new Size(370, 310);
                participantsPanel.Location = new Point(10, 10);
                participantsPanel.Region = new Region(FormUtilities.CreateRoundedRectPath(new Rectangle(0, 0, participantsPanel.Width, participantsPanel.Height), 14));
            }
            lblParticipants.Font = new Font("Segoe UI Semibold", 9F);
            lblParticipants.ForeColor = text;
            lblParticipants.Location = new Point(14, 12);

            chkParticipants.Location = new Point(8, 36);
            chkParticipants.Size = new Size(352, 262);

            if (planPanel != null)
            {
                planPanel.BackColor = card;
                planPanel.Size = new Size(370, 200);
                planPanel.Location = new Point(10, 328);
                planPanel.Region = new Region(FormUtilities.CreateRoundedRectPath(new Rectangle(0, 0, planPanel.Width, planPanel.Height), 14));
            }
            lblPlan.Location = new Point(14, 12);
            lblPlan.Font = new Font("Segoe UI Semibold", 9F);
            lblPlan.ForeColor = text;
            txtPlan.Location = new Point(14, 34);
            txtPlan.Size = new Size(340, 148);
            txtPlan.BackColor = Color.FromArgb(255, 246, 248);

            if (_participantCardsPanel != null)
            {
                _participantCardsPanel.Location = chkParticipants.Location;
                _participantCardsPanel.Size = chkParticipants.Size;
                _participantCardsPanel.BackColor = Color.FromArgb(252, 241, 243);
            }

            buttonPanel.BackColor = deep;
            buttonPanel.Padding = new Padding(24, 12, 0, 0);
            buttonPanel.FlowDirection = FlowDirection.LeftToRight;
            buttonPanel.WrapContents = false;

            btnSave.Size = new Size(88, 30);
            btnCancel.Size = new Size(88, 30);
            btnDelete.Size = new Size(88, 30);
            btnSave.Font = new Font("Segoe UI Semibold", 8.8F, FontStyle.Bold);
            btnCancel.Font = new Font("Segoe UI Semibold", 8.8F, FontStyle.Bold);
            btnDelete.Font = new Font("Segoe UI Semibold", 8.8F, FontStyle.Bold);
            btnDelete.ForeColor = Color.FromArgb(169, 70, 82);
            btnSave.Margin = new Padding(4, 0, 10, 0);
            btnCancel.Margin = new Padding(0, 0, 10, 0);
            btnDelete.Margin = new Padding(0, 0, 0, 0);

            cmbStatus.Visible = false;
            cmbStatus.Location = new Point(-500, -500);
            cmbCategory.Visible = false;
            cmbCategory.Location = new Point(-500, -500);
            cmbHost.Visible = false;
            cmbHost.Location = new Point(-500, -500);

            StyleComboBox(cmbStatus);
            StyleComboBox(cmbCategory);
            StyleComboBox(cmbHost);
        }

        private void OnDatePanelPaint(object? sender, PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            var rect = new Rectangle(0, 1, txtDateRange.Width + 12, datePanel.Height - 3);
            using var path = FormUtilities.CreateRoundedRectPath(rect, 10);
            using var brush = new SolidBrush(Color.FromArgb(248, 232, 235));
            e.Graphics.FillPath(brush, path);
            using var pen = new Pen(Color.FromArgb(214, 147, 156), 1.5f);
            e.Graphics.DrawPath(pen, path);
        }

        private void OnLocationPanelPaint(object? sender, PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            var rect = new Rectangle(0, 1, txtLocation.Width + 12, locationPanel.Height - 3);
            using var path = FormUtilities.CreateRoundedRectPath(rect, 10);
            using var brush = new SolidBrush(Color.FromArgb(248, 232, 235));
            e.Graphics.FillPath(brush, path);
            using var pen = new Pen(Color.FromArgb(214, 147, 156), 1.5f);
            e.Graphics.DrawPath(pen, path);
        }

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern bool ReleaseCapture();

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam);

        private const int WM_NCLBUTTONDOWN = 0x00A1;
        private const int HTCAPTION = 0x0002;

        private Panel pnlTitleBar;
        private Button btnWindowMinimize;
        private Button btnWindowClose;
        private Label lblWindowTitle;

        private void StylePillButtonWithDivider(Button btn)
        {
            btn.Tag = null;
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
            btn.BackColor = Color.Transparent;
            btn.Cursor = Cursors.Hand;
            btn.Region = null;

            Color fillColor = Color.FromArgb(214, 147, 156);
            Color textColor = Color.FromArgb(255, 248, 248);
            Color hoverFill = Color.FromArgb(197, 125, 143);
            bool isHover = false;

            btn.MouseEnter += (s, e) => { isHover = true; btn.Invalidate(); };
            btn.MouseLeave += (s, e) => { isHover = false; btn.Invalidate(); };

            btn.Paint += (s, e) =>
            {
                var g = e.Graphics;
                g.SmoothingMode = SmoothingMode.AntiAlias;

                Color fill = isHover ? hoverFill : fillColor;
                var rect = new Rectangle(0, 0, btn.Width - 1, btn.Height - 1);

                using var path = FormUtilities.CreateRoundedRectPath(rect, btn.Height / 2);
                using var brush = new SolidBrush(fill);
                g.FillPath(brush, path);
                using var borderPen = new Pen(Color.FromArgb(170, 110, 125), 1.2f);
                g.DrawPath(borderPen, path);

                int pad = 4;
                int circleSize = btn.Height - (pad * 2);
                var circleRect = new Rectangle(pad, pad, circleSize, circleSize);
                using var circleBrush = new SolidBrush(Color.FromArgb(235, 215, 220));
                g.FillEllipse(circleBrush, circleRect);
                using var circlePen = new Pen(Color.FromArgb(185, 135, 145), 1f);
                g.DrawEllipse(circlePen, circleRect);

                int lineX = pad + circleSize / 2;
                using var linePen = new Pen(Color.FromArgb(130, 75, 85), 1.8f);
                g.DrawLine(linePen, lineX, pad + 5, lineX, pad + circleSize - 5);

                int textLeft = pad + circleSize + 6;
                var textRect = new Rectangle(textLeft, 0, btn.Width - textLeft - pad, btn.Height);
                TextRenderer.DrawText(g, btn.Text, btn.Font, textRect, textColor,
                    TextFormatFlags.VerticalCenter | TextFormatFlags.Left);
            };
        }

        private void StyleDarkPillButton(Button btn)
        {
            btn.Tag = null;
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
            btn.BackColor = Color.Transparent;
            btn.Cursor = Cursors.Hand;
            btn.Region = null;

            Color fillColor = Color.FromArgb(180, 110, 125);
            Color textColor = Color.FromArgb(255, 248, 248);
            Color hoverFill = Color.FromArgb(160, 90, 108);
            bool isHover = false;

            btn.MouseEnter += (s, e) => { isHover = true; btn.Invalidate(); };
            btn.MouseLeave += (s, e) => { isHover = false; btn.Invalidate(); };

            btn.Paint += (s, e) =>
            {
                var g = e.Graphics;
                g.SmoothingMode = SmoothingMode.AntiAlias;
                Color fill = isHover ? hoverFill : fillColor;
                var rect = new Rectangle(0, 0, btn.Width - 1, btn.Height - 1);
                using var path = FormUtilities.CreateRoundedRectPath(rect, btn.Height / 2);
                using var brush = new SolidBrush(fill);
                g.FillPath(brush, path);
                TextRenderer.DrawText(g, btn.Text, btn.Font, rect, textColor,
                    TextFormatFlags.VerticalCenter | TextFormatFlags.HorizontalCenter);
            };
        }

        private void WrapTextBoxWithRoundedPanel(TextBox tb, Color fillColor, Color borderColor, int radius)
        {
            if (tb.Parent == null) return;

            if (tb.Parent is Panel p && p.Name == "roundedTbPanel_" + tb.Name) return;

            var parent = tb.Parent;
            var wrapper = new Panel
            {
                Name = "roundedTbPanel_" + tb.Name,
                Location = tb.Location,
                Size = new Size(tb.Width + 10, tb.Height + 10),
                BackColor = Color.Transparent
            };

            parent.Controls.Remove(tb);
            tb.BorderStyle = BorderStyle.None;
            tb.BackColor = fillColor;
            tb.Location = new Point(radius, (wrapper.Height - tb.Height) / 2);
            tb.Width = wrapper.Width - radius * 2;
            wrapper.Controls.Add(tb);
            parent.Controls.Add(wrapper);

            wrapper.Paint += (s, e) =>
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                var rect = new Rectangle(0, 0, wrapper.Width - 1, wrapper.Height - 1);
                using var path = FormUtilities.CreateRoundedRectPath(rect, radius);
                using var brush = new SolidBrush(fillColor);
                e.Graphics.FillPath(brush, path);
                using var pen = new Pen(borderColor, 1.5f);
                e.Graphics.DrawPath(pen, path);
            };

            wrapper.Click += (s, e) => tb.Focus();
        }

        private void BuildCategoryPill(Control? parent, Color textColor)
        {
            if (parent == null) return;

            if (_categoryPill != null)
            {
                parent.Controls.Remove(_categoryPill);
                _categoryPill.Dispose();
            }

            _categoryPill = new Panel
            {
                Location = new Point(140, 230),
                Size = new Size(220, 30),
                BackColor = Color.Transparent,
                Cursor = Cursors.Hand
            };

            _categoryPill.Paint += (s, e) =>
            {
                var g = e.Graphics;
                g.SmoothingMode = SmoothingMode.AntiAlias;

                var rect = new Rectangle(0, 0, _categoryPill.Width - 1, _categoryPill.Height - 1);
                using var path = FormUtilities.CreateRoundedRectPath(rect, _categoryPill.Height / 2);
                using var bgBrush = new SolidBrush(Color.FromArgb(248, 232, 235));
                g.FillPath(bgBrush, path);
                using var borderPen = new Pen(Color.FromArgb(214, 147, 156), 1.2f);
                g.DrawPath(borderPen, path);

                string selectedText = cmbCategory.SelectedItem?.ToString() ?? "Hotel";
                var textRect = new Rectangle(12, 0, _categoryPill.Width - 40, _categoryPill.Height);
                TextRenderer.DrawText(g, selectedText, new Font("Segoe UI", 8.5F), textRect, textColor,
                    TextFormatFlags.VerticalCenter | TextFormatFlags.Left);

                int iconX = _categoryPill.Width - 32;
                int iconY = 5;
                var iconRect = new Rectangle(iconX, iconY, 22, 20);
                using var iconBrush = new SolidBrush(Color.FromArgb(220, 195, 200));
                g.FillRectangle(iconBrush, iconRect);
                using var iconPen = new Pen(Color.FromArgb(180, 140, 150), 1f);
                g.DrawRectangle(iconPen, iconRect);
            };

            _categoryPill.Click += (s, e) =>
            {
                cmbCategory.Parent?.Controls.SetChildIndex(cmbCategory, 0);
                cmbCategory.Location = _categoryPill.Location;
                cmbCategory.Size = _categoryPill.Size;
                cmbCategory.Visible = true;
                cmbCategory.BringToFront();
                cmbCategory.DroppedDown = true;
                cmbCategory.Focus();
            };

            cmbCategory.Leave += (s, e) =>
            {
                cmbCategory.Visible = false;
                cmbCategory.Location = new Point(-500, -500);
            };

            cmbCategory.SelectedIndexChanged += (s, e) =>
            {
                _categoryPill?.Invalidate();
                cmbCategory.Visible = false;
                cmbCategory.Location = new Point(-500, -500);
            };

            parent.Controls.Add(_categoryPill);
        }

        private Panel? _statusPill;

        private void BuildStatusPill(Control? parent, Color textColor)
        {
            if (parent == null) return;

            if (_statusPill != null)
            {
                parent.Controls.Remove(_statusPill);
                _statusPill.Dispose();
            }

            var statusColors = new[]
            {
                Color.FromArgb(196, 120, 130),
                Color.FromArgb(80, 160, 200),
                Color.FromArgb(80, 170, 110),
                Color.FromArgb(200, 180, 80),
                Color.FromArgb(140, 140, 150)
            };

            _statusPill = new Panel
            {
                Location = new Point(140, 114),
                Size = new Size(240, 30),
                BackColor = Color.Transparent,
                Cursor = Cursors.Hand
            };

            _statusPill.Paint += (s, e) =>
            {
                var g = e.Graphics;
                g.SmoothingMode = SmoothingMode.AntiAlias;

                var rect = new Rectangle(0, 0, _statusPill.Width - 1, _statusPill.Height - 1);
                using var path = FormUtilities.CreateRoundedRectPath(rect, _statusPill.Height / 2);
                using var bgBrush = new SolidBrush(Color.FromArgb(248, 232, 235));
                g.FillPath(bgBrush, path);
                using var borderPen = new Pen(Color.FromArgb(214, 147, 156), 1.2f);
                g.DrawPath(borderPen, path);

                int barX = 8;
                int barY = 7;
                int barH = _statusPill.Height - 14;
                int barW = 18;
                int barGap = 4;

                for (int i = 0; i < statusColors.Length; i++)
                {
                    var barRect = new Rectangle(barX + i * (barW + barGap), barY, barW, barH);
                    using var barPath = FormUtilities.CreateRoundedRectPath(barRect, 4);
                    using var barBrush = new SolidBrush(statusColors[i]);
                    g.FillPath(barBrush, barPath);
                }

                string selectedText = cmbStatus.SelectedItem?.ToString() ?? "Planned";
                int textX = barX + statusColors.Length * (barW + barGap) + 6;
                var textRect = new Rectangle(textX, 0, _statusPill.Width - textX - 20, _statusPill.Height);
                TextRenderer.DrawText(g, selectedText, new Font("Segoe UI", 8.5F), textRect, textColor,
                    TextFormatFlags.VerticalCenter | TextFormatFlags.Left);

                int cx = _statusPill.Width - 14;
                int cy = _statusPill.Height / 2;
                using var chevPen = new Pen(textColor, 1.5f);
                g.DrawLine(chevPen, cx - 4, cy - 2, cx, cy + 2);
                g.DrawLine(chevPen, cx, cy + 2, cx + 4, cy - 2);
            };

            _statusPill.Click += (s, e) =>
            {
                cmbStatus.Parent?.Controls.SetChildIndex(cmbStatus, 0);
                cmbStatus.Location = _statusPill.Location;
                cmbStatus.Size = _statusPill.Size;
                cmbStatus.Visible = true;
                cmbStatus.BringToFront();
                cmbStatus.DroppedDown = true;
                cmbStatus.Focus();
            };

            cmbStatus.Leave += (s, e) =>
            {
                cmbStatus.Visible = false;
                cmbStatus.Location = new Point(-500, -500);
            };

            cmbStatus.SelectedIndexChanged += (s, e) =>
            {
                _statusPill?.Invalidate();
                cmbStatus.Visible = false;
                cmbStatus.Location = new Point(-500, -500);
            };

            parent.Controls.Add(_statusPill);
        }

        private void OnParticipantToggled(object? sender, ItemCheckEventArgs e)
        {
            var participant = chkParticipants.Items[e.Index] as Participant;
            if (participant != null)
            {
                if (e.NewValue == CheckState.Checked)
                {
                    if (!_selectedParticipants.Any(p => p.Id == participant.Id))
                        _selectedParticipants.Add(participant);
                }
                else
                {
                    _selectedParticipants.RemoveAll(p => p.Id == participant.Id);
                }
            }
        }

        private void OnSave(object sender, EventArgs e)
        {
            _gala.Name = txtName.Text.Trim();
            _gala.ScheduledDate = _selectedDate;
            _gala.Location = txtLocation.Text.Trim();
            _gala.Latitude = _selectedLatitude;
            _gala.Longitude = _selectedLongitude;
            _gala.Status = cmbStatus.SelectedItem?.ToString() ?? "Planned";
            _gala.Plan = txtPlan.Text.Trim();
            _gala.LocationItems = _selectedLocations;
            _gala.Participants = _selectedParticipants;
            _gala.RangeStartDate = _rangeStart;
            _gala.RangeEndDate = _rangeEnd;
            if (cmbHost.SelectedItem is Participant selectedHost)
                _gala.HostName = selectedHost.Name;
            else
                _gala.HostName = string.Empty;

            if (string.IsNullOrWhiteSpace(_gala.Name))
            {
                MessageBox.Show("Please enter a gala name.");
                return;
            }

            if (string.IsNullOrWhiteSpace(_gala.Location))
            {
                MessageBox.Show("Please enter an area name in the location field.");
                return;
            }

            if (_gala.Latitude == 0 && _gala.Longitude == 0)
            {
                MessageBox.Show("Please pin a location on the map or search for one.");
                return;
            }

            var conflicts = _dataService.GetDateConflicts(_gala);
            if (conflicts.Any())
            {
                var message = "This gala overlaps with other schedules on the same date:\n\n";
                foreach (var g in conflicts)
                    message += $"- {g.Name} ({g.ScheduledDate:d})\n";

                var result = MessageBox.Show(
                    message + "\nDo you still want to continue?",
                    "Gala Conflict",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning);

                if (result == DialogResult.No)
                    return;
            }

            _dataService.SaveGala(_gala);
            DialogResult = DialogResult.OK;
            Close();
        }

        private void OnCancel(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private static void ApplyRoundedButtonStyle(Button btn, Color back, Color border, Color hover, Color down, System.Media.SoundPlayer hoverSound)
        {
            if (btn.Tag is RoundedButtonStyleState) return;

            var state = new RoundedButtonStyleState
            {
                Back = back,
                Border = border,
                Hover = hover,
                Down = down,
                HoverSound = hoverSound
            };
            btn.Tag = state;
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
            btn.Region = new Region(CreateRoundedRectPath(new Rectangle(0, 0, btn.Width, btn.Height), 12));

            state.AnimationTimer = new System.Windows.Forms.Timer { Interval = 15 };
            state.AnimationTimer.Tick += (s, e) =>
            {
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
                if (state.HoverSound != null)
                {
                    try { state.HoverSound.Play(); } catch { }
                }
            };
            btn.MouseLeave += (s, e) => { state.IsHover = false; state.HoverTarget = 0f; state.AnimationTimer.Start(); };
            btn.MouseDown += (s, e) => { state.IsDown = true; btn.Invalidate(); };
            btn.MouseUp += (s, e) => { state.IsDown = false; btn.Invalidate(); };

            btn.Paint += (s, e) =>
            {
                e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                Color current = state.IsDown ? state.Down : Interpolate(state.Back, state.Hover, state.HoverProgress);
                using var path = CreateRoundedRectPath(new Rectangle(0, 0, btn.Width - 1, btn.Height - 1), 12);
                using var brush = new SolidBrush(current);
                e.Graphics.FillPath(brush, path);
                using var pen = new Pen(state.Border, 1f);
                e.Graphics.DrawPath(pen, path);
                TextRenderer.DrawText(e.Graphics, btn.Text, btn.Font, btn.ClientRectangle, btn.ForeColor,
                    TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
            };
        }

        private static Color Interpolate(Color b, Color t, float p) =>
            Color.FromArgb(
                (int)(b.R + (t.R - b.R) * p),
                (int)(b.G + (t.G - b.G) * p),
                (int)(b.B + (t.B - b.B) * p));

        public class RoundedButtonStyleState
        {
            public Color Back { get; set; }
            public Color Border { get; set; }
            public Color Hover { get; set; }
            public Color Down { get; set; }
            public float HoverProgress { get; set; }
            public float HoverTarget { get; set; }
            public bool IsHover { get; set; }
            public bool IsDown { get; set; }
            public System.Windows.Forms.Timer AnimationTimer { get; set; } = null!;
            public System.Media.SoundPlayer HoverSound { get; set; } = null!;
        }

        private class ParticipantAvatar : Control
        {
            private Image? _image;

            [Category("Appearance")]
            [Browsable(true)]
            [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
            public Image? Image
            {
                get => _image;
                set { _image = value; Invalidate(); }
            }

            [Category("Appearance")]
            [Browsable(true)]
            [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
            public Color BorderColorStart { get; set; } = Color.FromArgb(214, 147, 156);

            [Category("Appearance")]
            [Browsable(true)]
            [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
            public Color BorderColorEnd { get; set; } = Color.FromArgb(255, 220, 226);

            public ParticipantAvatar()
            {
                DoubleBuffered = true;
            }

            protected override void OnPaint(PaintEventArgs e)
            {
                base.OnPaint(e);
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

                Rectangle rect = new Rectangle(2, 2, Width - 4, Height - 4);
                using var circlePath = new GraphicsPath();
                circlePath.AddEllipse(rect);
                e.Graphics.FillEllipse(Brushes.White, rect);

                if (Image != null)
                {
                    e.Graphics.SetClip(circlePath);
                    e.Graphics.DrawImage(Image, rect);
                    e.Graphics.ResetClip();
                }
                else
                {
                    using var fallbackBrush = new SolidBrush(Color.FromArgb(234, 218, 220));
                    e.Graphics.FillEllipse(fallbackBrush, rect);
                    using var txtBrush = new SolidBrush(Color.FromArgb(120, 70, 80));
                    var sf = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };
                    e.Graphics.DrawString("?", new Font("Segoe UI", 14, FontStyle.Bold), txtBrush, rect, sf);
                }

                using var ringBrush = new LinearGradientBrush(rect, BorderColorStart, BorderColorEnd, 45f);
                using var ringPen = new Pen(ringBrush, 2.2f);
                e.Graphics.DrawEllipse(ringPen, rect);
            }
        }

        private void StyleComboBox(ComboBox cb)
        {
            cb.DrawMode = DrawMode.OwnerDrawFixed;
            cb.DropDownStyle = ComboBoxStyle.DropDownList;
            cb.BackColor = Color.FromArgb(255, 246, 248);
            cb.ForeColor = Color.FromArgb(94, 49, 58);
            cb.Font = new Font("Segoe UI", 9F);
            cb.DrawItem -= OnComboBoxDrawItem;
            cb.DrawItem += OnComboBoxDrawItem;
        }

        private void OnComboBoxDrawItem(object? sender, DrawItemEventArgs e)
        {
            if (e.Index < 0) return;
            var cb = sender as ComboBox;
            if (cb == null) return;

            e.DrawBackground();
            bool isSelected = (e.State & DrawItemState.Selected) == DrawItemState.Selected;

            using (var brush = new SolidBrush(isSelected ? Color.FromArgb(214, 147, 156) : cb.BackColor))
            {
                e.Graphics.FillRectangle(brush, e.Bounds);
            }

            using (var textBrush = new SolidBrush(isSelected ? Color.White : cb.ForeColor))
            {
                string text = cb.GetItemText(cb.Items[e.Index]);
                e.Graphics.DrawString(text, e.Font ?? cb.Font, textBrush, e.Bounds.X + 5, e.Bounds.Y + 2);
            }

            if ((e.State & DrawItemState.Focus) == DrawItemState.Focus)
                e.DrawFocusRectangle();
        }

        private Panel? _hostPill;
        private void BuildHostPill(Control? parent, Color textColor)
        {
            if (parent == null) return;
            if (_hostPill != null) { parent.Controls.Remove(_hostPill); _hostPill.Dispose(); }

            _hostPill = new Panel
            {
                Location = new Point(195, 6),
                Size = new Size(160, 30),
                BackColor = Color.Transparent,
                Cursor = Cursors.Hand
            };

            _hostPill.Paint += (s, e) =>
            {
                var g = e.Graphics;
                g.SmoothingMode = SmoothingMode.AntiAlias;
                var rect = new Rectangle(0, 0, _hostPill.Width - 1, _hostPill.Height - 1);
                using var path = FormUtilities.CreateRoundedRectPath(rect, _hostPill.Height / 2);
                using var bgBrush = new SolidBrush(Color.FromArgb(248, 232, 235));
                g.FillPath(bgBrush, path);
                using var borderPen = new Pen(Color.FromArgb(214, 147, 156), 1.2f);
                g.DrawPath(borderPen, path);

                string selectedText = (cmbHost.SelectedItem as Participant)?.Name ?? "Select Host";
                var textRect = new Rectangle(12, 0, _hostPill.Width - 30, _hostPill.Height);
                TextRenderer.DrawText(g, selectedText, new Font("Segoe UI", 8.5F), textRect, textColor,
                    TextFormatFlags.VerticalCenter | TextFormatFlags.Left | TextFormatFlags.EndEllipsis);

                int cx = _hostPill.Width - 14;
                int cy = _hostPill.Height / 2;
                using var chevPen = new Pen(textColor, 1.5f);
                g.DrawLine(chevPen, cx - 4, cy - 2, cx, cy + 2);
                g.DrawLine(chevPen, cx, cy + 2, cx + 4, cy - 2);
            };

            _hostPill.Click += (s, e) =>
            {
                cmbHost.Location = _hostPill.Location;
                cmbHost.Size = _hostPill.Size;
                cmbHost.Visible = true;
                cmbHost.BringToFront();
                cmbHost.DroppedDown = true;
                cmbHost.Focus();
            };

            cmbHost.Leave += (s, e) => { cmbHost.Visible = false; cmbHost.Location = new Point(-500, -500); };
            cmbHost.SelectedIndexChanged += (s, e) =>
            {
                _hostPill?.Invalidate();
                cmbHost.Visible = false;
                cmbHost.Location = new Point(-500, -500);
            };
            parent.Controls.Add(_hostPill);
            cmbHost.Visible = false;
        }
    }
}