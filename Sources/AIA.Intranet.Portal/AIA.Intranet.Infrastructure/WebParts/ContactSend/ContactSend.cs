using System;
using System.ComponentModel;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;

namespace AIA.Intranet.Infrastructure.WebParts.ContactSend
{
    [ToolboxItemAttribute(false)]
    public class ContactSend : WebPart
    {
        // Visual Studio might automatically update this path when you change the Visual Web Part project item.
        private const string _ascxPath = @"~/_CONTROLTEMPLATES/AIA.Intranet.Infrastructure.WebParts/ContactSend/ContactSendUserControl.ascx";


        private string webPartTitle = "Looking for the right person to talk to";
        [WebBrowsable(true),
        Category("AIA Setting"),
        Personalizable(PersonalizationScope.Shared),
        WebDisplayName("Title of webpart"),
        WebDescription("")]
        public string WebPartTitle
        {
            get { return webPartTitle; }
            set { webPartTitle = value; }
        }

        private string webPartDescription = "The information presented herein is intended to comply with the relevant disclosure requirements of the Rules Governing The Listing of Securities on the Stock Exchange of Hong Kong Limited.";
        [WebBrowsable(true),
        Category("AIA Setting"),
        Personalizable(PersonalizationScope.Shared),
        WebDisplayName("Description of webpart"),
        WebDescription("")]
        public string WebPartDescription
        {
            get { return webPartDescription; }
            set { webPartDescription = value; }
        }

        protected override void CreateChildControls()
        {
            ContactSendUserControl control = Page.LoadControl(_ascxPath) as ContactSendUserControl;
            control.WebPartTitle = this.WebPartTitle;
            control.WebPartDescription = this.WebPartDescription;
            Controls.Add(control);
        }
    }
}
