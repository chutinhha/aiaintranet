using System.ComponentModel;

namespace AIA.Intranet.Model
{
    /// <summary>
    /// 
    /// </summary>
    /// 
    /// 

    public class TaskApprovalStatus
    {
        public const string Initiated = "*";
        public const string Approved = "#";
        public const string Rejected = "!";
        public const string Reasigned = "@";
        public const string RequestChange = "%";
        public const string RequestInf = "^";
        public const string Canceled = "$";
    }

    public enum TypeStatistic
    {
        Task,
        TaskStatus,
        TaskRate
    }

    public enum ImageSize
    {
        Thumbnail,
        Large,
        Full
    }

    public enum Operators
    {
        Equal = 1,
        NotEqual = 2,
        GreaterThan = 3,
        LessThan = 4,
        StartsWith = 5,
        EndWith = 6,
        Contains = 7,
        EarlierThan = 8,
        LaterThan = 9,
        IsNull = 10,
        IsNotNull = 11
    }

    public enum SortType
    {
        NotApply = 0,
        Ascending = 1,
        Descending = 2,
    }

    public enum OperatorDisplayType
    {
        Hide,
        Display,
        Select,
    }

    public enum SearchScope
    {
        CurrentSite = 0,
        CurrentAndSubSite = 1,
        SiteCollection = 2
    }

    public enum SearchListBaseType
    {
        GenericList = 0,
        DocumentLibrary = 1,
        DiscussionForum = 3,
        VoteOrSurvey = 4,
        IssuesList = 5
    }

    public enum ESignStatus
    {
        Voided,
        Created,
        Deleted,
        Sent,
        Delivered,
        Signed,
        Completed,
        Declined,
        TimedOut,
        Template,
        Processing,
        Invalid,
        Initial,
        Exception
    }

    public enum IOfficeFeatures
    {

        [Description("Corridor .app Security")]
        CCIappDefaultSecurity = 0,
        [Description("Corridor .app DocuSign ")]
        CCIappDocuSign = 1,
        [Description("[I-Office] Infrastructure")]
        Infrastructure = 2,
        [Description("Corridor .app Item Disscustion")]
        CCIappItemDisscutions = 3,
        [Description("Corridor .app Search")]
        CCIappSearch = 4,
        [Description("[I-Office] Workflow")]
        Workflow = 5,
        [Description("Corridor .app  Custom Status Setting")]
        CCIappCustomStatusSettings = 6,
        [Description("Email Extenal Collaboration")]
        EEC = 7,
        [Description("Corridor .app Active Directory Management")]
        CCIappADManagement = 8,
        [Description("Corridor .app Clause Insertion Module")]
        CCIappCIM = 9,
        [Description("Corridor .app DocuSign Timer Job")]
        CCIappDocuSignTimerJob = 10,
        [Description("I-Office")]
        IOfficeApp = 11,
        [Description("Corridor .app CEM")]
        CCIappCEM = 12,
        [Description("[I-Office] Infrastructure Picture")]
        InfrastructurePicture = 14,
        [Description("[I-Office] Infrastructure Video")]
        InfrastructureVideo = 14,
    }

    public static class CCIFormType
    {
        public const string InfoPath = "InfoPath";
        public const string WebForm = "Web Form";
        public const string Default = "Default";
        public const string ASPXForm = "Aspx Form";
    }

    public enum TaskRuleMode
    {
        Approved = 0,
        Rejeted = 1,
        Reassigned = 2,
        Requested = 3,
        Termiation = 4,
        Finished = 5,
        SignatureVerified = 6,
        DataQualityCompleted = 7
    }

    public enum AutomatedReportDeliveryType
    {
        Email = 0,
        Save = 1
    }

    public enum AutomatedReportScheduleType
    {
        Daily = 0,
        Weekly = 1,
        Monthly = 2,
        BiWeekly = 3,
        Quarterly = 4
    }

