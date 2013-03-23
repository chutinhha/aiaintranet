using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Microsoft.SharePoint;
using AIA.Intranet.Common.Extensions;
using AIA.Intranet.Model;
using AIA.Intranet.Resources;

namespace AIA.Intranet.Infrastructure.ControlTemplates.AIA.Intranet.Infrastructure
{
    public partial class ILinkForm : UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (!SPContext.Current.Web.CurrentUser.IsSiteAdmin)
                {
                    SPUser currentUser = SPContext.Current.Web.CurrentUser;
                    SPGroup group = SPContext.Current.Web.Groups[CommonResources.PortalAdminGroup];
                    bool isAdmin = currentUser.InGroup(group);
                    if (!isAdmin)
                    {
                        ffdType.ControlMode = Microsoft.SharePoint.WebControls.SPControlMode.Display;
                    }   
                }
            }
        }
    }
}
