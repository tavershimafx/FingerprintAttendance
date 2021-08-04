
namespace FingerprintAttendance.Forms
{
    partial class UsersForm
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
            this.dataGridUsers = new System.Windows.Forms.DataGridView();
            this.progressUsers = new System.Windows.Forms.ProgressBar();
            this.UserName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.FirstName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.LastName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Left = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Right = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ction = new System.Windows.Forms.DataGridViewButtonColumn();
            this.ColDelete = new System.Windows.Forms.DataGridViewButtonColumn();
            this.UserId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridUsers)).BeginInit();
            this.SuspendLayout();
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
            this.Left,
            this.Right,
            this.ction,
            this.ColDelete,
            this.UserId});
            this.dataGridUsers.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridUsers.Location = new System.Drawing.Point(0, 0);
            this.dataGridUsers.Name = "dataGridUsers";
            this.dataGridUsers.ReadOnly = true;
            this.dataGridUsers.RowTemplate.Height = 25;
            this.dataGridUsers.Size = new System.Drawing.Size(744, 450);
            this.dataGridUsers.TabIndex = 0;
            this.dataGridUsers.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridUsers_CellClick);
            // 
            // progressUsers
            // 
            this.progressUsers.Location = new System.Drawing.Point(272, 203);
            this.progressUsers.MarqueeAnimationSpeed = 15;
            this.progressUsers.Name = "progressUsers";
            this.progressUsers.Size = new System.Drawing.Size(159, 23);
            this.progressUsers.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
            this.progressUsers.TabIndex = 1;
            this.progressUsers.Visible = false;
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
            // Left
            // 
            this.Left.HeaderText = "Left";
            this.Left.Name = "Left";
            this.Left.ReadOnly = true;
            // 
            // Right
            // 
            this.Right.HeaderText = "Right";
            this.Right.Name = "Right";
            this.Right.ReadOnly = true;
            // 
            // ction
            // 
            this.ction.HeaderText = "Action";
            this.ction.Name = "ction";
            this.ction.ReadOnly = true;
            // 
            // ColDelete
            // 
            this.ColDelete.HeaderText = "Delete";
            this.ColDelete.Name = "ColDelete";
            this.ColDelete.ReadOnly = true;
            this.ColDelete.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.ColDelete.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // UserId
            // 
            this.UserId.HeaderText = "User Id";
            this.UserId.Name = "UserId";
            this.UserId.ReadOnly = true;
            this.UserId.Visible = false;
            // 
            // UsersForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(744, 450);
            this.Controls.Add(this.progressUsers);
            this.Controls.Add(this.dataGridUsers);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "UsersForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Users";
            this.Load += new System.EventHandler(this.UsersForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridUsers)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridUsers;
        private System.Windows.Forms.ProgressBar progressUsers;
        private System.Windows.Forms.DataGridViewTextBoxColumn UserName;
        private System.Windows.Forms.DataGridViewTextBoxColumn FirstName;
        private System.Windows.Forms.DataGridViewTextBoxColumn LastName;
        private System.Windows.Forms.DataGridViewTextBoxColumn Left;
        private System.Windows.Forms.DataGridViewTextBoxColumn Right;
        private System.Windows.Forms.DataGridViewButtonColumn ction;
        private System.Windows.Forms.DataGridViewButtonColumn ColDelete;
        private System.Windows.Forms.DataGridViewTextBoxColumn UserId;
    }
}