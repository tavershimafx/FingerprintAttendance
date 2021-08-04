using FingerprintAttendance.Forms;
using FingerprintAttendance.Infrastructure.Services;
using FingerprintAttendance.Models;
using FingerprintAttendance.ViewModels;
using FingerPrintAttendance.Forms;
using Futronic.SDKHelper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FingerprintAttendance
{
    public partial class MainForm : Form
    {
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
        /// 
        /// </summary>
        delegate void SetUserData(User user);

        /// <summary>
        /// 
        /// </summary>
        delegate void ClearControl();

        private bool m_bExit = false;

        /// <summary>
        /// The backing store of user.
        /// </summary>
        private readonly IRepository<long, User> _userRepository;

        private readonly IRepository<long, AttendanceLog> _attendanceRepository;

        public MainForm()
        {
            InitializeComponent();
            _userRepository = new Repository<long, User>();
            _attendanceRepository = new Repository<long, AttendanceLog>();

            lblIdentifyStatus.Text = "";
        }

        private void MainForm_Load(object sender, EventArgs e)
        {

        }

        private void newToolStripButton_Click(object sender, EventArgs e)
        {
            UserRegistrationForm form = new ();
            form.ShowDialog();
        }

        private void openToolStripButton_Click(object sender, EventArgs e)
        {
            UsersForm form = new();
            form.ShowDialog();
        }

        private void helpToolStripButton_Click(object sender, EventArgs e)
        {
            SearchUserForm form = new();
            form.ShowDialog();
        }

        private void customizeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SearchUserForm form = new();
            form.ShowDialog();
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            Task.Run(KeepScanning);
        }

        private void KeepScanning()
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
            this.SetStatusText("Place finger on device");
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
                var users = _userRepository.AsQueryable()
                    .Include(x => x.LeftHand)
                    .Include(x => x.RightHand)
                    .ToListAsync().GetAwaiter().GetResult();

                int iRecords = 0;
                int nResult;
                foreach (var user in users)
                {
                    ClearControlData();
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
                            SetUserUIData(user);
                            _attendanceRepository.Insert(new AttendanceLog
                            {
                                UserId = user.Id,
                                RecordDate = DateTime.Now
                            });
                            _attendanceRepository.SaveChanges();
                            szMessage.Append("\nDone recording presence...");

                            break;
                        }
                        else
                        {
                            if (user == users.Last()) szMessage.Append("Not found");
                        }
                    }
                    else
                    {
                        szMessage.Append("Identification failed.");
                        szMessage.Append(FutronicSdkBase.SdkRetCode2Message(nResult));
                    }
                }
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

        private void SetUserUIData(User user)
        {
            if (this.InvokeRequired)
            {
                SetUserData d = new(SetUserUIData);
                this.Invoke(d, new object[] { user });
            }
            else
            {
                this.lblFirstName.Text = user.FirstName;
                lblLastName.Text = user.LastName;
                lblUserName.Text = user.UserName;
                picturePassport.Image = user.Picture != null? Image.FromStream(new MemoryStream(user.Picture)) : null;
            }
        }

        private void ClearControlData()
        {
            if (this.InvokeRequired)
            {
                ClearControl d = new(ClearControlData);
                this.Invoke(d);
            }
            else
            {
                lblFirstName.Text = "";
                lblLastName.Text = "";
                lblUserName.Text = "";
                picturePassport.Image = null;
            }
        }

        private void EnableControls(bool bEnable)
        {
            // Do not change the state control during application closing.
            if (m_bExit)
                return;
            if (this.InvokeRequired)
            {
                EnableControlsCallback d = new(this.EnableControls);
                this.Invoke(d, new object[] { bEnable });
            }
            else
            {
                btnStart.Enabled = bEnable;
                progressSearching.Visible = !bEnable;
            }
        }

        private void UpdateScreenImage(Bitmap hBitmap)
        {
            // Do not change the state control during application closing.
            if (m_bExit)
                return;

            if (picturePreview.InvokeRequired)
            {
                SetImageCallback d = new(this.UpdateScreenImage);
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

        private void SetStatusText(string text)
        {
            // Do not change the state control during application closing.
            if (m_bExit)
                return;

            if (this.lblIdentifyStatus.InvokeRequired)
            {
                SetTextCallback d = new(this.SetStatusText);
                this.Invoke(d, new object[] { text });
            }
            else
            {
                this.lblIdentifyStatus.Text = text;
                this.Update();
            }
        }

        private List<FtrIdentifyRecord> GetRecords(FingerPrint fingerPrint)
        {
            List<FtrIdentifyRecord> records = new();
            if (fingerPrint.Thumb != null)
            {
                records.Add(new FtrIdentifyRecord()
                {
                    KeyValue = Encoding.UTF8.GetBytes(fingerPrint.Id.ToString()),
                    Template = fingerPrint.Thumb
                });
            }

            if (fingerPrint.IndexFinger != null)
            {
                records.Add(new FtrIdentifyRecord()
                {
                    KeyValue = Encoding.UTF8.GetBytes(fingerPrint.Id.ToString()),
                    Template = fingerPrint.IndexFinger
                });
            }
            
            if (fingerPrint.MiddleFinger != null)
            {
                records.Add(new FtrIdentifyRecord()
                {
                    KeyValue = Encoding.UTF8.GetBytes(fingerPrint.Id.ToString()),
                    Template = fingerPrint.MiddleFinger
                });
            }
            
            if (fingerPrint.RingFinger != null)
            {
                records.Add(new FtrIdentifyRecord()
                {
                    KeyValue = Encoding.UTF8.GetBytes(fingerPrint.Id.ToString()),
                    Template = fingerPrint.RingFinger
                });
            }
            
            if (fingerPrint.LittleFinger != null)
            {
                records.Add(new FtrIdentifyRecord()
                {
                    KeyValue = Encoding.UTF8.GetBytes(fingerPrint.Id.ToString()),
                    Template = fingerPrint.LittleFinger
                });
            }
            
            return records;
        }

    }
}
