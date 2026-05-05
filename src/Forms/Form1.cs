using G_Tara.Models;
using G_Tara.Services;

namespace G_Tara
{
    public partial class Form1 : Form
    {
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
            _dataService = new GalaDataService();
            _participantsDataService = new ParticipantsDataService();
            _weatherService = new WeatherService();
            _galas = new List<Gala>();
            _participants = new List<Participant>();
        }

        public Form1(Host? host) : this()
        {
            _currentHost = host;
        }

        public void Form1_Load(object sender, EventArgs e)
        {
            LoadGalas();
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
        }
    }
