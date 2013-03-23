using System;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using AIA.Intranet.Model;
using AIA.Intranet.Common.Utilities;

namespace AIA.Intranet.Infrastructure.Layouts
{
    public partial class UnreadContentResolver : LayoutsPageBase
    {
        public SPListItem CurrentItem { get { return SPContext.Current.ListItem; } }

        protected void Page_Load(object sender, EventArgs e)
        {
            string relatedContent = CurrentItem[IOfficeColumnId.RelatedContent].ToString();
            string assignTo = CurrentItem[SPBuiltInFieldId.AssignedTo].ToString();
            int id = int.Parse(assignTo.Split(";#".ToCharArray(), StringSplitOptions.RemoveEmptyEntries)[0]);

            SPUser curUser = CCIUtility.GetRealCurrentSpUser(this.Page);

            if (curUser.ID == id)
            {
                SPSecurity.RunWithElevatedPrivileges(delegate()
                {
                    using (var site = new SPSite(SPContext.Current.Site.ID))
                    {
                        using (var web = site.OpenWeb(SPContext.Current.Web.ID))
                        {
                            try
                            {
                                web.AllowUnsafeUpdates = true;
                                var list = web.Lists[SPContext.Current.ListId];
                                var item = list.GetItemById(CurrentItem.ID);
                                item.Delete();
                            }
                            catch (Exception)
                            {


                            }
                            finally
                            {
                                web.AllowUnsafeUpdates = false;
                            }
                        }
                    }
                });
            }

            var strDialog = string.Empty;
            if (IsDialog)
                strDialog = "&IsDlg=1";

            Response.Redirect(relatedContent.Split(',')[0] + strDialog);
        }

        protected bool IsDialog
        {
            get
            {
                if (string.IsNullOrEmpty(Request.QueryString["IsDlg"]))
                    return false;
                try
                {
                    return Convert.ToBoolean(Convert.ToByte(Request.QueryString["IsDlg"].Split(',')[0]));
                }
                catch { return false; }
            }
        }
    }
}
