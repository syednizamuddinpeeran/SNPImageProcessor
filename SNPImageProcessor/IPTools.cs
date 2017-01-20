using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SNPImageProcessor
{
    public class IPTools
    {
        internal static byte[] ColorCorrect(byte[] rawData, PixelFormat imgPixelFormat, int Height, int StrideWidth,int newColor,char c)
        {
            int ByteOccupiciedByAPixel = Bitmap.GetPixelFormatSize(imgPixelFormat) / 8;
            for (int i = 0; i < Height; i++)
            {
                int currentRow = i * StrideWidth;
                for (int j = 0; j < StrideWidth; j += ByteOccupiciedByAPixel)
                {
                    if (c == 'r'||c=='a')
                    {
                        int R = rawData[currentRow + j + 0] + newColor;
                        rawData[currentRow + j + 0] = (byte)(((R) >= 255) ? 255 : ((R <= 0) ? 0 : R));
                    }
                    if (c == 'g' || c == 'a')
                    {
                        int G = rawData[currentRow + j + 1] + newColor;
                        rawData[currentRow + j + 1] = (byte)(((G) >= 255) ? 255 : ((G <= 0) ? 0 : G));
                    }
                    if (c == 'b' || c == 'a')
                    {
                        int B = rawData[currentRow + j + 2] + newColor;
                        rawData[currentRow + j + 2] = (byte)(((B) >= 255) ? 255 : ((B <= 0) ? 0 : B));
                    }
                }
            }
            return rawData;
        }
        internal static byte[] HistogramStretch(byte[] rawData, PixelFormat imgPixelFormat, int Height, int StrideWidth)
        {
            float Rmax = 0;
            float Gmax = 0;
            float Bmax = 0;
            float Rmin = 255;
            float Gmin = 255;
            float Bmin = 255;
            int ByteOccupiciedByAPixel = Bitmap.GetPixelFormatSize(imgPixelFormat) / 8;
            for (int i = 0; i < Height; i++)
            {
                int currentRow = i * StrideWidth;
                for (int j = 0; j < StrideWidth; j += ByteOccupiciedByAPixel)
                {
                    float R = rawData[currentRow + j + 0];
                    float G = rawData[currentRow + j + 1];
                    float B = rawData[currentRow + j + 2];
                    if (R > Rmax)
                        Rmax = R;
                    if (G > Gmax)
                        Gmax = G;
                    if (B > Bmax)
                        Bmax = B;
                    if (R < Rmin)
                        Rmin = R;
                    if (G < Gmin)
                        Gmin = G;
                    if (B < Bmin)
                        Bmin = B;
                }
            }
            for (int i = 0; i < Height; i++)
            {
                int currentRow = i * StrideWidth;
                for (int j = 0; j < StrideWidth; j += ByteOccupiciedByAPixel)
                {
                    rawData[currentRow + j + 0] =(byte) (((rawData[currentRow + j + 0] - Rmin) / (Rmax - Rmin))*255);
                    rawData[currentRow + j + 1] =(byte)(((rawData[currentRow + j + 1] - Gmin) / (Gmax - Gmin))*255);
                    rawData[currentRow + j + 2] =(byte)(((rawData[currentRow + j + 2] - Bmin) / (Bmax - Bmin))*255);
                }
            }
            return rawData;
        }
        internal static byte[] ContrastChange(byte[] rawData, PixelFormat imgPixelFormat, int Height, int StrideWidth,double alpha)
        {
            int ByteOccupiciedByAPixel = Bitmap.GetPixelFormatSize(imgPixelFormat) / 8;
            for (int i = 0; i < Height; i++)
            {
                int currentRow = i * StrideWidth;
                for (int j = 0; j < StrideWidth; j += ByteOccupiciedByAPixel)
                {
                    rawData[currentRow + j + 0] = (byte)((double)(alpha * (rawData[currentRow + j + 0] - 128) + 128));
                    rawData[currentRow + j + 1] = (byte)((double)(alpha * (rawData[currentRow + j + 1] - 128) + 128));
                    rawData[currentRow + j + 2] = (byte)((double)(alpha * (rawData[currentRow + j + 2] - 128) + 128));
                }
            }
            return rawData;
        }
        #region Direct use Functions
        /// <summary>
        /// Returns a new instance of the bitmap
        /// </summary>
        /// <param name="img">Bitmap to be copied</param>
        /// <returns>Copy of the original bitmap</returns>
        public static Bitmap CreateACopy(Bitmap img)
        {
            #region Extracting Marshal info
            int Width = img.Width;
            int Height = img.Height;
            PixelFormat imgPixelFormat = img.PixelFormat;
            BitmapData data = img.LockBits(new Rectangle(0, 0, Width, Height), ImageLockMode.ReadOnly, img.PixelFormat);
            int dataHeight = data.Height;
            int strideWidth = data.Stride;
            byte[] rawData = new byte[dataHeight * strideWidth];
            Marshal.Copy(data.Scan0, rawData, 0, rawData.Length);
            int ByteOccupiciedByAPixel = Bitmap.GetPixelFormatSize(imgPixelFormat) / 8;
            #endregion
            #region Create new image
            Bitmap result = new Bitmap(Width, Height, imgPixelFormat);
            BitmapData dataResult = result.LockBits(new Rectangle(0, 0, Width, Height), ImageLockMode.ReadOnly, result.PixelFormat);
            byte[] rawDataResult = new byte[dataHeight * strideWidth];
            Marshal.Copy(dataResult.Scan0, rawDataResult, 0, rawDataResult.Length);
            #endregion

            #region Processing
            for (int i = 0; i < Height; i++)
            {
                int currentRow = i * strideWidth;
                for (int j = 0; j < strideWidth; j += ByteOccupiciedByAPixel)
                {
                    rawDataResult[currentRow + j + 0] = rawData[currentRow + j + 0];
                    rawDataResult[currentRow + j + 1] = rawData[currentRow + j + 1];
                    rawDataResult[currentRow + j + 2] = rawData[currentRow + j + 2];
                }
            }
            #endregion
            Marshal.Copy(rawData, 0, data.Scan0, rawData.Length);
            img.UnlockBits(data);
            Marshal.Copy(rawDataResult, 0, dataResult.Scan0, rawDataResult.Length);
            result.UnlockBits(dataResult);
            return result;
        }
        /// <summary>
        /// Returns a new bitmap with a stretched histrogram
        /// </summary>
        /// <param name="img">Input bitmap</param>
        /// <returns>Output Bitmap With HistogarmStretched</returns>
        public static Bitmap HistogramStretch(Bitmap img)
        {
            #region Extracting Marshal info
            int Width = img.Width;
            int Height = img.Height;
            PixelFormat imgPixelFormat = img.PixelFormat;
            BitmapData data = img.LockBits(new Rectangle(0, 0, Width, Height), ImageLockMode.ReadOnly, img.PixelFormat);
            int dataHeight = data.Height;
            int strideWidth = data.Stride;
            byte[] rawData = new byte[dataHeight * strideWidth];
            Marshal.Copy(data.Scan0, rawData, 0, rawData.Length);
            int ByteOccupiciedByAPixel = Bitmap.GetPixelFormatSize(imgPixelFormat) / 8;
            #endregion
            #region Create new image
            Bitmap result = new Bitmap(Width, Height,imgPixelFormat);
            BitmapData dataResult = result.LockBits(new Rectangle(0, 0, Width, Height), ImageLockMode.ReadOnly, result.PixelFormat);
            byte[] rawDataResult = new byte[dataHeight * strideWidth];
            Marshal.Copy(dataResult.Scan0, rawDataResult, 0, rawDataResult.Length);
            #endregion

            #region Processing
            float Rmax = 0;
            float Gmax = 0;
            float Bmax = 0;
            float Rmin = 255;
            float Gmin = 255;
            float Bmin = 255;
            for (int i = 0; i < Height; i++)
            {
                int currentRow = i * strideWidth;
                for (int j = 0; j < strideWidth; j += ByteOccupiciedByAPixel)
                {
                    float R = rawData[currentRow + j + 0];
                    float G = rawData[currentRow + j + 1];
                    float B = rawData[currentRow + j + 2];
                    if (R > Rmax)
                        Rmax = R;
                    if (G > Gmax)
                        Gmax = G;
                    if (B > Bmax)
                        Bmax = B;
                    if (R < Rmin)
                        Rmin = R;
                    if (G < Gmin)
                        Gmin = G;
                    if (B < Bmin)
                        Bmin = B;
                }
            }
            for (int i = 0; i < Height; i++)
            {
                int currentRow = i * strideWidth;
                for (int j = 0; j < strideWidth; j += ByteOccupiciedByAPixel)
                {
                    rawDataResult[currentRow + j + 0] = (byte)(((rawData[currentRow + j + 0] - Rmin) / (Rmax - Rmin)) * 255);
                    rawDataResult[currentRow + j + 1] = (byte)(((rawData[currentRow + j + 1] - Gmin) / (Gmax - Gmin)) * 255);
                    rawDataResult[currentRow + j + 2] = (byte)(((rawData[currentRow + j + 2] - Bmin) / (Bmax - Bmin)) * 255);
                }
            }
            #endregion
            Marshal.Copy(rawData, 0, data.Scan0, rawData.Length);
            img.UnlockBits(data);
            Marshal.Copy(rawDataResult, 0, dataResult.Scan0, rawDataResult.Length);
            result.UnlockBits(dataResult);
            return result;
        }
        public static Bitmap LinearHistogramScaling(Bitmap img,int startVaue,int EndValue)
        {
            #region Extracting Marshal info
            int Width = img.Width;
            int Height = img.Height;
            PixelFormat imgPixelFormat = img.PixelFormat;
            BitmapData data = img.LockBits(new Rectangle(0, 0, Width, Height), ImageLockMode.ReadOnly, img.PixelFormat);
            int dataHeight = data.Height;
            int strideWidth = data.Stride;
            byte[] rawData = new byte[dataHeight * strideWidth];
            Marshal.Copy(data.Scan0, rawData, 0, rawData.Length);
            int ByteOccupiciedByAPixel = Bitmap.GetPixelFormatSize(imgPixelFormat) / 8;
            #endregion
            #region Create new image
            Bitmap result = new Bitmap(Width, Height,imgPixelFormat);
            BitmapData dataResult = result.LockBits(new Rectangle(0, 0, Width, Height), ImageLockMode.ReadOnly, result.PixelFormat);
            byte[] rawDataResult = new byte[dataHeight * strideWidth];
            Marshal.Copy(dataResult.Scan0, rawDataResult, 0, rawDataResult.Length);
            #endregion

            #region Processing
            float Rmax = EndValue;
            float Gmax = EndValue;
            float Bmax = EndValue;
            float Rmin = startVaue;
            float Gmin = startVaue;
            float Bmin = startVaue;
            for (int i = 0; i < Height; i++)
            {
                int currentRow = i * strideWidth;
                for (int j = 0; j < strideWidth; j += ByteOccupiciedByAPixel)
                {
                    #region Red
                    if (rawData[currentRow + j + 0] < Rmin)
                    {
                        rawDataResult[currentRow + j + 0] = 0;
                    }
                    else if (rawData[currentRow + j + 0] > Rmax)
                    {
                        rawDataResult[currentRow + j + 0] = 255;
                    }
                    else
                    {
                        rawDataResult[currentRow + j + 0] = (byte)(((rawData[currentRow + j + 0] - Rmin) / (Rmax - Rmin)) * 255);
                    }
                    #endregion
                    #region Green
                    if (rawData[currentRow + j + 1] < Gmin)
                    {
                        rawDataResult[currentRow + j + 1] = 0;
                    }
                    else if (rawData[currentRow + j + 1] > Gmax)
                    {
                        rawDataResult[currentRow + j + 1] = 255;
                    }
                    else
                    {
                        rawDataResult[currentRow + j + 1] = (byte)(((rawData[currentRow + j + 1] - Gmin) / (Gmax - Gmin)) * 255);
                    }
                    #endregion
                    #region Blue
                    if (rawData[currentRow + j + 2] < Bmin)
                    {
                        rawDataResult[currentRow + j + 2] = 0;
                    }
                    else if (rawData[currentRow + j + 2] > Bmax)
                    {
                        rawDataResult[currentRow + j + 2] = 255;
                    }
                    else
                    {
                        rawDataResult[currentRow + j + 2] = (byte)(((rawData[currentRow + j + 2] - Bmin) / (Bmax - Bmin)) * 255);
                    }
                    #endregion
                }
            }
            #endregion
            Marshal.Copy(rawData, 0, data.Scan0, rawData.Length);
            img.UnlockBits(data);
            Marshal.Copy(rawDataResult, 0, dataResult.Scan0, rawDataResult.Length);
            result.UnlockBits(dataResult);
            return result;
        }
        #endregion
    }
}
