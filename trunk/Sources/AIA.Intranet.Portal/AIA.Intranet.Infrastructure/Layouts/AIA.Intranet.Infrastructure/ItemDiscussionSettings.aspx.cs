using System;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using Microsoft.SharePoint.ApplicationPages;
using System.Web.UI.WebControls;
using Microsoft.SharePoint.Utilities;
using AIA.Intranet.Common.Extensions;
using AIA.Intranet.Resources;
using AIA.Intranet.Model;
using System.Linq;

namespace AIA.Intranet.Infrastructure.Layouts
{
    [CLSCompliant(false)]
    public partial class ItemDiscussionSettings : ListPageBase
    {
        protected InputFormCheckBox enableDocumentDiscussions;
        

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            listSelection.DataBound += new EventHandler(listSelection_DataBound);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Set the value of the enabled checkbox based on the previous value.
                enableDocumentDiscussions.Checked = this.List.DiscussionsEnabledGet();
            }
        }

        protected void listSelection_DataBound(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ListItem previousSelection = listSelection.Items.FindByValue(this.List.DiscussionBoardTitleGet());
                if (previousSelection != null)
                    previousSelection.Selected = true;
            }
        }


        protected void BtnSave_Click(object sender, EventArgs e)
        {
            if (enableDocumentDiscussions.Checked)
            {
                // Set the values that have currently been selected.
                this.List.DiscussionsEnabledSet(true);
                this.List.DiscussionBoardTitleSet(listSelection.SelectedValue);
                // Set ECB menu for this list
                SPUserCustomActionCollection spUserCustomActionCollection = this.List.UserCustomActions;
                var spUserCustomAction = spUserCustomActionCollection.FirstOrDefault(p => p.Title == CommonResources.DiscustionDisplay);
                if (spUserCustomAction == null)
                {
                    spUserCustomAction = spUserCustomActionCollection.Add();
                    spUserCustomAction.Location = "EditControlBlock";
                    spUserCustomAction.Sequence = 100;
                    spUserCustomAction.Title = CommonResources.DiscustionDisplay;
                    spUserCustomAction.Url = "~site/_layouts/AIA.Intranet.Infrastructure/ItemDiscussionResolver.aspx?List={ListId}&Item={ItemId}";
                    spUserCustomAction.Update();
                }
            }
            else
            {
                // Set the values that have currently been selected.
                this.List.DiscussionsEnabledSet(false);
                this.List.DiscussionBoardTitleSet(string.Empty);
                // Set ECB menu for this list
                SPUserCustomActionCollection spUserCustomActionCollection = this.List.UserCustomActions;
                var spUserCustomAction = spUserCustomActionCollection.FirstOrDefault(p => p.Title == CommonResources.DiscustionDisplay);
                if (spUserCustomAction != null)
                {
                    spUserCustomAction.Delete();
                }
            }
            // Redirect now that we are done.
            SPUtility.Redirect(base.ListEditPageLayoutsRelativeUrl, SPRedirectFlags.RelativeToLayoutsPage, this.Context);
        }


        protected void BtnCancel_Click(object sender, EventArgs e)
        {
            SPUtility.Redirect(base.ListEditPageLayoutsRelativeUrl, SPRedirectFlags.RelativeToLayoutsPage, this.Context);
        }
    }    
}
