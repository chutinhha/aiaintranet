using System;
using System.ComponentModel;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;

namespace AIA.Intranet.Infrastructure.WebParts.ViewNewsCategoryWebPart
{
    [ToolboxItemAttribute(false)]
    public class ViewNewsCategoryWebPart : WebPart
    {
        // Visual Studio might automatically update this path when you change the Visual Web Part project item.
        private const string _ascxPath = @"~/_CONTROLTEMPLATES/AIA.Intranet.Infrastructure.WebParts/ViewNewsCategoryWebPart/ViewNewsCategoryWebPartUserControl.ascx";

        private string _strWebID = string.Empty;
        [WebBrowsable(true),
        Category("Configuration"),
        Personalizable(PersonalizationScope.Shared),
        WebDisplayName("Web ID"),
        WebDescription("")]
        public string WebID
        {
            get { return _strWebID; }
            set { _strWebID = value; }
        }

        private string _strListID = string.Empty;
        [WebBrowsable(true),
        Category("Configuration"),
        Personalizable(PersonalizationScope.Shared),
        WebDisplayName("List ID"),
        WebDescription("")]
        public string ListID
        {
            get { return _strListID; }
            set { _strListID = value; }
        }

        private string _strListUrl = string.Empty;
        [WebBrowsable(true),
        Category("Configuration"),
        Personalizable(PersonalizationScope.Shared),
        WebDisplayName("List URL (use incase of no List ID)"),
        WebDescription("")]
        public string ListUrl
        {
            get { return _strListUrl; }
            set { _strListUrl = value; }
        }

        private string _strItemCount = "5";
        [WebBrowsable(true),
        Category("Configuration"),
        Personalizable(PersonalizationScope.Shared),
        WebDisplayName("Number of displaying items"),
        WebDescription("")]
        public string ItemCount
        {
            get { return _strItemCount; }
            set { _strItemCount = value; }
        }

        private bool _bolShowDateTime = true;
        [WebBrowsable(true),
        Category("Configuration"),
        Personalizable(PersonalizationScope.Shared),
        WebDisplayName("Show datetime ?"),
        WebDescription("")]
        public bool ShowDateTime
        {
            get { return _bolShowDateTime; }
            set { _bolShowDateTime = value; }
        }

        private string _strDateTimeFormat = string.Empty;
        [WebBrowsable(true),
        Category("Configuration"),
        Personalizable(PersonalizationScope.Shared),
        WebDisplayName("DateTime format"),
        WebDescription("")]
        public string DateTimeFormat
        {
            get { return _strDateTimeFormat; }
            set { _strDateTimeFormat = value; }
        }

        private string _strMainPicWidth = "100px";
        [WebBrowsable(true),
        Category("Configuration"),
        Personalizable(PersonalizationScope.Shared),
        WebDisplayName("Main picture's width (default: 100px)"),
        WebDescription("")]
        public string MainPicWidth
        {
            get { return _strMainPicWidth; }
            set { _strMainPicWidth = value; }
        }

        protected override void CreateChildControls()
        {
            ViewNewsCategoryWebPartUserControl control = Page.LoadControl(_ascxPath) as ViewNewsCategoryWebPartUserControl;
            if (control != null)
                control.WebPart = this;
            Controls.Add(control);
        }
    }
}
