using G_Tara.Models;
using G_Tara.Services;

namespace G_Tara
{
    public partial class Form1 : Form
    {
        private readonly GalaDataService _dataService;
        private readonly ParticipantsDataService _participantsDataService;
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

        private void btnAutoEmail_Click(object sender, EventArgs e)
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
                    SendPlanEmailToParticipants(gala);
                }
            }
        }

        private void SendPlanEmailToParticipants(Gala gala)
        {
            var gmailParticipants = gala.Participants.Where(p => !string.IsNullOrWhiteSpace(p.Email) && p.Email.EndsWith("@gmail.com", StringComparison.OrdinalIgnoreCase)).ToList();
            if (!gmailParticipants.Any())
            {
                MessageBox.Show("No participants with Gmail addresses found.");
                return;
            }

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

            foreach (var participant in gmailParticipants)
            {
                try
                {
                    var mail = new System.Net.Mail.MailMessage(smtpUser, participant.Email)
                    {
                        Subject = $"Gala Plan: {gala.Name}",
                        Body = $"Plan Details:\n{gala.Plan}\n\nDate: {gala.ScheduledDate:yyyy-MM-dd}\nLocation: {gala.Location}",
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
        }
    }