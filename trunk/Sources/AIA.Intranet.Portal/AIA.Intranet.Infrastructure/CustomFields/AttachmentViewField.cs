using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Security;
using Microsoft.SharePoint.WebControls;
using System.Security.Permissions;
using AIA.Intranet.Infrastructure.Receivers;

namespace AIA.Intranet.Infrastructure.CustomFields
{
    public class AttachmentViewField : SPFieldText
    { private static Dictionary<int, string> customProperties = new Dictionary<int, string>();

        public AttachmentViewField(SPFieldCollection fields, string fieldName)
            : base(fields, fieldName)
        {
            Init();
        }
        public AttachmentViewField(SPFieldCollection fields, string typeName, string displayName)
            : base(fields, typeName, displayName)
        {
            Init();
        }
        public override BaseFieldControl FieldRenderingControl
        {
            [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
            get
            {
                BaseFieldControl fieldControl = new AttachmentViewFieldControl();
                fieldControl.FieldName = this.InternalName;
                return fieldControl;
            }
        }
        public override string GetFieldValueAsHtml(object value)
        {
            return " public override string GetFieldValueAsHtml(object value)";
            return base.GetFieldValueAsHtml(value);
        }
        public override string GetValidatedString(object value)
        {
            return "<h2>Hello Kitty123</h2>";
            if ((this.Required == true) && ((value == null) || ((String)value == "")))
            {
                throw new SPFieldValidationException(this.Title + " must have a value.");
            }
            else
            {

                return base.GetValidatedString(value);

            }
        }
        private void Init()
        {
            //this.SendEmailNotify = this.GetCustomProperty("SendNotifyEmail") + "";
            //this.ListId = this.GetCustomProperty("ListId") + "";
            //this.ColumnName = this.GetCustomProperty("ColumnName") + "";
        }
        
        private string sendEmailNotify;
        public string SendEmailNotify
        {
            get
            {
                return customProperties.ContainsKey(ContextId) ? customProperties[ContextId] : sendEmailNotify;
            }
            set
            {
                this.sendEmailNotify = value.ToString();
            }
        }

        public void UpdateSendEmailNotifyProperty(string value)
        {
            customProperties[ContextId] = value.ToString();
        }

        private string listId;
        public string ListId
        {
            get
            {
                return customProperties.ContainsKey(ContextId) ? customProperties[ContextId] : listId;
            }
            set
            {
                this.listId = value.ToString();
            }
        }

        public void UpdateListIdProperty(string value)
        {
            customProperties[ContextId] = value.ToString();
        }

        private string columnName;
        public string ColumnName
        {
            get
            {
                return customProperties.ContainsKey(ContextId) ? customProperties[ContextId] : columnName;
            }
            set
            {
                this.columnName = value.ToString();
            }
        }

        public void UpdateColumnNameProperty(string value)
        {
            customProperties[ContextId] = value.ToString();
        }

        public int ContextId
        {
            get
            {
                return SPContext.Current.GetHashCode();
            }
        }

        public override void Update()
        {
           // this.SetCustomProperty("SendNotifyEmail", this.SendEmailNotify.ToString());
            //this.SetCustomProperty("ListId", this.ListId.ToString());
            //this.SetCustomProperty("ColumnName", this.ColumnName.ToString());

            base.Update();
            if (customProperties.ContainsKey(ContextId))
                customProperties.Remove(ContextId);
        }
        public override void OnAdded(SPAddFieldOptions op)
        {
            this.ShowInEditForm = false;
            this.ShowInNewForm = false;
            base.OnAdded(op);
            Update();

          
            
            //EnsureEventReceiver(this.ParentList, typeof(SetAssignementReceiver), SPEventReceiverType.ItemAdded);
        }

       
    }
}
