using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace SNPImageProcessor
{
    class IPDetermine
    {
        public static int[] GetHistogram(byte[] rawData, PixelFormat imgPixelFormat, int Height, int StrideWidth)
        {

            int[] hist = new int[256];
            for (int i = 0; i < 256; i++)
            {
                hist[i] = 0;
            }
            int ByteOccupiciedByAPixel = Bitmap.GetPixelFormatSize(imgPixelFormat) / 8;
            for (int i = 0; i < Height; i++)
            {
                int currentRow = i * StrideWidth;
                for (int j = 0; j < StrideWidth; j += ByteOccupiciedByAPixel)
                {
                    int R = rawData[currentRow + j + 0];
                    int G = rawData[currentRow + j + 1];
                    int B = rawData[currentRow + j + 2];
                    hist[(B + G + R) / 3]++;
                }
            }
            return hist;
        }
        public static byte[] Getpixels(byte[] rawData, PixelFormat imgPixelFormat, int Height, int StrideWidth, Color c)
        {
            int r = c.R;
            int g = c.G;
            int b = c.B;
            int ByteOccupiciedByAPixel = Bitmap.GetPixelFormatSize(imgPixelFormat) / 8;
            for (int i = 0; i < Height; i++)
            {
                int currentRow = i * StrideWidth;
                for (int j = 0; j < StrideWidth; j += ByteOccupiciedByAPixel)
                {
                    int B = rawData[currentRow + j + 0];
                    int G = rawData[currentRow + j + 1];
                    int R = rawData[currentRow + j + 2];
                    if (!((r + 50 >= R && r - 50 <= R) && (g + 50 >= G && g - 50 <= G) && (b + 50 >= B && b - 50 <= B)))
                    {
                        rawData[currentRow + j + 0] = 0;
                        rawData[currentRow + j + 1] = 0;
                        rawData[currentRow + j + 2] = 0;
                    }
                }
            }
            return rawData;
        }
    }
}
