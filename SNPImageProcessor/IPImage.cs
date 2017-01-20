using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;

namespace SNPImageProcessor
{
    public class IPImage
    {

        #region Property decleration

        private Bitmap img;
        /// <summary>
        /// Stores the file as bitmap
        /// </summary>
        public Bitmap Img
        {
            get
            {
                return img;
            }
            set
            {
                img = value;
            }
        }

        private byte[] rawImg;

        public byte[] RawImg
        {
            get { return rawImg; }
            set { rawImg = value; }
        }

        private int width;

        public int Width
        {
            get { return width; }
            set { width = value; }
        }

        private int height;

        public int Height
        {
            get { return height; }
            set { height = value; }
        }

        private int strideWidth;

        private int dataHeight;

        private PixelFormat imgPixelFormat;

        public PixelFormat ImgPixelFormat
        {
            get { return imgPixelFormat; }
            set { imgPixelFormat = value; }
        }


        #endregion

        #region Methods
        
        #region Constructors

        public IPImage()
        {

        }
        public IPImage(string filename)
        {
            img = new Bitmap(filename);
            width = img.Width;
            height = img.Height;
            imgPixelFormat = img.PixelFormat;
            reset();
        }
        public IPImage(Bitmap bit)
        {
            strideWidth = bit.Width;
            dataHeight = bit.Height;
            img = bit;
            imgPixelFormat = img.PixelFormat;
            reset();
        }
        public IPImage(Stream filestream)
        {
            strideWidth = new Bitmap(filestream).Width;
            dataHeight = new Bitmap(filestream).Height;
            img = new Bitmap(filestream);
            imgPixelFormat = img.PixelFormat;
            reset();
        }

        #endregion
        
        #region CoreFucntionalities

        public void Dispose()
        {
            img.Dispose();
        }
        public void reset() 
        {
            BitmapData data= img.LockBits(new Rectangle(0, 0, Width, Height), ImageLockMode.ReadOnly, img.PixelFormat);
            dataHeight = data.Height;
            strideWidth = data.Stride;
            rawImg = new byte[dataHeight * strideWidth];
            Marshal.Copy(data.Scan0, rawImg, 0, rawImg.Length);
            img.UnlockBits(data);
        }
        public void ComfirmChanges() 
        {
            BitmapData data = img.LockBits(new Rectangle(0, 0, Width, Height), ImageLockMode.ReadOnly, img.PixelFormat);
            Marshal.Copy(rawImg, 0, data.Scan0,rawImg.Length);
            img.UnlockBits(data);
        }

        #endregion

        #region Edit type of methods
        /// <summary>
        /// Convert the Bitmap into a grayscale
        /// Changes are permenent in the object's scope
        /// </summary>
        public void ConvertToGray()
        {
            rawImg = IPConversion.rgb2gray(rawImg, imgPixelFormat, dataHeight, strideWidth);
            ComfirmChanges();
        }
        /// <summary>
        /// Converts the bit map into an black and white bitmap with tresholding operation
        /// Changes are permenent in the object's scope
        /// </summary>
        /// <param name="tresh">Treshold to value for trsholding the image</param>
        public void ConvertToBw(int tresh)
        {
            rawImg = IPConversion.gray2bw(rawImg, imgPixelFormat, dataHeight, strideWidth, tresh);
            ComfirmChanges();
        }
        /// <summary>
        /// Extracts the red plane and removes the othe planes by making their values zero
        /// Changes are permenent in the object's scope
        /// </summary>
        public void KeepOnlyRed()
        {
            rawImg = IPConversion.rgbChannelExtract(rawImg, imgPixelFormat, dataHeight, strideWidth, 'r');
            ComfirmChanges();
        }
        /// <summary>
        /// Extracts the green plane and removes the othe planes by making their values zero
        /// Changes are permenent in the object's scope
        /// </summary>
        public void KeepOnlyGreen()
        {
            rawImg = IPConversion.rgbChannelExtract(rawImg, imgPixelFormat, dataHeight, strideWidth, 'g');
            ComfirmChanges();
        }
        /// <summary>
        /// Extracts the blue plane and removes the othe planes by making their values zero
        /// Changes are permenent in the object's scope
        /// </summary>
        public void KeepOnlyBlue()
        {
            rawImg = IPConversion.rgbChannelExtract(rawImg, imgPixelFormat, dataHeight, strideWidth, 'b');
            ComfirmChanges();
        }
        /// <summary>
        /// Extracts the pixels that match input colour
        /// Changes are permenent in the object's scope
        /// </summary>
        /// <param name="c">Colour System.Drawing.Colour(is a struct)</param>
        public void KeepOnlyPixelAround(Color c)
        {
            rawImg = IPDetermine.Getpixels(rawImg, imgPixelFormat, dataHeight, strideWidth, c);
            ComfirmChanges();
        }
        /// <summary>
        /// Corrects the colour of the image
        /// Changes are permenent in the object's scope
        /// </summary>
        /// <param name="val">value by the the chosen colour has to be changed</param>
        /// <param name="c">colour name as a char 'r' or 'g' or 'b'</param>
        public void ColourCorrect(int val, char c)
        {
            rawImg = IPTools.ColorCorrect(rawImg, imgPixelFormat, dataHeight, strideWidth, val, c);
            ComfirmChanges();
        }
        /// <summary>
        /// Stretches the hystogram of the object
        /// Changes are permenent in the object's scope
        /// </summary>
        public void StretchHistogram()
        {
            rawImg = IPTools.HistogramStretch(rawImg, imgPixelFormat, dataHeight, strideWidth);
            ComfirmChanges();
        }
        /// <summary>
        /// Changes the contrast value of the image
        /// Changes are permenent in the object's scope
        /// </summary>
        /// <param name="alpha">comtrast alpha value</param>
        public void ContrastChange(double alpha)
        {
            rawImg = IPTools.ContrastChange(rawImg, imgPixelFormat, dataHeight, strideWidth, alpha);
            ComfirmChanges();
        }
        #endregion

