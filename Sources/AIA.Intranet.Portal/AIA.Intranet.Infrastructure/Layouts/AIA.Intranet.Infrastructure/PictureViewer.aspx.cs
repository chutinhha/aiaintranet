using System;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;

namespace AIA.Intranet.Infrastructure.Layouts
{
    public partial class PictureViewer : LayoutsPageBase
    {
        public SPList CurrentList { get { return SPContext.Current.List; } }
        public SPListItem CurrentItem { get { return SPContext.Current.ListItem; } }
        public string DisplayName
        {
            get
            {
                return string.IsNullOrEmpty(CurrentItem.Title) ? CurrentItem["Name"].ToString() : CurrentItem.Title;
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
        }
    }
}
