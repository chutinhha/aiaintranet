using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System;
using System.Web;
using Microsoft.SharePoint;
using System.Drawing;
using AIA.Intranet.Common.Utilities;
using System.Drawing.Imaging;

namespace AIA.Intranet.Infrastructure.Layouts
{
    public class ImageViewer : IHttpHandler
    {
        /// <summary>
        /// You will need to configure this handler in the web.config file of your 
        /// web and register it with IIS before being able to use it. For more information
        /// see the following link: http://go.microsoft.com/?linkid=8101007
        /// </summary>
        #region IHttpHandler Members

        public bool IsReusable
        {
            // Return false in case your Managed Handler cannot be reused for another request.
            // Usually this would be false in case you have some state information preserved per request.
            get { return true; }
        }

        public void ProcessRequest(HttpContext context)
        {
            string fileUrl = context.Request["File"];
            string w = context.Request["W"];
            string h= context.Request["H"];

            string targetUrl = string.Empty;
            using (SPSite site = new SPSite(fileUrl))
            using (SPWeb web = site.OpenWeb())
            {
                var file = web.GetFile(fileUrl);
                //var listItem= file.Item;
                //var list = file.Item.ParentList;
                var contentType = "image/jpg";
                context.Response.ContentType = contentType;

                context.Response.Cache.SetCacheability(HttpCacheability.Public);
                context.Response.Cache.SetExpires(DateTime.Now.AddMinutes(10));
                context.Response.Cache.SetMaxAge(new TimeSpan(0, 10, 0));
                context.Response.AddHeader("Last-Modified", file.TimeLastModified.ToLongDateString());

                
                HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.Server);
                HttpContext.Current.Response.Cache.SetValidUntilExpires(true);

                using (Image image = Bitmap.FromStream(file.OpenBinaryStream()))
                {

                    if (string.IsNullOrEmpty(w) && string.IsNullOrEmpty(h))
                    {
                        image.Save(context.Response.OutputStream, ImageFormat.Jpeg);
                    }
                    else
                    {
                        int width = !string.IsNullOrEmpty(w) ? int.Parse(w) : 0;
                        int height = !string.IsNullOrEmpty(h) ? int.Parse(h) : 0;


                        //var output = ImageUtility.ResizeImage(width, height, image);
                        var output = ImageUtility.RewriteImageFix((Bitmap)image, width, height, Color.Black);
                        output.Save(context.Response.OutputStream, ImageFormat.Jpeg);
                        output.Dispose();
                    }
                }
                context.Response.End();
            }
        }

        #endregion
    }
}
