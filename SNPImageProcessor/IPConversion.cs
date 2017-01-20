using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace SNPImageProcessor
{
    class IPConversion
    {
        public static byte[] rgb2gray(byte[] rawData, PixelFormat imgPixelFormat, int Height, int StrideWidth)
        {
            int ByteOccupiciedByAPixel = Bitmap.GetPixelFormatSize(imgPixelFormat) / 8;
            for (int i = 0; i < Height; i++)
            {
                int currentRow = i * StrideWidth;
                for (int j = 0; j < StrideWidth; j += ByteOccupiciedByAPixel)
                {
                    int R = rawData[currentRow + j + 0];
                    int G = rawData[currentRow + j + 1];
                    int B = rawData[currentRow + j + 2];
                    int g = (R + G + B) / 3;
                    rawData[currentRow + j + 0] = (byte)g;
                    rawData[currentRow + j + 1] = (byte)g;
                    rawData[currentRow + j + 2] = (byte)g;
                }
            }
            return rawData;
        }
        public static Bitmap rgb2gray(Bitmap img) 
        {
            BitmapData rawData = img.LockBits(new Rectangle(0, 0, img.Width, img.Height), ImageLockMode.ReadWrite, img.PixelFormat);
            int byteArraySize = rawData.Stride * rawData.Height;
            byte[] result = new byte[byteArraySize];
            IntPtr intPtr = rawData.Scan0;
            Marshal.Copy(intPtr, result, 0, result.Length);
            rgb2gray(result, img.PixelFormat, rawData.Height, rawData.Stride);
            Marshal.Copy(result, 0, intPtr, byteArraySize);
            img.UnlockBits(rawData);
            return img;
        }
        public static byte[] rgbChannelExtract(byte[] rawData, PixelFormat imgPixelFormat, int Height, int StrideWidth,char c)
        {
            int ByteOccupiciedByAPixel = Bitmap.GetPixelFormatSize(imgPixelFormat) / 8;
            for (int i = 0; i < Height; i++)
            {
                int currentRow = i * StrideWidth;
                for (int j = 0; j < StrideWidth; j += ByteOccupiciedByAPixel)
                {
                    int R = rawData[currentRow + j + 0];
                    int G = rawData[currentRow + j + 1];
                    int B = rawData[currentRow + j + 2];
                    switch (c)
                    {
                        case 'b':
                            rawData[currentRow + j + 0] = (byte)R;
                            rawData[currentRow + j + 1] = (byte)R;
                            rawData[currentRow + j + 2] = (byte)R;
                            break;
                        case 'g':
                            rawData[currentRow + j + 0] = (byte)G;
                            rawData[currentRow + j + 1] = (byte)G;
                            rawData[currentRow + j + 2] = (byte)G;
                            break;
                        case 'r':
                            rawData[currentRow + j + 0] = (byte)B;
                            rawData[currentRow + j + 1] = (byte)B;
                            rawData[currentRow + j + 2] = (byte)B;
                            break;
                        default:
                            global::System.Windows.Forms.MessageBox.Show("Improper inputs recieved");
                            break;
                    }
                }
            }
            return rawData;
        }
        public static Bitmap rgbChannelExtract(Bitmap img,char c)
        {
            BitmapData rawData = img.LockBits(new Rectangle(0, 0, img.Width, img.Height), ImageLockMode.ReadWrite, img.PixelFormat);
            int byteArraySize = rawData.Stride * rawData.Height;
            byte[] result = new byte[byteArraySize];
            IntPtr intPtr = rawData.Scan0;
            Marshal.Copy(intPtr, result, 0, result.Length);
            rgbChannelExtract(result, img.PixelFormat, rawData.Height, rawData.Stride, c);
            Marshal.Copy(result, 0, intPtr, byteArraySize);
            img.UnlockBits(rawData);
            return img;
        }
        public static byte[] gray2bw(byte[] rawData, PixelFormat imgPixelFormat, int Height, int StrideWidth, int tresh)
        {
            int ByteOccupiciedByAPixel = Bitmap.GetPixelFormatSize(imgPixelFormat) / 8;
            for (int i = 0; i < Height; i++)
            {
                int currentRow = i * StrideWidth;
                for (int j = 0; j < StrideWidth; j += ByteOccupiciedByAPixel)
                {
                    int R = rawData[currentRow + j + 0];
                    int G = rawData[currentRow + j + 1];
                    int B = rawData[currentRow + j + 2];
                    if ((R + G + B) / 3 < tresh)
                    {
                        rawData[currentRow + j + 0] = 0;
                        rawData[currentRow + j + 1] = 0;
                        rawData[currentRow + j + 2] = 0;
                    }
                    else
                    {
                        rawData[currentRow + j + 0] = 255;
                        rawData[currentRow + j + 1] = 255;
                        rawData[currentRow + j + 2] = 255;
                    }
                }
            }
            return rawData;
        }
        public static Bitmap gray2bw(Bitmap img,int tresh)
        {
            BitmapData rawData = img.LockBits(new Rectangle(0, 0, img.Width, img.Height), ImageLockMode.ReadWrite, img.PixelFormat);
            int byteArraySize = rawData.Stride * rawData.Height;
            byte[] result = new byte[byteArraySize];
            IntPtr intPtr = rawData.Scan0;
            Marshal.Copy(intPtr, result, 0, result.Length);
            int ByteOccupiciedByAPixel = Bitmap.GetPixelFormatSize(img.PixelFormat) / 8;
            gray2bw(result, img.PixelFormat, rawData.Height, rawData.Stride, tresh);
            Marshal.Copy(result, 0, intPtr, byteArraySize);
            img.UnlockBits(rawData);
            return img;
        }
    }
}
