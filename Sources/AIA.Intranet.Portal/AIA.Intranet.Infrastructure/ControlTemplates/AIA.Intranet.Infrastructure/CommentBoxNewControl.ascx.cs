using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Microsoft.SharePoint;
using AIA.Intranet.Model;
using AIA.Intranet.Common.Extensions;

namespace AIA.Intranet.Infrastructure.ControlTemplates.AIA.Intranet.Infrastructure
{
    public partial class CommentBoxNewControl : UserControl
    {
        protected override void OnInit(EventArgs e)
        {
            try
            {
                SPContext.Current.Web.AllowUnsafeUpdates = true;
                var list = SPContext.Current.List;
                var field = list.Fields[IOfficeColumnId.ReplyTo] as SPFieldLookup;
                field.UpdateLookupReferences(list.DefaultViewUrl);

            }
            catch (Exception)
            {

                // throw;
            }
            finally
            {
                SPContext.Current.Web.AllowUnsafeUpdates = false;
            }
           
            base.OnInit(e);
        }
        protected void Page_Load(object sender, EventArgs e)
        {
        }
    }
}
