using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using Microsoft.SharePoint.Workflow;

namespace AIA.Intranet.Infrastructure.Layouts
{
    public partial class ApproveAll : LayoutsPageBase
    {
        private List<int> ItemIDs
        {
            get
            {
                string ids = HttpUtility.UrlDecode(Request.QueryString["ids"]).Trim(new[] { ',' });
                return ids.Split(new[] { ',' })
                    .Select(strId => Convert.ToInt32(strId))
                    .ToList();
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            btnSubmit.Enabled = ItemIDs.Count > 0;

        }
        protected void btnOk_Click(object sender, EventArgs e)
        {
            try
            {
                var moderationStatusType = (SPModerationStatusType)Enum.Parse(typeof(SPModerationStatusType), ddlAprovalOptions.SelectedValue);



                var siteId = SPContext.Current.Site.ID;
                var webId = SPContext.Current.Web.ID;
                var listId = Request.QueryString["listId"];


                ApproveRejectItems(SPContext.Current.Web, listId, moderationStatusType, ItemIDs);
                divMessage.InnerText = "Operation completed successfully";
                btnSubmit.Visible = false;
                btnCancel.Value = "Finish";
                btnCancel.Attributes["onclick"] = "finisheDialog()";
            }
            catch (Exception)
            {

                divMessage.InnerText = "Failed to complete operation";
            }
        }

        private void ApproveRejectItems(SPWeb web, string listId, SPModerationStatusType moderationStatusType, List<int> itemIDs)
        {
            web.AllowUnsafeUpdates = true;
            SPList spList = web.Lists[new Guid(listId)];
            foreach (var itemId in itemIDs)
            {
                SPListItem spListItem = spList.GetItemById(itemId);

                //disable workflow
                foreach (SPWorkflow workflow in spListItem.Workflows)
                {
                    if (workflow.ParentAssociation.Id == spList.DefaultContentApprovalWorkflowId)
                    {
                        SPWorkflowManager.CancelWorkflow(workflow);
                    }
                }
                if (spListItem.ModerationInformation != null)
                {
                    //update moderation status
                    spListItem.ModerationInformation.Comment = txtComments.Text;
                    spListItem.ModerationInformation.Status = moderationStatusType;
                    spListItem.Update();
                }
            }
        }

        
    }
}
