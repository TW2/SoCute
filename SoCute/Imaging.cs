using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SoCute
{
    public class Imaging
    {
        public static Bitmap ReDraw(Bitmap bmp)
        {
            //On récupère les deux couleurs (plus utilisée et moins utilisée)
            //1e passe - On définit la plus utilisée en BLANC
            //2e passe - On définit la moins utilisée en NOIR
            Color c_1 = SearchMostUsedColors(bmp);
            ColorSubstitutionFilter csfilter = new ColorSubstitutionFilter();
            csfilter.SourceColor = c_1;
            csfilter.NewColor = Color.White;
            Bitmap first_pass = ColorSubstitutionFilter.ColorSubstitution(bmp, csfilter);

            Bitmap second_pass = BlackAndWhite(first_pass);

            return second_pass;
        }

        //Partially @https://www.codeproject.com/Questions/677506/Csharp-find-the-majority-color-of-an-image
        private static Color SearchMostUsedColors(Bitmap bmp)
        {
            List<Color> TenMostUsedColors = new List<Color>(); ;
            List<int> TenMostUsedColorIncidences = new List<int>();

            Color MostUsedColor = Color.Empty;
            //int MostUsedColorIncidence = 0;

            int pixelColor;

            Dictionary<int, int> dctColorIncidence;

            dctColorIncidence = new Dictionary<int, int>();

            // this is what you want to speed up with unmanaged code
            for (int row = 0; row < bmp.Size.Width; row++)
            {
                for (int col = 0; col < bmp.Size.Height; col++)
                {
                    pixelColor = bmp.GetPixel(row, col).ToArgb();

                    if (dctColorIncidence.Keys.Contains(pixelColor))
                    {
                        dctColorIncidence[pixelColor]++;
                    }
                    else
                    {
                        dctColorIncidence.Add(pixelColor, 1);
                    }
                }
            }

            // note that there are those who argue that a
            // .NET Generic Dictionary is never guaranteed
            // to be sorted by methods like this
            var dctSortedByValueHighToLow = dctColorIncidence.OrderByDescending(x => x.Value).ToDictionary(x => x.Key, x => x.Value);

            // this should be replaced with some elegant Linq ?
            foreach (KeyValuePair<int, int> kvp in dctSortedByValueHighToLow.Take(10))
            {
                TenMostUsedColors.Add(Color.FromArgb(kvp.Key));
                TenMostUsedColorIncidences.Add(kvp.Value);
            }

            MostUsedColor = Color.FromArgb(dctSortedByValueHighToLow.First().Key);
            //MostUsedColorIncidence = dctSortedByValueHighToLow.First().Value;

            return MostUsedColor;
        }

        private static Bitmap BlackAndWhite(Bitmap bmp)
        {
            int rgb;

            BitmapData data = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
            int stride = data.Stride;
            unsafe
            {
                byte* ptr = (byte*)data.Scan0;

                // Go through the draw area and set the pixels as they should be
                for (int y = 0; y < bmp.Height; y++)
                {
                    for (int x = 0; x < bmp.Width; x++)
                    {
                        //ptr[(x * 3) + y * stride] >> BLUE
                        //ptr[(x * 3) + y * stride + 1] >> GREEN
                        //ptr[(x * 3) + y * stride + 2] >> RED

                        rgb = ptr[(x * 3) + y * stride + 2] * ptr[(x * 3) + y * stride + 1] * ptr[(x * 3) + y * stride];

                        if(rgb > (Color.WhiteSmoke.R * Color.WhiteSmoke.G * Color.WhiteSmoke.B))
                        {
                            //Put in WHITE
                            ptr[(x * 3) + y * stride] = Color.White.B;
                            ptr[(x * 3) + y * stride + 1] = Color.White.G;
                            ptr[(x * 3) + y * stride + 2] = Color.White.R;
                        }
                        else
                        {
                            //Put in BLACK
                            ptr[(x * 3) + y * stride] = Color.Black.B;
                            ptr[(x * 3) + y * stride + 1] = Color.Black.G;
                            ptr[(x * 3) + y * stride + 2] = Color.Black.R;                            
                        }
                    }
                }
            }

            bmp.UnlockBits(data);

            return bmp;
        }
    }

    //@https://softwarebydefault.com/2013/03/16/bitmap-color-substitution/
    //UNSAFE
    public class ColorSubstitutionFilter
    {
        public ColorSubstitutionFilter()
        {

        }

        private int thresholdValue = 10;
        public int ThresholdValue
        {
            get { return thresholdValue; }
            set { thresholdValue = value; }
        }

        private Color sourceColor = Color.White;
        public Color SourceColor
        {
            get { return sourceColor; }
            set { sourceColor = value; }
        }

        private Color newColor = Color.White;
        public Color NewColor
        {
            get { return newColor; }
            set { newColor = value; }
        }

        public static Bitmap ColorSubstitution(Bitmap sourceBitmap, ColorSubstitutionFilter filterData)
        {
            //Etape 1 - Formate en 32 Bpp Argb
            Bitmap copyBitmap = new Bitmap(sourceBitmap.Width, sourceBitmap.Height, PixelFormat.Format32bppArgb);

            using (Graphics graphicsObject = Graphics.FromImage(copyBitmap))
            {
                graphicsObject.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                graphicsObject.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                graphicsObject.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
                graphicsObject.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;


                graphicsObject.DrawImage(sourceBitmap, new Rectangle(0, 0, sourceBitmap.Width, sourceBitmap.Height),
                 new Rectangle(0, 0, sourceBitmap.Width, sourceBitmap.Height), GraphicsUnit.Pixel);
            }


            //Etape 2 - Replacement de la couleur
            Bitmap resultBitmap = new Bitmap(copyBitmap.Width, copyBitmap.Height, PixelFormat.Format32bppArgb);


            BitmapData sourceData = copyBitmap.LockBits(new Rectangle(0, 0, copyBitmap.Width, copyBitmap.Height),
                                                          ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
            BitmapData resultData = resultBitmap.LockBits(new Rectangle(0, 0, resultBitmap.Width, resultBitmap.Height),
                                                          ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);


            byte[] resultBuffer = new byte[resultData.Stride * resultData.Height];
            Marshal.Copy(sourceData.Scan0, resultBuffer, 0, resultBuffer.Length);


            copyBitmap.UnlockBits(sourceData);


            byte sourceRed = 0, sourceGreen = 0, sourceBlue = 0, sourceAlpha = 0;
            int resultRed = 0, resultGreen = 0, resultBlue = 0;


            byte newRedValue = filterData.NewColor.R;
            byte newGreenValue = filterData.NewColor.G;
            byte newBlueValue = filterData.NewColor.B;


            byte redFilter = filterData.SourceColor.R;
            byte greenFilter = filterData.SourceColor.G;
            byte blueFilter = filterData.SourceColor.B;


            byte minValue = 0;
            byte maxValue = 255;


            for (int k = 0; k < resultBuffer.Length; k += 4)
            {
                sourceAlpha = resultBuffer[k + 3];


                if (sourceAlpha != 0)
                {
                    sourceBlue = resultBuffer[k];
                    sourceGreen = resultBuffer[k + 1];
                    sourceRed = resultBuffer[k + 2];


                    if ((sourceBlue < blueFilter + filterData.ThresholdValue &&
                            sourceBlue > blueFilter - filterData.ThresholdValue) &&


                        (sourceGreen < greenFilter + filterData.ThresholdValue &&
                            sourceGreen > greenFilter - filterData.ThresholdValue) &&


                        (sourceRed < redFilter + filterData.ThresholdValue &&
                            sourceRed > redFilter - filterData.ThresholdValue))
                    {
                        resultBlue = blueFilter - sourceBlue + newBlueValue;


                        if (resultBlue > maxValue)
                        { resultBlue = maxValue; }
                        else if (resultBlue < minValue)
                        { resultBlue = minValue; }


                        resultGreen = greenFilter - sourceGreen + newGreenValue;


                        if (resultGreen > maxValue)
                        { resultGreen = maxValue; }
                        else if (resultGreen < minValue)
                        { resultGreen = minValue; }


                        resultRed = redFilter - sourceRed + newRedValue;


                        if (resultRed > maxValue)
                        { resultRed = maxValue; }
                        else if (resultRed < minValue)
                        { resultRed = minValue; }


                        resultBuffer[k] = (byte)resultBlue;
                        resultBuffer[k + 1] = (byte)resultGreen;
                        resultBuffer[k + 2] = (byte)resultRed;
                        resultBuffer[k + 3] = sourceAlpha;
                    }
                }
            }


            Marshal.Copy(resultBuffer, 0, resultData.Scan0, resultBuffer.Length);
            resultBitmap.UnlockBits(resultData);


            return resultBitmap;
        }

    }

}
