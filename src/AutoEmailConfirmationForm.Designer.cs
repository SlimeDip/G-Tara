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
            this.lblParticipants = new System.Windows.Forms.Label();
            this.txtParticipants = new System.Windows.Forms.TextBox();
            this.lblPlanDetails = new System.Windows.Forms.Label();
            this.txtPlanDetails = new System.Windows.Forms.TextBox();
            this.btnConfirm = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lblParticipants
            // 
            this.lblParticipants.AutoSize = true;
            this.lblParticipants.Location = new System.Drawing.Point(12, 9);
            this.lblParticipants.Name = "lblParticipants";
            this.lblParticipants.Size = new System.Drawing.Size(120, 15);
            this.lblParticipants.TabIndex = 0;
            this.lblParticipants.Text = "Participants' Gmails:";
            // 
            // txtParticipants
            // 
            this.txtParticipants.Location = new System.Drawing.Point(12, 27);
            this.txtParticipants.Multiline = true;
            this.txtParticipants.Name = "txtParticipants";
            this.txtParticipants.ReadOnly = true;
            this.txtParticipants.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtParticipants.Size = new System.Drawing.Size(360, 80);
            this.txtParticipants.TabIndex = 1;
            // 
            // lblPlanDetails
            // 
            this.lblPlanDetails.AutoSize = true;
            this.lblPlanDetails.Location = new System.Drawing.Point(12, 120);
            this.lblPlanDetails.Name = "lblPlanDetails";
            this.lblPlanDetails.Size = new System.Drawing.Size(71, 15);
            this.lblPlanDetails.TabIndex = 2;
            this.lblPlanDetails.Text = "Plan Details:";
            // 
            // txtPlanDetails
            // 
            this.txtPlanDetails.Location = new System.Drawing.Point(12, 138);
            this.txtPlanDetails.Multiline = true;
            this.txtPlanDetails.Name = "txtPlanDetails";
            this.txtPlanDetails.ReadOnly = true;
            this.txtPlanDetails.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtPlanDetails.Size = new System.Drawing.Size(360, 80);
            this.txtPlanDetails.TabIndex = 3;
            // 
            // btnConfirm
            // 
            this.btnConfirm.Location = new System.Drawing.Point(216, 230);
            this.btnConfirm.Name = "btnConfirm";
            this.btnConfirm.Size = new System.Drawing.Size(75, 23);
            this.btnConfirm.TabIndex = 4;
            this.btnConfirm.Text = "Confirm";
            this.btnConfirm.UseVisualStyleBackColor = true;
            this.btnConfirm.Click += new System.EventHandler(this.btnConfirm_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(297, 230);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 5;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // AutoEmailConfirmationForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(384, 261);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnConfirm);
            this.Controls.Add(this.txtPlanDetails);
            this.Controls.Add(this.lblPlanDetails);
            this.Controls.Add(this.txtParticipants);
            this.Controls.Add(this.lblParticipants);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AutoEmailConfirmationForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Auto Email Confirmation";
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}
