using G_Tara.Models;
using G_Tara.Services;

namespace G_Tara
{
    public partial class ParticipantsManagementForm : Form
    {
        private readonly ParticipantsDataService _dataService;
        private readonly Host? _currentHost;
        private List<Participant> _participants;
        private DataGridView dgvParticipants;
        private TextBox txtSearch;
        private Button btnSearch;
        private Button btnAdd;
        private Button btnEdit;
        private Button btnRemove;
        private Button btnClose;

        public ParticipantsManagementForm(ParticipantsDataService dataService, Host? currentHost)
        {
            _dataService = dataService;
            _currentHost = currentHost;
            _participants = new List<Participant>();
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.Text = "Manage Participants";
            this.Size = new Size(700, 500);
            this.StartPosition = FormStartPosition.CenterParent;
            this.ShowInTaskbar = false;

            var mainLayout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                RowCount = 3,
                ColumnCount = 1,
                Padding = new Padding(10)
            };

            mainLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 40));
            mainLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
            mainLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 45));

            var searchPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(5)
            };

            txtSearch = new TextBox
            {
                Width = 200,
                Height = 25,
                Margin = new Padding(3)
            };

            btnSearch = new Button
            {
                Text = "Search",
                Width = 80,
                Height = 25,
                Margin = new Padding(3)
            };
            btnSearch.Click += OnSearch;

            searchPanel.Controls.Add(new Label { Text = "Search:", Height = 25, AutoSize = true, Margin = new Padding(3) });
            searchPanel.Controls.Add(txtSearch);
            searchPanel.Controls.Add(btnSearch);

            dgvParticipants = new DataGridView
            {
                Dock = DockStyle.Fill,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                AutoGenerateColumns = false,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false
            };

            dgvParticipants.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Name", HeaderText = "Name", Width = 150 });
            dgvParticipants.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Email", HeaderText = "Email", Width = 200 });
            dgvParticipants.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "AvailableStartDate", HeaderText = "Start Date", Width = 100, DefaultCellStyle = new DataGridViewCellStyle { Format = "yyyy-MM-dd" } });
            dgvParticipants.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "AvailableEndDate", HeaderText = "End Date", Width = 100, DefaultCellStyle = new DataGridViewCellStyle { Format = "yyyy-MM-dd" } });

            dgvParticipants.SelectionChanged += OnParticipantSelectionChanged;

            var buttonPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(5),
                FlowDirection = FlowDirection.RightToLeft
            };

            btnClose = new Button { Text = "Close", Width = 80, Height = 32, Margin = new Padding(3) };
            btnClose.Click += (s, e) => this.Close();

            btnRemove = new Button { Text = "Remove", Width = 80, Height = 32, Margin = new Padding(3), Enabled = false };
            btnRemove.Click += OnRemoveParticipant;

            btnEdit = new Button { Text = "Edit", Width = 80, Height = 32, Margin = new Padding(3), Enabled = false };
            btnEdit.Click += OnEditParticipant;

            btnAdd = new Button { Text = "Add", Width = 80, Height = 32, Margin = new Padding(3) };
            btnAdd.Click += OnAddParticipant;

            buttonPanel.Controls.Add(btnClose);
            buttonPanel.Controls.Add(btnRemove);
            buttonPanel.Controls.Add(btnEdit);
            buttonPanel.Controls.Add(btnAdd);

            mainLayout.Controls.Add(searchPanel, 0, 0);
            mainLayout.Controls.Add(dgvParticipants, 0, 1);
            mainLayout.Controls.Add(buttonPanel, 0, 2);

            this.Controls.Add(mainLayout);
            LoadParticipants();
        }

        private void LoadParticipants()
        {
            _participants = _dataService.LoadParticipants();
            RefreshParticipantsGrid();
        }

        private void RefreshParticipantsGrid()
        {
            dgvParticipants.DataSource = null;
            dgvParticipants.DataSource = new BindingSource { DataSource = _participants };
        }

        private void OnSearch(object sender, EventArgs e)
        {
            var searchTerm = txtSearch.Text.Trim();
            var results = _dataService.SearchParticipants(searchTerm);
            dgvParticipants.DataSource = null;
            dgvParticipants.DataSource = new BindingSource { DataSource = results };
        }

        private void OnAddParticipant(object sender, EventArgs e)
        {
            using var dialog = new ParticipantInputDialog();
            if (dialog.ShowDialog(this) == DialogResult.OK)
            {
                var participant = new Participant
                {
                    Name = dialog.ParticipantName,
                    Email = dialog.ParticipantEmail,
                    AvailableStartDate = dialog.ParticipantAvailableStartDate,
                    AvailableEndDate = dialog.ParticipantAvailableEndDate
                };
                _dataService.SaveParticipant(participant);
                LoadParticipants();
                txtSearch.Clear();
            }
        }

        private void OnEditParticipant(object sender, EventArgs e)
        {
            if (dgvParticipants.SelectedRows.Count == 0) return;

            var selectedRow = dgvParticipants.SelectedRows[0];
            var participant = (Participant)selectedRow.DataBoundItem;

            using var dialog = new ParticipantInputDialog
            {
                ParticipantName = participant.Name,
                ParticipantEmail = participant.Email,
                ParticipantAvailableStartDate = participant.AvailableStartDate,
                ParticipantAvailableEndDate = participant.AvailableEndDate
            };

            if (dialog.ShowDialog(this) == DialogResult.OK)
            {
                participant.Name = dialog.ParticipantName;
                participant.Email = dialog.ParticipantEmail;
                participant.AvailableStartDate = dialog.ParticipantAvailableStartDate;
                participant.AvailableEndDate = dialog.ParticipantAvailableEndDate;
                _dataService.SaveParticipant(participant);
                LoadParticipants();
                txtSearch.Clear();
            }
        }

        private void OnRemoveParticipant(object sender, EventArgs e)
        {
            if (dgvParticipants.SelectedRows.Count == 0) return;

            var selectedRow = dgvParticipants.SelectedRows[0];
            var participant = (Participant)selectedRow.DataBoundItem;

            if (MessageBox.Show($"Remove {participant.Name}?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                _dataService.DeleteParticipant(participant.Id);
                LoadParticipants();
                txtSearch.Clear();
            }
        }

        private void OnParticipantSelectionChanged(object sender, EventArgs e)
        {
            var enabled = dgvParticipants.SelectedRows.Count > 0;
            btnEdit.Enabled = enabled;
            btnRemove.Enabled = enabled;
        }
    }
}
