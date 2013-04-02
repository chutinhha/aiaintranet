using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Microsoft.SharePoint;

namespace AIA.Intranet.Infrastructure.WebParts.SiteMap
{
    public partial class SiteMapUserControl : UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            lnkHome.HRef = SPContext.Current.Site.RootWeb.Url;
        }
    }
}
