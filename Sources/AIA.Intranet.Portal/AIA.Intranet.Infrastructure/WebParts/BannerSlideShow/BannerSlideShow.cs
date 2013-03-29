using System;
using System.ComponentModel;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;

namespace AIA.Intranet.Infrastructure.WebParts.BannerSlideShow
{
    [ToolboxItemAttribute(false)]
    public class BannerSlideShow : WebPart
    {
        // Visual Studio might automatically update this path when you change the Visual Web Part project item.
        private const string _ascxPath = @"~/_CONTROLTEMPLATES/AIA.Intranet.Infrastructure.WebParts/BannerSlideShow/BannerSlideShowUserControl.ascx";

        [Category("T2V Group"),
        Personalizable(PersonalizationScope.Shared),
        WebBrowsable(true),
        DefaultValue(""),
        WebDisplayName("Banner Folder"),
        WebDescription("Please Enter a Banner Folder")]
        public string BannerFolder { get; set; }

        protected override void CreateChildControls()
        {
            BannerSlideShowUserControl control = Page.LoadControl(_ascxPath) as BannerSlideShowUserControl;
            control.BannerFolder = this.BannerFolder;
            Controls.Add(control);
        }
    }
}
