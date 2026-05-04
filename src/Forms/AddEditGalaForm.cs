using G_Tara.Models;
using G_Tara.Services;
using System.Diagnostics;

namespace G_Tara
{
    public partial class AddEditGalaForm : Form
    {
        private Gala _gala;
        private Host? _currentHost;
        private readonly GalaDataService _dataService;
        private readonly MemberService _memberService;
        private readonly WeatherService _weatherService;
        private readonly LocationService _locationService;
        private readonly ParticipantsDataService _participantsDataService;
        private readonly List<LocationItem> _selectedLocations;
        private List<Participant> _availableParticipants;
        private List<Participant> _selectedParticipants;
        private double _selectedLatitude;
        private double _selectedLongitude;
        private readonly Label _lblCoordinates = new Label { AutoSize = true, Margin = new Padding(3, 7, 3, 3) };

        public AddEditGalaForm(Gala? gala, GalaDataService dataService, ParticipantsDataService? participantsDataService = null)
        {
            InitializeComponent();
            WireUiEvents();
            _gala = gala ?? new Gala();
            _dataService = dataService;
            _participantsDataService = participantsDataService ?? new ParticipantsDataService();
            _memberService = new MemberService();
            _weatherService = new WeatherService();
            _locationService = new LocationService();
            _selectedLocations = new List<LocationItem>(_gala.LocationItems);
            _availableParticipants = new List<Participant>();
            _selectedParticipants = new List<Participant>(_gala.Participants);
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

            dtpDate.ValueChanged -= OnDateChangedAsync;
            dtpDate.ValueChanged += OnDateChangedAsync;

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
            {
                return;
            }

            _selectedLatitude = picker.SelectedLatitude;
            _selectedLongitude = picker.SelectedLongitude;
            UpdateCoordinatesLabel();

            Debug.WriteLine($"[MapPin:selected] lat={_selectedLatitude}, lon={_selectedLongitude}");
            await UpdateWeatherSilentlyAsync();
        }

        private void AddEditGalaForm_Load(object sender, EventArgs e)
        {
            InitializeControls();
        }

        private void InitializeControls()
        {
            if (cmbCategory.Items.Count > 0 && cmbCategory.SelectedIndex < 0)
            {
                cmbCategory.SelectedIndex = 0;
            }

            _selectedLatitude = _gala.Latitude;
            _selectedLongitude = _gala.Longitude;

            locationPanel.Controls.Add(_lblCoordinates);
            UpdateCoordinatesLabel();

            txtName.Text = _gala.Name;
            dtpDate.Value = _gala.ScheduledDate == default ? DateTime.Now : _gala.ScheduledDate;
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
        }

        private void EnableParticipantControls()
        {
            chkParticipants.Enabled = true;
        }

