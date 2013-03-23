using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.IO;

namespace AIA.Intranet.Common.Utilities
{
    public class ImageUtility
	{
		enum Dimensions 
		{
			Width,
			Height
		}
		enum AnchorPosition
		{
			Top,
			Center,
			Bottom,
			Left,
			Right
		}
		[STAThread]
		static void Main(string[] args)
		{
			//set a working directory
			string WorkingDirectory = @"C:\Projects\Tutorials\ImageResize";

			//create a image object containing a verticel photograph
			Image imgPhotoVert = Image.FromFile(WorkingDirectory + @"\imageresize_vert.jpg");
			Image imgPhotoHoriz = Image.FromFile(WorkingDirectory + @"\imageresize_horiz.jpg");
			Image imgPhoto = null;

			imgPhoto = ScaleByPercent(imgPhotoVert, 50);
			imgPhoto.Save(WorkingDirectory + @"\images\imageresize_1.jpg", ImageFormat.Jpeg);
			imgPhoto.Dispose();

			imgPhoto = ConstrainProportions(imgPhotoVert, 200, Dimensions.Width);
			imgPhoto.Save(WorkingDirectory + @"\images\imageresize_2.jpg", ImageFormat.Jpeg);
			imgPhoto.Dispose();

			imgPhoto = FixedSize(imgPhotoVert, 200, 200);
			imgPhoto.Save(WorkingDirectory + @"\images\imageresize_3.jpg", ImageFormat.Jpeg);
			imgPhoto.Dispose();

			imgPhoto = Crop(imgPhotoVert, 200, 200, AnchorPosition.Center);
			imgPhoto.Save(WorkingDirectory + @"\images\imageresize_4.jpg", ImageFormat.Jpeg);
			imgPhoto.Dispose();

			imgPhoto = Crop(imgPhotoHoriz, 200, 200, AnchorPosition.Center);
			imgPhoto.Save(WorkingDirectory + @"\images\imageresize_5.jpg", ImageFormat.Jpeg);
			imgPhoto.Dispose();

		}
		static Image ScaleByPercent(Image imgPhoto, int Percent)
		{
			float nPercent = ((float)Percent/100);

			int sourceWidth = imgPhoto.Width;
			int sourceHeight = imgPhoto.Height;
			int sourceX = 0;
			int sourceY = 0;

			int destX = 0;
			int destY = 0; 
			int destWidth  = (int)(sourceWidth * nPercent);
			int destHeight = (int)(sourceHeight * nPercent);

			Bitmap bmPhoto = new Bitmap(destWidth, destHeight, PixelFormat.Format24bppRgb);
			bmPhoto.SetResolution(imgPhoto.HorizontalResolution, imgPhoto.VerticalResolution);

			Graphics grPhoto = Graphics.FromImage(bmPhoto);
			grPhoto.InterpolationMode = InterpolationMode.HighQualityBicubic;

			grPhoto.DrawImage(imgPhoto, 
				new Rectangle(destX,destY,destWidth,destHeight),
				new Rectangle(sourceX,sourceY,sourceWidth,sourceHeight),
				GraphicsUnit.Pixel);

			grPhoto.Dispose();
			return bmPhoto;
		}
		static Image ConstrainProportions(Image imgPhoto, int Size, Dimensions Dimension)
		{
			int sourceWidth = imgPhoto.Width;
			int sourceHeight = imgPhoto.Height;
			int sourceX = 0;
			int sourceY = 0;
			int destX = 0;
			int destY = 0; 
			float nPercent = 0;

			switch(Dimension)
			{
				case Dimensions.Width:
					nPercent = ((float)Size/(float)sourceWidth);
					break;
				default:
					nPercent = ((float)Size/(float)sourceHeight);
					break;
			}
				
			int destWidth  = (int)(sourceWidth * nPercent);
			int destHeight = (int)(sourceHeight * nPercent);

			Bitmap bmPhoto = new Bitmap(destWidth, destHeight, PixelFormat.Format24bppRgb);
			bmPhoto.SetResolution(imgPhoto.HorizontalResolution, imgPhoto.VerticalResolution);

			Graphics grPhoto = Graphics.FromImage(bmPhoto);
			grPhoto.InterpolationMode = InterpolationMode.HighQualityBicubic;

			grPhoto.DrawImage(imgPhoto, 
			new Rectangle(destX,destY,destWidth,destHeight),
			new Rectangle(sourceX,sourceY,sourceWidth,sourceHeight),
			GraphicsUnit.Pixel);

			grPhoto.Dispose();
			return bmPhoto;
		}

