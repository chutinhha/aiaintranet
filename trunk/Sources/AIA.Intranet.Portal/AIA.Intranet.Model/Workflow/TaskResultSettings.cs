using System;
using Microsoft.SharePoint;

namespace AIA.Intranet.Model.Workflow
{
    [Serializable]
    public class TaskResultSettings
    {
        public TaskResultSettings(Guid siteId)
        {
            ApprovedText = Constants.Workflow.STATUS_APPROVED_TEXT;
            RejectedText = Constants.Workflow.STATUS_REJECTED_TEXT;
            SignatureVerifiedText = Constants.Workflow.STATUS_SIGNATURE_VERIFIED_TEXT;
            DataQualityCompletedText = Constants.Workflow.STATUS_DATA_QUALITY_COMPLETED_TEXT;

            if (Guid.Empty == siteId)
                return;

            using (SPSite site = new SPSite(siteId))
            {
                try
                {
                    SPList globalList = site.RootWeb.GetList(site.RootWeb.Url + Constants.CONFIG_LIST_URL);
                    if (globalList == null)
                        return;

                    SPQuery query = new SPQuery();
                    query.Query = string.Format(
                        "<Query>" +
                           "<Where>" +
                                "<Eq>" +
                                    "<FieldRef Name=\"Type\" /><Value Type=\"Choice\">Task Result Status</Value>" +
                                "</Eq>" +
                           "</Where>" +
                        "</Query>");
                    SPListItemCollection items = globalList.GetItems(query);
                    
                    foreach (SPListItem item in items)
                    {
                        if (item["Title"] == null || item["Value"] == null ||
                            string.IsNullOrEmpty(item["Title"].ToString().Trim()) ||
                            string.IsNullOrEmpty(item["Value"].ToString().Trim()))
                                continue;

                        string name = item["Title"].ToString().Trim().ToLower();
                        string value = item["Value"].ToString().Trim();
                        
                        if (string.Compare(name, TaskStatusVariableName.APPROVED, true) == 0)
                            ApprovedText = value;

                        if (string.Compare(name, TaskStatusVariableName.REJECTED, true) == 0)
                            RejectedText = value;

                        if (string.Compare(name, TaskStatusVariableName.SIGNATURE_VERIFIED, true) == 0)
                            SignatureVerifiedText = value;

                        if (string.Compare(name, TaskStatusVariableName.DATA_QUALITY_COMPLETED, true) == 0)
                            DataQualityCompletedText = value;
                    }
                }
                catch { }
            }
        }

        public TaskResultSettings()
        {
            ApprovedText = Constants.Workflow.STATUS_APPROVED_TEXT;
            RejectedText = Constants.Workflow.STATUS_REJECTED_TEXT;
            SignatureVerifiedText = Constants.Workflow.STATUS_SIGNATURE_VERIFIED_TEXT;
            DataQualityCompletedText = Constants.Workflow.STATUS_DATA_QUALITY_COMPLETED_TEXT;
        }
        
        public string ApprovedText { get; set; }
        public string RejectedText { get; set; }
        public string SignatureVerifiedText { get; set; }
        public string DataQualityCompletedText { get; set; }
    }
}
