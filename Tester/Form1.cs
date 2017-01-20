using System;
using System.Windows.Forms;
using SNPImageProcessor;
using System.Drawing;
using System.Diagnostics;
using System.Windows.Forms.DataVisualization.Charting;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;

namespace Tester
{
    public partial class Form1 : Form
    {
        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr GetDesktopWindow();
        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr GetWindowDC(IntPtr window);
        [DllImport("gdi32.dll", SetLastError = true)]
        public static extern uint GetPixel(IntPtr dc, int x, int y);
        [DllImport("user32.dll", SetLastError = true)]
        public static extern int ReleaseDC(IntPtr window, IntPtr dc);
        ////////////////////////////////////////objects///////////////////////////
        /// <summary>
        /// Original instance of the IPImage class
        /// </summary>
        IPImage img;
        /// <summary>
        /// Output instance of the IPImage class
        /// </summary>
        IPImage Oimg;
        /// <summary>
        /// Array to store the histogram
        /// </summary>
        int[] histVal;
        /// <summary>
        /// property determins the case of the background worker
        /// </summary>
        string _case = "hist";
        /// <summary>
        /// Property to determine if histogram show be displayed
        /// </summary>
        bool histOn = true;
        /// <summary>
        /// value of the bw treshold
        /// </summary>
        int intTresh = 100;
        int rC = 0;
        int gC = 0;
        int bC = 0;
        int intbright = 0;
        double doubelContarst = 0;
        /// <summary>
        /// Property to determine if original image show be displayed
        /// </summary>
        bool originalImageOn = true;
        /// <summary>
        /// Color value of the pixel under the mouse pointer
        /// </summary>
        Color mousePixelColor;
        /// <summary>
        /// sets thevalue of colour picker
        /// </summary>
        Color colorPicker;
        /// <summary>
        /// Creates the openFile dialogue
        /// </summary>
        OpenFileDialog openFile = new OpenFileDialog();
        SaveFileDialog saveFile = new SaveFileDialog();
        /// <summary>
        /// TresholdTool window
        /// </summary>
        Treshold treshForm = new Treshold();
        ColorCorrectioncs ccForm = new ColorCorrectioncs();
        Brightness brightForm = new Brightness();
        contrast contrastForm = new contrast();
        /// <summary>
        /// Label to display the color value of the hovered pixel
        /// </summary>
        Label color = new Label();
        /// <summary>
        /// First bigger picture box used to display the output
        /// </summary>
        PictureBox pb_1 = new PictureBox();
        /// <summary>
        /// Second smaller picture box to display original image
        /// </summary>
        PictureBox pb_2 = new PictureBox();
        /// <summary>
        /// Button to preview the status of th ebackground worker
        /// </summary>
        Button bt_busy = new Button();
        /// <summary>
        /// Stop watch fopr diagonstics
        /// </summary>
        Stopwatch sw = new Stopwatch();
        /////////////////////////////////////Methords/////////////////////////////
        /// <summary>
        /// First methord
        /// </summary>
        public Form1()
        {
            InitializeComponent();
            this.Resize += Form1_Resize;
            backgroundWorker1.RunWorkerCompleted += BackgroundWorker1_RunWorkerCompleted;
            pb_1.SizeMode = PictureBoxSizeMode.Zoom;
            pb_2.SizeMode = PictureBoxSizeMode.Zoom;
            hist.Series[0].Name = "Histogarm";
            hist.Series[0].ChartType = SeriesChartType.Area;
            hist.ChartAreas[0].AxisX.MajorGrid.LineWidth = 0;
            hist.ChartAreas[0].AxisY.MajorGrid.LineWidth = 0;
            hist.Series[0].Color = Color.Gray;
            this.Controls.Add(pb_1);
            this.Controls.Add(pb_2);
            this.Controls.Add(bt_busy);
            this.Controls.Add(color);
            color.Font = new Font("Ariel", 8);
            color.Text = "0,0:0,0,0";
            bt_busy.BackColor = Color.Green;
            resizeAll();
            bt_busy.BackColor = Color.Green;
            timer1.Start();
            this.MouseDoubleClick+=Form1_MouseClick;
            pb_1.MouseClick += Form1_MouseClick;
            pb_2.MouseClick += Form1_MouseClick;
            if (File.Exists("temp.jpeg"))
            {
                if (MessageBox.Show("Would you like to reload last session?", "", MessageBoxButtons.YesNo)== DialogResult.OK) 
                {
                    img = new IPImage("temp.jpeg");
                    Oimg = new IPImage("temp.jpeg");
                    pb_1.Image = img.Img;
                    pb_2.Image = img.Img;
                }
            }
        }
        /// <summary>
        /// Resize event handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_Resize(object sender, EventArgs e)
        {
            resizeAll();
        }
        /// <summary>
        /// Turn off/on the original image preview
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void originalImageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (originalImageOn)
                originalImageOn = false;
            else
                originalImageOn = true;
            if (originalImageOn)
                pb_2.Show();
            else
                pb_2.Hide();
        }
        /// <summary>
        /// Turn off/on histogram
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void onoffToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (histOn)
                histOn = false;
            else
                histOn = true;
            if (histOn)
                hist.Show();
            else
                hist.Hide();
        }
        /// <summary>
        /// Open new image file
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            bt_busy.BackColor = Color.Red;
            if(img!=null)
                img.Dispose();
            img = null;
            Oimg = null;
            GC.Collect();
            File.Delete("temp.jpeg");
            if (openFile.ShowDialog() == DialogResult.OK)
            {
                if (File.Exists("temp.jpeg")) 
                {
                    File.Delete("temp.jpeg");
                }
                File.Copy(openFile.FileName, "temp.jpeg");
                img = new IPImage("temp.jpeg");
                pb_1.Image = img.Img;
                pb_2.Image = new Bitmap(openFile.FileName);
            }
            bt_busy.ForeColor = Color.Green;
            backgroundWorker1.RunWorkerAsync();
        }
        /// <summary>
        /// Reload the file
        /// Also overrides the temp file
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void reloadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            img.Dispose();
            img = null;
            Oimg = null;
            GC.Collect();
            bt_busy.BackColor = Color.Red;
            File.Delete("temp.jpeg");
            File.Copy(openFile.FileName, "temp.jpeg");
            img = new IPImage("temp.jpeg");
            pb_1.Image = img.Img;
            pb_2.Image = new Bitmap(openFile.FileName);
            bt_busy.ForeColor = Color.Green;
            backgroundWorker1.RunWorkerAsync();
            saveBitMap(img.Img);
            MessageBox.Show("works");
        }
        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!(saveFile.FileName.Equals("")))
            {
                pb_1.Image = img.Img;
                img.Img.Save(saveFile.FileName+".bmp");
            }
            else
                saveAsToolStripMenuItem_Click(sender, e);
        }
        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveFile.ShowDialog();
            saveToolStripMenuItem_Click(sender, e);
        }
        /// <summary>
        /// Normal Histogram Display
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void normalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            bt_busy.BackColor = Color.Red;                        
            if (backgroundWorker1.IsBusy)
            {
                Thread.Sleep(500);
                bt_busy.BackColor = Color.Red;
            }
            _case = "hist";
            backgroundWorker1.RunWorkerAsync();
        }
        /// <summary>
        /// Convert the image to gray scale
        /// Present problem: unbale to overwrite the temp file
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void grayScaleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _case = "gray";
            callBackgroundWorker();
        }
        /// <summary>
        /// Convert to red only
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void redToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            _case = "red";
            callBackgroundWorker();
        }
        /// <summary>
        /// Convert to green
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void greenToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            _case = "green";
            callBackgroundWorker();
        }
        /// <summary>
        /// Convert to blue
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void blueToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            _case = "blue";
            callBackgroundWorker();
        }
        /// <summary>
        /// Preview Actually Processed image
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void allToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pb_1.Image = img.Img;
        }
        /// <summary>
        /// Preview red channel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void redToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _case = "getred";
            callBackgroundWorker();
        }
        /// <summary>
        /// Preview green channel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void greenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _case = "getgreen";
            callBackgroundWorker();
        }
        /// <summary>
        /// Preview blue channel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void blueToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _case = "getblue";
            callBackgroundWorker();
        }
        /// <summary>
        /// Preview grayscale
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void grayToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _case = "getgray";
            callBackgroundWorker();
        }
        /// <summary>
        /// Converts to gray scale
        /// Calls showTreshControl
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void normalToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            _case = "bw";
            callBackgroundWorker();
        }
        /// <summary>
        /// Occurs when treshold is changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tresh_Scroll(object sender, EventArgs e)
        {
            intTresh = treshForm.treshTrack.Value;
            _case = "bw";
            callBackgroundWorker();
        }
        /// <summary>
        /// Display the treshold window
        /// </summary>
        private void showTreshControl()
        {
            treshForm.treshTrack.Scroll += tresh_Scroll;
            treshForm.FormClosed += treshForm_FormClosed;
            treshForm.Show();
        }
        /// <summary>
        /// Safely closes the treshold window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void treshForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            treshForm.Dispose();
            treshForm = new Treshold();
            treshForm.treshTrack.Scroll += tresh_Scroll;
            treshForm.FormClosed += treshForm_FormClosed;
            treshForm.Hide();
        }
        /// <summary>
        /// Extracts the selected pixels from the image
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void getPixelsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _case = "getpixels";
            callBackgroundWorker();
        }
        private void colorCorrectionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ccForm.Show();
            ccForm.FormClosed += ccForm_FormClosed;
            ccForm.Rup.Click += RupClick;
            ccForm.Gup.Click += GupClick;
            ccForm.Bup.Click += BupClick;
            ccForm.Rdown.Click += RdownClick;
            ccForm.Gdown.Click += GdownClick;
            ccForm.Bdown.Click += BdownClick;
        }
        void RupClick(object sender, EventArgs e)
        {
            rC++;
            _case = "rcc";
            callBackgroundWorker();
        }
        void GupClick(object sender, EventArgs e)
        {
            gC++;
            _case = "gcc";
            callBackgroundWorker();
        }
        void BupClick(object sender, EventArgs e)
        {
            bC++;
            _case = "bcc";
            callBackgroundWorker();
        }
        void RdownClick(object sender, EventArgs e)
        {
            rC--;
            _case = "rcc";
            callBackgroundWorker();
        }
        void GdownClick(object sender, EventArgs e)
        {
            gC--;
            _case = "gcc";
            callBackgroundWorker();
        }
        void BdownClick(object sender, EventArgs e)
        {
            bC--;
            _case = "bcc";
            callBackgroundWorker();
        }
        void ccForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            ccForm.Dispose();
            ccForm = new ColorCorrectioncs();
            ccForm.Rup.Click += RupClick;
            ccForm.Gup.Click += GupClick;
            ccForm.Bup.Click += BupClick;
            ccForm.Rdown.Click += RdownClick;
            ccForm.Gdown.Click += GdownClick;
            ccForm.Bdown.Click += BdownClick;
            ccForm.FormClosed += treshForm_FormClosed;
            ccForm.Hide();
        }
        private void brightnessToolStripMenuItem_Click(object sender, EventArgs e)
        {
            brightForm.Show();
            brightForm.up.Click += BrightclickUp;
            brightForm.down.Click += BrightclickDown;
            brightForm.FormClosed += brightForm_FormClosed;
        }
        void BrightclickUp(object sender, EventArgs e)
        {
            intbright ++;
            _case = "bright";
            callBackgroundWorker();
        }
        void BrightclickDown(object sender, EventArgs e)
        {
            intbright--;
            _case = "bright";
            callBackgroundWorker();
        }
        void brightForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            brightForm.Dispose();
            brightForm = new Brightness();
            brightForm.Hide();
        }
        private void stretchHistogramToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _case = "stretchHistogram";
            callBackgroundWorker();
        }
        private void contrastToolStripMenuItem_Click(object sender, EventArgs e)
        {
            contrastForm.Show();
            contrastForm.up.Click += ContrastclickUp;
            contrastForm.down.Click += ContrastclickDown;
            contrastForm.FormClosed += contrastForm_FormClosed;
        }
        private void ContrastclickUp(object sender, EventArgs e)
        {
            _case = "contrast";
            callBackgroundWorker();
        }
        private void ContrastclickDown(object sender, EventArgs e)
        {
            doubelContarst -= .1;
            _case = "contrast";
            callBackgroundWorker();
        }
        void contrastForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            contrastForm.Dispose();
            contrastForm = new contrast();
            contrastForm.Hide();
        }
        void callBackgroundWorker() 
        {
            bt_busy.BackColor = Color.Red;
            if (backgroundWorker1.IsBusy)
            {
                Thread.Sleep(500);
                bt_busy.BackColor = Color.Red;
            }
            backgroundWorker1.RunWorkerAsync();        
        }
        /// <summary>
        /// Occurs when BackgroundWorker1 has completed the given task
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BackgroundWorker1_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            if (!(_case.Contains("get")))
            {
                hist.Series[0].Points.DataBindY(histVal);
                pb_1.Image = img.Img;
            }
            sw.Stop();
            bt_busy.BackColor = Color.Green;                        
            //MessageBox.Show(sw.ElapsedMilliseconds.ToString());
        }
        /// <summary>
        /// Executes when back ground worker is called
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void backgroundWorker1_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            try
            {
                sw.Restart();
                switch (_case)
                {
                    case "hist":
                        histVal = img.GetHist();
                        break;
                    case "getred":
                        pb_1.Image=img.GetRed();
                        //hist.Series[0].Points.DataBindY(new IPImage(img.GetRed()).GetHist());
                        break;
                    case "getblue":
                        pb_1.Image = img.GetBlue();
                        //histVal =new IPImage(img.GetBlue()).GetHist();
                        break;
                    case "getgreen":
                        pb_1.Image = img.GetGreen();
                        //histVal = new IPImage(img.GetGreen()).GetHist();
                        break;
                    case "getgray":
                        pb_1.Image =img.GetGray();
                        //histVal = new IPImage(img.GetGray()).GetHist();
                        break;
                    case "red":
                        img.KeepOnlyRed();
                        histVal = img.GetHist();
                        break;
                    case "blue":
                        img.KeepOnlyBlue();
                        histVal = img.GetHist();
                        break;
                    case "green":
                        img.KeepOnlyGreen();
                        histVal = img.GetHist();
                        break;
                    case "gray":
                        img.ConvertToGray();
                        histVal = img.GetHist();
                        break;
                    case "getpixels":
                        img.KeepOnlyPixelAround(colorPicker);
                        histVal = img.GetHist();
                        break;
                    case "bw":
                        img.ConvertToBw(intTresh);
                        histVal = img.GetHist();
                        break;
                    case "rcc":
                        img.ColourCorrect(rC, 'r');
                        histVal = img.GetHist();
                        break;
                    case "gcc":
                        img.ColourCorrect(rC, 'g');
                        histVal = img.GetHist();
                        break;
                    case "bcc":
                        img.ColourCorrect(rC, 'b');
                        histVal = img.GetHist();
                        break;
                    case "bright":
                        img.ColourCorrect(intbright, 'a');
                        histVal = img.GetHist();
                        break;
                    case "stretchHistogram":
                        img.StretchHistogram();
                        histVal = img.GetHist();
                        break;
                    case "contrast":
                        img.ContrastChange(doubelContarst);
                        histVal = img.GetHist();
                        break;
                    default:
                        MessageBox.Show("Unrecogonised operation");
                        break;
                }
            }
            catch (Exception ee)
            {
                MessageBox.Show("Openration wasn't performed because "+ee.Message+" "+ee.Source);
            }
            GC.Collect();
        }
        /// <summary>
        /// Resizes all the controls on the window
        /// </summary>
        private void resizeAll()
        {
            bt_busy.Location = new Point((int)(this.Width -100), (int)(20));
            bt_busy.Width = 20;
            bt_busy.Height = 20;
            color.Location = new Point((int)(this.Width - 500), (int)(25));
            color.ForeColor = Color.Black;
            color.AutoSize = true;
            pb_1.Location = new Point((int)(this.Width * .05), (int)(this.Height * .05));
            pb_1.Width = (int)(this.Width * .95 / 2);
            pb_1.Height = (int)(this.Height * .95);
            pb_2.Location = new Point((int)(this.Width * 1.05 / 2), (int)(this.Height * 1.05 / 2));
            pb_2.Width = (int)(this.Width * .95 / 2);
            pb_2.Height = (int)(this.Height * .95 / 2);
            hist.Location = new Point((int)(this.Width * 1.05 / 2), (int)(this.Height * .05));
            hist.Width = (int)(this.Width * .95 / 2);
            hist.Height = (int)(this.Height * .95 / 2);
        }
        /// <summary>
        /// Save the bitmap
        /// </summary>
        /// <param name="outputFileName">outputfilename</param>
        private void saveBitMap(Bitmap tempImg) 
        {
            tempImg.Save("temp.jpg");
        }
        private Color GetColorAt(int x, int y)
        {
            IntPtr desk = GetDesktopWindow();
            IntPtr dc = GetWindowDC(desk);
            int a = (int)GetPixel(dc, x, y);
            ReleaseDC(desk, dc);
            return Color.FromArgb(255, (a >> 0) & 0xff, (a >> 8) & 0xff, (a >> 16) & 0xff);
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            mousePixelColor = GetColorAt(MousePosition.X, MousePosition.Y);
            color.Text = "X:" + MousePosition.X + ",Y:" + MousePosition.Y + "Red:" + mousePixelColor.R.ToString() + ",Green" + mousePixelColor.G.ToString() + ",Blue:" + mousePixelColor.B.ToString();
            color.BackColor = mousePixelColor;
        }
        private void Form1_MouseClick(object sender, MouseEventArgs e)
        {
            colorPicker = GetColorAt(MousePosition.X, MousePosition.Y);
            bt_busy.BackColor = colorPicker;
        }
    }
}