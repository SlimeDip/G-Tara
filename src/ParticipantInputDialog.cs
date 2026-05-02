using System;
using System.Windows.Forms;

namespace G_Tara
{
    public partial class ParticipantInputDialog : Form
    {
        private Label lblName;
        private TextBox txtParticipantName;
        private Label lblEmail;
        private TextBox txtParticipantEmail;
        private Label lblStartDate;
        private DateTimePicker dtpParticipantAvailableStartDate;
        private Label lblEndDate;
        private DateTimePicker dtpParticipantAvailableEndDate;
        private Button btnOK;
        private Button btnCancel;

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
            this.lblStartDate = new System.Windows.Forms.Label();
            this.dtpParticipantAvailableStartDate = new System.Windows.Forms.DateTimePicker();
            this.lblEndDate = new System.Windows.Forms.Label();
            this.dtpParticipantAvailableEndDate = new System.Windows.Forms.DateTimePicker();
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

            // lblStartDate
            this.lblStartDate.AutoSize = true;
            this.lblStartDate.Location = new System.Drawing.Point(12, 75);
            this.lblStartDate.Name = "lblStartDate";
            this.lblStartDate.Size = new System.Drawing.Size(58, 13);
            this.lblStartDate.TabIndex = 4;
            this.lblStartDate.Text = "Start Date:";

            // dtpParticipantAvailableStartDate
            this.dtpParticipantAvailableStartDate.Location = new System.Drawing.Point(100, 72);
            this.dtpParticipantAvailableStartDate.Name = "dtpParticipantAvailableStartDate";
            this.dtpParticipantAvailableStartDate.Size = new System.Drawing.Size(250, 20);
            this.dtpParticipantAvailableStartDate.TabIndex = 5;

            // lblEndDate
            this.lblEndDate.AutoSize = true;
            this.lblEndDate.Location = new System.Drawing.Point(12, 105);
            this.lblEndDate.Name = "lblEndDate";
            this.lblEndDate.Size = new System.Drawing.Size(55, 13);
            this.lblEndDate.TabIndex = 6;
            this.lblEndDate.Text = "End Date:";

            // dtpParticipantAvailableEndDate
            this.dtpParticipantAvailableEndDate.Location = new System.Drawing.Point(100, 102);
            this.dtpParticipantAvailableEndDate.Name = "dtpParticipantAvailableEndDate";
            this.dtpParticipantAvailableEndDate.Size = new System.Drawing.Size(250, 20);
            this.dtpParticipantAvailableEndDate.TabIndex = 7;

            // btnOK
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Location = new System.Drawing.Point(194, 140);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 8;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;

            // btnCancel
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(275, 140);
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
            this.ClientSize = new System.Drawing.Size(362, 175);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.dtpParticipantAvailableEndDate);
            this.Controls.Add(this.lblEndDate);
            this.Controls.Add(this.dtpParticipantAvailableStartDate);
            this.Controls.Add(this.lblStartDate);
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
        public DateTime ParticipantAvailableStartDate
        {
            get => dtpParticipantAvailableStartDate.Value;
            set => dtpParticipantAvailableStartDate.Value = value;
        }

        [System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Hidden)]
        public DateTime ParticipantAvailableEndDate
        {
            get => dtpParticipantAvailableEndDate.Value;
            set => dtpParticipantAvailableEndDate.Value = value;
        }

        private void WireUiEvents()
        {
            btnOK.Click -= OnOKClick;
            btnOK.Click += OnOKClick;

            btnCancel.Click -= OnCancelClick;
            btnCancel.Click += OnCancelClick;
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
            dtpParticipantAvailableStartDate.Value = DateTime.Now;
            dtpParticipantAvailableEndDate.Value = DateTime.Now;
        }
    }
}
