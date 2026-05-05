using System;
using System.Linq;
using System.Windows.Forms;

namespace G_Tara
{
    public partial class ParticipantInputDialog : Form
    {
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

        public ParticipantInputDialog()
        {
            InitializeComponent();
            WireUiEvents();
        }

        private void InitializeComponent()
        {
            this.lblName = new System.Windows.Forms.Label();
            this.txtParticipantName = new System.Windows.Forms.TextBox();
            this.lblEmail = new System.Windows.Forms.Label();
            this.txtParticipantEmail = new System.Windows.Forms.TextBox();
            this.lblAvailableDates = new System.Windows.Forms.Label();
            this.txtAvailableDates = new System.Windows.Forms.TextBox();
            this.btnPickDates = new System.Windows.Forms.Button();
            this.btnClearDates = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();

            // lblName
            this.lblName.AutoSize = true;
            this.lblName.Location = new System.Drawing.Point(12, 15);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(35, 13);
            this.lblName.TabIndex = 0;
            this.lblName.Text = "Name:";

            // txtParticipantName
            this.txtParticipantName.Location = new System.Drawing.Point(100, 12);
            this.txtParticipantName.Name = "txtParticipantName";
            this.txtParticipantName.Size = new System.Drawing.Size(250, 20);
            this.txtParticipantName.TabIndex = 1;

            // lblEmail
            this.lblEmail.AutoSize = true;
            this.lblEmail.Location = new System.Drawing.Point(12, 45);
            this.lblEmail.Name = "lblEmail";
            this.lblEmail.Size = new System.Drawing.Size(35, 13);
            this.lblEmail.TabIndex = 2;
            this.lblEmail.Text = "Email:";

            // txtParticipantEmail
            this.txtParticipantEmail.Location = new System.Drawing.Point(100, 42);
            this.txtParticipantEmail.Name = "txtParticipantEmail";
            this.txtParticipantEmail.Size = new System.Drawing.Size(250, 20);
            this.txtParticipantEmail.TabIndex = 3;

            // lblAvailableDates
            this.lblAvailableDates.AutoSize = true;
            this.lblAvailableDates.Location = new System.Drawing.Point(12, 75);
            this.lblAvailableDates.Name = "lblAvailableDates";
            this.lblAvailableDates.Size = new System.Drawing.Size(81, 13);
            this.lblAvailableDates.TabIndex = 4;
            this.lblAvailableDates.Text = "Available Dates:";

            // txtAvailableDates
            this.txtAvailableDates.Location = new System.Drawing.Point(100, 72);
            this.txtAvailableDates.Multiline = true;
            this.txtAvailableDates.Name = "txtAvailableDates";
            this.txtAvailableDates.ReadOnly = true;
            this.txtAvailableDates.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtAvailableDates.Size = new System.Drawing.Size(250, 60);
            this.txtAvailableDates.TabIndex = 5;

            // btnPickDates
            this.btnPickDates.Location = new System.Drawing.Point(100, 138);
            this.btnPickDates.Name = "btnPickDates";
            this.btnPickDates.Size = new System.Drawing.Size(120, 23);
            this.btnPickDates.TabIndex = 6;
            this.btnPickDates.Text = "Pick Dates";
            this.btnPickDates.UseVisualStyleBackColor = true;

            // btnClearDates
            this.btnClearDates.Location = new System.Drawing.Point(230, 138);
            this.btnClearDates.Name = "btnClearDates";
            this.btnClearDates.Size = new System.Drawing.Size(120, 23);
            this.btnClearDates.TabIndex = 7;
            this.btnClearDates.Text = "Clear";
            this.btnClearDates.UseVisualStyleBackColor = true;

            // btnOK
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Location = new System.Drawing.Point(194, 175);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 8;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;

            // btnCancel
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(275, 175);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 9;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;

            // ParticipantInputDialog
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(362, 215);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnClearDates);
            this.Controls.Add(this.btnPickDates);
            this.Controls.Add(this.txtAvailableDates);
            this.Controls.Add(this.lblAvailableDates);
            this.Controls.Add(this.txtParticipantEmail);
            this.Controls.Add(this.lblEmail);
            this.Controls.Add(this.txtParticipantName);
            this.Controls.Add(this.lblName);
            this.Name = "ParticipantInputDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Add Participant";
            this.Load += new System.EventHandler(this.ParticipantInputDialog_Load);
            this.ResumeLayout(false);
            this.PerformLayout();
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

            txtAvailableDates.Text = "Dates Selected";
        }
    }
}