    public enum OutputType
    {
        Text,
        LoginName,
        DisplayName,
        EmailAddress,
        LookupValue,
        LookupId
    }

    public enum LinkIPFormType
    {
        View,
        Edit
    }
    public enum RecieverEventTypes
    {
        ContextEvent,

        ItemAdded,

        ItemAdding,

        ItemAttachmentAdded,

        ItemAttachmentAdding,

        ItemAttachmentDeleted,

        ItemAttachmentDeleting,

        ItemCheckedIn,

        ItemCheckedOut,

        ItemCheckingIn,

        ItemCheckingOut,

        ItemDeleted,

        ItemDeleting,

        ItemFileConverted,

        ItemFileMoved,

        ItemFileMoving,

        ItemUncheckedOut,

        ItemUncheckingOut,

        ItemUpdated,

        ItemUpdating,
    }
    //Extend Properties
    public static class TaskExtendProperties
    {
        public const string STB_MESS_TO_APPROVER = "STBMessToApprover";
        public const string STB_ASSIGN_TO = "STBAssignTo";
        public const string STB_TASK_COMMENTS = "STBComments";
        public const string UPDATED_PROPERTIES = "FieldsToUpdated";
        public const string CCI_ALLOW_ON_HOLD = "CMappAllowPlaceOnHold";
        public const string CCI_ALLOW_SEND_EEC = "CMappAllowSendEEC";
        public const string CCI_TASK_TYPE = "CMappTaskType";
        public const string CCI_TASK_STATUS = "CMappTaskStatus";
        public const string CCI_ASSIGN_TO = "CMappAssignTo";
        public const string CCI_REQUEST_TO = "CMappRequestTo";
        public const string CCI_COMMENT = "CMappComment";
        public const string CCI_NEW_DESCRIPTION = "CMappNewDescription";
        public const string CCI_TASK_INSTRUCTION = "CMappTaskInstruction";
        public const string CCI_ALLOW_REASSIGN = "CMappAllowReassign";
        public const string CCI_ALLOW_DUEDATE_CHANGE_ON_REASSIGNMENT = "CMappAllowDueDateChangeOnReassign";
        public const string CCI_ALLOW_REQUEST_INFORMATION = "CMappAllowRequestInformation";
        public const string CCI_ALLOW_DUEDATE_CHANGE_ON_REQUEST_INFORMATION = "CMappAllowDueDateChangeOnRequestionInfomation";
        public const string CCI_NEW_DUEDATE = "CMappNewDueDate";
        public const string CCI_PREVIOUS_TASK_ID = "CMappPreviousTaskId";
        public const string CCI_EEC_FROM = "EECFrom";
        public const string CCI_EEC_TO = "EECTo";
        public const string CCI_EEC_CC = "EECCC";
        public const string CCI_EEC_SUBJECT = "EECSubject";
        public const string CCI_EEC_BODY = "EECBody";
        public const string CCI_EEC_BUTTON_LABEL = "EECButtonLabel";
        public const string CCI_EEC_TASK_STATUS = "EECTaskStatus";
        public const string CCI_EEC_TASK_BACK_STATUS = "EECTaskBackStatus";
        public const string OWS_TASK_STATUS = "TaskStatus";
        public const string CCI_DATA_QUALITY_COMPLETE = "DataQualityProceduresComplete";
        public const string CCI_CONTRACT_PUTAWAY_COMPLETE = "ContractPutawayProceduresComplete";
        public const string CCI_APPROVED_TEXT = "ApprovedText";
        public const string CCI_REJECTED_TEXT = "RejectedText";
        public const string CCI_SIGNATURE_VERIFIED_TEXT = "SignatureVerifiedText";
        public const string CCI_DATA_QUALITY_COMPLETED_TEXT = "DataQualityCompletedText";
    }

    public static class NavigationKeys
    {
        public  const string ADMINISTRATION = "ADMINISTRATION";
    }

   
}
