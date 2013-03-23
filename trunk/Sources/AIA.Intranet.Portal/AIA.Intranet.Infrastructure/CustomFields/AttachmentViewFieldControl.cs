using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SharePoint.WebControls;
using System.Web.UI.WebControls;
using Microsoft.SharePoint;

namespace AIA.Intranet.Infrastructure.CustomFields
{
    public class AttachmentViewFieldControl : BaseFieldControl
    {

        //protected override string DefaultTemplateName
        //{
        //    get
        //    {
        //        if (this.ControlMode == SPControlMode.Display)
        //        {
        //            return this.DisplayTemplateName;
        //        }
        //        else
        //        {
        //            return "AssignmentFieldControlTemplate";
        //        }
        //    }
        //}
        public override string DisplayTemplateName
        {
            get
            {
                return "AttachmentDisplayFieldControlTemplate";
            }
            set
            {
                base.DisplayTemplateName = value;
            }
        }
        protected RadioButton radAll;
        protected RadioButton radCustom;
        protected DropDownList ddlLists;
        protected DropDownList ddlColumns;
        AttachmentViewField field = null;
        Panel pnlDisplay = null;
        protected Panel lblDisplay;
        protected RadioButton radDefault;
        protected LookupFieldWithPickerEntityEditor lookupEditor = null;

        protected override void CreateChildControls()
        {
            
            if (this.Field != null)
            {
                field = this.Field as AttachmentViewField;

                base.CreateChildControls();

                this.pnlDisplay = (Panel)TemplateContainer.FindControl("pnlDisplay");
            }

            if (this.ControlMode == SPControlMode.Display)
            {
                string img = string.Format("<img src='/_layouts/AIA.Intranet.Infrastructure/DisplayAttachments.ashx?item={0}&urlencode={1}&ID={2}&List={3}' width='62px' height='62px'/>", field.ParentList.DefaultViewUrl, field.ParentList.DefaultViewUrl, SPContext.Current.ItemId, field.ParentList.ID);
                pnlDisplay.Controls.Add(new Literal() { Text = img});
            }
           
        }

        public override object Value
        {
            get
            {
                
                EnsureChildControls();
                return string.Empty;
            }
            set
            {
                EnsureChildControls();
                if (this.ItemFieldValue != null)
                {
                  
                }
            }
        }
       


    }
}
