using G_Tara.Models;
using System.Text;
using System.Windows.Forms;

namespace G_Tara
{
    public partial class AutoEmailConfirmationForm : Form
    {
        public AutoEmailConfirmationForm(Gala gala, string hostEmail)
        {
            InitializeComponent();
            txtPlanDetails.Text = gala.Plan;
            txtParticipants.Text = string.Join("\r\n", gala.Participants.Select(p => p.Email));
            txtHostEmail.Text = string.IsNullOrWhiteSpace(hostEmail) ? "Not set" : hostEmail;
        }

        public bool IsConfirmed => DialogResult == DialogResult.OK;

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
