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

namespace Example1
{
    public partial class FormMain : Form
    {
        private VideoCapture _capture = null;
        private Mat _frame;
        private bool IsConnect = true;
        private bool isCapturing = true; // Track connection state
        private void ProcessFrame(object sender, EventArgs e)
        {
            if (_capture != null && _capture.Ptr != IntPtr.Zero)
            {
                bool canCapture = _capture.Retrieve(_frame, 0);
                if (canCapture)
                {
                    imageBox1.Image = _frame;
                }
            }

        }
        public FormMain()
        {
            InitializeComponent();
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
    }
}
