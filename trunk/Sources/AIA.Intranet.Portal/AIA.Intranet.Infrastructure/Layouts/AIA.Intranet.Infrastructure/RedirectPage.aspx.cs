using System;
using System.Web;
using AIA.Intranet.Common.Extensions;
using AIA.Intranet.Model;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Utilities;
using Microsoft.SharePoint.WebControls;

namespace AIA.Intranet.Infrastructure.Layouts
{
    public partial class RedirectPage : LayoutsPageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(Request.QueryString["Type"]))
            {
                switch (Request.QueryString["Type"])
                {
                    case "ProjectDocuments":
                        Redirect(Constants.PROJECT_CUSTOM_PROPERTIE_DOCUMENT_ID);
                        break;
                    default:
                        Redirect(Constants.PROJECT_CUSTOM_PROPERTIE_DERIVED_EXPENSES_ID);
                        break;
                }
            }
        }

        private void Redirect(string property)
        {
            if (!string.IsNullOrEmpty(SPContext.Current.ListItem.GetCustomProperty(property)))
            {
                Guid listId = new Guid(SPContext.Current.ListItem.GetCustomProperty(property));
                var list = SPContext.Current.Web.Lists[listId];
                SPUtility.Redirect(list.DefaultViewUrl, SPRedirectFlags.Default, HttpContext.Current);
            }
        }
    }
}
