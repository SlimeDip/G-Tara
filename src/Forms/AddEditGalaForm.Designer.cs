namespace G_Tara
{
    partial class AddEditGalaForm
    {
        private System.ComponentModel.IContainer components = null;

        private TableLayoutPanel container;
        private Panel scrollPanel;
        private TableLayoutPanel mainPanel;
        private FlowLayoutPanel buttonPanel;

        private Label lblName;
        private TextBox txtName;
        private Label lblDate;
        private Panel datePanel;
        private TextBox txtDateRange;
        private Button btnPickRange;
        private Label lblStatus;
        private ComboBox cmbStatus;
        private Label lblLocation;
        private FlowLayoutPanel locationPanel;
        private TextBox txtLocation;
        private Button btnPickMap;
        private Label lblCategory;
        private ComboBox cmbCategory;
        private Label lblSearchResults;
        private ListBox lstSearchResults;
        private Button btnAdd;
        private Label lblSelected;
        private ListBox lstSelectedLocations;
        private Button btnRemove;
        private Label lblWeatherTitle;
        private FlowLayoutPanel weatherPanel;
        private Button btnGetWeather;
        private Label lblWeather;
        private Label lblPlan;
        private TextBox txtPlan;
        private Button btnSave;
        private Button btnCancel;
        private Button btnDelete;
        private Button btnRefreshPlaces;
        private Label lblParticipants;
        private CheckedListBox chkParticipants;
        private Label lblHost;
        private ComboBox cmbHost;
        private PictureBox picCalendar;
        private PictureBox picCheckmark;
        private PictureBox picLocation;
        private PictureBox picCategory;
        private PictureBox picLogo;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            container = new TableLayoutPanel();
            scrollPanel = new Panel();
            mainPanel = new TableLayoutPanel();
            leftPanel = new Panel();
            picCalendar = new PictureBox();
            lblName = new Label();
            txtName = new TextBox();
            lblDate = new Label();
            datePanel = new Panel();
            txtDateRange = new TextBox();
            btnPickRange = new Button();
            lblStatus = new Label();
            cmbStatus = new ComboBox();
            lblLocation = new Label();
            locationPanel = new FlowLayoutPanel();
            txtLocation = new TextBox();
            btnPickMap = new Button();
            lblCategory = new Label();
            cmbCategory = new ComboBox();
            lblSearchResults = new Label();
            lstSearchResults = new ListBox();
            btnRefreshPlaces = new Button();
            btnAdd = new Button();
            lblSelected = new Label();
            lstSelectedLocations = new ListBox();
            btnRemove = new Button();
            lblWeatherTitle = new Label();
            weatherPanel = new FlowLayoutPanel();
            btnGetWeather = new Button();
            lblWeather = new Label();
            rightPanel = new Panel();
            lblHost = new Label();
            cmbHost = new ComboBox();
            participantsPanel = new Panel();
            lblParticipants = new Label();
            chkParticipants = new CheckedListBox();
            planPanel = new Panel();
            lblPlan = new Label();
            txtPlan = new TextBox();
            buttonPanel = new FlowLayoutPanel();
            btnSave = new Button();
            btnCancel = new Button();
            btnDelete = new Button();
            picCheckmark = new PictureBox();
            picLocation = new PictureBox();
            picCategory = new PictureBox();
            picLogo = new PictureBox();
            container.SuspendLayout();
            scrollPanel.SuspendLayout();
            mainPanel.SuspendLayout();
            leftPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)picCalendar).BeginInit();
            datePanel.SuspendLayout();
            locationPanel.SuspendLayout();
            weatherPanel.SuspendLayout();
            rightPanel.SuspendLayout();
            participantsPanel.SuspendLayout();
            planPanel.SuspendLayout();
            buttonPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)picCheckmark).BeginInit();
            ((System.ComponentModel.ISupportInitialize)picLocation).BeginInit();
            ((System.ComponentModel.ISupportInitialize)picCategory).BeginInit();
            ((System.ComponentModel.ISupportInitialize)picLogo).BeginInit();
            SuspendLayout();
            // 
            // container
            // 
            container.BackColor = Color.FromArgb(243, 198, 208);
            container.ColumnCount = 1;
            container.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            container.Controls.Add(scrollPanel, 0, 0);
            container.Controls.Add(buttonPanel, 0, 1);
            container.Dock = DockStyle.Fill;
            container.Location = new Point(0, 0);
            container.Margin = new Padding(0);
            container.Name = "container";
            container.RowCount = 2;
            container.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            container.RowStyles.Add(new RowStyle(SizeType.Absolute, 64F));
            container.Size = new Size(920, 640);
            container.TabIndex = 0;
            // 
            // scrollPanel
            // 
            scrollPanel.AutoScroll = true;
            scrollPanel.BackColor = Color.FromArgb(243, 198, 208);
            scrollPanel.Controls.Add(mainPanel);
            scrollPanel.Dock = DockStyle.Fill;
            scrollPanel.Location = new Point(0, 0);
            scrollPanel.Margin = new Padding(0);
            scrollPanel.Name = "scrollPanel";
            scrollPanel.Size = new Size(920, 576);
            scrollPanel.TabIndex = 0;
            // 
            // mainPanel
            // 
            mainPanel.ColumnCount = 2;
            mainPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 54F));
            mainPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 46F));
            mainPanel.Controls.Add(leftPanel, 0, 0);
            mainPanel.Controls.Add(rightPanel, 1, 0);
            mainPanel.Dock = DockStyle.Top;
            mainPanel.Location = new Point(0, 0);
            mainPanel.Name = "mainPanel";
            mainPanel.Padding = new Padding(14, 10, 14, 8);
            mainPanel.RowCount = 1;
            mainPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            mainPanel.Size = new Size(903, 680);
            mainPanel.TabIndex = 0;
            // 
            // leftPanel
            // 
            leftPanel.BackColor = Color.FromArgb(243, 198, 208);
            leftPanel.Controls.Add(picCalendar);
            leftPanel.Controls.Add(lblName);
            leftPanel.Controls.Add(txtName);
            leftPanel.Controls.Add(lblDate);
            leftPanel.Controls.Add(datePanel);
            leftPanel.Controls.Add(lblStatus);
            leftPanel.Controls.Add(cmbStatus);
            leftPanel.Controls.Add(lblLocation);
            leftPanel.Controls.Add(locationPanel);
            leftPanel.Controls.Add(lblCategory);
            leftPanel.Controls.Add(cmbCategory);
            leftPanel.Controls.Add(lblSearchResults);
            leftPanel.Controls.Add(lstSearchResults);
            leftPanel.Controls.Add(btnRefreshPlaces);
            leftPanel.Controls.Add(btnAdd);
            leftPanel.Controls.Add(lblSelected);
            leftPanel.Controls.Add(lstSelectedLocations);
            leftPanel.Controls.Add(btnRemove);
            leftPanel.Controls.Add(lblWeatherTitle);
            leftPanel.Controls.Add(weatherPanel);
            leftPanel.Dock = DockStyle.Fill;
            leftPanel.Location = new Point(17, 13);
            leftPanel.Name = "leftPanel";
            leftPanel.Size = new Size(466, 656);
            leftPanel.TabIndex = 0;
            // 
            // picCalendar
            // 
            picCalendar.Location = new Point(0, 0);
            picCalendar.Name = "picCalendar";
            picCalendar.Size = new Size(100, 50);
            picCalendar.TabIndex = 0;
            picCalendar.TabStop = false;
            picCalendar.Visible = false;
            // 
            // lblName
            // 
            lblName.AutoSize = true;
            lblName.Font = new Font("Segoe UI Semibold", 9.2F);
            lblName.ForeColor = Color.FromArgb(94, 49, 58);
            lblName.Location = new Point(12, 10);
            lblName.Name = "lblName";
            lblName.Size = new Size(77, 17);
            lblName.TabIndex = 0;
            lblName.Text = "Gala Name:";
            // 
            // txtName
            // 
            txtName.BackColor = Color.FromArgb(255, 246, 248);
            txtName.Font = new Font("Segoe UI", 8.75F);
            txtName.Location = new Point(12, 30);
            txtName.Name = "txtName";
            txtName.Size = new Size(440, 23);
            txtName.TabIndex = 1;
            // 
            // lblDate
            // 
            lblDate.AutoSize = true;
            lblDate.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold);
            lblDate.ForeColor = Color.FromArgb(94, 49, 58);
            lblDate.Location = new Point(12, 66);
            lblDate.Name = "lblDate";
            lblDate.Size = new Size(75, 15);
            lblDate.TabIndex = 2;
            lblDate.Text = "EVENT DATE";
            // 
            // datePanel
            // 
            datePanel.Controls.Add(txtDateRange);
            datePanel.Controls.Add(btnPickRange);
            datePanel.Location = new Point(12, 86);
            datePanel.Name = "datePanel";
            datePanel.Size = new Size(310, 32);
            datePanel.TabIndex = 3;
            // 
            // txtDateRange
            // 
            txtDateRange.Font = new Font("Segoe UI", 8.75F);
            txtDateRange.Location = new Point(3, 3);
            txtDateRange.Name = "txtDateRange";
            txtDateRange.ReadOnly = true;
            txtDateRange.Size = new Size(176, 23);
            txtDateRange.TabIndex = 0;
            // 
            // btnPickRange
            // 
            btnPickRange.Location = new Point(185, 3);
            btnPickRange.Name = "btnPickRange";
            btnPickRange.Size = new Size(110, 26);
            btnPickRange.TabIndex = 1;
            btnPickRange.Text = "Manage Dates";
            btnPickRange.UseVisualStyleBackColor = true;
            // 
            // lblStatus
            // 
            lblStatus.AutoSize = true;
            lblStatus.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold);
            lblStatus.ForeColor = Color.FromArgb(94, 49, 58);
            lblStatus.Location = new Point(12, 126);
            lblStatus.Name = "lblStatus";
            lblStatus.Size = new Size(52, 15);
            lblStatus.TabIndex = 4;
            lblStatus.Text = "STATUS:";
            // 
            // cmbStatus
            // 
            cmbStatus.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbStatus.FormattingEnabled = true;
            cmbStatus.Items.AddRange(new object[] { "Planned", "Confirmed", "Done", "Cancelled" });
            cmbStatus.Location = new Point(12, 146);
            cmbStatus.Name = "cmbStatus";
            cmbStatus.Size = new Size(200, 23);
            cmbStatus.TabIndex = 5;
            // 
            // lblLocation
            // 
            lblLocation.AutoSize = true;
            lblLocation.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold);
            lblLocation.ForeColor = Color.FromArgb(94, 49, 58);
            lblLocation.Location = new Point(12, 180);
            lblLocation.Name = "lblLocation";
            lblLocation.Size = new Size(88, 15);
            lblLocation.TabIndex = 6;
            lblLocation.Text = "LOCATION PIN";
            // 
            // locationPanel
            // 
            locationPanel.Controls.Add(txtLocation);
            locationPanel.Controls.Add(btnPickMap);
            locationPanel.Location = new Point(12, 200);
            locationPanel.Name = "locationPanel";
            locationPanel.Size = new Size(350, 56);
            locationPanel.TabIndex = 7;
            // 
            // txtLocation
            // 
            txtLocation.Font = new Font("Segoe UI", 8.75F);
            txtLocation.Location = new Point(3, 3);
            txtLocation.Name = "txtLocation";
            txtLocation.Size = new Size(220, 23);
            txtLocation.TabIndex = 0;
            // 
            // btnPickMap
            // 
            btnPickMap.Location = new Point(229, 3);
            btnPickMap.Name = "btnPickMap";
            btnPickMap.Size = new Size(92, 26);
            btnPickMap.TabIndex = 1;
            btnPickMap.Text = "Pin Venue";
            btnPickMap.UseVisualStyleBackColor = true;
            // 
            // lblCategory
            // 
            lblCategory.AutoSize = true;
            lblCategory.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold);
            lblCategory.ForeColor = Color.FromArgb(94, 49, 58);
            lblCategory.Location = new Point(12, 264);
            lblCategory.Name = "lblCategory";
            lblCategory.Size = new Size(65, 15);
            lblCategory.TabIndex = 8;
            lblCategory.Text = "CATEGORY";
            // 
            // cmbCategory
            // 
            cmbCategory.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbCategory.FormattingEnabled = true;
            cmbCategory.Items.AddRange(new object[] { "Hotel", "Resort", "Cafe", "Restaurant", "Bar" });
            cmbCategory.Location = new Point(12, 284);
            cmbCategory.Name = "cmbCategory";
            cmbCategory.Size = new Size(176, 23);
            cmbCategory.TabIndex = 9;
            // 
            // lblSearchResults
            // 
            lblSearchResults.AutoSize = true;
            lblSearchResults.Font = new Font("Segoe UI Semibold", 8.5F, FontStyle.Bold);
            lblSearchResults.ForeColor = Color.FromArgb(94, 49, 58);
            lblSearchResults.Location = new Point(8, 8);
            lblSearchResults.Name = "lblSearchResults";
            lblSearchResults.Size = new Size(182, 15);
            lblSearchResults.TabIndex = 0;
            lblSearchResults.Text = "SEARCH & DISCOVER LOCATIONS";
            // 
            // lstSearchResults
            // 
            lstSearchResults.BackColor = Color.FromArgb(255, 246, 248);
            lstSearchResults.FormattingEnabled = true;
            lstSearchResults.Location = new Point(8, 28);
            lstSearchResults.Name = "lstSearchResults";
            lstSearchResults.Size = new Size(310, 64);
            lstSearchResults.TabIndex = 1;
            // 
            // btnRefreshPlaces
            // 
            btnRefreshPlaces.Location = new Point(8, 110);
            btnRefreshPlaces.Name = "btnRefreshPlaces";
            btnRefreshPlaces.Size = new Size(130, 28);
            btnRefreshPlaces.TabIndex = 2;
            btnRefreshPlaces.Text = "Refresh Locations";
            btnRefreshPlaces.UseVisualStyleBackColor = true;
            // 
            // btnAdd
            // 
            btnAdd.Location = new Point(146, 110);
            btnAdd.Name = "btnAdd";
            btnAdd.Size = new Size(120, 28);
            btnAdd.TabIndex = 3;
            btnAdd.Text = "Add Location";
            btnAdd.UseVisualStyleBackColor = true;
            // 
            // lblSelected
            // 
            lblSelected.AutoSize = true;
            lblSelected.Font = new Font("Segoe UI Semibold", 8.6F, FontStyle.Bold);
            lblSelected.ForeColor = Color.FromArgb(94, 49, 58);
            lblSelected.Location = new Point(12, 474);
            lblSelected.Name = "lblSelected";
            lblSelected.Size = new Size(109, 15);
            lblSelected.TabIndex = 11;
            lblSelected.Text = "Selected Locations:";
            // 
            // lstSelectedLocations
            // 
            lstSelectedLocations.BackColor = Color.FromArgb(255, 246, 248);
            lstSelectedLocations.FormattingEnabled = true;
            lstSelectedLocations.Location = new Point(12, 494);
            lstSelectedLocations.Name = "lstSelectedLocations";
            lstSelectedLocations.Size = new Size(310, 49);
            lstSelectedLocations.TabIndex = 12;
            // 
            // btnRemove
            // 
            btnRemove.Location = new Point(12, 548);
            btnRemove.Name = "btnRemove";
            btnRemove.Size = new Size(130, 28);
            btnRemove.TabIndex = 13;
            btnRemove.Text = "Remove Location";
            btnRemove.UseVisualStyleBackColor = true;
            // 
            // lblWeatherTitle
            // 
            lblWeatherTitle.AutoSize = true;
            lblWeatherTitle.Font = new Font("Segoe UI Semibold", 8.8F, FontStyle.Bold);
            lblWeatherTitle.ForeColor = Color.FromArgb(94, 49, 58);
            lblWeatherTitle.Location = new Point(12, 586);
            lblWeatherTitle.Name = "lblWeatherTitle";
            lblWeatherTitle.Size = new Size(100, 15);
            lblWeatherTitle.TabIndex = 14;
            lblWeatherTitle.Text = "CHECK WEATHER";
            // 
            // weatherPanel
            // 
            weatherPanel.Controls.Add(btnGetWeather);
            weatherPanel.Controls.Add(lblWeather);
            weatherPanel.Location = new Point(12, 606);
            weatherPanel.Name = "weatherPanel";
            weatherPanel.Size = new Size(440, 34);
            weatherPanel.TabIndex = 15;
            // 
            // btnGetWeather
            // 
            btnGetWeather.Location = new Point(3, 3);
            btnGetWeather.Name = "btnGetWeather";
            btnGetWeather.Size = new Size(120, 28);
            btnGetWeather.TabIndex = 0;
            btnGetWeather.Text = "Check Weather";
            btnGetWeather.UseVisualStyleBackColor = true;
            // 
            // lblWeather
            // 
            lblWeather.AutoSize = true;
            lblWeather.Font = new Font("Segoe UI", 8.2F);
            lblWeather.ForeColor = Color.FromArgb(94, 49, 58);
            lblWeather.Location = new Point(129, 0);
            lblWeather.Name = "lblWeather";
            lblWeather.Size = new Size(93, 13);
            lblWeather.TabIndex = 1;
            lblWeather.Text = "No weather data";
            // 
            // rightPanel
            // 
            rightPanel.BackColor = Color.FromArgb(243, 198, 208);
            rightPanel.Controls.Add(participantsPanel);
            rightPanel.Controls.Add(planPanel);
            rightPanel.Dock = DockStyle.Fill;
            rightPanel.Location = new Point(489, 13);
            rightPanel.Name = "rightPanel";
            rightPanel.Padding = new Padding(10, 0, 0, 0);
            rightPanel.Size = new Size(397, 656);
            rightPanel.TabIndex = 1;
            // 
            // lblHost
            // 
            lblHost.AutoSize = true;
            lblHost.Font = new Font("Segoe UI Semibold", 8.9F);
            lblHost.ForeColor = Color.FromArgb(94, 49, 58);
            lblHost.Location = new Point(14, 13);
            lblHost.Name = "lblHost";
            lblHost.Size = new Size(61, 15);
            lblHost.TabIndex = 0;
            lblHost.Text = "Gala Host:";
            // 
            // cmbHost
            // 
            cmbHost.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbHost.FormattingEnabled = true;
            cmbHost.Location = new Point(85, 10);
            cmbHost.Name = "cmbHost";
            cmbHost.Size = new Size(290, 23);
            cmbHost.TabIndex = 1;
            // 
            // participantsPanel
            // 
            participantsPanel.BackColor = Color.FromArgb(249, 230, 235);
            participantsPanel.BorderStyle = BorderStyle.FixedSingle;
            participantsPanel.Controls.Add(lblParticipants);
            participantsPanel.Controls.Add(lblHost);
            participantsPanel.Controls.Add(cmbHost);
            participantsPanel.Controls.Add(chkParticipants);
            participantsPanel.Location = new Point(10, 10);
            participantsPanel.Name = "participantsPanel";
            participantsPanel.Size = new Size(370, 310);
            participantsPanel.TabIndex = 2;
            // 
            // lblParticipants
            // 
            lblParticipants.AutoSize = true;
            lblParticipants.Font = new Font("Segoe UI Semibold", 9F);
            lblParticipants.ForeColor = Color.FromArgb(94, 49, 58);
            lblParticipants.Location = new Point(14, 12);
            lblParticipants.Name = "lblParticipants";
            lblParticipants.Size = new Size(87, 15);
            lblParticipants.TabIndex = 0;
            lblParticipants.Text = "PARTICIPANTS";
            // 
            // chkParticipants
            // 
            chkParticipants.FormattingEnabled = true;
            chkParticipants.Location = new Point(14, 34);
            chkParticipants.Name = "chkParticipants";
            chkParticipants.Size = new Size(344, 256);
            chkParticipants.TabIndex = 1;
            // 
            // planPanel
            // 
            planPanel.BackColor = Color.FromArgb(249, 230, 235);
            planPanel.BorderStyle = BorderStyle.FixedSingle;
            planPanel.Controls.Add(lblPlan);
            planPanel.Controls.Add(txtPlan);
            planPanel.Location = new Point(10, 364);
            planPanel.Name = "planPanel";
            planPanel.Size = new Size(374, 200);
            planPanel.TabIndex = 3;
            // 
            // lblPlan
            // 
            lblPlan.AutoSize = true;
            lblPlan.Font = new Font("Segoe UI Semibold", 9F);
            lblPlan.ForeColor = Color.FromArgb(94, 49, 58);
            lblPlan.Location = new Point(14, 12);
            lblPlan.Name = "lblPlan";
            lblPlan.Size = new Size(91, 15);
            lblPlan.TabIndex = 0;
            lblPlan.Text = "PLAN & AGENDA";
            // 
            // txtPlan
            // 
            txtPlan.BackColor = Color.FromArgb(255, 246, 248);
            txtPlan.Location = new Point(14, 34);
            txtPlan.Multiline = true;
            txtPlan.Name = "txtPlan";
            txtPlan.ScrollBars = ScrollBars.Vertical;
            txtPlan.Size = new Size(344, 148);
            txtPlan.TabIndex = 1;
            // 
            // buttonPanel
            // 
            buttonPanel.BackColor = Color.FromArgb(197, 125, 143);
            buttonPanel.Controls.Add(btnSave);
            buttonPanel.Controls.Add(btnCancel);
            buttonPanel.Controls.Add(btnDelete);
            buttonPanel.Dock = DockStyle.Fill;
            buttonPanel.Location = new Point(0, 576);
            buttonPanel.Margin = new Padding(0);
            buttonPanel.Name = "buttonPanel";
            buttonPanel.Padding = new Padding(24, 12, 0, 0);
            buttonPanel.Size = new Size(920, 64);
            buttonPanel.TabIndex = 1;
            buttonPanel.WrapContents = false;
            // 
            // btnSave
            // 
            btnSave.Font = new Font("Segoe UI Semibold", 8.8F, FontStyle.Bold);
            btnSave.Location = new Point(28, 12);
            btnSave.Margin = new Padding(4, 0, 10, 0);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(88, 30);
            btnSave.TabIndex = 0;
            btnSave.Text = "SAVE";
            btnSave.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            btnCancel.Font = new Font("Segoe UI Semibold", 8.8F, FontStyle.Bold);
            btnCancel.Location = new Point(126, 12);
            btnCancel.Margin = new Padding(0, 0, 10, 0);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(88, 30);
            btnCancel.TabIndex = 1;
            btnCancel.Text = "CANCEL";
            btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnDelete
            // 
            btnDelete.Font = new Font("Segoe UI Semibold", 8.8F, FontStyle.Bold);
            btnDelete.ForeColor = Color.FromArgb(169, 70, 82);
            btnDelete.Location = new Point(224, 12);
            btnDelete.Margin = new Padding(0);
            btnDelete.Name = "btnDelete";
            btnDelete.Size = new Size(88, 30);
            btnDelete.TabIndex = 2;
            btnDelete.Text = "DELETE";
            btnDelete.UseVisualStyleBackColor = true;
            // 
            // picCheckmark
            // 
            picCheckmark.Location = new Point(0, 0);
            picCheckmark.Name = "picCheckmark";
            picCheckmark.Size = new Size(100, 50);
            picCheckmark.TabIndex = 0;
            picCheckmark.TabStop = false;
            picCheckmark.Visible = false;
            // 
            // picLocation
            // 
            picLocation.Location = new Point(0, 0);
            picLocation.Name = "picLocation";
            picLocation.Size = new Size(100, 50);
            picLocation.TabIndex = 0;
            picLocation.TabStop = false;
            picLocation.Visible = false;
            // 
            // picCategory
            // 
            picCategory.Location = new Point(0, 0);
            picCategory.Name = "picCategory";
            picCategory.Size = new Size(100, 50);
            picCategory.TabIndex = 0;
            picCategory.TabStop = false;
            picCategory.Visible = false;
            // 
            // picLogo
            // 
            picLogo.Location = new Point(0, 0);
            picLogo.Name = "picLogo";
            picLogo.Size = new Size(100, 50);
            picLogo.TabIndex = 0;
            picLogo.TabStop = false;
            picLogo.Visible = false;
            // 
            // AddEditGalaForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(243, 198, 208);
            ClientSize = new Size(920, 640);
            Controls.Add(container);
            Font = new Font("Segoe UI", 8.75F);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "AddEditGalaForm";
            StartPosition = FormStartPosition.CenterParent;
            Text = "ADD/EDIT GALA DETAILS";
            Load += AddEditGalaForm_Load;
            container.ResumeLayout(false);
            scrollPanel.ResumeLayout(false);
            mainPanel.ResumeLayout(false);
            leftPanel.ResumeLayout(false);
            leftPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)picCalendar).EndInit();
            datePanel.ResumeLayout(false);
            datePanel.PerformLayout();
            locationPanel.ResumeLayout(false);
            locationPanel.PerformLayout();
            weatherPanel.ResumeLayout(false);
            weatherPanel.PerformLayout();
            rightPanel.ResumeLayout(false);
            rightPanel.PerformLayout();
            participantsPanel.ResumeLayout(false);
            participantsPanel.PerformLayout();
            planPanel.ResumeLayout(false);
            planPanel.PerformLayout();
            buttonPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)picCheckmark).EndInit();
            ((System.ComponentModel.ISupportInitialize)picLocation).EndInit();
            ((System.ComponentModel.ISupportInitialize)picCategory).EndInit();
            ((System.ComponentModel.ISupportInitialize)picLogo).EndInit();
            ResumeLayout(false);
        }

        private Panel leftPanel;
        private Panel rightPanel;
        private Panel participantsPanel;
        private Panel planPanel;

    }
}