		static Image FixedSize(Image imgPhoto, int Width, int Height)
		{
			int sourceWidth = imgPhoto.Width;
			int sourceHeight = imgPhoto.Height;
			int sourceX = 0;
			int sourceY = 0;
			int destX = 0;
			int destY = 0; 

			float nPercent = 0;
			float nPercentW = 0;
			float nPercentH = 0;

			nPercentW = ((float)Width/(float)sourceWidth);
			nPercentH = ((float)Height/(float)sourceHeight);

			//if we have to pad the height pad both the top and the bottom
			//with the difference between the scaled height and the desired height
			if(nPercentH < nPercentW)
			{
				nPercent = nPercentH;
				destX = (int)((Width - (sourceWidth * nPercent))/2);
			}
			else
			{
				nPercent = nPercentW;
				destY = (int)((Height - (sourceHeight * nPercent))/2);
			}
		
			int destWidth  = (int)(sourceWidth * nPercent);
			int destHeight = (int)(sourceHeight * nPercent);

			Bitmap bmPhoto = new Bitmap(Width, Height, PixelFormat.Format24bppRgb);
			bmPhoto.SetResolution(imgPhoto.HorizontalResolution, imgPhoto.VerticalResolution);

			Graphics grPhoto = Graphics.FromImage(bmPhoto);
			grPhoto.Clear(Color.Red);
			grPhoto.InterpolationMode = InterpolationMode.HighQualityBicubic;

			grPhoto.DrawImage(imgPhoto, 
				new Rectangle(destX,destY,destWidth,destHeight),
				new Rectangle(sourceX,sourceY,sourceWidth,sourceHeight),
				GraphicsUnit.Pixel);

			grPhoto.Dispose();
			return bmPhoto;
		}
		static Image Crop(Image imgPhoto, int Width, int Height, AnchorPosition Anchor)
		{
			int sourceWidth = imgPhoto.Width;
			int sourceHeight = imgPhoto.Height;
			int sourceX = 0;
			int sourceY = 0;
			int destX = 0;
			int destY = 0;

			float nPercent = 0;
			float nPercentW = 0;
			float nPercentH = 0;

			nPercentW = ((float)Width/(float)sourceWidth);
			nPercentH = ((float)Height/(float)sourceHeight);

			if(nPercentH < nPercentW)
			{
				nPercent = nPercentW;
				switch(Anchor)
				{
					case AnchorPosition.Top:
						destY = 0;
						break;
					case AnchorPosition.Bottom:
						destY = (int)(Height - (sourceHeight * nPercent));
						break;
					default:
						destY = (int)((Height - (sourceHeight * nPercent))/2);
						break;
				}				
			}
			else
			{
				nPercent = nPercentH;
				switch(Anchor)
				{
					case AnchorPosition.Left:
						destX = 0;
						break;
					case AnchorPosition.Right:
						destX = (int)(Width - (sourceWidth * nPercent));
						break;
					default:
						destX = (int)((Width - (sourceWidth * nPercent))/2);
						break;
				}			
			}

			int destWidth  = (int)(sourceWidth * nPercent);
			int destHeight = (int)(sourceHeight * nPercent);

			Bitmap bmPhoto = new Bitmap(Width, Height, PixelFormat.Format24bppRgb);
			bmPhoto.SetResolution(imgPhoto.HorizontalResolution, imgPhoto.VerticalResolution);

			Graphics grPhoto = Graphics.FromImage(bmPhoto);
			grPhoto.InterpolationMode = InterpolationMode.HighQualityBicubic;

			grPhoto.DrawImage(imgPhoto, 
				new Rectangle(destX,destY,destWidth,destHeight),
				new Rectangle(sourceX,sourceY,sourceWidth,sourceHeight),
				GraphicsUnit.Pixel);

			grPhoto.Dispose();
			return bmPhoto;
		}


        //Overload for crop that default starts top left of the image.
        public static System.Drawing.Image CropImage(System.Drawing.Image Image, int Height, int Width)
        {
            return CropImage(Image, Height, Width, 0, 0);
        }

        //The crop image sub
        public static System.Drawing.Image CropImage(System.Drawing.Image Image, int Height, int Width, int StartAtX, int StartAtY)
        {
            Image outimage;
            MemoryStream mm = null;
            try
            {
                //check the image height against our desired image height
                if (Image.Height < Height)
                {
                    Height = Image.Height;
                }

                if (Image.Width < Width)
                {
                    Width = Image.Width;
                }

                //create a bitmap window for cropping
                Bitmap bmPhoto = new Bitmap(Width, Height, PixelFormat.Format24bppRgb);
                bmPhoto.SetResolution(72, 72);

                //create a new graphics object from our image and set properties
                Graphics grPhoto = Graphics.FromImage(bmPhoto);
                grPhoto.SmoothingMode = SmoothingMode.AntiAlias;
                grPhoto.InterpolationMode = InterpolationMode.HighQualityBicubic;
                grPhoto.PixelOffsetMode = PixelOffsetMode.HighQuality;

                //now do the crop
                grPhoto.DrawImage(Image, new Rectangle(0, 0, Width, Height), StartAtX, StartAtY, Width, Height, GraphicsUnit.Pixel);

                // Save out to memory and get an image from it to send back out the method.
                mm = new MemoryStream();
                bmPhoto.Save(mm, System.Drawing.Imaging.ImageFormat.Jpeg);
                Image.Dispose();
                bmPhoto.Dispose();
                grPhoto.Dispose();
                outimage = Image.FromStream(mm);

                return outimage;
            }
            catch (Exception ex)
            {
                throw new Exception("Error cropping image, the error was: " + ex.Message);
            }
        }

