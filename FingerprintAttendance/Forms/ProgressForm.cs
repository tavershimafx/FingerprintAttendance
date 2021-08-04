using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FingerprintAttendance.Forms
{
    public partial class ProgressForm : Form
    {
        private string _message;
        public ProgressForm()
        {
            InitializeComponent();
            this.lblStatus.Text = "";
        }

        public ProgressForm(string message)
        {
            InitializeComponent();
            this.lblStatus.Text = "";
            _message = message;
            UpdateStatusControl();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        public void UpdateStatusMessage(string message)
        {
            this._message = message;
            UpdateStatusControl();
        }

        private void UpdateStatusControl()
        {
            this.lblStatus.Text = _message;
        }
    }
}
