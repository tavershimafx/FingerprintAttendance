using FingerprintAttendance.Infrastructure.Services;
using FingerprintAttendance.Models;
using Futronic.SDKHelper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FingerPrintAttendance.Forms
{
    [Serializable]
    public partial class FingerprintForm : Form
    {
        private byte[] Thumb { get; set; }
        private byte[] ThumbThumbnail { get; set; }
        private byte[] IndexFinger { get; set; }
        private byte[] IndexFingerThumbnail { get; set; }
        private byte[] MiddleFinger { get; set; }
        private byte[] MiddleFingerThumbnail { get; set; }
        private byte[] RingFinger { get; set; }
        private byte[] RingFingerThumbnail { get; set; }
        private byte[] LittleFinger { get; set; }
        private byte[] LittleFingerThumbnail { get; set; }

        /// <summary>
        /// Contain reference for current operation object
        /// </summary>
        private FutronicSdkBase m_Operation;

        private bool m_bExit;

        /// <summary>
        /// This delegate enables asynchronous calls for setting
        /// the Enable property on a buttons.
        /// </summary>
        /// <param name="bEnable">true to enable buttons, otherwise to disable</param>
        delegate void EnableControlsCallback(bool bEnable);

        /// <summary>
        /// This delegate enables asynchronous calls for setting
        /// the text property on a status control.
        /// </summary>
        /// <param name="text"></param>
        delegate void SetTextCallback(string text);

        /// <summary>
        /// This delegate enables asynchronous calls for setting
        /// the Image property on a PictureBox control.
        /// </summary>
        /// <param name="hBitmap">the instance of Bitmap class</param>
        delegate void SetImageCallback(Bitmap hBitmap);

        /// <summary>
        /// The data read from the fingerprint. This data is not a single image. Its a collection of 
        /// consecutive number of scans as set during initialization for a single finger.
        /// </summary>
        private byte[] fingerprintData;

        /// <summary>
        /// The backing store of user.
        /// </summary>
        private IRepository<long, User> _repository;

        /// <summary>
        /// The name of the label mapped to the current verification finger
        /// </summary>
        private string statusLabel;

        public FingerprintForm()
        {
            InitializeComponent();
            lblEnrollmentProgress.Text = "";
            lblThumbStatus.Text = "";
            lblIndexStatus.Text = "";
            lblMiddleStatus.Text = "";
            lblRingStatus.Text = "";
            lblLittleStatus.Text = "";
        }

        public FingerprintForm(FingerPrint fingerPrint)
        {
            InitializeComponent();
            lblEnrollmentProgress.Text = "";

            PopulateUI(fingerPrint);
        }

        private void FingerprintForm_Load(object sender, EventArgs e)
        {
            _repository = new Repository<long, User>();
        }

        private void PopulateUI(FingerPrint fingerPrint)
        {
            fingerprintData = fingerPrint.Thumb;// RecoverOriginalBytes();
            ThumbThumbnail = fingerPrint.ThumbThumbnail;
            pictureThumb.Image = Image.FromStream(new MemoryStream(fingerPrint.ThumbThumbnail));
            NotifyThumbEnrolled();

            fingerprintData = fingerPrint.IndexFinger;// RecoverOriginalBytes();
            IndexFingerThumbnail = fingerPrint.IndexFingerThumbnail;
            pictureIndex.Image = Image.FromStream(new MemoryStream(fingerPrint.IndexFingerThumbnail));
            NotifyIndexEnrolled();

            fingerprintData = fingerPrint.MiddleFinger;// RecoverOriginalBytes();
            MiddleFingerThumbnail = fingerPrint.MiddleFingerThumbnail;
            pictureMiddle.Image = Image.FromStream(new MemoryStream(fingerPrint.MiddleFingerThumbnail));
            NotifyMiddleEnrolled();

            fingerprintData = fingerPrint.RingFinger;// RecoverOriginalBytes();
            RingFingerThumbnail = fingerPrint.RingFingerThumbnail;
            pictureRing.Image = Image.FromStream(new MemoryStream(fingerPrint.RingFingerThumbnail));
            NotifyRingEnrolled();

            fingerprintData = fingerPrint.LittleFinger;// RecoverOriginalBytes();
            LittleFingerThumbnail = fingerPrint.LittleFingerThumbnail;
            pictureLittle.Image = Image.FromStream(new MemoryStream(fingerPrint.LittleFingerThumbnail));
            NotifyLittleEnrolled();
        }

        private void btnBeginCapture_Click(object sender, EventArgs e)
        {
            FutronicSdkBase dummy = new FutronicEnrollment();
            if (m_Operation != null)
            {
                m_Operation.Dispose();
                m_Operation = null;
            }
            m_Operation = dummy;

            // Set control properties
            m_Operation.FakeDetection = false; 
            m_Operation.FFDControl = true;
            m_Operation.Version = VersionCompatible.ftr_version_compatible;
            m_Operation.FastMode = false; 
            ((FutronicEnrollment)m_Operation).MIOTControlOff = false;
            ((FutronicEnrollment)m_Operation).MaxModels = 5; 

            EnableControls(false);

            // register events
            m_Operation.OnPutOn += new OnPutOnHandler(this.OnPutOn);
            m_Operation.OnTakeOff += new OnTakeOffHandler(this.OnTakeOff);
            m_Operation.UpdateScreenImage += new UpdateScreenImageHandler(this.UpdateScreenImage);
            m_Operation.OnFakeSource += new OnFakeSourceHandler(this.OnFakeSource);
            ((FutronicEnrollment)m_Operation).OnEnrollmentComplete += new OnEnrollmentCompleteHandler(this.OnEnrollmentComplete);

            // start enrollment process
            ((FutronicEnrollment)m_Operation).Enrollment();
        }

        private void OnPutOn(FTR_PROGRESS Progress)
        {
            this.SetStatusText("Put finger on device");
        }

        private void OnTakeOff(FTR_PROGRESS Progress)
        {
            this.SetStatusText("Take off finger from device");
        }

        private void UpdateScreenImage(Bitmap hBitmap)
        {
            // Do not change the state control during application closing.
            if (m_bExit)
                return;

            if (picturePreview.InvokeRequired)
            {
                SetImageCallback d = new SetImageCallback(this.UpdateScreenImage);
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

        private void OnEnrollmentComplete(bool bSuccess, int nRetCode)
        {
            StringBuilder szMessage = new StringBuilder();
            if (bSuccess)
            {
                // set status string
                szMessage.Append("Enrollment completed successfully.");
                szMessage.Append("Quality: ");
                szMessage.Append(((FutronicEnrollment)m_Operation).Quality);
                SetStatusText(szMessage.ToString());

                fingerprintData = ((FutronicEnrollment)m_Operation).Template;
                //GroupFingerprintDataBeforSave();
            }
            else
            {
                szMessage.Append("Enrollment process failed.");
                szMessage.Append("Error description: ");
                szMessage.Append(FutronicSdkBase.SdkRetCode2Message(nRetCode));
                this.SetStatusText(szMessage.ToString());
            }

            // unregister events
            m_Operation.OnPutOn -= new OnPutOnHandler(this.OnPutOn);
            m_Operation.OnTakeOff -= new OnTakeOffHandler(this.OnTakeOff);
            m_Operation.UpdateScreenImage -= new UpdateScreenImageHandler(this.UpdateScreenImage);
            m_Operation.OnFakeSource -= new OnFakeSourceHandler(this.OnFakeSource);
            ((FutronicEnrollment)m_Operation).OnEnrollmentComplete -= new OnEnrollmentCompleteHandler(this.OnEnrollmentComplete);

            EnableControls(true);
        }

        private void btnCaptureThumb_Click(object sender, EventArgs e)
        {
            NotifyThumbEnrolled();
        }

        private void btnCaptureIndex_Click(object sender, EventArgs e)
        {
            NotifyIndexEnrolled();
        }

        private void btnCaptureMiddle_Click(object sender, EventArgs e)
        {
            NotifyMiddleEnrolled();
        }

        private void btnCaptureRing_Click(object sender, EventArgs e)
        {
            NotifyRingEnrolled();
        }

        private void btnCaptureLittle_Click(object sender, EventArgs e)
        {
            NotifyLittleEnrolled();
        }

        private void btnVerifyThumb_Click(object sender, EventArgs e)
        {
            statusLabel = lblThumbStatus.Name;
            VerifyFingerprint(Thumb);
        }

        private void btnVerifyIndex_Click(object sender, EventArgs e)
        {
            statusLabel = lblIndexStatus.Name;
            VerifyFingerprint(IndexFinger);
        }

        private void btnVerifyMiddle_Click(object sender, EventArgs e)
        {
            statusLabel = lblMiddleStatus.Name;
            VerifyFingerprint(MiddleFinger);
        }

        private void btnVerifyRing_Click(object sender, EventArgs e)
        {
            statusLabel = lblRingStatus.Name;
            VerifyFingerprint(RingFinger);
        }

        private void btnVerifyLittle_Click(object sender, EventArgs e)
        {
            statusLabel = lblLittleStatus.Name;
            VerifyFingerprint(LittleFinger);
        }

        private void pictureThumb_DoubleClick(object sender, EventArgs e)
        {
            ThumbThumbnail = GetImageBytes(picturePreview.Image);
            pictureThumb.Image = picturePreview.Image;
        }

        private void pictureIndex_DoubleClick(object sender, EventArgs e)
        {
            IndexFingerThumbnail = GetImageBytes(picturePreview.Image);
            pictureIndex.Image = picturePreview.Image;
        }

        private void pictureMiddle_DoubleClick(object sender, EventArgs e)
        {
            MiddleFingerThumbnail = GetImageBytes(picturePreview.Image);
            pictureMiddle.Image = picturePreview.Image;
        }

        private void pictureRing_DoubleClick(object sender, EventArgs e)
        {
            RingFingerThumbnail = GetImageBytes(picturePreview.Image);
            pictureRing.Image = picturePreview.Image;
        }

        private void pictureLittle_DoubleClick(object sender, EventArgs e)
        {
            LittleFingerThumbnail = GetImageBytes(picturePreview.Image);
            pictureLittle.Image = picturePreview.Image;
        }

        private void VerifyFingerprint(byte[] previousData)
        {
            EnableControls(false);
            SetStatusText(string.Empty);
            
            FutronicSdkBase dummy = new FutronicVerification(previousData);
            if (m_Operation != null)
            {
                m_Operation.Dispose();
                m_Operation = null;
            }
            m_Operation = dummy;

            // Set control properties
            m_Operation.FakeDetection = false;
            m_Operation.FFDControl = true;
            //m_Operation.FARN = Int32.Parse(tbFARN.Text);
            m_Operation.Version = VersionCompatible.ftr_version_compatible;
            m_Operation.FastMode = false;

            // register events
            m_Operation.OnPutOn += new OnPutOnHandler(this.OnPutOn);
            m_Operation.OnTakeOff += new OnTakeOffHandler(this.OnTakeOff);
            m_Operation.UpdateScreenImage += new UpdateScreenImageHandler(this.UpdateScreenImage);
            m_Operation.OnFakeSource += new OnFakeSourceHandler(this.OnFakeSource);
            ((FutronicVerification)m_Operation).OnVerificationComplete += new OnVerificationCompleteHandler(this.OnVerificationComplete);

            // start verification process
            ((FutronicVerification)m_Operation).Verification();
        }

        private void OnVerificationComplete(bool bSuccess, int nRetCode, bool bVerificationSuccess)
        {
            StringBuilder szResult = new ();
            if (bSuccess)
            {
                if (bVerificationSuccess)
                {
                    szResult.Append("Verification successful.");

                    SetVerificationStatus(true, szResult.ToString());
                }
                else
                    szResult.Append("Verification failed.");

                SetVerificationStatus(false, szResult.ToString());
            }
            else
            {
                szResult.Append("Verification process failed.");
                szResult.Append("Error description: ");
                szResult.Append(FutronicSdkBase.SdkRetCode2Message(nRetCode));

                SetVerificationStatus(false, szResult.ToString());
            }

            // unregister events
            m_Operation.OnPutOn -= new OnPutOnHandler(this.OnPutOn);
            m_Operation.OnTakeOff -= new OnTakeOffHandler(this.OnTakeOff);
            m_Operation.UpdateScreenImage -= new UpdateScreenImageHandler(this.UpdateScreenImage);
            m_Operation.OnFakeSource -= new OnFakeSourceHandler(this.OnFakeSource);
            ((FutronicVerification)m_Operation).OnVerificationComplete -= new OnVerificationCompleteHandler(this.OnVerificationComplete);

            EnableControls(true);
        }

        delegate void SetVerificationStat(bool succeeded, string message);
        private void SetVerificationStatus(bool succeeded, string message)
        {
            if (this.InvokeRequired)
            {
                SetVerificationStat d = new (SetVerificationStatus);
                this.Invoke(d, new object[] { succeeded, message });
            }
            else
            {
                ((Label)this.Controls[statusLabel]).Text = message;
                if (succeeded)
                {
                    ((Label)this.Controls[statusLabel]).ForeColor = Color.Green;
                }
                else
                {
                    ((Label)this.Controls[statusLabel]).ForeColor = Color.Red;
                }
            }
        }

        private void SetStatusText(string text)
        {
            // Do not change the state control during application closing.
            if (m_bExit)
                return;

            if (this.lblEnrollmentProgress.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(this.SetStatusText);
                this.Invoke(d, new object[] { text });
            }
            else
            {
                this.lblEnrollmentProgress.Text = text;
                this.Update();
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
                btnVerifyThumb.Enabled = bEnable;
                btnVerifyIndex.Enabled = bEnable;
                btnVerifyMiddle.Enabled = bEnable;
                btnVerifyRing.Enabled = bEnable;
                btnVerifyLittle.Enabled = bEnable;

                btnDone.Enabled = bEnable;
                btnBeginCapture.Enabled = bEnable;
            }
        }

        private void NotifyThumbEnrolled()
        {
            this.Thumb = fingerprintData;
            chkThumbCaptured.Checked = true;
        }

        private void NotifyIndexEnrolled()
        {
            this.IndexFinger = fingerprintData;
            chkIndexCaptured.Checked = true;
        }

        private void NotifyMiddleEnrolled()
        {
            this.MiddleFinger = fingerprintData;
            chkMiddleCaptured.Checked = true;
        }

        private void NotifyRingEnrolled()
        {
            this.RingFinger = fingerprintData;
            chkRingCaptured.Checked = true;
        }

        private void NotifyLittleEnrolled()
        {
            this.LittleFinger = fingerprintData;
            chkLittleCaptured.Checked = true;
        }

        private void btnDone_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private byte[] GetImageBytes(Image image)
        {
            MemoryStream ms = new();
            image.Save(ms, ImageFormat.Png);
            return ms.ToArray();
        }

        public FingerPrint GetFingerPrints()
        {
            return new FingerPrint
            {
                Thumb = this.Thumb,
                ThumbThumbnail = ThumbThumbnail,
                IndexFinger = IndexFinger,
                IndexFingerThumbnail = IndexFingerThumbnail,
                MiddleFinger = MiddleFinger,
                MiddleFingerThumbnail = MiddleFingerThumbnail,
                RingFinger = RingFinger,
                RingFingerThumbnail = RingFingerThumbnail,
                LittleFinger = LittleFinger,
                LittleFingerThumbnail = LittleFingerThumbnail
            };
        }
    }
}
