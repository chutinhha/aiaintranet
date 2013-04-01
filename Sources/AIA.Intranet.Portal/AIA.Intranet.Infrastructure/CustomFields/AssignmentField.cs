using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using Microsoft.SharePoint.Security;
using System.Security.Permissions;
using AIA.Intranet.Infrastructure.Receivers;
using AIA.Intranet.Model;

namespace AIA.Intranet.Infrastructure.CustomFields
{
    public class AssignmentField : SPFieldText
    {    
        private static Dictionary<int, string> customProperties = new Dictionary<int, string>();

        public AssignmentField(SPFieldCollection fields, string fieldName)
            : base(fields, fieldName)
        {
            Init();
        }
        public AssignmentField(SPFieldCollection fields, string typeName, string displayName)
            : base(fields, typeName, displayName)
        {
            Init();
        }
        public override BaseFieldControl FieldRenderingControl
        {
            [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
            get
            {
                BaseFieldControl fieldControl = new AssignmentFieldControl();
                fieldControl.FieldName = this.InternalName;
                return fieldControl;
            }
        }

        public override string GetValidatedString(object value)
        {
            if ((this.Required == true) && ((value == null) || ((String)value == "")))
            {
                throw new SPFieldValidationException(this.Title + " must have a value.");
            }
            else
            {

                return base.GetValidatedString(value);

            }
        }
        public override string GetFieldValueAsText(object value)
        {
            return "base.GetFieldValueAsText(value);";
        }
        private void Init()
        {
            this.SendEmailNotify = this.GetCustomProperty(Constants.Infrastructure.SendNotifyEmailProperty) + "";
            this.WebId = this.GetCustomProperty(Constants.Infrastructure.WebIdProperty) + "";
            this.ListId = this.GetCustomProperty(Constants.Infrastructure.ListIdProperty) + "";
            this.ColumnName = this.GetCustomProperty(Constants.Infrastructure.ColumnNameProperty) + "";
        }
        
        private string sendEmailNotify;
        public string SendEmailNotify
        {
            get
            {
                return customProperties.ContainsKey(this.SendNotifyEmailProperty) ?
                    customProperties[this.SendNotifyEmailProperty] : sendEmailNotify;
            }
            set
            {
                this.sendEmailNotify = value.ToString();
            }
        }

        private string webId;
        public string WebId
        {
            get
            {
                return customProperties.ContainsKey(this.WebIdProperty) ? customProperties[this.WebIdProperty] : webId;
            }
            set
            {
                this.webId = value.ToString();
            }
        }

        private string listId;
        public string ListId
        {
            get
            {
                return customProperties.ContainsKey(this.ListIdProperty) ? customProperties[this.ListIdProperty] : listId;
            }
            set
            {
                this.listId = value.ToString();
            }
        }

        private string columnName;
        public string ColumnName
        {
            get
            {
                return customProperties.ContainsKey(this.ColumnNameProperty) ? customProperties[this.ColumnNameProperty] : columnName;
            }
            set
            {
                this.columnName = value.ToString();
            }
        }

        public void UpdateCustomProperty(string property, string value)
        {
            customProperties[GetCatchedId(property)] = value.ToString();
        }

        public int GetCatchedId(string name)
        {
            return name.GetHashCode();
        }

        public int WebIdProperty
        {
            get
            {
                return GetCatchedId(Constants.Infrastructure.WebIdProperty);
            }
        }

        public int ListIdProperty
        {
            get
            {
                return GetCatchedId(Constants.Infrastructure.ListIdProperty);
            }
        }

        public int SendNotifyEmailProperty
        {
            get
            {
                return GetCatchedId(Constants.Infrastructure.SendNotifyEmailProperty);
            }
        }

        public int ColumnNameProperty
        {
            get
            {
                return GetCatchedId(Constants.Infrastructure.ColumnNameProperty);
            }
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
            this.SetCustomProperty(Constants.Infrastructure.SendNotifyEmailProperty, this.SendEmailNotify.ToString());
            this.SetCustomProperty(Constants.Infrastructure.WebIdProperty, this.WebId.ToString());
            this.SetCustomProperty(Constants.Infrastructure.ListIdProperty, this.ListId.ToString());
            this.SetCustomProperty(Constants.Infrastructure.ColumnNameProperty, this.ColumnName.ToString());

            base.Update();

            if (customProperties.ContainsKey(this.SendNotifyEmailProperty))
                customProperties.Remove(this.SendNotifyEmailProperty);
            if (customProperties.ContainsKey(this.WebIdProperty))
                customProperties.Remove(this.WebIdProperty);
            if (customProperties.ContainsKey(this.ListIdProperty))
                customProperties.Remove(this.ListIdProperty);
            if (customProperties.ContainsKey(this.ColumnNameProperty))
                customProperties.Remove(this.ColumnNameProperty);
        }
        public override void OnAdded(SPAddFieldOptions op)
        {
            base.OnAdded(op);
            Update();
            
            EnsureEventReceiver(this.ParentList, typeof(SetAssignementReceiver), SPEventReceiverType.ItemAdded);
        }

        private void EnsureEventReceiver(SPList list, System.Type ReceiverClass, params SPEventReceiverType[] ReceiverTypes)
        {
            if (list == null) return;

            string assembly = ReceiverClass.Assembly.FullName;
            foreach (var item in ReceiverTypes)
            {
                if (!list.EventReceivers.Cast<SPEventReceiverDefinition>().Any(P => P.Class == ReceiverClass.FullName && 
                    P.Assembly == assembly && 
                    P.Type == item))
                { 
                    list.EventReceivers.Add(item, assembly, ReceiverClass.FullName); 
                }
            }
            list.Update();

        }

    }

}
