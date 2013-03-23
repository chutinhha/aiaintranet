using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;
using Microsoft.SharePoint;

namespace AIA.Intranet.Infrastructure.Controls
{
    public class InsertScript : UserControl
    {
        public string SiteUrl;
        protected override void OnLoad(EventArgs e)
        {
            SiteUrl = SPContext.Current.Site.RootWeb.Url;
        }
    }
}
