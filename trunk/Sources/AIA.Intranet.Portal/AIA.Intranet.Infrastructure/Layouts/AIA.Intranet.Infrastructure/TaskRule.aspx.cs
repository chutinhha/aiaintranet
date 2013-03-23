using System;
using System.Linq;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using AIA.Intranet.Model.Workflow;
using AIA.Intranet.Infrastructure.Pages;
using AIA.Intranet.Model;
using AIA.Intranet.Common.Extensions;
using AIA.Intranet.Infrastructure.Controls;
using AIA.Intranet.Model.Search;
using System.Collections.Generic;

namespace AIA.Intranet.Infrastructure.Layouts
{
    public partial class TaskRule : TaskRulePage
    {
        protected string FormTitle;
        protected override void CreateChildControls()
        {
            ruleEditor.Fields = CurrentContentType.Fields.Cast<SPField>().Where(p=>p.Id != SPBuiltInFieldId.Predecessors).ToList();
            ruleEditor.FormReadOnly = FormReadOnly;
            ruleEditor.CriteriaList = (List<Criteria>)Session[SessionKey];

            TaskRuleSettings settings = null;
            if (CurrentListItem != null && Session[SessionKey] == null)
                settings = SPContext.Current.ListItem.GetCustomSettings<TaskRuleSettings>(IOfficeFeatures.Infrastructure);

            switch (FormMode)
            {
                case TaskRuleMode.Approved:
                    FormTitle = "Approved Rule";
                    if (settings != null && settings.ApprovedCriteriaList.Count > 0)
                        ruleEditor.CriteriaList = settings.ApprovedCriteriaList;
                    break;

                case TaskRuleMode.Rejeted:
                    FormTitle = "Rejected Rule";
                    if (settings != null && settings.RejectedCriteriaList.Count > 0)
                        ruleEditor.CriteriaList = settings.RejectedCriteriaList;
                    break;

                case TaskRuleMode.Reassigned:
                    FormTitle = "Reassigned Rule";
                    if (settings != null && settings.ReassignCriteriaList.Count > 0)
                        ruleEditor.CriteriaList = settings.ReassignCriteriaList;
                    break;

                case TaskRuleMode.Requested:
                    FormTitle = "Requested Rule";
                    if (settings != null && settings.RequestInformationCriteriaList.Count > 0)
                        ruleEditor.CriteriaList = settings.RequestInformationCriteriaList;
                    break;

                case TaskRuleMode.Termiation:
                    FormTitle = "Termination Rule";
                    if (settings != null && settings.TerminationCriteriaList.Count > 0)
                        ruleEditor.CriteriaList = settings.TerminationCriteriaList;
                    break;

                case TaskRuleMode.Finished:
                    FormTitle = "Finished Rule - Apply To Request Information";
                    if (settings != null && settings.FinishedCriteriaList.Count > 0)
                        ruleEditor.CriteriaList = settings.FinishedCriteriaList;
                    break;

                case TaskRuleMode.SignatureVerified:
                    FormTitle = "Signature Verification Completed Rule";
                    if (settings != null && settings.SignatureVerificationCriteriaList.Count > 0)
                        ruleEditor.CriteriaList = settings.SignatureVerificationCriteriaList;
                    break;

                case TaskRuleMode.DataQualityCompleted:
                    FormTitle = "Data Quality Completed Rule";
                    if (settings != null && settings.DataQualityCompletedCriteriaList.Count > 0)
                        ruleEditor.CriteriaList = settings.DataQualityCompletedCriteriaList;
                    break;
            }
            base.CreateChildControls();
        }

        protected override void OnInit(EventArgs e)
        {
            ruleEditor.OnAfterSaveSuccess += new TaskRuleEditor.ExtenalHandler(TaskRuleEditor_AfterSaveSuccess);
            base.OnInit(e);
        }

        public override void TaskRuleEditor_AfterSaveSuccess()
        {
            Session[SessionKey] = ruleEditor.CriteriaList;
            base.TaskRuleEditor_AfterSaveSuccess();
        }
    }
}