        #region Preview methods

        public int[] GetHist()
        {
            int[] hist = IPDetermine.GetHistogram(rawImg, imgPixelFormat, dataHeight, strideWidth);
            return hist;
        }
        public Bitmap GetGray()
        {
            Bitmap B = new Bitmap(Width, Height);
            BitmapData bd = B.LockBits(new Rectangle(0,0,width, height), ImageLockMode.ReadWrite, img.PixelFormat);
            rawImg = IPConversion.rgb2gray(rawImg, imgPixelFormat, dataHeight, strideWidth);
            Marshal.Copy(rawImg, 0, bd.Scan0, rawImg.Length);
            B.UnlockBits(bd);
            reset();
            return B;
        }
        public Bitmap GetBw(int tresh)
        {
            rawImg = IPConversion.gray2bw(rawImg, imgPixelFormat, dataHeight, strideWidth, tresh);
            Bitmap B = new Bitmap(Width, Height);
            BitmapData bd = B.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.ReadWrite, img.PixelFormat);
            Marshal.Copy(rawImg, 0, bd.Scan0, rawImg.Length);
            B.UnlockBits(bd);
            reset();
            return B;
        }
        public Bitmap GetRed()
        {
            rawImg = IPConversion.rgbChannelExtract(rawImg, imgPixelFormat, dataHeight, strideWidth, 'r');
            Bitmap B = new Bitmap(Width, Height);
            BitmapData bd = B.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.ReadWrite, img.PixelFormat);
            Marshal.Copy(rawImg, 0, bd.Scan0, rawImg.Length);
            B.UnlockBits(bd);
            reset();
            return B;
        }
        public Bitmap GetGreen()
        {
            rawImg = IPConversion.rgbChannelExtract(rawImg, imgPixelFormat, dataHeight, strideWidth, 'g');
            Bitmap B = new Bitmap(Width, Height);
            BitmapData bd = B.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.ReadWrite, img.PixelFormat);
            Marshal.Copy(rawImg, 0, bd.Scan0, rawImg.Length);
            B.UnlockBits(bd);
            reset();
            return B;
        }
        public Bitmap GetBlue()
        {
            rawImg = IPConversion.rgbChannelExtract(rawImg, imgPixelFormat, dataHeight, strideWidth, 'b');
            Bitmap B = new Bitmap(Width, Height);
            BitmapData bd = B.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.ReadWrite, img.PixelFormat);
            Marshal.Copy(rawImg, 0, bd.Scan0, rawImg.Length);
            B.UnlockBits(bd);
            reset();
            return B;
        }
        public Bitmap GetOnlyPixel(Color c)
        {
            rawImg = IPDetermine.Getpixels(rawImg, imgPixelFormat, dataHeight, strideWidth, c);
            Bitmap B = new Bitmap(Width, Height);
            BitmapData bd = B.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.ReadWrite, img.PixelFormat);
            Marshal.Copy(rawImg, 0, bd.Scan0, rawImg.Length);
            B.UnlockBits(bd);
            reset();
            return B;
        }
        #endregion

        #endregion

    }
}
