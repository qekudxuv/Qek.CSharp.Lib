using System.Drawing;
using System.IO;

namespace Qek.IO
{
    public class ImageHandler
    {
        /// <summary>
        /// 比較兩張圖片是否相同
        /// </summary>
        /// <param name="graphicFilePath1">The graphic file path1.</param>
        /// <param name="graphicFilePath2">The graphic file path2.</param>
        /// <param name="pixelAccuracy">pixel Accuracy. default(最小 & 最精確) is 1</param>
        /// <returns>回傳兩張圖片是否相同</returns>
        public static bool Compare(string imgFilePath1, string imgFilePath2, int pixelAccuracy = 1)
        {
            bool flag = true;
            Bitmap img1 = null;
            Bitmap img2 = null;
            try
            {
                img1 = new Bitmap(imgFilePath1);
                img2 = new Bitmap(imgFilePath2);
                flag = Compare(img1, img2, pixelAccuracy);
            }
            finally
            {
                if (img1 != null) img1.Dispose();
                if (img2 != null) img2.Dispose();
            }

            return flag;
        }

        /// <summary>
        /// 比較兩張圖片是否相同
        /// </summary>
        /// <param name="buffer1">The buffer1.</param>
        /// <param name="buffer2">The buffer2.</param>
        /// <param name="pixelAccuracy">pixel Accuracy. default(最小 & 最精確) is 1</param>
        /// <returns>回傳兩張圖片是否相同</returns>
        public static bool Compare(byte[] imgBuffer1, byte[] imgBuffer2, int pixelAccuracy = 1)
        {
            bool flag = true;
            Bitmap img1 = FileConverter.BufferToImage(imgBuffer1);
            Bitmap img2 = FileConverter.BufferToImage(imgBuffer2);

            flag = Compare(img1, img2, pixelAccuracy);
            return flag;
        }

        /// <summary>
        /// 比較兩張圖片是否相同
        /// </summary>
        /// <param name="img1">The img1.</param>
        /// <param name="img2">The img2.</param>
        /// <param name="pixelAccuracy">pixel Accuracy. default(最小 & 最精確) is 1</param>
        /// <returns>回傳兩張圖片是否相同</returns>
        public static bool Compare(Bitmap img1, Bitmap img2, int pixelAccuracy = 1)
        {
            bool flag = true;
            string img1_ref, img2_ref;

            if (img1.Width == img2.Width && img1.Height == img2.Height)
            {
                for (int i = 0; i < img1.Width; i = i + pixelAccuracy)
                {
                    for (int j = 0; j < img1.Height; j = j + pixelAccuracy)
                    {
                        img1_ref = img1.GetPixel(i, j).ToString();
                        img2_ref = img2.GetPixel(i, j).ToString();
                        if (img1_ref != img2_ref)
                        {
                            flag = false;
                            break;
                        }
                    }
                }
            }
            else
            {
                flag = false;
            }

            return flag;
        }

        /// <summary>
        /// Resizes the image.
        /// </summary>
        /// <param name="passedImage">The passed image.</param>
        /// <param name="largestSide">The largest side.</param>
        /// <returns></returns>
        public static byte[] ResizeImage(byte[] passedImage, int largestSide)
        {
            byte[] newImage;

            using (MemoryStream startMemoryStream = new MemoryStream(),
                                newMemoryStream = new MemoryStream())
            {
                // write the string to the stream  
                startMemoryStream.Write(passedImage, 0, passedImage.Length);

                // create the start Bitmap from the MemoryStream that contains the image  
                Bitmap startBitmap = new Bitmap(startMemoryStream);

                // set thumbnail height and width proportional to the original image.  
                int newHeight;
                int newWidth;
                double HW_ratio;
                if (startBitmap.Height > startBitmap.Width)
                {
                    newHeight = largestSide;
                    HW_ratio = (double)((double)largestSide / (double)startBitmap.Height);
                    newWidth = (int)(HW_ratio * (double)startBitmap.Width);
                }
                else
                {
                    newWidth = largestSide;
                    HW_ratio = (double)((double)largestSide / (double)startBitmap.Width);
                    newHeight = (int)(HW_ratio * (double)startBitmap.Height);
                }

                // create a new Bitmap with dimensions for the thumbnail.  
                //Bitmap newBitmap = new Bitmap(newWidth, newHeight);

                // Copy the image from the START Bitmap into the NEW Bitmap.  
                // This will create a thumnail size of the same image.  
                Bitmap newBitmap = ResizeImage(startBitmap, newWidth, newHeight);

                // Save this image to the specified stream in the specified format.  
                newBitmap.Save(newMemoryStream, startBitmap.RawFormat);//System.Drawing.Imaging.ImageFormat.Jpeg

                //newBitmap.Save("D:\\ABC");

                // Fill the byte[] for the thumbnail from the new MemoryStream.  
                newImage = newMemoryStream.ToArray();
            }

            // return the resized image as a string of bytes.  
            return newImage;
        }

        /// <summary>
        /// Resizes the image.
        /// </summary>
        /// <param name="image">The image.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <returns></returns>
        public static Bitmap ResizeImage(Bitmap image, int width, int height)
        {
            Bitmap resizedImage = new Bitmap(width, height);
            using (Graphics g = Graphics.FromImage(resizedImage))
            {
                g.DrawImage(image, new Rectangle(0, 0, width, height),
                    new Rectangle(0, 0, image.Width, image.Height), GraphicsUnit.Pixel);

            }
            return resizedImage;
        }
    }
}
