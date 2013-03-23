using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using AIA.Intranet.Common.Extensions;

namespace AIA.Intranet.Infrastructure.ControlTemplates.AIA.Intranet.Infrastructure
{
    public partial class EntryApprovalEditor : UserControl
    {
        private string ApprovalsFieldId = "{91DC60BB-923E-4e4f-9AA1-FD5DF37B3650}";
        private string ApprovalKeyFieldId = "{fa564e0f-0c70-4ab9-b863-0177e6ddd247}";

        protected void Page_Load(object sender, EventArgs e)
        {
            EnsureLookupFields(ApprovalsFieldId);
            if ((SPContext.Current.FormContext.FormMode == SPControlMode.Edit ||
                SPContext.Current.FormContext.FormMode == SPControlMode.New)
                && IsPostBack)
            {
                EnsureFieldValueUnique(ApprovalKeyFieldId);
            }
        }


        public static void EnsureLookupFields(string fieldId)
        {
            SPContext.Current.Web.AllowUnsafeUpdates = true;
            string approvalConfigListId = SPContext.Current.Web.GetList(SPContext.Current.Web.Url + "/Lists/TaskConfigurations").ID.ToString();
            SPFieldLookup approvalsField = (SPFieldLookup)SPContext.Current.List.Fields[new Guid(fieldId)];
            approvalsField.UpdateTargetList(approvalConfigListId);
            SPContext.Current.Web.AllowUnsafeUpdates = false;
        }


        public static void EnsureFieldValueUnique(string fieldId)
        {
            BaseFieldControl fieldControl = SPContext.Current.FormContext.FieldControlCollection.Cast<BaseFieldControl>()
                                        .FirstOrDefault(f => (f.Field != null && f.Field.Id.Equals(new Guid(fieldId))));
            if (fieldControl != null && fieldControl.Value != null)
            {
                string value = fieldControl.Value.ToString();
                bool unique = SPContext.Current.List.IsValueUnique(fieldControl.FieldName, value, SPContext.Current.ItemId);
                if (!unique)
                {
                    fieldControl.IsValid = false;
                    fieldControl.ErrorMessage = "The value had been defined in this list";
                }
            }
        }
    }
}
