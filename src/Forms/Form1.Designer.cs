namespace G_Tara
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;

        private Panel titleBar;
        private Label lblTitle;
        private FlowLayoutPanel windowButtonsPanel;
        private Button btnWindowMinimize;
        private Button btnWindowClose;

        private TableLayoutPanel mainPanel;
        private TableLayoutPanel toolbarContainer;
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
            titleBar = new Panel();
            lblTitle = new Label();
            windowButtonsPanel = new FlowLayoutPanel();
            btnWindowMinimize = new Button();
            btnWindowClose = new Button();
            mainPanel = new TableLayoutPanel();
            toolbarContainer = new TableLayoutPanel();
            toolbarPanel = new FlowLayoutPanel();
            btnAdd = new Button();
            btnEdit = new Button();
            btnDelete = new Button();
            btnManageParticipants = new Button();
            btnAutoEmail = new Button();
            listPanel = new Panel();
            dgvGalas = new DataGridView();
            colHost = new DataGridViewTextBoxColumn();
            titleBar.SuspendLayout();
            windowButtonsPanel.SuspendLayout();
            mainPanel.SuspendLayout();
            toolbarContainer.SuspendLayout();
            toolbarPanel.SuspendLayout();
            listPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvGalas).BeginInit();
            SuspendLayout();
            // 
            // titleBar
            // 
            titleBar.BackColor = Color.FromArgb(214, 147, 156);
            picMiniLogo = new PictureBox();
            ((System.ComponentModel.ISupportInitialize)picMiniLogo).BeginInit();
            titleBar.Controls.Add(lblTitle);
            titleBar.Controls.Add(windowButtonsPanel);
            titleBar.Dock = DockStyle.Top;
            titleBar.Location = new Point(0, 0);
            titleBar.Name = "titleBar";
            titleBar.Size = new Size(938, 36);
            titleBar.TabIndex = 0;
            titleBar.MouseDown += TitleBar_MouseDown;
            //
            // picMiniLogo properties
            //
            titleBar.Controls.Add(picMiniLogo);
            picMiniLogo.BringToFront();
            picMiniLogo.Size = new Size(24, 24);
            picMiniLogo.Location = new Point(10, 6);
            picMiniLogo.SizeMode = PictureBoxSizeMode.Zoom;
            picMiniLogo.BackColor = Color.Transparent;
            picMiniLogo.Image = Image.FromFile(Path.Combine(AppContext.BaseDirectory, "Assets", "G-tara Logo.png"));
            // 
            // lblTitle
            // 
            lblTitle.AutoSize = true;
            lblTitle.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblTitle.ForeColor = Color.FromArgb(250, 247, 245);
            lblTitle.Location = new Point(40, 9);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(156, 19);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "G-Tara - Gala Scheduler";
            lblTitle.MouseDown += TitleBar_MouseDown;
            // 
            // windowButtonsPanel
            // 
            windowButtonsPanel.BackColor = Color.Transparent;
            windowButtonsPanel.Controls.Add(btnWindowMinimize);
            windowButtonsPanel.Controls.Add(btnWindowClose);
            windowButtonsPanel.Dock = DockStyle.Right;
            windowButtonsPanel.FlowDirection = FlowDirection.LeftToRight;
            windowButtonsPanel.Location = new Point(860, 0);
            windowButtonsPanel.Margin = new Padding(0);
            windowButtonsPanel.Name = "windowButtonsPanel";
            windowButtonsPanel.Padding = new Padding(0, 6, 8, 0);
            windowButtonsPanel.Size = new Size(78, 36);
            windowButtonsPanel.TabIndex = 1;
            windowButtonsPanel.WrapContents = false;
            // 
            // btnWindowMinimize
            // 
            btnWindowMinimize.FlatAppearance.BorderSize = 0;
            btnWindowMinimize.FlatStyle = FlatStyle.Flat;
            btnWindowMinimize.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            btnWindowMinimize.ForeColor = Color.FromArgb(250, 247, 245);
            btnWindowMinimize.Location = new Point(0, 6);
            btnWindowMinimize.Margin = new Padding(0, 0, 6, 0);
            btnWindowMinimize.Name = "btnWindowMinimize";
            btnWindowMinimize.Size = new Size(28, 24);
            btnWindowMinimize.TabIndex = 0;
            btnWindowMinimize.Text = "–";
            btnWindowMinimize.UseVisualStyleBackColor = true;
            btnWindowMinimize.Click += BtnWindowMinimize_Click;
            // 
            // btnWindowClose
            // 
            btnWindowClose.FlatAppearance.BorderSize = 0;
            btnWindowClose.FlatStyle = FlatStyle.Flat;
            btnWindowClose.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            btnWindowClose.ForeColor = Color.FromArgb(250, 247, 245);
            btnWindowClose.Location = new Point(34, 6);
            btnWindowClose.Margin = new Padding(0);
            btnWindowClose.Name = "btnWindowClose";
            btnWindowClose.Size = new Size(28, 24);
            btnWindowClose.TabIndex = 1;
            btnWindowClose.Text = "×";
            btnWindowClose.UseVisualStyleBackColor = true;
            btnWindowClose.Click += BtnWindowClose_Click;
            // 
            // mainPanel
            // 
            mainPanel.ColumnCount = 1;
            mainPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            mainPanel.Controls.Add(toolbarContainer, 0, 0);
            mainPanel.Controls.Add(listPanel, 0, 1);
            mainPanel.Dock = DockStyle.Fill;
            mainPanel.Location = new Point(0, 36);
            mainPanel.Name = "mainPanel";
            mainPanel.RowCount = 2;
            mainPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 110F));
            mainPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            mainPanel.Size = new Size(938, 593);
            mainPanel.TabIndex = 1;
            // 
            // toolbarContainer
            // 
            toolbarContainer.ColumnCount = 3;
            toolbarContainer.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            toolbarContainer.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            toolbarContainer.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            toolbarContainer.Controls.Add(toolbarPanel, 1, 0);
            toolbarContainer.Dock = DockStyle.Fill;
            toolbarContainer.Location = new Point(3, 3);
            toolbarContainer.Name = "toolbarContainer";
            toolbarContainer.RowCount = 1;
            toolbarContainer.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            toolbarContainer.Size = new Size(932, 104);
            toolbarContainer.TabIndex = 2;
            // 
            // toolbarPanel
            // 
            toolbarPanel.Controls.Add(btnAdd);
            toolbarPanel.Controls.Add(btnEdit);
            toolbarPanel.Controls.Add(btnDelete);
            toolbarPanel.Controls.Add(btnManageParticipants);
            toolbarPanel.Controls.Add(btnAutoEmail);
            toolbarPanel.AutoSize = true;
            toolbarPanel.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            toolbarPanel.BackColor = Color.Transparent;
            toolbarPanel.FlowDirection = FlowDirection.LeftToRight;
            toolbarPanel.Location = new Point(61, 3);
            toolbarPanel.Margin = new Padding(0);
            toolbarPanel.Name = "toolbarPanel";
            toolbarPanel.Padding = new Padding(0, 18, 0, 0);
            toolbarPanel.Size = new Size(810, 86);
            toolbarPanel.TabIndex = 0;
            toolbarPanel.WrapContents = false;
            // 
            // btnAdd
            // 
            btnAdd.Location = new Point(123, 17);
            btnAdd.Name = "btnAdd";
            btnAdd.Size = new Size(130, 48);
            btnAdd.TabIndex = 0;
            btnAdd.Text = "Add Gala";
            btnAdd.UseVisualStyleBackColor = true;
            btnAdd.Click += OnAddClick;
            // 
            // btnEdit
            // 
            btnEdit.Location = new Point(259, 17);
            btnEdit.Name = "btnEdit";
            btnEdit.Size = new Size(130, 48);
            btnEdit.TabIndex = 1;
            btnEdit.Text = "Edit";
            btnEdit.UseVisualStyleBackColor = true;
            btnEdit.Click += OnEditClick;
            // 
            // btnDelete
            // 
            btnDelete.Location = new Point(395, 17);
            btnDelete.Name = "btnDelete";
            btnDelete.Size = new Size(130, 48);
            btnDelete.TabIndex = 2;
            btnDelete.Text = "Delete";
            btnDelete.UseVisualStyleBackColor = true;
            btnDelete.Click += OnDeleteClick;
            // 
            // btnManageParticipants
            // 
            btnManageParticipants.Location = new Point(531, 17);
            btnManageParticipants.Name = "btnManageParticipants";
            btnManageParticipants.Size = new Size(130, 48);
            btnManageParticipants.TabIndex = 3;
            btnManageParticipants.Text = "Manage Participants";
            btnManageParticipants.UseVisualStyleBackColor = true;
            btnManageParticipants.Click += OnManageParticipantsClick;
            // 
            // btnAutoEmail
            // 
            btnAutoEmail.Location = new Point(667, 17);
            btnAutoEmail.Name = "btnAutoEmail";
            btnAutoEmail.Size = new Size(130, 48);
            btnAutoEmail.TabIndex = 4;
            btnAutoEmail.Text = "Auto Email";
            btnAutoEmail.UseVisualStyleBackColor = true;
            btnAutoEmail.Click += btnAutoEmail_Click;
            // 
            // listPanel
            // 
            listPanel.Controls.Add(dgvGalas);
            listPanel.BackColor = Color.FromArgb(240, 220, 226);
            listPanel.Dock = DockStyle.Fill;
            listPanel.Location = new Point(3, 116);
            listPanel.Name = "listPanel";
            listPanel.Padding = new Padding(120, 0, 120, 80);
            listPanel.Size = new Size(932, 474);
            listPanel.TabIndex = 1;
            // 
            // dgvGalas
            // 
            dgvGalas.AllowUserToAddRows = false;
            dgvGalas.AllowUserToDeleteRows = false;
            dgvGalas.BackgroundColor = Color.FromArgb(248, 238, 241);
            dgvGalas.BorderStyle = BorderStyle.None;
            dgvGalas.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgvGalas.EnableHeadersVisualStyles = false;
            dgvGalas.GridColor = Color.FromArgb(210, 170, 178);
            dgvGalas.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvGalas.Dock = DockStyle.Fill;
            dgvGalas.Location = new Point(120, 0);
            dgvGalas.MultiSelect = false;
            dgvGalas.Name = "dgvGalas";
            dgvGalas.ReadOnly = true;
            dgvGalas.RowHeadersWidth = 51;
            dgvGalas.ScrollBars = ScrollBars.Both;
            dgvGalas.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvGalas.Size = new Size(692, 394);
            dgvGalas.TabIndex = 0;
            dgvGalas.CellContentClick += dgvGalas_CellContentClick;
            dgvGalas.CellDoubleClick += OnGridCellDoubleClick;
            dgvGalas.ColumnHeaderMouseClick += OnGridColumnHeaderMouseClick;
            dgvGalas.KeyDown += OnGridKeyDown;
            // 
            // colHost
            // 
            colHost.Name = "colHost";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(198, 135, 146);
            ClientSize = new Size(938, 629);
            Controls.Add(mainPanel);
            Controls.Add(titleBar);
            FormBorderStyle = FormBorderStyle.None;
            Name = "Form1";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "G-Tara - Gala Scheduler";
            Load += Form1_Load;
            titleBar.ResumeLayout(false);
            titleBar.PerformLayout();
            windowButtonsPanel.ResumeLayout(false);
            mainPanel.ResumeLayout(false);
            toolbarContainer.ResumeLayout(false);
            toolbarContainer.PerformLayout();
            toolbarPanel.ResumeLayout(false);
            toolbarPanel.PerformLayout();
            listPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvGalas).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private PictureBox picMiniLogo;
        private Panel listPanel;
        private Button btnAutoEmail;
        private Button btnManageParticipants;
        private Button btnDelete;
        private Button btnEdit;
        private Button btnAdd;
        private DataGridView dgvGalas;
        private FlowLayoutPanel toolbarPanel;
    }
}

