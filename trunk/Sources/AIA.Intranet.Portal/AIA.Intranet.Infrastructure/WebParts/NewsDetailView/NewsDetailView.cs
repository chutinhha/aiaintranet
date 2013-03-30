using System;
using System.ComponentModel;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;

namespace AIA.Intranet.Infrastructure.WebParts.NewsDetailView
{
    [ToolboxItemAttribute(false)]
    public class NewsDetailView : WebPart
    {
        // Visual Studio might automatically update this path when you change the Visual Web Part project item.
        private const string _ascxPath = @"~/_CONTROLTEMPLATES/AIA.Intranet.Infrastructure.WebParts/NewsDetailView/NewsDetailViewUserControl.ascx";

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

        protected override void CreateChildControls()
        {
            NewsDetailViewUserControl control = Page.LoadControl(_ascxPath) as NewsDetailViewUserControl;
            if (control != null)
                control.WebPart = this;
            Controls.Add(control);
        }
    }
}
