using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SharePoint;
using System.Data;

namespace AIA.Intranet.Model.Entities
{
    public class MediaItem: BaseEntity
    {
        public MediaItem(SPListItem item)
            : base(item)
        {
            try
            {

            }
            catch (Exception)
            {
                
            }
        }

        public MediaItem(DataRow item)
            : base(item)
        {
            
        }


    }
}