        private void UpdateCoordinatesLabel()
        {
            if (_selectedLatitude != 0 || _selectedLongitude != 0)
            {
                _lblCoordinates.Text = $"Lat: {_selectedLatitude:F6}, Lon: {_selectedLongitude:F6}";
            }
            else
            {
                _lblCoordinates.Text = "No coordinates selected";
            }
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

        private async void OnDateChangedAsync(object? sender, EventArgs e)
        {
            // Update gala scheduled date for participant filtering
            _gala.ScheduledDate = dtpDate.Value;

            // Refresh participants UI based on new date
            RefreshParticipantsUI();

            // Update weather
            await UpdateWeatherSilentlyAsync();
        }

        private async Task UpdateWeatherSilentlyAsync()
        {
            if (_selectedLatitude == 0 || _selectedLongitude == 0) return;

            LogWeatherCoordinates("request_silent");
            lblWeather.Text = "Loading weather...";
            var weather = await _weatherService.GetWeatherAsync(_selectedLatitude, _selectedLongitude, dtpDate.Value);
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
            var weather = await _weatherService.GetWeatherAsync(_selectedLatitude, _selectedLongitude, dtpDate.Value);
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
                    var query = Uri.EscapeDataString($"{location.Name} near {location.Latitude.ToString(System.Globalization.CultureInfo.InvariantCulture)},{location.Longitude.ToString(System.Globalization.CultureInfo.InvariantCulture)}");
                    var url = $"https://www.google.com/maps/search/?api=1&query={query}";

                    try
                    {
                        Process.Start(new ProcessStartInfo
                        {
                            FileName = url,
                            UseShellExecute = true
                        });
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
            // Ensure gala scheduled date is set to current picker value
            _gala.ScheduledDate = dtpDate.Value;

            _availableParticipants = _participantsDataService.LoadParticipants();
            RefreshParticipantsUI();
        }

        private void RefreshParticipantsUI()
        {
            // Get the currently selected gala date
            DateTime galaDate = dtpDate.Value.Date;

            // Filter participants to show only those available on the gala date
            var availableForDate = _availableParticipants
                .Where(p => p.AvailableStartDate.Date <= galaDate && p.AvailableEndDate.Date >= galaDate)
                .ToList();

            // Store participants that are no longer available due to date change
            var noLongerAvailable = _selectedParticipants
                .Where(sp => !availableForDate.Any(p => p.Id == sp.Id))
                .ToList();

            // Warn user if selected participants are no longer available
            if (noLongerAvailable.Any())
            {
                var names = string.Join(", ", noLongerAvailable.Select(p => p.Name));
                MessageBox.Show(
                    $"The following participants are not available on the selected date and have been removed:\n{names}",
                    "Participant Availability Warning",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );

                // Remove no longer available participants from selection
                foreach (var p in noLongerAvailable)
                {
                    _selectedParticipants.RemoveAll(sp => sp.Id == p.Id);
                }
            }

            // Populate CheckedListBox with available participants
            chkParticipants.Items.Clear();
            foreach (var p in availableForDate)
            {
                bool isSelected = _selectedParticipants.Any(sp => sp.Id == p.Id);
                chkParticipants.Items.Add(p, isSelected);
            }
            chkParticipants.DisplayMember = "Name";
        }

        private void OnParticipantToggled(object? sender, ItemCheckEventArgs e)
        {
            if (!IsHostOrPermitted())
            {
                e.NewValue = e.CurrentValue;
                MessageBox.Show("Only the host can manage participants.", "Permission Denied", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var participant = chkParticipants.Items[e.Index] as Participant;
            if (participant != null)
            {
                if (e.NewValue == CheckState.Checked)
                {
                    if (!_selectedParticipants.Any(p => p.Id == participant.Id))
                    {
                        _selectedParticipants.Add(participant);
                    }
                }
                else
                {
                    _selectedParticipants.RemoveAll(p => p.Id == participant.Id);
                }
            }
        }

        private bool IsHostOrPermitted()
        {
            return true;
        }

        private void OnSave(object sender, EventArgs e)
        {
            _gala.Name = txtName.Text.Trim();
            _gala.ScheduledDate = dtpDate.Value;
            _gala.Location = txtLocation.Text.Trim();
            _gala.Latitude = _selectedLatitude;
            _gala.Longitude = _selectedLongitude;
            _gala.Status = cmbStatus.SelectedItem?.ToString() ?? "Planned";
            _gala.Plan = txtPlan.Text.Trim();
            _gala.LocationItems = _selectedLocations;
            _gala.Participants = _selectedParticipants;

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

            if (_dataService.HasDateConflict(_gala))
            {
                MessageBox.Show("This date is already booked.", "Gala Conflict", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }


            _dataService.SaveGala(_gala);
            var conflicts = _dataService.SaveGala(_gala);

            if (conflicts.Any())
            {

            var message = "This gala overlaps with other schedules on the same date:\n\n";

                foreach (var g in conflicts)
                {
                    message += $"- {g.Name} ({g.ScheduledDate:d})\n";
                }

                var result = MessageBox.Show(
                    message + "\nDo you still want to continue?",
                    "Gala Conflict",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning
                );

                if (result == DialogResult.No)
                {
                    return; 
                }
            }

            DialogResult = DialogResult.OK;
            Close();
        }

        private void OnCancel(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
