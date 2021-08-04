
namespace FingerprintAttendance.Forms
{
    partial class SearchUserForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.lblIdentifyStatus = new System.Windows.Forms.Label();
            this.btnStart = new System.Windows.Forms.Button();
            this.lblSearchStatus = new System.Windows.Forms.Label();
            this.progressSearching = new System.Windows.Forms.ProgressBar();
            this.picturePreview = new System.Windows.Forms.PictureBox();
            this.dataGridUsers = new System.Windows.Forms.DataGridView();
            this.UserName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.FirstName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.LastName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.LeftEnrolled = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.RightEnrolled = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.Action = new System.Windows.Forms.DataGridViewButtonColumn();
            this.UserId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picturePreview)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridUsers)).BeginInit();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.lblIdentifyStatus);
            this.splitContainer1.Panel1.Controls.Add(this.btnStart);
            this.splitContainer1.Panel1.Controls.Add(this.lblSearchStatus);
            this.splitContainer1.Panel1.Controls.Add(this.progressSearching);
            this.splitContainer1.Panel1.Controls.Add(this.picturePreview);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.dataGridUsers);
            this.splitContainer1.Size = new System.Drawing.Size(821, 450);
            this.splitContainer1.SplitterDistance = 273;
            this.splitContainer1.TabIndex = 0;
            // 
            // lblIdentifyStatus
            // 
            this.lblIdentifyStatus.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblIdentifyStatus.Location = new System.Drawing.Point(23, 179);
            this.lblIdentifyStatus.Name = "lblIdentifyStatus";
            this.lblIdentifyStatus.Size = new System.Drawing.Size(214, 68);
            this.lblIdentifyStatus.TabIndex = 5;
            this.lblIdentifyStatus.Text = "label1";
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(88, 250);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(75, 23);
            this.btnStart.TabIndex = 4;
            this.btnStart.Text = "Start";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // lblSearchStatus
            // 
            this.lblSearchStatus.AutoSize = true;
            this.lblSearchStatus.Location = new System.Drawing.Point(23, 286);
            this.lblSearchStatus.Name = "lblSearchStatus";
            this.lblSearchStatus.Size = new System.Drawing.Size(38, 15);
            this.lblSearchStatus.TabIndex = 3;
            this.lblSearchStatus.Text = "label1";
            // 
            // progressSearching
            // 
            this.progressSearching.Location = new System.Drawing.Point(23, 315);
            this.progressSearching.Name = "progressSearching";
            this.progressSearching.Size = new System.Drawing.Size(214, 29);
            this.progressSearching.TabIndex = 2;
            // 
            // picturePreview
            // 
            this.picturePreview.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.picturePreview.Location = new System.Drawing.Point(62, 30);
            this.picturePreview.Name = "picturePreview";
            this.picturePreview.Size = new System.Drawing.Size(132, 146);
            this.picturePreview.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picturePreview.TabIndex = 1;
            this.picturePreview.TabStop = false;
            // 
            // dataGridUsers
            // 
            this.dataGridUsers.AllowUserToAddRows = false;
            this.dataGridUsers.AllowUserToDeleteRows = false;
            this.dataGridUsers.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridUsers.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.UserName,
            this.FirstName,
            this.LastName,
            this.LeftEnrolled,
            this.RightEnrolled,
            this.Action,
            this.UserId});
            this.dataGridUsers.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridUsers.Location = new System.Drawing.Point(0, 0);
            this.dataGridUsers.Name = "dataGridUsers";
            this.dataGridUsers.ReadOnly = true;
            this.dataGridUsers.RowTemplate.Height = 25;
            this.dataGridUsers.Size = new System.Drawing.Size(544, 450);
            this.dataGridUsers.TabIndex = 0;
            this.dataGridUsers.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridUsers_CellClick);
            // 
            // UserName
            // 
            this.UserName.HeaderText = "Username";
            this.UserName.Name = "UserName";
            this.UserName.ReadOnly = true;
            // 
            // FirstName
            // 
            this.FirstName.HeaderText = "First Name";
            this.FirstName.Name = "FirstName";
            this.FirstName.ReadOnly = true;
            // 
            // LastName
            // 
            this.LastName.HeaderText = "Last Name";
            this.LastName.Name = "LastName";
            this.LastName.ReadOnly = true;
            // 
            // LeftEnrolled
            // 
            this.LeftEnrolled.HeaderText = "Left";
            this.LeftEnrolled.Name = "LeftEnrolled";
            this.LeftEnrolled.ReadOnly = true;
            this.LeftEnrolled.Width = 50;
            // 
            // RightEnrolled
            // 
            this.RightEnrolled.HeaderText = "Right";
            this.RightEnrolled.Name = "RightEnrolled";
            this.RightEnrolled.ReadOnly = true;
            this.RightEnrolled.Width = 50;
            // 
            // Action
            // 
            this.Action.HeaderText = "Select";
            this.Action.Name = "Action";
            this.Action.ReadOnly = true;
            // 
            // UserId
            // 
            this.UserId.HeaderText = "UserId";
            this.UserId.Name = "UserId";
            this.UserId.ReadOnly = true;
            this.UserId.Visible = false;
            // 
            // SearchUserForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(821, 450);
            this.Controls.Add(this.splitContainer1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "SearchUserForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "Search User";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picturePreview)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridUsers)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Label lblSearchStatus;
        private System.Windows.Forms.ProgressBar progressSearching;
        private System.Windows.Forms.PictureBox picturePreview;
        private System.Windows.Forms.DataGridView dataGridUsers;
        private System.Windows.Forms.Label lblIdentifyStatus;
        private System.Windows.Forms.DataGridViewTextBoxColumn UserName;
        private System.Windows.Forms.DataGridViewTextBoxColumn FirstName;
        private System.Windows.Forms.DataGridViewTextBoxColumn LastName;
        private System.Windows.Forms.DataGridViewCheckBoxColumn LeftEnrolled;
        private System.Windows.Forms.DataGridViewCheckBoxColumn RightEnrolled;
        private System.Windows.Forms.DataGridViewButtonColumn Action;
        private System.Windows.Forms.DataGridViewTextBoxColumn UserId;
    }
}