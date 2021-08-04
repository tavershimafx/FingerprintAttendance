using FingerprintAttendance.Infrastructure.Services;
using FingerprintAttendance.Models;
using FingerPrintAttendance.Forms;
using Futronic.SDKHelper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using FingerprintAttendance.ViewModels;
using System.Windows.Forms;

namespace FingerprintAttendance.Forms
{
    public partial class SearchUserForm : Form
    {

        /// <summary>
        /// This delegate enables asynchronous calls for setting
        /// the Image property on a PictureBox control.
        /// </summary>
        /// <param name="hBitmap">the instance of Bitmap class</param>
        delegate void SetImageCallback(Bitmap hBitmap);

        /// <summary>
        /// This delegate enables asynchronous calls for setting
        /// the text property on a status control.
        /// </summary>
        /// <param name="text"></param>
        delegate void SetTextCallback(string text);

        /// <summary>
        /// Contain reference for current operation object
        /// </summary>
        private FutronicSdkBase m_Operation;

        /// <summary>
        /// This delegate enables asynchronous calls for setting
        /// the Enable property on a buttons.
        /// </summary>
        /// <param name="bEnable">true to enable buttons, otherwise to disable</param>
        delegate void EnableControlsCallback(bool bEnable);

        /// <summary>
        /// A delegate used to invoke a function to clear the current data in the datagridView
        /// </summary>
        delegate void ClearDatagrid();

        /// <summary>
        /// A delegate to populate the datagridView with the users
        /// </summary>
        /// <param name="matchedUsers"></param>
        delegate void PopulateDatagrid(List<UserSnapshot> matchedUsers);

        private bool m_bExit = false;

        /// <summary>
        /// The backing store of user.
        /// </summary>
        private readonly IRepository<long, User> _userRepository;

        //private 
        public SearchUserForm()
        {
            InitializeComponent();
            _userRepository = new Repository<long, User>();
            lblSearchStatus.Text = "";
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            EnableControls(false);
            FutronicSdkBase dummy = new FutronicIdentification();
            if (m_Operation != null)
            {
                m_Operation.Dispose();
                m_Operation = null;
            }
            m_Operation = dummy;

            // Set control property
            m_Operation.FakeDetection = false;
            m_Operation.FFDControl = true;
            m_Operation.Version = VersionCompatible.ftr_version_compatible;
            m_Operation.FastMode = false;

            // register events
            m_Operation.OnPutOn += new OnPutOnHandler(this.OnPutOn);
            m_Operation.OnTakeOff += new OnTakeOffHandler(this.OnTakeOff);
            m_Operation.UpdateScreenImage += new UpdateScreenImageHandler(this.UpdateScreenImage);
            m_Operation.OnFakeSource += new OnFakeSourceHandler(this.OnFakeSource);
            ((FutronicIdentification)m_Operation).OnGetBaseTemplateComplete +=
                    new OnGetBaseTemplateCompleteHandler(this.OnGetBaseTemplateComplete);

            // start identification process
            ((FutronicIdentification)m_Operation).GetBaseTemplate();
        }

        private void OnPutOn(FTR_PROGRESS Progress)
        {
            this.SetStatusText("Put finger on device");
        }

        private void OnTakeOff(FTR_PROGRESS Progress)
        {
            this.SetStatusText("Take off finger from device");
        }


        private void OnGetBaseTemplateComplete(bool bSuccess, int nRetCode)
        {
            StringBuilder szMessage = new();
            if (bSuccess)
            {
                this.SetStatusText("Searching user...");
                List<UserSnapshot> matchedUsers = new();

                var users = _userRepository.AsQueryable()
                    .Include(x => x.LeftHand)
                    .Include(x => x.RightHand)
                    .ToListAsync().GetAwaiter().GetResult();

                int iRecords = 0;
                int nResult;
                foreach (var user in users)
                {
                    List<FtrIdentifyRecord> allPrints = new();
                    if (user.LeftHand != null) allPrints.AddRange(GetRecords(user.LeftHand));
                    if (user.RightHand != null) allPrints.AddRange(GetRecords(user.RightHand));

                    iRecords = allPrints.Count;
                    nResult = ((FutronicIdentification)m_Operation).Identification(allPrints.ToArray(), ref iRecords);
                    if (nResult == FutronicSdkBase.RETCODE_OK)
                    {
                        if (iRecords != -1)
                        {
                            szMessage.Append($"Identification completed for user: {user.UserName} ");
                            matchedUsers.Add(new UserSnapshot
                            {
                                Id = user.Id,
                                FirstName = user.FirstName,
                                LastName = user.LastName,
                                UserName = user.UserName,
                                Left = user.LeftHand != null,
                                Right = user.RightHand != null
                            });
                        }
                        else
                        {
                            if (user == users.Last()) szMessage.Append("not found");
                        }
                    }
                    else
                    {
                        szMessage.Append("Identification failed.");
                        szMessage.Append(FutronicSdkBase.SdkRetCode2Message(nResult));
                    }
                }

                ClearDataGridView();
                PopulateDatagridView( matchedUsers);
            }
            else
            {
                szMessage.Append("Can not retrieve base template.");
                szMessage.Append("Error description: ");
                szMessage.Append(FutronicSdkBase.SdkRetCode2Message(nRetCode));
            }
            this.SetStatusText(szMessage.ToString());

            // unregister events
            m_Operation.OnPutOn -= new OnPutOnHandler(this.OnPutOn);
            m_Operation.OnTakeOff -= new OnTakeOffHandler(this.OnTakeOff);
            m_Operation.UpdateScreenImage -= new UpdateScreenImageHandler(this.UpdateScreenImage);
            m_Operation.OnFakeSource -= new OnFakeSourceHandler(this.OnFakeSource);
            ((FutronicIdentification)m_Operation).OnGetBaseTemplateComplete -=
                    new OnGetBaseTemplateCompleteHandler(this.OnGetBaseTemplateComplete);

            EnableControls(true);
        }

