namespace G_Tara
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;

        private TableLayoutPanel mainPanel;
        private FlowLayoutPanel toolbarPanel;
        private Button btnAdd;
        private Button btnEdit;
        private Button btnDelete;
        private Button btnManageParticipants;
        private Button btnAutoEmail;
        private Panel listPanel;
        private DataGridView dgvGalas;
        private DataGridViewTextBoxColumn colId;
        private DataGridViewTextBoxColumn colName;
        private DataGridViewTextBoxColumn colDate;
        private DataGridViewTextBoxColumn colLocation;
        private DataGridViewTextBoxColumn colStatus;
        private DataGridViewTextBoxColumn colPlan;
        private DataGridViewTextBoxColumn colHost;
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            mainPanel = new TableLayoutPanel();
            toolbarPanel = new FlowLayoutPanel();
            btnAdd = new Button();
            btnEdit = new Button();
            btnDelete = new Button();
            btnManageParticipants = new Button();
            btnAutoEmail = new Button();
            listPanel = new Panel();
            dgvGalas = new DataGridView();
            colId = new DataGridViewTextBoxColumn();
            colName = new DataGridViewTextBoxColumn();
            colDate = new DataGridViewTextBoxColumn();
            colLocation = new DataGridViewTextBoxColumn();
            colStatus = new DataGridViewTextBoxColumn();
            colHost = new DataGridViewTextBoxColumn();
            colPlan = new DataGridViewTextBoxColumn();
            mainPanel.SuspendLayout();
            toolbarPanel.SuspendLayout();
            listPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvGalas).BeginInit();
            SuspendLayout();
            // 
            // mainPanel
            // 
            mainPanel.ColumnCount = 1;
            mainPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            mainPanel.Controls.Add(toolbarPanel, 0, 0);
            mainPanel.Controls.Add(listPanel, 0, 1);
            mainPanel.Dock = DockStyle.Fill;
            mainPanel.Location = new Point(0, 0);
            mainPanel.Name = "mainPanel";
            mainPanel.RowCount = 2;
            mainPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 52F));
            mainPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            mainPanel.Size = new Size(900, 600);
            mainPanel.TabIndex = 0;
            // 
            // toolbarPanel
            // 
            toolbarPanel.Controls.Add(btnAdd);
            toolbarPanel.Controls.Add(btnEdit);
            toolbarPanel.Controls.Add(btnDelete);
            toolbarPanel.Controls.Add(btnManageParticipants);
            toolbarPanel.Controls.Add(btnAutoEmail);
            toolbarPanel.Dock = DockStyle.Fill;
            toolbarPanel.Location = new Point(3, 3);
            toolbarPanel.Name = "toolbarPanel";
            toolbarPanel.Padding = new Padding(10);
            toolbarPanel.Size = new Size(894, 46);
            toolbarPanel.TabIndex = 0;
            // 
            // btnAdd
            // 
            btnAdd.Location = new Point(13, 13);
            btnAdd.Name = "btnAdd";
            btnAdd.Size = new Size(100, 28);
            btnAdd.TabIndex = 0;
            btnAdd.Text = "Add Gala";
            btnAdd.UseVisualStyleBackColor = true;
            btnAdd.Click += OnAddClick;
            // 
            // btnEdit
            // 
            btnEdit.Location = new Point(119, 13);
            btnEdit.Name = "btnEdit";
            btnEdit.Size = new Size(100, 28);
            btnEdit.TabIndex = 1;
            btnEdit.Text = "Edit";
            btnEdit.UseVisualStyleBackColor = true;
            btnEdit.Click += OnEditClick;
            // 
            // btnDelete
            // 
            btnDelete.Location = new Point(225, 13);
            btnDelete.Name = "btnDelete";
            btnDelete.Size = new Size(100, 28);
            btnDelete.TabIndex = 2;
            btnDelete.Text = "Delete";
            btnDelete.UseVisualStyleBackColor = true;
            btnDelete.Click += OnDeleteClick;
            // 
            // btnManageParticipants
            // 
            btnManageParticipants.Location = new Point(331, 13);
            btnManageParticipants.Name = "btnManageParticipants";
            btnManageParticipants.Size = new Size(150, 28);
            btnManageParticipants.TabIndex = 3;
            btnManageParticipants.Text = "Manage Participants";
            btnManageParticipants.UseVisualStyleBackColor = true;
            btnManageParticipants.Click += OnManageParticipantsClick;

            // btnAutoEmail
            // 
            btnAutoEmail.Location = new Point(491, 13);
            btnAutoEmail.Name = "btnAutoEmail";
            btnAutoEmail.Size = new Size(120, 28);
            btnAutoEmail.TabIndex = 4;
            btnAutoEmail.Text = "Auto Email";
            btnAutoEmail.UseVisualStyleBackColor = true;
            btnAutoEmail.Click += btnAutoEmail_Click;
            // 
            // listPanel
            // 
            listPanel.Controls.Add(dgvGalas);
            listPanel.Dock = DockStyle.Fill;
            listPanel.Location = new Point(3, 55);
            listPanel.Name = "listPanel";
            listPanel.Padding = new Padding(10);
            listPanel.Size = new Size(894, 542);
            listPanel.TabIndex = 1;
            // 
            // dgvGalas
            // 
            dgvGalas.AllowUserToAddRows = false;
            dgvGalas.AllowUserToDeleteRows = false;
            dgvGalas.AutoGenerateColumns = false;
            dgvGalas.BackgroundColor = SystemColors.ControlLight;
            dgvGalas.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvGalas.Columns.AddRange(new DataGridViewColumn[] { colId, colName, colDate, colLocation, colStatus, colPlan });
            dgvGalas.Dock = DockStyle.Fill;
            dgvGalas.Location = new Point(10, 10);
            dgvGalas.MultiSelect = false;
            dgvGalas.Name = "dgvGalas";
            dgvGalas.ReadOnly = true;
            dgvGalas.RowHeadersWidth = 51;
            dgvGalas.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvGalas.Size = new Size(874, 522);
            dgvGalas.TabIndex = 0;
            dgvGalas.CellDoubleClick += OnGridCellDoubleClick;
            dgvGalas.ColumnHeaderMouseClick += OnGridColumnHeaderMouseClick;
            dgvGalas.KeyDown += OnGridKeyDown;
            // 
            // colId
            // 
            colId.DataPropertyName = "Id";
            colId.HeaderText = "Id";
            colId.MinimumWidth = 6;
            colId.Name = "Id";
            colId.ReadOnly = true;
            colId.Visible = false;
            colId.Width = 125;
            // 
            // colName
            // 
            colName.DataPropertyName = "Name";
            colName.HeaderText = "Gala Name";
            colName.MinimumWidth = 6;
            colName.Name = "colName";
            colName.ReadOnly = true;
            colName.Width = 140;
            // 
            // colDate
            // 
            colDate.DataPropertyName = "ScheduledDate";
            colDate.HeaderText = "Date";
            colDate.MinimumWidth = 6;
            colDate.Name = "colDate";
            colDate.ReadOnly = true;
            colDate.Width = 120;
            // 
            // colLocation
            // 
            colLocation.DataPropertyName = "Location";
            colLocation.HeaderText = "Location";
            colLocation.MinimumWidth = 6;
            colLocation.Name = "colLocation";
            colLocation.ReadOnly = true;
            colLocation.Width = 150;
            // 
            // colStatus
            // 
            colStatus.DataPropertyName = "Status";
            colStatus.HeaderText = "Status";
            colStatus.MinimumWidth = 6;
            colStatus.Name = "colStatus";
            colStatus.ReadOnly = true;
            colStatus.Width = 110;
            // 
            // colPlan
            // 
            colPlan.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            colPlan.DataPropertyName = "Plan";
            colPlan.HeaderText = "Plan";
            colPlan.MinimumWidth = 6;
            colPlan.Name = "colPlan";
            colPlan.ReadOnly = true;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(900, 600);
            Controls.Add(mainPanel);
            Name = "Form1";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "G-Tara - Gala Scheduler";
            Load += Form1_Load;
            mainPanel.ResumeLayout(false);
            toolbarPanel.ResumeLayout(false);
            listPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvGalas).EndInit();
            ResumeLayout(false);
        }

        #endregion
    }
}

