using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AIA.Intranet.Model
{
   public class Constants
   {
       #region AIA
       public const string BANNER_LIBRARY_URL = "BannerLibrary";
       public const string BANNER_CONTENT_TYPE_ID = "0x01010200ec98bbf978904280be10a8bbde810ef7";
       public const string NEWS_CATEGORY_LIST_URL = "/Lists/NewsCategory";
       public const string NEWS_DEFAULT_CATEGORY = "News";
       public const string NEWS_DEFAULT_LISTS_URL = "/Lists/News";

       public const string HEADER_MENU_LIST_URL = "Lists/HeaderMenu";
       public const string FOOTER_MENU_LIST_URL = "Lists/FooterMenu";
       public const string LEFT_MENU_LIST_URL = "Lists/LeftMenu";

       public const string ORDER_NUMBER_COLUMN = "OrderNumber";
       public const string MENU_KEYWORDS_COLUMN = "MenuKeywords";
       public const string ACTIVE_COLUMN = "Active";

       public const string CONTACT_INTERNAL_EMAIL_PROPERTY = "CONTACT_INTERNAL_EMAIL_PROPERTY";
       public const string CONTACT_EXTERNAL_EMAIL_PROPERTY = "CONTACT_EXTERNAL_EMAIL_PROPERTY";
       public const string CONTACT_TITLE_EMAIL_PROPERTY = "CONTACT_TITLE_EMAIL_PROPERTY";
       public const string CONTACT_BODY_HTML_EMAIL_PROPERTY = "CONTACT_BODY_HTML_EMAIL_PROPERTY";
       public const string CONTACT_ADD_DATE_EMAIL_PROPERTY = "CONTACT_ADD_DATE_EMAIL_PROPERTY";

       public const string TYPE_OF_ENQUIRY_LIST_URL = "/Lists/TypeOfEnquiry";
       public const string CONTACT_LIST_URL = "/Lists/Opinions";
       #endregion AIA

       #region Approval Status
       public const string APPROVED = "0";

       public const string DENIED = "1";

       public const string PENDING = "2";

       public const string DRAFT = "3";

       public const string SCHEDULED = "4";
       #endregion Approval Status

       #region Permission Level
       public const string APPROVER_PERMISSION_LEVEL = "IOffice Approver";
       public const string APPROVER_PERMISSION_LEVEL_DESCRIPTION = "Can view, add, update, approve, and delete list items and documents.";
       public const string CONTRIBUTE_NOT_DELETE_PERMISSION_LEVEL = "IOffice Contribute Not Delete";
       public const string CONTRIBUTE_NOT_DELETE_PERMISSION_LEVEL_DESCRIPTION = "Can view, add, update, but NOT delete list items and documents.";
       public const string NO_ACCESS_PERMISSION_LEVEL = "IOffice Limited Access";
       public const string NO_ACCESS_PERMISSION_LEVEL_DESCRIPTION = "Can view specific lists, document libraries, list items, folders, or documents when given permissions.";
       #endregion

       #region Department Site
       public const string DEPARTMENT_PREFIX_EMPLOYEE_GROUP = "Employee";
       public const string DEPARTMENT_PREFIX_ADMIN_GROUP = "Admin";
       public const string DEPARTMENT_PREFIX_MANAGER_GROUP = "Manager";
       public const string DEPARTMENT_INFRASTRUCTURE_FEATURE_ID = "cabf828a-7396-46eb-888a-38fd264345db";
       public const string DEPARTMENT_DOCUMENT_LIST_URL = "Documents";
       public const string DEPARTMENT_DISCUSSIONBOARD_LIST_URL = "Lists/Discussions";
       public const string DEPARTMENT_ANNOUNCEMENT_LIST_URL = "Lists/Announcements";
       public const string DEPARTMENT_CALENDAR_LIST_URL = "Lists/Calendar";
       public const string DEPARTMENT_CUSTOM_PROPERTIE_SITE_NAME = "DepartmentSiteName";
       public const string DEPARTMENT_CUSTOM_PROPERTIE_DEPARTMENT_ITEM_ID = "DepartmentItemId";
       public const string DEPARTMENT_CUSTOM_PROPERTIE_EMPLOYEE_GROUP = "DepartmentEmployeeGroup";
       public const string DEPARTMENT_CUSTOM_PROPERTIE_MANAGER_GROUP = "DepartmentManagerGroup";
       public const string DEPARTMENT_CUSTOM_PROPERTIE_ADMIN_GROUP = "DepartmentAdminGroup";
       public const string DEPARTMENT_CUSTOM_PROPERTIE_SITE_NAVIGATION_KEY = "DepartmentSiteNavigationKey";
       public const string DEPARTMENT_CUSTOM_PROPERTIE_SITE_DEPARTMENT_GUID = "DepartmentSiteGUID";
       public const string DEPARTMENT_EMPLOYEE_VIEW = "DepartmentEmployeeView";
       #endregion Department Site

       #region Employees
       public const string EMPLOYEE_TEMPLATE_NAME = "EmployeeTemplate";
       public const string EMPLOYEE_CONTENT_TYPE = "[I-Office] - Employee Content Type";
       public const string EMPLOYEE_CUSTOM_PROPERTIE_DOCUMENT_NAME = "EmployeeDocumentGui";
       #endregion Employees

       #region News
       public const string NEWS_HOME_PAGE = "/Pages/Home.aspx";
       public const string NEWS_LISTPAGE = "List"; //create List.aspx
       public const string NEWS_DISPLAYPAGE = "View"; //create View.aspx

       #endregion News

       #region RoomBookingSystem
       public const string RBS_HOME_PAGE = "/SitePages/General.aspx"; //create List.aspx
       #endregion RoomBookingSystem

       #region Projects
       public const string PROJECT_LIST_URL = "/Lists/Projects";
       public const string PROJECT_STATUS_LIST_URL = "/Lists/ProjectStatus";
       public const string PROJECT_CUSTOM_PROPERTIE_DOCUMENT_ID = "ProjectDocumentsGui";
       public const string PROJECT_CUSTOM_PROPERTIE_DERIVED_EXPENSES_ID = "ProjectDerivedExpensesGui";
       public const string PROJECT_REDIRECT_PAGE_DOCUMENT_URL = "{SiteUrl}/_layouts/AIA.Intranet.Infrastructure/RedirectPage.aspx?ID={ItemId}&List={ListId}&Type=ProjectDocuments";
       public const string PROJECT_REDIRECT_PAGE_DERIVED_EXPENSES_URL = "{SiteUrl}/_layouts/AIA.Intranet.Infrastructure/RedirectPage.aspx?ID={ItemId}&List={ListId}&Type=ProjectDerivedExpenses";
       #endregion Projects

       #region Picture, Video
       
       #endregion Picture, Video

       #region WebPart Zone
       public const string WEBPART_ZONE_HEADER = "Header";
       public const string WEBPART_ZONE_FOOTER = "Footer";
       public const string WEBPART_ZONE_BODY = "Body";
       public const string WEBPART_ZONE_RIGHT_COLUMN = "RightColumn";
       public const string WEBPART_PAGE_DEFAULT = "Pages/Default.aspx";
       #endregion WebPart Zone

       #region Task
       public const string TASK_LIST_URL = "/Lists/ITasks";
       public const string TASK_CONTENT_TYPE_PRIVATE = "0x0108008a8b86f25451dfb96e65a47187c7f295";
       public const string TASK_CONTENT_TYPE_DEPARTMENT = "0x0108008a8b86f25d157fbe9664174a8ac7f297";
       public const string TASK_CONTENT_TYPE_PROJECT = "0x0108008a8b862f5d157fb9e661474a8ac7f729";
       #endregion Task

       #region Timesheet
       public const string TIMESHEET_LIST_TEMPLATE_FEATURE_ID = "00bfea71-ec85-4903-972d-ebe475780106";
       public const string TIMESHEET_WEB_FEATURE_GUID = "45d6afed-0165-4d75-9b77-e77dddba8448";
       public const string TIMESHEET_LIST_GUI_CUSTOM_PROPERTIE = "TIMESHEET_LIST_GUID";
       public const string TIMESHEET_SITE_URL = "/Timesheets";
       public const string TIMESHEET_LIST_URL = "/Lists/Timesheets";
       public const string TIMESHEET_CONTENT_TYPE_ID = "0x0102007b2a803acf714e46a8718b4066ada225";
       #endregion Timesheet
       public const string CAT_ID = "CatID";

       #region Stationery
       public const string STATIONERY_CATEGORY_LIST_URL = "/Lists/StationeryCategory";
       public const string STATIONERY_LIST_URL = "/Lists/Stationery";
       public const string STATIONERY_APPROVAL_LIST_URL = "/Lists/StationeryApproval";
       #endregion Stationery

       public const string DATA_INFRASTRUCTURE_FEATURE_ID = "6f3122af-6a20-4aa5-8e3a-02a65a20d548";
       public const string FORUM_USER_PAGE_URL = "ForumUserURL";
       public const string PICTURE_FLAG_FIELD_NAME = "PictureFlag";
       public const string LAST_VISIT_FIELD_NAME = "LastVisisted";
       public const string MAIL_LIST_TYPE = "Emaillist";
       public const string STATISTIC_LIST_URL = "/Lists/Statistics";
       public const string FORUM_CATEGORY_URL = "/Lists/Categories";
       public const string ASSIGMENT_FIELD_NAME = "AssignementField";
       public const string UNREAD_CONTENT_LIST_URL = "/Lists/UnreadContent";
       public const string NO_IMAGE_URL = "/Style Library/images/no_image.gif";
       public const string DEFAULT_THEME = "Red";
       public const string SITEMAP_MAPPING_LIST = "/Lists/SiteMapMappings";
       public const string NAVIGATION_LIST = "/Lists/Navigations";
       public const string COMMENT_LIST_URL = "/Lists/PageComments";
       
       public const string CONFIG_LIST_URL = "/Lists/GlobalConfigurations";
       public const string DEPARTMENT_LIST_URL = "/Lists/Departments";
       public const string EMPLOYEE_LIST_URL = "/Lists/Employees";
       public const string ILINK_LIST_URL = "/Lists/ILinks";
       public const string TOPEMPLOYEE_LIST_URL = "/Lists/TopEmployees";
       public const string POSITION_LIST_URL = "Lists/Position";
       public const string COMPANY_CALENDAR_LIST_URL = "/Lists/CompanyCalendar";
       public const string EMAIL_TEMPLATES_LIST_URL = "/Lists/EmailTemplates";
       public const string EMAIL_LIST_URL = "/Lists/EmailList";
       
       public const string USER_INFOMATION_LIS_NAME = "User Information List";
       public const string THEME_NAME_FIELD = "ThemeName";
       
       public const string CHOOSE_CONTENT_TYPE = "Choose content type";
       public const string NONE_VALUE = "(None)";
       
       public const string UsersToAdd = "UsersToAdd";
       public const string GroupDn = "groupDn";
       public const string GroupLogInName = "GroupLogInName";
       public const string UserDn = "userDn";
       public const string UserLogInName = "UserLogInName";
       public const string GroupsToAdd = "GroupsToAdd";
       public const string Referer = "Referer";

       public const string LINK_TO_ITEM_TYPE_NAME = "LinkToItem";

       #region ADList
       public const string ADManagerListUrl = "Lists/ADManager";
       public const string OUField = "OU";
       public const string UrlField = "Site Url";
       #endregion

       #region Page
       public const string EditGroupPageTitle = "Edit Active Directory Group";
       public const string ADDGroupPageTitle = "Add New Active Directory Group";
       public const string EditUserPageTitle = "Edit Active Directory User";
       public const string ADDUserPageTitle = "Add New Active Directory User";
       #endregion

       //GENERAL CONSTANT
       public const string LINK_VIEW_COLUMN = "LinkView";
       public const string LINK_EDIT_COLUMN = "LinkEdit";
       public const string LINK_TO_FILE_COLUMN = "FileLeafRef";
       public const string NOT_APPLY_VALUE = "(Not Apply)";
       public const string CCI_WORKFLOW_TASK_ID = "0x01080100E6FA232BCA3B4B25B9DF4B2E3791D3FC";
       public const string CCI_WORKFLOW_TASK_CONTENT_TYPE_ID = "0x01080100e6fa232bca3b4b25b9df4b2e3791d3fc";
       public const string TASK_EVENT_SETTING_EDITOR_KEY = "TaskEventEditor";

       //ECB Menu
       public const string LINK_WORKFLOW_COLUMN = "LinkWorkflow";
       public const string LINK_SEND_ESIGN_COLUMN = "LinkSendESign";
       public const string LINK_ESIGN_HISTORY_COLUMN = "LinkESignHistory";
       public const string LINK_VERSION_HISTORY_COLUMN = "LinkVersionHistory";
       public const string LINK_MANAGE_PERMISSIONS_COLUMN = "LinkManagePermissions";
       public const string LINK_DOCUMENT_DISCUSSIONS_COLUMN = "LinkDocumentDiscussions";
       public const string LINK_UPLOAD_EXECUTED_COLUMN = "LinkUploadExecuted";
       public const string LINK_ALERT_ME_COLUMN = "LinkAlertMe";
       public const string LINK_SEND_TO_OTHER_LOCATION_COLUMN = "LinkSendToOtherLocation";
       public const string LINK_SEND_TO_EMAIL_COLUMN = "LinkSendToEmail";
       public const string LINK_SEND_TO_WORKSPACE_COLUMN = "LinkSendToWorkSpace";
       public const string LINK_SEND_TO_DOWNLOAD_COPY_COLUMN = "LinkSendToDownloadCopy";
       public const string LINK_SEND_TO_DOWNLOAD_PDF_COPY_COLUMN = "LinkSendToDownloadPDFdCopy";
       public const string LINK_RELATED_METRICS_COLUMN = "LinkRelatedMetrics";
       public const string LINK_RELATED_RISKS_COLUMN = "LinkRelatedRisks";
       public const string LINK_RELATED_DOCUMENT_COLUMN = "LinkRelatedDocument";
       public const string LINK_CREATE_RELATED_DOCUMENT_COLUMN = "LinkCreateRelatedDocument";
       public const string LINK_SEND_EEC_COLUMN = "LinkSendEEC";
       public const string LINK_EMAIL_ATTACHMENT_COLUMN = "LinkEmailAttachment";
       public const string LINK_CREATE_PDF_COLUMN = "LinkCreatePDF";

       //WORKFLOW
       public const string CCI_WORKFLOW_SIGNATURE_VERIFICATION_CONTENT_TYPE_ID = "0x01080100e6fa232bca3b4b25b9df4b2e3791d3fc05";
       public const string DATA_QUALITY_CONTENT_TYPE_ID = "0x01080100e6fa232bca3b4b25b9df4b2e3791d3fc06";
       public const string LOOKUP_WITH_PICKER_TYPE_NAME = "LookupFieldWithPicker";
       public const string IMAGE_FIELD_TYPE_NAME = "ImageField";
       // SEARCH CONSTANT ------------------------!>
       public const string KEY_TODAY = "[today]";
       public const string KEY_ME = "[me]";
       public const string LEFT = "Left";
       public const string RIGHT = "Right";
       public const string TITLE = "Title";
       public const string TYPE_COLUMN = "Type";
       public const string CHOOSE_PROPERTY = "Choose property";
       public const string NO_DATA = "No data";
       public const string SUBMIT_BUTTON_ID = "btnSubmit";
       public const string NO_TITLE = "No title";
       public const string NO_NAME = "No name";
       public const string SETTING_PROPERTY_NAME = "CCIappSearchDefinitionPackage";
       public const string LINK_FILE_NAME_TEXT_COMBO = "Name (Display Name)";
       public const string LINK_FILE_NAME_STATIC = "LinkFilename";
       public const int MAX_NUMBER_OF_FIELDS = 20;
       public const string SEARCH_SETTING_PROPERTY_NAME = "CCIappSearchSettings";
       public const string SEARCH_DEFINITION_PROPERTY_NAME = "CCIappSearchDefinition";
       public const string AUTOMATED_REPORT_DEFINITION_PROPERTY_NAME = "CCIappAutomatedReportDefinition";
       public const string AUTOMATED_REPORT_JOB_PROPERTY_NAME = "CCIappAutomatedReportJob";

       public static Dictionary<Operators, string> OperatorDisplayNames =
          new Dictionary<Operators, string>{  {Operators.Equal, "Equals"},
                                                {Operators.NotEqual, "Not Equals"},
                                                {Operators.GreaterThan, "Greater Than"},
                                                {Operators.LessThan, "Less Than"},
                                                {Operators.StartsWith, "Starts With"},
                                                {Operators.EndWith, "End With"},
                                                {Operators.Contains, "Contains"},
                                                {Operators.EarlierThan, "Earlier Than"},
                                                {Operators.LaterThan, "Later Than"},
                                                {Operators.IsNull, "Is Null"},
                                                {Operators.IsNotNull, "Is Not Null"}
                                            };

       public static Dictionary<SortType, string> SortTypeValue =
          new Dictionary<SortType, string>{  {SortType.Ascending, "asc"},
                                                {SortType.Descending, "desc"}};

       public static Dictionary<SearchScope, string> SearchScopeDisplayNames =
       new Dictionary<SearchScope, string>{  {SearchScope.CurrentSite, "Current site"},
                                                {SearchScope.SiteCollection, "Site collection"},
                                                {SearchScope.CurrentAndSubSite, "Current site and sub site"}};

       public static Dictionary<SearchListBaseType, string> SearchListBaseTypeDisplayNames =
       new Dictionary<SearchListBaseType, string>{  {SearchListBaseType.GenericList, "Generic list"},
                                                        {SearchListBaseType.DocumentLibrary, "Document library"},
                                                        {SearchListBaseType.DiscussionForum, "Discussion forum"},
                                                        {SearchListBaseType.VoteOrSurvey, "Vote or Survey"},
                                                        {SearchListBaseType.IssuesList, "Issues list"}};
       

       //Extend Properties
       public static class TaskExtendProperties
       {
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

       public class EEC
       {
           public const string CCIUniqueIdFieldName = "CCIUniqueId";
           public const string CCIappEECFeatureName = "CCI app External Email Collaboration";
           public const string EECOverrideCheckout = "EECOverrideCheckout";
       }

       public class Workflow
       {
           public const string NONE = "none";
           public const string ITEM = "item";
           public const string TASK = "task";
           public const string TASK_LIST = "tasklist";
           public const string ESIGN = "esign";
           public const string GLOBAL = "global";
           public const string CURRENT_USER = "user";

           public const string COMPLETED_TEXT = "Completed";
           public const string STATUS_APPROVED_TEXT = "Approved";
           public const string STATUS_REJECTED_TEXT = "Rejected";
           public const string STATUS_CANCEL_TEXT = "Cancel";
           public const string STATUS_REASSIGN_TEXT = "Reassigned";
           public const string STATUS_REQUEST_INFORMATION_TEXT = "Requested";
           public const string STATUS_FINISHED_TEXT = "Finished";
           public const string STATUS_SIGNATURE_VERIFIED_TEXT = "Signature Verified";
           public const string STATUS_DATA_QUALITY_COMPLETED_TEXT = "Data Quality Completed";
           public const string STATUS_ON_HOLD_TEXT = "On Hold";
           public const string STATUS_SEND_EEC_TEXT = "On EEC hold";
           public const string EEC_BUTTON_LABEL_TEXT = "Send EEC";

           //Extend Properties
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

           // Mirgrate from Intel.WF.
           public const string COUNTERPARTY_COUNTRY_PROPERTY = "CounterpartyCountryFieldName";
           public const string INTEL_WF_FEATURE_ID = "4f07e81b-0247-470b-8cd0-aef5b3076602";
           public const string ELECTRONIC_SIGNATURE_FLAG_FIELD_NAME = "ElectronicSignatureFlagFieldName";
           public const string SIGNATURE_DELIVERY_METHOD_FIELD_NAME = "SignatureDeliveryMethodFieldName";
           public const string INTEL_FINAL_APPROVER_FIELD_NAME = "IntelFinalApproverFieldName";
           public const string INTEL_REQUESTER_NAME_FIELD_NAME = "IntelRequesterNameFieldName";
           public const string CONTRACT_NUMBER_FIELD_NAME = "ContractNumber";
           public const string CONTENT_TYPE_FIELD_NAME = "ContentType";
           public const string INCLUDE_WITH_DELIVERY_FIELD_NAME = "IncludeWithDelivery";
           public const string RELATED_CONTRACT_NUMBER_FIELD_NAME = "RelatedContractNumber";
           public const string DELAY_TIME_FOR_SEND_RELATED_DOCUMENTS = "DelayTimeForSendRelatedDocuments";

           //For InfoPath Action
           public const string DEFAULT_CONTENT_TYPE = "default";
           public const string ERROR_LOAD_INFOPATH_FORM_DATA = "Error loading form data file. Likely causes: this file is not a valid InfoPath Form!";
           public const string ERROR_GET_INFOPATH_FORM_VALUE = "Error getting form value from path: {0}";
           public const string ERROR_GET_SITE = "Cannot open site at the URL: {0}";
           public const string ERROR_GET_WEB = "Cannot open web at the URL: {0}";
           public const string ERROR_GET_FOLDER = "Cannot get folder at the URL: {0}";
           public const string ERROR_UPLOAD_FILE_TO_HIDDEN_FOLDER = "Cannot place files into : {0}";
           public const string ERROR_CONTENT_TYPE_NOT_EXIST_ON_LIBRARY = "The \"{0}\" content type does not exist on the library";
           public const string ERROR_FILE_EXIST = "The file name: {0} already exists in {1}";
           public const string ERROR_NO_ATTACHMENT = "There are no attachments at {0}";
           public const string ERROR_INFOPATH_FORM_VALUE_IS_NULL_OR_NOT_EXIST = "The form value is null or does not exist at path: {0}";
           public const string ERROR_FIELD_NAME_NOT_EXIST_ON_LIBRARY = "Cannot copy description to destination library because the \"{0}\" field name does not exist on the library";
           public const string ERROR_LOAD_DESCRIPTION_INFOPATH_FORM_VALUE = "Cannot copy description to destination library because the form value is null or does not exist at path: {0}";
           public const string ERROR_UPLOADING_FILE = "Error uploading file";
           public const string UPLOAD_FILE_SUCESSFULLY = "\"{0}\" has been uploaded to {1}";


          
       }

       public class Common
       {
           public const string WEBCONFIG_FEATURE_ID = "7dea7681-2cbe-45cb-8638-7ad66a5e3dce";
           public const string ASSEMBLIES = "assemblies";
       }

       public class Infrastructure
       {
           public const string ADGroup = "ADGroup";
           public const string ViewItems = "All Items";
           public const string ReportTemplate = "ReportTemplate.xls";
           public const string ReportTemplatePath = @"TEMPLATE\LAYOUTS\";
           public const string NewForm = "NewForm.aspx";
           public const string EditForm = "EditForm.aspx";
           public const string NULL = "NULL";
           public const string LookupType = "Lookup";
           public const string REMINDER_TIMERJOB_NAME = "RemiderBirthDay";
           public const string FeatureID = "ef6ad61c-e4a8-4f5a-87d4-9e7b8665e680";
           public const string DisplayAttachments = "{0}/_layouts/AIA.Intranet.Infrastructure/DisplayAttachments.ashx?item={0}/Lists/Employees/&ID={1}&List={2}";
           public const string DepartmentSiteTemplateName = "STS#1";
           //custom list properties
           public const string SendNotifyEmailProperty = "SendNotifyEmail";
           public const string WebIdProperty = "WebId";
           public const string ListIdProperty = "ListId";
           public const string ColumnNameProperty = "ColumnName";

       }

       //Official Document
       public class OfficialDocument
       {
           public static string HomepageUrl = "/Pages/Home.aspx";
           public static string LegislationTypesListUrl = "/Lists/LegislationTypes";

       }
    }
   public static class CCIappWorkflowTaskView
   {
       public const string APPROVAL = "CCIappWorkflowTaskApproval.aspx";
       public const string CHANGE = "CCIappWorkflowTaskChange.aspx";
       public const string REASSIGN = "CCIappWorkflowTaskReassign.aspx";
       public const string EEC = "CCIappWorkflowSendEEC.aspx";
       public const string ONHOLD = "CCIappWorkflowTaskOnHold.aspx";
       public const string READONLY = "CCIappWorkflowTaskDisplay.aspx";
       public const string REQUESTCHANGE = "CCIappWorkflowTaskRequestChange.aspx";
   }

   public static class TaskConfigurationFieldIds
   {
       public static Guid CogfigName = new Guid("{fa564e0f-0c70-4ab9-b863-0177e6ddd247}");
       public static Guid TaskContentTypeId = new Guid("{4B44BA81-AEB1-43d6-866E-EA89ECEA088F}");
       public static Guid Approvers = new Guid("{F840B2EE-1741-44e6-865F-B86B994A43E5}");
       public static Guid ExpandGroup = new Guid("{00769AF0-98EC-4581-815E-FA87884BBAF2}");
       public static Guid AssignmentType = new Guid("{82451111-490A-4e1e-AA0A-4190A42152ED}");
       public static Guid AssignmentEmailTemplate = new Guid("{A2DA8CEC-844A-480a-8BBD-246B03E97CE3}");
       public static Guid DueDateDuration = new Guid("{84A57D4C-1025-459a-B08B-09D4DBB3BDE6}");
       public static Guid DueDateMeasure = new Guid("{C443DB54-6701-4024-A567-AF32CAAC3CA3}");
       public static Guid ReminderDateDuration = new Guid("{3BE0E01A-1391-4b5f-97AA-CE9681A8C36B}");
       public static Guid ReminderDateMeasure = new Guid("{764AB4F8-E082-4bcc-A6C1-B7A79E3BFF3A}");
       public static Guid ReminderEmailTemplate = new Guid("{35C00C3F-51EC-4be5-B386-0836C7D2B18D}");
       public static Guid EscalationDateDuration = new Guid("{4FE044EF-D584-4f78-9FAC-05C5D488A446}");
       public static Guid EscalationDateMeasure = new Guid("{34FFCB9C-E0CB-4871-AEF6-084CA39A9F52}");
       public static Guid EscalationParty = new Guid("{5BE0CA0E-FE66-4386-A4A0-942BE77D0864}");
       public static Guid EscalationEmailTemplate = new Guid("{004DD7F7-9E68-4f5c-9887-A7DD275490F7}");
       public static Guid UseNumberRequired = new Guid("{76145FE9-3BF5-4af0-9442-CE865064CFBA}");
       public static Guid NumberRequired = new Guid("{5BC7D8E7-6008-400d-A2A5-BBA03DEFEC2B}");
       public static Guid EmailTemplateUrl = new Guid("{A2A58311-E04A-4da4-AB4A-C69F87FAF5B8}");
       public static Guid ApprovedStatus = new Guid("{23BB6B39-607F-4c66-ADFE-71C4F9326491}");
       public static Guid TerminateStatuses = new Guid("{B654CE8E-D586-4e4e-BCCC-8AFAF8567F0D}");
       public static Guid TaskContributors = new Guid("{11DE4419-8CB0-4c7c-8EA1-D09CF6983BEB}");
       public static Guid TaskObservers = new Guid("{9BE13702-90C8-4fc9-87B8-825E06B615A2}");
       public static Guid TaskInstructions = new Guid("{219D79AE-2B38-45ef-981C-1593D21EFF20}");
       public static Guid TaskTitlePrefix = new Guid("{E8C78DE5-0715-4d72-B534-4B2FB9ADABEE}");
       public static Guid AllowDueDateChangeRessignment = new Guid("{FED59D37-E0B6-495a-AA45-79910A4162A7}");
       public static Guid AllowDueDateChangeRequestInfomation = new Guid("{874D9D59-F8AA-4793-B63F-2ABC3055CD39}");
       public static Guid AllowReassign = new Guid("{D968761F-5887-4853-BA30-E9EF61422396}");
       public static Guid AllowPlaceHoldOn = new Guid("{F7E03B30-6E71-49BA-9A52-04084F76F47C}");
       public static Guid AllowRequestInfomation = new Guid("{C367BF21-528C-4a3c-A8E0-AC6EC75C284C}");
       public static Guid UseMetaDataAssignment = new Guid("{93A75A66-E1FC-4630-880A-045B8CDD10CD}");
       public static Guid ApproversFieldId = new Guid("{C78517B8-F65F-448b-AC3F-D3B5C3A2070A}");
       public static Guid ByPassTask = new Guid("{C71DF0FB-9955-4E25-89F6-69ECE6A1A421}");
       public static Guid IgnoreIfNoParticipant = new Guid("{D127EB69-24FB-4803-B2F7-9304C57AB771}");
       public static Guid AllowSendEEC = new Guid("{C5E9ED40-3B4E-4C79-BD93-6ED9AABD0AB8}");
   }
   public static class TaskStatusVariableName
   {
       public const string APPROVED = "Approved Status";
       public const string REJECTED = "Rejected Status";
       public const string SIGNATURE_VERIFIED = "Signature Verified Status";
       public const string DATA_QUALITY_COMPLETED = "Data Quality Completed Status";
   }

   [Serializable]
   public class NameValue
   {
       public string Name { get; set; }
       public string Value { get; set; }
   }

  

    public class ColumnName
    {
        public static Guid Link = new Guid("{379847fc-ac82-4719-890f-aa2dc49ffcd8}");
    }

    //Extend Properties
    public static class IncomingDocumentTaskExtendProperties
    {
        public const string FocalPointUnit = "FocalPointUnit";
        public const string ImplementationUnit = "ImplementationUnit";
        public const string ApproverNote = "ApproverNote";
        public const string ApprovedDate = "ApprovedDate";
    }
}
