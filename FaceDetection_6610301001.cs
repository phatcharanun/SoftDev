using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.UI;

namespace Example1
{
    public partial class FormMain : Form
    {
        private VideoCapture _capture = null;
        private Mat _frame = new Mat(); // Mat สำหรับเก็บเฟรมจากกล้อง
        private bool IsConnect = true;
        private bool isCapturing = true; // Track connection state

        CascadeClassifier _cascadeClassifier = new CascadeClassifier(@"D:\New folder\haarcascade_frontalface_default.xml");

        private void ProcessFrame(object sender, EventArgs e)
        {
            if (_capture == null || _capture.Ptr == IntPtr.Zero) return;

            // ดึงเฟรมจากกล้องโดยใช้ Retrieve
            _capture.Retrieve(_frame);
            if (!_frame.IsEmpty)
            {
                // แปลงเฟรมเป็นรูปแบบ Image<Bgr, Byte>
                using (var imageFrame = _frame.ToImage<Bgr, Byte>())
                {
                    if (imageFrame != null)
                    {
                        // แปลงภาพเป็น Gray สำหรับการตรวจจับใบหน้า
                        using (var grayFrame = imageFrame.Convert<Gray, byte>())
                        {
                            // ตรวจจับใบหน้า
                            var faces = _cascadeClassifier.DetectMultiScale(grayFrame, 1.1, 10);

                            // วาดกรอบรอบใบหน้าที่ตรวจพบ
                            foreach (var face in faces)
                            {
                                imageFrame.Draw(face, new Bgr(Color.MistyRose), 3);
                            }

                            // แสดงผลใน imageBox1
                            Invoke(new Action(() =>
                            {
                                imageBox1.Image = imageFrame;

                                // แสดงภาพใบหน้าที่ตัดมาใน imageBox2
                                if (faces.Length > 0)
                                {
                                    Rectangle face_roi = new Rectangle(faces[0].X, faces[0].Y, 190, 190);
                                    grayFrame.ROI = face_roi;
                                    imageBox2.Image = grayFrame.Copy();
                                }
                            }));
                        }
                    }
                }
            }
        }

        private void FormMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (_capture != null)
            {
                _capture.Pause();
                _capture.Dispose();
                _capture = null;
            }
            // แสดงข้อความแจ้งเตือน (ถ้าต้องการ)
            DialogResult result = MessageBox.Show("คุณต้องการปิดโปรแกรมใช่หรือไม่?",
                                                  "ยืนยันการปิด",
                                                  MessageBoxButtons.YesNo);
            if (result == DialogResult.No)
            {
                e.Cancel = true; // ยกเลิกการปิดฟอร์ม
            }
        }



        public FormMain()
        {
            InitializeComponent();
            buttonStsrt.Enabled = false; // ปิดปุ่ม Start ไว้ก่อนจนกว่าจะ Connect
            timerClock.Enabled = true;   // เปิดตัวโชว์นาฬิกา
        }


        private void FormMain_Load(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {

        }



        private void buttonFlipHor_Click(object sender, EventArgs e)
        {
            if (_capture != null)
                _capture.FlipHorizontal = !_capture.FlipHorizontal;
        }

        private void buttonFlipVer_Click(object sender, EventArgs e)
        {
            if (_capture != null)
                _capture.FlipVertical = !_capture.FlipVertical;
        }

        private void imageBox1_Click(object sender, EventArgs e)
        {

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void fileToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged_1(object sender, EventArgs e)
        {
            try
            {
                _capture = new VideoCapture();
                _capture.ImageGrabbed += ProcessFrame;
                _frame = new Mat();



            }
            catch (NullReferenceException excpt)
            {
                MessageBox.Show(excpt.Message);
            }

        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            
            if (IsConnect)
            {
                
                buttonConnect.Text = "Disconnect";
                tbCarmera.BackColor = Color.Green;
                tbCarmera.Text = "Connected";
                buttonStsrt.Enabled = true;

                try
                {
                    _capture = new VideoCapture();
                    _capture.ImageGrabbed += ProcessFrame;
                    _frame = new Mat();

                }
                catch (NullReferenceException excpt)
                {
                    MessageBox.Show(excpt.Message);
                }
               
            }
            else
            {
                
                buttonConnect.Text = "Connect";
                tbCarmera.BackColor = Color.Red;
                tbCarmera.Text = "DisConnected";
                buttonStsrt.Enabled = false;

                try
                {
                    if (_capture != null)
                    {
                        _capture.Pause();
                        _capture.Dispose();

                    }
                 

                }
                catch (NullReferenceException excpt)
                {
                    MessageBox.Show(excpt.Message);
                }
               

            }
            IsConnect = !IsConnect;

        }



        private void button5_Click(object sender, EventArgs e)
        {
            if (isCapturing)
            {
               
                buttonStsrt.Text = "Pause";
                textBox2.Text = "Record";
                textBox2.BackColor = Color.Green;
                buttonConnect.Enabled = false;
                if (_capture != null)
                {
                    _capture.Start();
                  
                }

            }
            else
            {
                buttonStsrt.Text = "Start";
                textBox2.BackColor = Color.Red;
                textBox2.Text = "No record";
                buttonConnect.Enabled = true;
                if (_capture != null)
                {
                    _capture.Pause();
                }

                
            }
            isCapturing = !isCapturing;

        }
    


        private void textBox1_TextChanged_2(object sender, EventArgs e)
        {
            try
            {
                _capture = new VideoCapture();
                _capture.ImageGrabbed += ProcessFrame;
                _frame = new Mat();

            }
            catch (NullReferenceException excpt)
            {
                MessageBox.Show(excpt.Message);
            }
        }

        private void timerClock_Tick(object sender, EventArgs e)
        {
            string formatStringClock = "HH:mm:ss";
            string formatStringDate = "yyyy-MMM-dd";

            DateTime dtNow = DateTime.Now;
            statusLabelClock.Text = dtNow.ToString(formatStringClock);
            statusLabalDate.Text = dtNow.ToString(formatStringDate);
        }
    }
}
