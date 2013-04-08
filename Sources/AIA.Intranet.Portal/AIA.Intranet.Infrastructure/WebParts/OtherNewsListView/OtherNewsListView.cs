using System;
using System.ComponentModel;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;

namespace AIA.Intranet.Infrastructure.WebParts.OtherNewsListView
{
    [ToolboxItemAttribute(false)]
    public class OtherNewsListView : WebPart
    {
        // Visual Studio might automatically update this path when you change the Visual Web Part project item.
        private const string _ascxPath = @"~/_CONTROLTEMPLATES/AIA.Intranet.Infrastructure.WebParts/OtherNewsListView/OtherNewsListViewUserControl.ascx";

        private string _strItemCount = "5";
        [WebBrowsable(true),
        Category("AIA Setting"),
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
        Category("AIA Setting"),
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
        Category("AIA Setting"),
        Personalizable(PersonalizationScope.Shared),
        WebDisplayName("DateTime format"),
        WebDescription("")]
        public string DateTimeFormat
        {
            get { return _strDateTimeFormat; }
            set { _strDateTimeFormat = value; }
        }

        private string webPartTitle = "Other news";
        [WebBrowsable(true),
        Category("AIA Setting"),
        Personalizable(PersonalizationScope.Shared),
        WebDisplayName("Display name of webpart"),
        WebDescription("")]
        public string WebPartTitle
        {
            get { return webPartTitle; }
            set { webPartTitle = value; }
        }

        protected override void CreateChildControls()
        {
            OtherNewsListViewUserControl control = Page.LoadControl(_ascxPath) as OtherNewsListViewUserControl;
            if (control != null)
                control.WebPart = this;
            Controls.Add(control);
        }
    }
}
