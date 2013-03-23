using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Microsoft.SharePoint;

namespace AIA.Intranet.Model.Models
{
    public class EECSettings
    {
        public bool Enabled { get; set; }
        public string EmailTemplateListUrl { get; set; }
        public string EmailTemplateName { get; set; }
        public bool AllowEditEmailBeforeSend { get; set; }
        public bool CheckoutOnSending { get; set; }
        public bool CheckInAfterUploaded { get; set; }
        public string CheckinComment { get; set; }
        public bool UseRedlineVersion { get; set; }
        public string RedlineCheckinComment { get; set; }
        public bool AppendEmailContentToComment { get; set; }

        public string DocSetIncludedFieldName { get; set; }
        public string DocSetIncludedFieldValue { get; set; }
        public string EmailToFieldId { get; set; }
        public string EmailCCFieldId { get; set; }

        public string EmailUponCheckInTemplateName { get; set; }

        public string EmailUponCheckInUserFieldId { get; set; }
        public bool EmailToInitiator { get; set; }
        public bool EmailToMetadata { get; set; }

        public string ReceiveAddress { get; set; }
        public string SendAddress { get; set; }
        public EmailOption OptionReceiveAddress { get; set; }
        public EmailOption OptionSendAddress { get; set; }

        public bool EnableNotifyEmailNoAttachment { get; set; }
        public bool EnableNotifyEECRecepient { get; set; }
        public string EECRecipientTemplateName { get; set; }
        public bool EnableNotifyInternalUser { get; set; }
        public string NotifyInternalUserTemplateName { get; set; }
        public List<string> NotifyInternalUser { get; set; }
        public bool EnableIncludeRecipientEmail { get; set; }
        public bool EnableNotifyEECRecepientNonEECAttachment { get; set; }
        public string EECRecipientNonEECAttachmentTempalteName { get; set; }

        //public bool EnableCheckoutOverride { get; set; }
        public bool EnableAllowEECSenderOverrideEECCheckout { get; set; }
        public bool EnableAllowUserOverideEECCheckout { get; set; }
        public List<string> EECCheckoutOverrideUser { get; set; }

        public bool KeepNonEECDocuments { get; set; }

        public string EECTaskButtonLabel { get; set; }
        public string EECTaskStatus { get; set; }
    }

    // EEC Document Settings
    [Serializable]
    public class EECDS
    {
        // sender email
        public string SE { get; set; }
    }

    // EEC document settings: store information about workfkow, use for EEC send from IP form
    [Serializable]
    public class EECB
    {
        //Workflow instance Id
        public Guid? WIId { get; set; }
        //Task Id
        public int? TId { get; set; }
    }

    public class EECTrackingItemIds
    {
        public static Guid DocumentName = SPBuiltInFieldId.Title;
        public static Guid EECUniqueId = new Guid("502D2BB2-B6C3-4026-823E-B47527D3EB1E");
        public static Guid Sender = new Guid("917D8B48-8E94-4975-A1F4-3F7B6C4AF229");
        public static Guid WorkflowId = new Guid("90557D72-1499-41FC-B898-DD2F36C9178C");
        public static Guid TaskId = new Guid("AD138AED-3E38-428F-A7B1-A51FD88425F0");
        public static Guid Recipient = new Guid("9AF0CC33-1E90-4D5C-A6D0-331CF369DE8B");
        public static Guid DocumentUrl = new Guid("6B566ADE-1012-4A76-B2B5-B752770E0123");
        public static Guid Status = new Guid("A338DF6C-C4B2-4BA5-B1C2-C4022F7761E7");
    }
}
