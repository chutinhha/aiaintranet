using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Utilities;
using System.IO;
using System.Data;

namespace AIA.Intranet.Model.Entities
{
    public class PictureItem : BaseEntity
    {
        [Field(FieldName = "Description")]
        public string Comments { get; set; }

        public bool PictureFlag { get; set; }
        public string Keywords { get; set; }
        [Field(FieldName="ImageCreateDate")]
        public DateTime DateTaken { get; set; }

        [Field(Ignore =true)]
        public string ThumbnailUrl { get; set; }

        [Field(Ignore = true)]
        public string LargeUrl { get; set; }

        [Field(Ignore = true)]
        public string OriginalUrl { get; set; }

        public string FileRef { get; set; }
        public string EncodedAbsUrl { get; set; }

        public PictureItem(DataRow item)
            : base(item)
        {
            try
            {
                OriginalUrl = item["EncodedAbsUrl"] + item["FileRef"].ToString().Substring(item["FileRef"].ToString().IndexOf("#") + 1);
                ThumbnailUrl = OriginalUrl;
                string filename = Path.GetFileName(OriginalUrl);
                string libraryFolder = OriginalUrl.Replace(filename, string.Empty);

                ThumbnailUrl = libraryFolder+"/_t/" + filename.Replace(".","_") + Path.GetExtension(filename);
                
            }
            catch (Exception ex)
            {

            }
            //string relativeUrl =  item["FileRef"].ToString().Substring(item["FileRef"].ToString().IndexOf("#") + 1);
        //string fullUrl = site.MakeFullUrl(relativeUrl);
        }

        public PictureItem(SPListItem item)
            : base(item)
        {
            try
            {
                ThumbnailUrl = GetPictureUrl(item, ImageSize.Thumbnail);
                LargeUrl = GetPictureUrl(item, ImageSize.Large);
                OriginalUrl = GetPictureUrl(item, ImageSize.Full);
            }
            catch (Exception)
            {
                
                
            }
           
        }

        private string GetPictureUrl( SPListItem listItem, ImageSize imageSize)
        {
            StringBuilder url = new StringBuilder();
            // Build the url up until the final portion
            url.Append(SPEncode.UrlEncodeAsUrl(listItem.Web.Url));
            url.Append('/');
            url.Append(SPEncode.UrlEncodeAsUrl(listItem.ParentList.RootFolder.Url));
            url.Append('/');

            // Determine the final portion based on the requested image size
            string filename = listItem.File.Name;
            if (imageSize == ImageSize.Full)
            {
                url.Append(SPEncode.UrlEncodeAsUrl(filename));
            }
            else
            {
                string basefilename = Path.GetFileNameWithoutExtension(filename);
                string extension = Path.GetExtension(filename);
                string dir = (imageSize == ImageSize.Thumbnail) ? "_t/" : "_w/";
                url.Append(dir);
                url.Append(SPEncode.UrlEncodeAsUrl(basefilename));
                url.Append(SPEncode.UrlEncodeAsUrl(extension).Replace('.', '_'));
                url.Append(".jpg");
            }
            return url.ToString();
        }

    }
}
