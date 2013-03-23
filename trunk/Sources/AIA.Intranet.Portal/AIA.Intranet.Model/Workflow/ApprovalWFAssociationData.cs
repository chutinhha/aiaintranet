using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AIA.Intranet.Model.Infrastructure;

namespace AIA.Intranet.Model.Workflow
{
    [Serializable]
    public class PermissionUpdate
    {
        public string Name { get; set; }
        public List<Guid> Columns { get; set; }
        public bool Ower { get; set; }
        public bool Approvers { get; set; }
    }

    [Serializable]
    public class ApprovalWFAssociationData
    {

        public ApprovalWFAssociationData()
        {
            Permissions = new List<PermissionUpdate>();
            ApproverData = new List<ApprovalWFApprover>();
            SpecificUserGroup = new List<string>();
            EnableApprove = true;
            EnableReject = true;
            TaskFormOption = new TaskFormOption();
            
        }

        public List<PermissionUpdate> Permissions { get; set; }
        public List<ApprovalWFApprover> ApproverData { get; set; }

        public bool EndOnFirstReject { get; set; }
        public bool EndOnItemDocumentChange { get; set; }
        public bool EnableContentApproval { get; set; }

        public int DelayOnStart { get; set; }
        public bool StartNotification { get; set; }
        //public string EmailTemplateUrl { get; set; }
        //public string TemplateName { get; set; }
        public EmailTemplateSettings EmailTemplate { get; set; }
        public string ColumnName { get; set; }
        public bool UseSpecificUserGroup { get; set; }
        public bool UseMetaData { get; set; }

        public List<string> SpecificUserGroup { get; set; }

        public bool EnableVerboseLog { get; set; }
        public bool EnableReassign { get; set; }
        public bool EnableHoldOn { get; set; }
        public bool EnableRequestChange { get; set; }

        public string ApproveLabel { get; set; }

        public string RejectLabel { get; set; }

        public string OnHoldLabel { get; set; }

        public string RequestChangeLabel { get; set; }

        public string RequestInformationLabel { get; set; }

        public string ReassignLabel { get; set; }

        public bool EnableApprove { get; set; }

        public bool EnableReject { get; set; }

        public TaskFormOption TaskFormOption { get; set; }

        public bool EnableStartingCondition { get; set; }

        public string ConditionFieldId { get; set; }

        public string ConditionFieldValue { get; set; }

        public bool ApproveIfByPass { get; set; }

        public bool EnableUpdatePermission { get; set; }

        public bool KeepCurrentPermissions { get; set; }

        public TaskEventSettings WFEvents { get; set; }
    }
}