        //Hard resize attempts to resize as close as it can to the desired size and then crops the excess
        public static System.Drawing.Image HardResizeImage(int Width, int Height, System.Drawing.Image Image)
        {
            int width = Image.Width;
            int height = Image.Height;
            Image resized = null;
            if (Width > Height)
            {
                resized = ResizeImage(Width, Width, Image);
            }
            else
            {
                resized = ResizeImage(Height, Height, Image);
            }
            Image output = CropImage(resized, Height, Width);
            //return the original resized image
            return output;
        }

        //Image resizing
        public static System.Drawing.Image ResizeImage(int maxWidth, int maxHeight, System.Drawing.Image Image)
        {
            int width = Image.Width;
            int height = Image.Height;
            if (maxWidth == 0)
            {
                maxWidth = (int)(((float)maxHeight / height) * width);
            }
            if (maxHeight == 0)
            {
                maxHeight = (int)(((float)maxWidth / width) * height);
            }

            if (width > maxWidth || height > maxHeight)
            {
                //The flips are in here to prevent any embedded image thumbnails -- usually from cameras
                //from displaying as the thumbnail image later, in other words, we want a clean
                //resize, not a grainy one.
                Image.RotateFlip(System.Drawing.RotateFlipType.Rotate180FlipX);
                Image.RotateFlip(System.Drawing.RotateFlipType.Rotate180FlipX);

                float ratio = 0;
                if (width > height)
                {
                    ratio = (float)width / (float)height;
                    width = maxWidth;
                    height = Convert.ToInt32(Math.Round((float)width / ratio));
                }
                else
                {
                    ratio = (float)height / (float)width;
                    height = maxHeight;
                    width = Convert.ToInt32(Math.Round((float)height / ratio));
                }

                //return the resized image
                return Image.GetThumbnailImage(width, height, null, IntPtr.Zero);
            }
            //return the original resized image
            return Image;
        }



        public static Bitmap RewriteImageFix(Bitmap bitmap, int width, int height, Color bgColor)
        {
            try
            {
                #region Caculate thumb width, height
                int widthTh, heightTh;
                float widthOrig, heightOrig;
                float fx, fy, f;
                float beginX = 0, beginY = 0;

                // retain aspect ratio
                widthOrig = bitmap.Width;
                heightOrig = bitmap.Height;
                //
                // subsample factors
                //
                fx = widthOrig / width;
                fy = heightOrig / height;
                // must fit in thumbnail size
                f = Math.Max(fx, fy);
                if (f < 1) f = 1;
                // Recalculate thumbnail size
                widthTh = (int)(widthOrig / f);
                heightTh = (int)(heightOrig / f);

                beginX = (width - widthTh) / 2;
                beginY = (height - heightTh) / 2;
                #endregion

                Bitmap bmpSave = new Bitmap(width, height);

                Graphics graph;
                graph = Graphics.FromImage(bmpSave);

                graph.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graph.Clear(bgColor);
                graph.DrawImage(bitmap, beginX, beginY, widthTh, heightTh);
                /*
                string contentType = "image/jpeg";
                ImageCodecInfo codec = GetEncoderInfo(contentType);
                if (codec == null)
                    codec = FindEncoder(ImageFormat.Jpeg);
                EncoderParameters eps = new EncoderParameters();
                eps.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, 80L);
                bmpSave.Save(filename, codec, eps);
                bmpSave.Dispose();
                 */
                ImageCodecInfo[] info = ImageCodecInfo.GetImageEncoders();
                var eps = new EncoderParameters(1);
                eps.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, 90L);
                //bmpSave.Save(filename, info[1], eps);

                //Finish
                graph.Dispose();
                eps.Dispose();
                return bmpSave;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        /// <summary>
        /// Gets the encoder information for the specified mimetype.  Used in imagescaling
        /// </summary>
        /// <param name="mimeType">The mimetype of the picture.</param>
        /// <returns>System.Drawing.Imaging.ImageCodecInfo</returns>
        public static ImageCodecInfo GetEncoderInfo(string mimeType)
        {
            ImageCodecInfo[] myEncoders = ImageCodecInfo.GetImageEncoders();
            foreach (ImageCodecInfo myEncoder in myEncoders)
                if (myEncoder.MimeType == mimeType)
                    return myEncoder;

            return null;
        }

        internal static ImageCodecInfo FindEncoder(ImageFormat format)
        {
            ImageCodecInfo[] infoArray1 = ImageCodecInfo.GetImageEncoders();
            ImageCodecInfo[] infoArray2 = infoArray1;
            for (int num1 = 0; num1 < infoArray2.Length; num1++)
            {
                ImageCodecInfo info1 = infoArray2[num1];
                if (info1.FormatID.Equals(format.Guid))
                    return info1;
            }
            return null;
        }
	}
}
