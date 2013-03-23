using System;
using System.Xml.Serialization;
//using AIA.Intranet.Model.DocuSign;
using System.Collections.Generic;

namespace AIA.Intranet.Model.Workflow
{
    [Serializable]
    [XmlInclude(typeof(SendEmailToStaticAddressesSettings))]
    [XmlInclude(typeof(SendEmailToWfItemUserColumnSettings))]
    [XmlInclude(typeof(SendEmailtoWorkflowTaskUserColumnSettings))]
    [XmlInclude(typeof(UpdateWorkflowItemMetadataSettings))]
    [XmlInclude(typeof(UpdateWorkflowTaskMetadataSettings))]
    //[XmlInclude(typeof(SendEmailWithESignMetadataToWfItemUserColumnSettings))]
    //[XmlInclude(typeof(SendEmailWithESignMetadataToWfItemUserColumnSettings))]
    //[XmlInclude(typeof(UpdateWorkflowItemWithESignMetadataSettings))]
    //[XmlInclude(typeof(UpdateWorkflowItemWithESignVariablesSettings))]
    [XmlInclude(typeof(UpdateExecutedDocumentMetaDataEditorSettings))]
    [XmlInclude(typeof(UpdateWorkflowItemWithKeywordSettings))]
    [XmlInclude(typeof(UpdateWorkflowItemWithTaskPropertySettings))]
    [XmlInclude(typeof(UpdateTaskItemWithItemPropertySettings))]
    [XmlInclude(typeof(UpdateWFItemPermissionSettings))]
    [XmlInclude(typeof(CreatUnreadTaskSettings))]
    [XmlInclude(typeof(UpdateTaskPermissionSettings))]

    public class TaskActionSettings
    {
        public TaskActionTypes Type { get; set; }
        public int Order { get; set; }
    }


     [Serializable]
    public class UpdateTaskPermissionSettings : TaskActionSettings
    {
         public UpdateTaskPermissionSettings()
        {
            StaticUsers = new List<string>();
            Columns = new List<string>();
            Type = TaskActionTypes.UpdateTaskPermission;
        }
        public string RoleId { get; set; }
        public bool KeepExisting { get; set; }
        public List<string> Columns { get; set; }


        public bool AllParticipiants { get; set; }

        public List<string> StaticUsers { get; set; }

        public int TaskId { get; set; }
    }
    [Serializable]
    public class UpdateWFItemPermissionSettings : TaskActionSettings
    {
        public UpdateWFItemPermissionSettings()
        {
            StaticUsers = new List<string>();
            Columns = new List<string>();
            Type = TaskActionTypes.UpdateWFPermission;
        }
        public string RoleId { get; set; }
        public bool KeepExisting { get; set; }
        public List<string> Columns { get; set; }


        public bool AllParticipiants { get; set; }

        public List<string> StaticUsers { get; set; }
    }

    [Serializable]
    public class CreatUnreadTaskSettings : TaskActionSettings
    {
        public CreatUnreadTaskSettings()
        {
            Type = TaskActionTypes.CreateUnreadTask;
        }
        public bool UsePredefine { get; set; }
        public bool UseCustom { get; set; }
    }

    [Serializable]
    [XmlInclude(typeof(SendEmailToWfItemUserColumnSettings))]
    [XmlInclude(typeof(SendEmailtoWorkflowTaskUserColumnSettings))]
    //[XmlInclude(typeof(SendEmailWithESignMetadataToStaticAddressesSettings))]
   // [XmlInclude(typeof(SendEmailWithESignVariableToStaticAddressesSettings))]

    public class SendEmailToStaticAddressesSettings : TaskActionSettings
    {
        public SendEmailToStaticAddressesSettings()
        {
            Type = TaskActionTypes.SendEmailToStaticAddresses;
            StaticUsers = new List<string>();
        }
        public string EmailTemplateUrl { get; set; }
        public string EmailTemplateName { get; set; }
        public string EmailAddress { get; set; }
        public int TaskId { get; set; }
        public bool AttachTaskLink { get; set; }
        public List<string> StaticUsers { get; set; }
    }

    [Serializable]
    public class SendEmailToWfItemUserColumnSettings : SendEmailToStaticAddressesSettings
    {
        public SendEmailToWfItemUserColumnSettings()
        {
            Type = TaskActionTypes.SendEmailToWorkflowItemUserColumn;
        }
        public string FieldId { get; set; }
    }

    [Serializable]
    public class SendEmailtoWorkflowTaskUserColumnSettings : SendEmailToStaticAddressesSettings
    {
        public SendEmailtoWorkflowTaskUserColumnSettings()
        {
            Type = TaskActionTypes.SendEmailToWorkflowTaskUserColumn;
        }
        public string FieldId { get; set; }
    }


    //[Serializable]
    //[XmlInclude(typeof(SendEmailWithESignMetadataToWfItemUserColumnSettings))]
    //public class SendEmailWithESignMetadataToStaticAddressesSettings : SendEmailToStaticAddressesSettings
    //{
    //    public SendEmailWithESignMetadataToStaticAddressesSettings()
    //    {
    //        Type = TaskActionTypes.SendEmailWithESignMetadataToStaticAddresses;
    //    }
    //    public ESignEventMetadata ESignMetadata { get; set; }
    //}


