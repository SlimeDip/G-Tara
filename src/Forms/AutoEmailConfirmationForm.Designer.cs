namespace G_Tara
{
    partial class AutoEmailConfirmationForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Label lblParticipants;
        private System.Windows.Forms.TextBox txtParticipants;
        private System.Windows.Forms.Label lblPlanDetails;
        private System.Windows.Forms.TextBox txtPlanDetails;
        private System.Windows.Forms.Button btnConfirm;
        private System.Windows.Forms.Button btnCancel;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            //
            // Define Theme Colors
            //
            Color galaPinkLight = Color.FromArgb(242, 215, 217);
            Color galaWhite = Color.FromArgb(255, 248, 248);
            Color galaText = Color.FromArgb(100, 60, 65);

            this.lblParticipants = new System.Windows.Forms.Label();
            this.txtParticipants = new System.Windows.Forms.TextBox();
            this.lblPlanDetails = new System.Windows.Forms.Label();
            this.txtPlanDetails = new System.Windows.Forms.TextBox();
            this.btnConfirm = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            //
            // Form Styles
            //
            this.BackColor = galaPinkLight;
            this.Font = new Font("Segoe UI Semibold", 9.5F);
            this.ForeColor = galaText;
            //
            // lblParticipants
            //
            this.lblParticipants.AutoSize = true;
            this.lblParticipants.Location = new System.Drawing.Point(12, 15);
            this.lblParticipants.Name = "lblParticipants";
            this.lblParticipants.Size = new System.Drawing.Size(120, 15);
            this.lblParticipants.Text = "Participants' Gmails:";
            //
            // txtParticipants
            //
            this.txtParticipants.BackColor = galaWhite;
            this.txtParticipants.BorderStyle = BorderStyle.FixedSingle;
            this.txtParticipants.Location = new System.Drawing.Point(12, 35);
            this.txtParticipants.Multiline = true;
            this.txtParticipants.Name = "txtParticipants";
            this.txtParticipants.ReadOnly = true;
            this.txtParticipants.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtParticipants.Size = new System.Drawing.Size(360, 80);
            //
            // lblPlanDetails
            //
            this.lblPlanDetails.AutoSize = true;
            this.lblPlanDetails.Location = new System.Drawing.Point(12, 130);
            this.lblPlanDetails.Name = "lblPlanDetails";
            this.lblPlanDetails.Size = new System.Drawing.Size(71, 15);
            this.lblPlanDetails.Text = "Plan Details:";
            //
            // txtPlanDetails
            //
            this.txtPlanDetails.BackColor = galaWhite;
            this.txtPlanDetails.BorderStyle = BorderStyle.FixedSingle;
            this.txtPlanDetails.Location = new System.Drawing.Point(12, 150);
            this.txtPlanDetails.Multiline = true;
            this.txtPlanDetails.Name = "txtPlanDetails";
            this.txtPlanDetails.ReadOnly = true;
            this.txtPlanDetails.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtPlanDetails.Size = new System.Drawing.Size(360, 80);
            //
            // btnConfirm
            //
            this.btnConfirm.Location = new System.Drawing.Point(210, 245);
            this.btnConfirm.Name = "btnConfirm";
            this.btnConfirm.Size = new System.Drawing.Size(80, 30);
            this.btnConfirm.Text = "Confirm";
            this.btnConfirm.Click += new System.EventHandler(this.btnConfirm_Click);
            //
            // btnCancel
            //
            this.btnCancel.Location = new System.Drawing.Point(295, 245);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(80, 30);
            this.btnCancel.Text = "Cancel";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            //
            // AutoEmailConfirmationForm
            //
            this.ClientSize = new System.Drawing.Size(384, 290);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnConfirm);
            this.Controls.Add(this.txtPlanDetails);
            this.Controls.Add(this.lblPlanDetails);
            this.Controls.Add(this.txtParticipants);
            this.Controls.Add(this.lblParticipants);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Auto Email Confirmation";
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}