        private void ClearDataGridView()
        {
            if (this.InvokeRequired)
            {
                ClearDatagrid clear = new (ClearDataGridView);
                this.Invoke(clear);
            }
            else
            {
                dataGridUsers.Rows.Clear();
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
                foreach (var user in users)
                {
                    dataGridUsers.Rows.Add(user.UserName, user.FirstName, user.LastName, user.Left, user.Right, "View", user.Id);
                }
            }
        }

        private List<FtrIdentifyRecord> GetRecords(FingerPrint fingerPrint)
        {
            List<FtrIdentifyRecord> records = new ();
            records.Add(new FtrIdentifyRecord()
            {
                KeyValue = Encoding.UTF8.GetBytes(fingerPrint.Id.ToString()),
                Template = fingerPrint.Thumb
            });

            records.Add(new FtrIdentifyRecord()
            {
                KeyValue = Encoding.UTF8.GetBytes(fingerPrint.Id.ToString()),
                Template = fingerPrint.IndexFinger
            });

            records.Add(new FtrIdentifyRecord()
            {
                KeyValue = Encoding.UTF8.GetBytes(fingerPrint.Id.ToString()),
                Template = fingerPrint.MiddleFinger
            });

            records.Add(new FtrIdentifyRecord()
            {
                KeyValue = Encoding.UTF8.GetBytes(fingerPrint.Id.ToString()),
                Template = fingerPrint.RingFinger
            });

            records.Add(new FtrIdentifyRecord()
            {
                KeyValue = Encoding.UTF8.GetBytes(fingerPrint.Id.ToString()),
                Template = fingerPrint.LittleFinger
            });

            return records;
        }

        private void EnableControls(bool bEnable)
        {
            // Do not change the state control during application closing.
            if (m_bExit)
                return;
            if (this.InvokeRequired)
            {
                EnableControlsCallback d = new (this.EnableControls);
                this.Invoke(d, new object[] { bEnable });
            }
            else
            {
                if (bEnable)
                {
                    progressSearching.MarqueeAnimationSpeed = 0;
                    progressSearching.Style = ProgressBarStyle.Blocks;
                }
                else
                {
                    progressSearching.MarqueeAnimationSpeed = 20;
                    progressSearching.Style = ProgressBarStyle.Marquee;
                }
                
                btnStart.Enabled = bEnable;
            }
        }

        private void SetStatusText(string text)
        {
            // Do not change the state control during application closing.
            if (m_bExit)
                return;

            if (this.lblIdentifyStatus.InvokeRequired)
            {
                SetTextCallback d = new (this.SetStatusText);
                this.Invoke(d, new object[] { text });
            }
            else
            {
                this.lblIdentifyStatus.Text = text;
                this.Update();
            }
        }

        private void UpdateScreenImage(Bitmap hBitmap)
        {
            // Do not change the state control during application closing.
            if (m_bExit)
                return;

            if (picturePreview.InvokeRequired)
            {
                SetImageCallback d = new (this.UpdateScreenImage);
                this.Invoke(d, new object[] { hBitmap });
            }
            else
            {
                picturePreview.Image = hBitmap;
            }
        }

        private bool OnFakeSource(FTR_PROGRESS Progress)
        {
            if (m_bExit)
                return true;

            DialogResult result;
            result = MessageBox.Show("Fake source detected. Do you want continue process?",
                                     "Fingerprint enrollment",
                                     MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            return (result == DialogResult.No);
        }

        private void dataGridUsers_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 5)
            {
                var userId = (long)dataGridUsers.Rows[e.RowIndex].Cells[6].Value;
                Thread thread = new(options =>
                {
                    UserRegistrationForm form = new(userId);
                    form.ShowDialog();
                });
                thread.Start();
                this.Close();
            }
        }
    }
}
