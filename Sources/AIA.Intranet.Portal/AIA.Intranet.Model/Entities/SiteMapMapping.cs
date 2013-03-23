using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SharePoint;

namespace AIA.Intranet.Model.Entities
{
    public class SiteMapMapping : BaseEntity
    {
        [Field(Ignore = true)]
        public List<string> Urls { get; set; }

        public string MappingURL { get; set; }

        public SiteMapMapping(SPListItem item)
            : base(item)
        {
            try
            {
                Urls = MappingURL.Split("\r\n".ToCharArray(), StringSplitOptions.RemoveEmptyEntries).ToList();
            }
            catch (Exception)
            {


            }
        }
        public SiteMapMapping()
        {
            Urls = new List<string>();
        }

    }
}
