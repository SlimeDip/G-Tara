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
        private DateTimePicker dtpDate;
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

        private Button btnRefreshPlaces;

        private Label lblParticipants;
        private CheckedListBox chkParticipants;

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
            container = new TableLayoutPanel();
            scrollPanel = new Panel();
            mainPanel = new TableLayoutPanel();
            lblName = new Label();
            txtName = new TextBox();
            lblDate = new Label();
            dtpDate = new DateTimePicker();
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
            btnAdd = new Button();
            lblSelected = new Label();
            lstSelectedLocations = new ListBox();
            btnRemove = new Button();
            lblWeatherTitle = new Label();
            weatherPanel = new FlowLayoutPanel();
            btnGetWeather = new Button();
            lblWeather = new Label();
            lblPlan = new Label();
            txtPlan = new TextBox();
            btnRefreshPlaces = new Button();
            lblParticipants = new Label();
            chkParticipants = new CheckedListBox();
            buttonPanel = new FlowLayoutPanel();
            btnSave = new Button();
            btnCancel = new Button();
            container.SuspendLayout();
            scrollPanel.SuspendLayout();
            mainPanel.SuspendLayout();
            locationPanel.SuspendLayout();
            weatherPanel.SuspendLayout();
            buttonPanel.SuspendLayout();
            SuspendLayout();
            // 
            // container
            // 
            container.ColumnCount = 1;
            container.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            container.Controls.Add(scrollPanel, 0, 0);
            container.Controls.Add(buttonPanel, 0, 1);
            container.Dock = DockStyle.Fill;
            container.Location = new Point(0, 0);
            container.Name = "container";
            container.RowCount = 2;
            container.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            container.RowStyles.Add(new RowStyle(SizeType.Absolute, 52F));
            container.Size = new Size(700, 1050);
            container.TabIndex = 0;
            // 
            // scrollPanel
            // 
            scrollPanel.AutoScroll = true;
            scrollPanel.Controls.Add(mainPanel);
            scrollPanel.Dock = DockStyle.Fill;
            scrollPanel.Location = new Point(3, 3);
            scrollPanel.Name = "scrollPanel";
            scrollPanel.Size = new Size(694, 702);
            scrollPanel.TabIndex = 0;
            // 
            // mainPanel
            // 
            mainPanel.AutoSize = true;
            mainPanel.ColumnCount = 2;
            mainPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 150F));
            mainPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            mainPanel.Controls.Add(lblName, 0, 0);
            mainPanel.Controls.Add(txtName, 1, 0);
            mainPanel.Controls.Add(lblDate, 0, 1);
            mainPanel.Controls.Add(dtpDate, 1, 1);
            mainPanel.Controls.Add(lblStatus, 0, 2);
            mainPanel.Controls.Add(cmbStatus, 1, 2);
            mainPanel.Controls.Add(lblLocation, 0, 3);
            mainPanel.Controls.Add(locationPanel, 1, 3);
            mainPanel.Controls.Add(lblCategory, 0, 4);
            mainPanel.Controls.Add(cmbCategory, 1, 4);
            mainPanel.Controls.Add(lblSearchResults, 0, 5);
            mainPanel.Controls.Add(lstSearchResults, 1, 5);
            mainPanel.Controls.Add(btnRefreshPlaces, 0, 6);
            mainPanel.Controls.Add(btnAdd, 1, 6);
            mainPanel.Controls.Add(lblSelected, 0, 7);
            mainPanel.Controls.Add(lstSelectedLocations, 1, 7);
            mainPanel.Controls.Add(btnRemove, 1, 8);
            mainPanel.Controls.Add(lblWeatherTitle, 0, 9);
            mainPanel.Controls.Add(weatherPanel, 1, 9);
            mainPanel.Controls.Add(lblPlan, 0, 10);
            mainPanel.Controls.Add(txtPlan, 1, 10);
            mainPanel.Controls.Add(lblParticipants, 0, 11);
            mainPanel.Controls.Add(chkParticipants, 1, 11);
            mainPanel.Dock = DockStyle.Top;
            mainPanel.Location = new Point(0, 0);
            mainPanel.Name = "mainPanel";
            mainPanel.Padding = new Padding(12);
            mainPanel.RowCount = 12;
            mainPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 42F));
            mainPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 42F));
            mainPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 42F));
            mainPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 78F));
            mainPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 29F));
            mainPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 135F));
            mainPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 42F));
            mainPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 150F));
            mainPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 42F));
            mainPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 56F));
            mainPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 170F));
            mainPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 170F));
            mainPanel.Size = new Size(677, 850);
            mainPanel.TabIndex = 0;
            // 
            // lblName
            // 
            lblName.Anchor = AnchorStyles.Left;
            lblName.AutoSize = true;
            lblName.Location = new Point(15, 25);
            lblName.Name = "lblName";
            lblName.Size = new Size(68, 15);
            lblName.TabIndex = 0;
            lblName.Text = "Gala Name:";
            // 
            // txtName
            // 
            txtName.Dock = DockStyle.Fill;
            txtName.Location = new Point(165, 15);
            txtName.Name = "txtName";
            txtName.Size = new Size(497, 23);
            txtName.TabIndex = 1;
            // 
            // lblDate
            // 
            lblDate.Anchor = AnchorStyles.Left;
            lblDate.AutoSize = true;
            lblDate.Location = new Point(15, 67);
            lblDate.Name = "lblDate";
            lblDate.Size = new Size(92, 15);
            lblDate.TabIndex = 2;
            lblDate.Text = "Scheduled Date:";
            // 
            // dtpDate
            // 
            dtpDate.Dock = DockStyle.Left;
            dtpDate.Location = new Point(165, 57);
            dtpDate.Name = "dtpDate";
            dtpDate.Size = new Size(260, 23);
            dtpDate.TabIndex = 3;
            // 
            // lblStatus
            // 
            lblStatus.Anchor = AnchorStyles.Left;
            lblStatus.AutoSize = true;
            lblStatus.Location = new Point(15, 109);
            lblStatus.Name = "lblStatus";
            lblStatus.Size = new Size(42, 15);
            lblStatus.TabIndex = 4;
            lblStatus.Text = "Status:";
            // 
            // cmbStatus
            // 
            cmbStatus.Dock = DockStyle.Left;
            cmbStatus.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbStatus.FormattingEnabled = true;
            cmbStatus.Items.AddRange(new object[] { "Planned", "Confirmed", "Done", "Cancelled" });
            cmbStatus.Location = new Point(165, 99);
            cmbStatus.Name = "cmbStatus";
            cmbStatus.Size = new Size(180, 23);
            cmbStatus.TabIndex = 5;
            // 
            // lblLocation
            // 
            lblLocation.Anchor = AnchorStyles.Left;
            lblLocation.AutoSize = true;
            lblLocation.Location = new Point(15, 169);
            lblLocation.Name = "lblLocation";
            lblLocation.Size = new Size(56, 15);
            lblLocation.TabIndex = 6;
            lblLocation.Text = "Location:";
            // 
            // locationPanel
            // 
            locationPanel.Controls.Add(txtLocation);
            locationPanel.Controls.Add(btnPickMap);
            locationPanel.Dock = DockStyle.Fill;
            locationPanel.Location = new Point(165, 141);
            locationPanel.Name = "locationPanel";
            locationPanel.Size = new Size(497, 72);
            locationPanel.TabIndex = 7;
            // 
            // txtLocation
            // 
            txtLocation.Location = new Point(3, 3);
            txtLocation.Name = "txtLocation";
            txtLocation.Size = new Size(320, 23);
            txtLocation.TabIndex = 0;
            // 
            // btnPickMap
            // 
            btnPickMap.Location = new Point(329, 3);
            btnPickMap.Name = "btnPickMap";
            btnPickMap.Size = new Size(90, 27);
            btnPickMap.TabIndex = 3;
            btnPickMap.Text = "Pin on Map";
            btnPickMap.UseVisualStyleBackColor = true;
            // 
            // lblCategory
            // 
            lblCategory.Anchor = AnchorStyles.Left;
            lblCategory.AutoSize = true;
            lblCategory.Location = new Point(15, 223);
            lblCategory.Name = "lblCategory";
            lblCategory.Size = new Size(58, 15);
            lblCategory.TabIndex = 8;
            lblCategory.Text = "Category:";
            // 
            // cmbCategory
            // 
            cmbCategory.Dock = DockStyle.Left;
            cmbCategory.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbCategory.FormattingEnabled = true;
            cmbCategory.Items.AddRange(new object[] { "Hotel", "Resort", "Cafe", "Restaurant", "Bar" });
            cmbCategory.Location = new Point(165, 219);
            cmbCategory.Name = "cmbCategory";
            cmbCategory.Size = new Size(180, 23);
            cmbCategory.TabIndex = 9;
            // 
            // lblSearchResults
            // 
            lblSearchResults.Anchor = AnchorStyles.Left;
            lblSearchResults.AutoSize = true;
            lblSearchResults.Location = new Point(15, 305);
            lblSearchResults.Name = "lblSearchResults";
            lblSearchResults.Size = new Size(112, 15);
            lblSearchResults.TabIndex = 10;
            lblSearchResults.Text = "Available Locations:";
            // 
            // lstSearchResults
            // 
            lstSearchResults.Dock = DockStyle.Fill;
            lstSearchResults.FormattingEnabled = true;
            lstSearchResults.Location = new Point(165, 248);
            lstSearchResults.Name = "lstSearchResults";
            lstSearchResults.Size = new Size(497, 129);
            lstSearchResults.TabIndex = 11;
            // 
            // btnAdd
            // 
            btnAdd.Anchor = AnchorStyles.Left;
            btnAdd.Location = new Point(165, 386);
            btnAdd.Name = "btnAdd";
            btnAdd.Size = new Size(120, 30);
            btnAdd.TabIndex = 12;
            btnAdd.Text = "Add Location";
            btnAdd.UseVisualStyleBackColor = true;
            btnAdd.Click += OnAddLocation;
            // 
            // btnRefreshPlaces
            // 
            btnRefreshPlaces.Anchor = AnchorStyles.Left;
            btnRefreshPlaces.Location = new Point(15, 386);
            btnRefreshPlaces.Name = "btnRefreshPlaces";
            btnRefreshPlaces.Size = new Size(120, 30);
            btnRefreshPlaces.TabIndex = 21;
            btnRefreshPlaces.Text = "Refresh Locations";
            btnRefreshPlaces.UseVisualStyleBackColor = true;
            btnRefreshPlaces.Click += OnRefreshPlaces;
            // 
            // lblSelected
            // 
            lblSelected.Anchor = AnchorStyles.Left;
            lblSelected.AutoSize = true;
            lblSelected.Location = new Point(15, 489);
            lblSelected.Name = "lblSelected";
            lblSelected.Size = new Size(108, 15);
            lblSelected.TabIndex = 13;
            lblSelected.Text = "Selected Locations:";
            // 
            // lstSelectedLocations
            // 
            lstSelectedLocations.Dock = DockStyle.Fill;
            lstSelectedLocations.FormattingEnabled = true;
            lstSelectedLocations.Location = new Point(165, 425);
            lstSelectedLocations.Name = "lstSelectedLocations";
            lstSelectedLocations.Size = new Size(497, 144);
            lstSelectedLocations.TabIndex = 14;
            // 
            // btnRemove
            // 
            btnRemove.Anchor = AnchorStyles.Left;
            btnRemove.Location = new Point(165, 578);
            btnRemove.Name = "btnRemove";
            btnRemove.Size = new Size(132, 30);
            btnRemove.TabIndex = 15;
            btnRemove.Text = "Remove Location";
            btnRemove.UseVisualStyleBackColor = true;
            btnRemove.Click += OnRemoveLocation;
            // 
            // lblWeatherTitle
            // 
            lblWeatherTitle.Anchor = AnchorStyles.Left;
            lblWeatherTitle.AutoSize = true;
            lblWeatherTitle.Location = new Point(15, 634);
            lblWeatherTitle.Name = "lblWeatherTitle";
            lblWeatherTitle.Size = new Size(54, 15);
            lblWeatherTitle.TabIndex = 16;
            lblWeatherTitle.Text = "Weather:";
            // 
            // weatherPanel
            // 
            weatherPanel.Controls.Add(btnGetWeather);
            weatherPanel.Controls.Add(lblWeather);
            weatherPanel.Dock = DockStyle.Fill;
            weatherPanel.Location = new Point(165, 617);
            weatherPanel.Name = "weatherPanel";
            weatherPanel.Size = new Size(497, 50);
            weatherPanel.TabIndex = 17;
            // 
            // btnGetWeather
            // 
            btnGetWeather.Location = new Point(3, 3);
            btnGetWeather.Name = "btnGetWeather";
            btnGetWeather.Size = new Size(110, 27);
            btnGetWeather.TabIndex = 0;
            btnGetWeather.Text = "Get Weather";
            btnGetWeather.UseVisualStyleBackColor = true;
            btnGetWeather.Click += OnGetWeather;
            // 
            // lblWeather
            // 
            lblWeather.AutoSize = true;
            lblWeather.Location = new Point(119, 0);
            lblWeather.Name = "lblWeather";
            lblWeather.Size = new Size(94, 15);
            lblWeather.TabIndex = 1;
            lblWeather.Text = "No weather data";
            // 
            // lblPlan
            // 
            lblPlan.Anchor = AnchorStyles.Left;
            lblPlan.AutoSize = true;
            lblPlan.Location = new Point(15, 747);
            lblPlan.Name = "lblPlan";
            lblPlan.Size = new Size(71, 15);
            lblPlan.TabIndex = 18;
            lblPlan.Text = "Plan Details:";
            // 
            // txtPlan
            // 
            txtPlan.Dock = DockStyle.Fill;
            txtPlan.Location = new Point(165, 673);
            txtPlan.Multiline = true;
            txtPlan.Name = "txtPlan";
            txtPlan.Size = new Size(497, 164);
            txtPlan.TabIndex = 19;
            // 
            // lblParticipants
            // 
            lblParticipants.Anchor = AnchorStyles.Left;
            lblParticipants.AutoSize = true;
            lblParticipants.Location = new Point(15, 867);
            lblParticipants.Name = "lblParticipants";
            lblParticipants.Size = new Size(138, 15);
            lblParticipants.TabIndex = 20;
            lblParticipants.Text = "Participants:";
            // 
            // chkParticipants
            // 
            chkParticipants.Dock = DockStyle.Fill;
            chkParticipants.FormattingEnabled = true;
            chkParticipants.Location = new Point(165, 857);
            chkParticipants.Name = "chkParticipants";
            chkParticipants.Size = new Size(497, 129);
            chkParticipants.TabIndex = 21;
            // 
            // buttonPanel
            // 
            buttonPanel.Controls.Add(btnSave);
            buttonPanel.Controls.Add(btnCancel);
            buttonPanel.Dock = DockStyle.Fill;
            buttonPanel.Location = new Point(3, 711);
            buttonPanel.Name = "buttonPanel";
            buttonPanel.Padding = new Padding(12, 8, 0, 0);
            buttonPanel.Size = new Size(694, 46);
            buttonPanel.TabIndex = 1;
            // 
            // btnSave
            // 
            btnSave.Location = new Point(15, 11);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(95, 28);
            btnSave.TabIndex = 0;
            btnSave.Text = "Save";
            btnSave.UseVisualStyleBackColor = true;
            btnSave.Click += OnSave;
            // 
            // btnCancel
            // 
            btnCancel.Location = new Point(116, 11);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(95, 28);
            btnCancel.TabIndex = 1;
            btnCancel.Text = "Cancel";
            btnCancel.UseVisualStyleBackColor = true;
            btnCancel.Click += OnCancel;
            // 
            // AddEditGalaForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(700, 450);
            Controls.Add(container);
            Name = "AddEditGalaForm";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Add/Edit Gala";
            Load += AddEditGalaForm_Load;
            container.ResumeLayout(false);
            scrollPanel.ResumeLayout(false);
            scrollPanel.PerformLayout();
            mainPanel.ResumeLayout(false);
            mainPanel.PerformLayout();
            locationPanel.ResumeLayout(false);
            locationPanel.PerformLayout();
            weatherPanel.ResumeLayout(false);
            weatherPanel.PerformLayout();
            buttonPanel.ResumeLayout(false);
            ResumeLayout(false);
        }
    }
}
