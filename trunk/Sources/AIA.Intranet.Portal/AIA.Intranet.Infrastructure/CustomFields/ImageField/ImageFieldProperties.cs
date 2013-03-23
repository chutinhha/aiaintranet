using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AIA.Intranet.Infrastructure.CustomFields
{
    /// <summary>
    /// Used to store image field property
    /// </summary>
    public class ImageFieldProperties
    {
        public string WebId { get; set; }
        public string PictureLibraryId { get; set; }
        public string FormatName { get; set; }
        public bool Overwrite { get; set; }
        public int DefaultPictureId { get; set; }
        public ImageFieldProperties()
        {
            FormatName = "[[Date]][Name]";
            DefaultPictureId = -1;
        }
    }
}
