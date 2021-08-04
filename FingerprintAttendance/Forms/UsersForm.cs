using FingerprintAttendance.Infrastructure.Services;
using FingerprintAttendance.Models;
using FingerprintAttendance.ViewModels;
using FingerPrintAttendance.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FingerprintAttendance.Forms
{
    public partial class UsersForm : Form
    {
        /// <summary>
        /// A delegate to populate the datagridView with the users
        /// </summary>
        /// <param name="matchedUsers"></param>
        delegate void PopulateDatagrid(List<UserSnapshot> matchedUsers);
        private readonly IRepository<long, User>_userRepository ;

        public UsersForm()
        {
            InitializeComponent();
            _userRepository = new Repository<long, User>();
        }

        private void UsersForm_Load(object sender, EventArgs e)
        {
            Task.Run(GetUsers);
        }

        private void dataGridUsers_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 5)
            {
                var userId = (long)dataGridUsers.Rows[e.RowIndex].Cells[7].Value;
                OpenUser(userId);
            }

            if (e.ColumnIndex == 6)
            {
                var userId = (long)dataGridUsers.Rows[e.RowIndex].Cells[7].Value;
                DeleteUser(userId);
            }
        }

        private void GetUsers()
        {
           bool isRunningAsyncOperation = true;
            this.Invoke(new MethodInvoker(delegate
            {
                progressUsers.Visible = true;
            }));
            
            Thread t = new(() =>
            {
                var users = _userRepository
                .AsQueryable()
                .Select(x => new UserSnapshot
                {
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    UserName = x.UserName,
                    Id = x.Id,
                    Left = x.LeftHand != null,
                    Right = x.RightHand != null
                }).ToList();

                isRunningAsyncOperation = false;
                PopulateDatagridView(users);
            });
            t.Start();
            t.Join();

            while (!isRunningAsyncOperation)
            {
                this.Invoke(new MethodInvoker(delegate
                {
                    progressUsers.Visible = false;
                }));
                
                break;
            }
        }

        private void PopulateDatagridView(List<UserSnapshot> users)
        {
            if (this.InvokeRequired)
            {
                PopulateDatagrid grid = new(PopulateDatagridView);
                this.Invoke(grid, new object[] { users });
            }
            else
            {
                dataGridUsers.Rows.Clear();
                foreach (var user in users)
                {
                    dataGridUsers.Rows.Add(user.UserName, user.FirstName, user.LastName, user.Left, user.Right, "View", "Delete", user.Id);
                }
            }
        }

        private void DeleteUser(long userId)
        {
            if (MessageBox.Show("Are you sure you want to delete this user?", "Delete User", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                _userRepository.Delete(userId);
                _userRepository.SaveChanges();
                GetUsers();
            }
        }

        private void OpenUser(long userId)
        {
            Thread thread = new(options =>
            {
                UserRegistrationForm form = new(userId);
                form.ShowDialog();
            });
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
            thread.Join();
        }
    }
}