    //[Serializable]
    //[XmlInclude(typeof(SendEmailWithESignVariableToWfItemUserColumnSettings))]
    //public class SendEmailWithESignVariableToStaticAddressesSettings : SendEmailToStaticAddressesSettings
    //{
    //    public SendEmailWithESignVariableToStaticAddressesSettings()
    //    {
    //        Type = TaskActionTypes.SendEmailWithESignVariableToStaticAddresses;
    //    }
    //    public List<NameValue> Variables { get; set; }
    //    public ESignEventMetadata ESignMetadata { get; set; }
    //}
    


    //[Serializable]
    //public class SendEmailWithESignVariableToWfItemUserColumnSettings : SendEmailWithESignVariableToStaticAddressesSettings
    //{
    //    public SendEmailWithESignVariableToWfItemUserColumnSettings()
    //    {
    //        Type = TaskActionTypes.SendEmailWithESignVariableToWfItemUserColumn;
    //    }
    //    public string FieldId { get; set; }
    //}

    //[Serializable]
    //public class SendEmailWithESignMetadataToWfItemUserColumnSettings : SendEmailWithESignMetadataToStaticAddressesSettings
    //{
    //    public SendEmailWithESignMetadataToWfItemUserColumnSettings()
    //    {
    //        Type = TaskActionTypes.SendEmailWithESignMetadataToWorkflowItemUserColumn;
    //    }
    //    public string FieldId { get; set; }
    //}

    [Serializable]
    public class UpdateWorkflowItemMetadataSettings : TaskActionSettings
    {
        public UpdateWorkflowItemMetadataSettings()
        {
            Type = TaskActionTypes.UpdateWorkflowItemMetadata;
        }
        public string FieldId { get; set; }
        public string Value { get; set; }
    }

    [Serializable]
    public class UpdateWorkflowItemWithKeywordSettings : TaskActionSettings
    {
        public UpdateWorkflowItemWithKeywordSettings()
        {
            Type = TaskActionTypes.UpdateWorkflowItemWithKeyword;
        }
        public string FieldId { get; set; }
        public string Value { get; set; }
    }

    [Serializable]
    public class UpdateWorkflowTaskMetadataSettings : TaskActionSettings
    {
        public UpdateWorkflowTaskMetadataSettings()
        {
            Type = TaskActionTypes.UpdateWorkflowTaskMetadata;
        }
        public string FieldId { get; set; }
        public int TaskId { get; set; }
        public string Value { get; set; }
    }

    //[Serializable]
    //public class UpdateWorkflowItemWithESignMetadataSettings : TaskActionSettings
    //{
    //    public UpdateWorkflowItemWithESignMetadataSettings()
    //    {
    //        Type = TaskActionTypes.UpdateWorkflowItemWithESignMetadata;
    //    }
    //    public string FieldId { get; set; }
    //    public string ESignProperty { get; set; }
    //    public ESignEventMetadata ESignMetadata { get; set; }
    //}

    //[Serializable]
    //public class UpdateWorkflowItemWithESignVariablesSettings : TaskActionSettings
    //{
    //    public UpdateWorkflowItemWithESignVariablesSettings()
    //    {
    //        Type = TaskActionTypes.UpdateWorkflowItemWithEsignVariables;
    //    }
    //    public ESignEventMetadata ESignMetadata { get; set; }
    //    public List<NameValue> Variables { get; set; }
    //    public string FieldId { get; set; }
    //    public string VariableName { get; set; }
    //}

    [Serializable]
    [XmlInclude(typeof(UpdateTaskItemWithItemPropertySettings))]
    public class UpdateWorkflowItemWithTaskPropertySettings : TaskActionSettings
    {
        public UpdateWorkflowItemWithTaskPropertySettings()
        {
            Type = TaskActionTypes.UpdateWorkflowItemWithTaskProperty;
        }
        
        public string ItemFieldId { get; set; }
        public string TaskFieldId { get; set; }
        public int TaskId { get; set; }

    }

    [Serializable]
    public class UpdateTaskItemWithItemPropertySettings : UpdateWorkflowItemWithTaskPropertySettings
    {

        public UpdateTaskItemWithItemPropertySettings()
        {
            Type = TaskActionTypes.UpdateTaskItemWithItemProperty;
        }

       
    }

    [Serializable]
    public class UpdateExecutedDocumentMetaDataEditorSettings : TaskActionSettings
    {
        public UpdateExecutedDocumentMetaDataEditorSettings()
        {
            Type = TaskActionTypes.UpdateExecutedDocumentMetadata;
        }
        public string DestinationListUrl { get; set; }
        public int DestinationItemId { get; set; }
        public string FieldId { get; set; }
        public string Value { get; set; }
    }

    [Serializable]
    public enum TaskActionTypes
    {
        SendEmailToStaticAddresses,
        SendEmailToWorkflowItemUserColumn,
        SendEmailToWorkflowTaskUserColumn,
        UpdateWorkflowTaskMetadata,
        UpdateWorkflowItemMetadata,
        UpdateWorkflowItemWithESignMetadata,
        SendEmailWithESignMetadataToStaticAddresses,
        SendEmailWithESignMetadataToWorkflowItemUserColumn,
        UpdateWorkflowItemWithEsignVariables,
        SendEmailWithESignVariableToStaticAddresses,
        SendEmailWithESignVariableToWfItemUserColumn,
        UpdateExecutedDocumentMetadata,
        UpdateWorkflowItemWithKeyword,
        UpdateWorkflowItemWithTaskProperty,
        UpdateTaskItemWithItemProperty,
        UpdateWFPermission,
        CreateUnreadTask,
        UpdateTaskPermission
    }
}
