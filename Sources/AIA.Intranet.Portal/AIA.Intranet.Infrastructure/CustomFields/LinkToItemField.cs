using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using Microsoft.SharePoint.Security;
using System.Globalization;
using System.Windows.Controls;
using System.Security.Permissions;
using AIA.Intranet.Common.Extensions;
using AIA.Intranet.Infrastructure.Receivers;

namespace AIA.Intranet.Infrastructure.CustomFields
{
    public class LinkToItemField : SPFieldText
    {

        public LinkToItemField(SPFieldCollection fields, string fieldName)
            : base(fields, fieldName)
        {
        }

        public LinkToItemField(SPFieldCollection fields, string typeName, string displayName)
            : base(fields, typeName, displayName)
        {

        }
        public override void OnAdded(SPAddFieldOptions op)
        {
            base.OnAdded(op);
            if (this.ParentList != null)
            {
                this.ParentList.EnsureEventReceiver(typeof(UpdateLinkToItemFieldReceiver), 1000, SPEventReceiverSynchronization.Synchronous, SPEventReceiverType.ItemAdded);
            }
            this.ShowInDisplayForm = false;
            this.ShowInEditForm = false;
            this.ShowInNewForm = false;
            this.ShowInVersionHistory = false;
            this.Update();
        }
        public override void OnUpdated()
        {
            base.OnUpdated();
            //if (this.ParentList != null)
            //{
            //    this.ParentList.EnsureEventReceiver(typeof(UpdateLinkToItemFieldReceiver), 1000, SPEventReceiverSynchronization.Synchronous, SPEventReceiverType.ItemAdded);
            //}
            //this.ShowInDisplayForm = false;
            //this.ShowInEditForm = false;
            //this.ShowInNewForm = false;
            //this.ShowInVersionHistory = false;
            //this.Update();
        }
        public override BaseFieldControl FieldRenderingControl
        {
            [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
            get
            {
                BaseFieldControl fieldControl = new LinkToItemFieldControl();
                fieldControl.FieldName = this.InternalName;

                return fieldControl;
            }
        }
    }
}


