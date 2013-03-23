using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SharePoint.WebControls;
using System.Web.UI.WebControls;

namespace AIA.Intranet.Infrastructure.CustomFields
{
    public class RegularExpressionValidatorFieldControl: BaseFieldControl
    {
        private const string RENDERING_TEMPLATE = "RegularExpressionValidatorFieldControlTemplate";

        protected override string DefaultTemplateName
        {
            get
            {
                return RENDERING_TEMPLATE;
            }
        }
        

        protected TextBox txtValidateRegex;
        protected RegularExpressionValidatorField field;

        protected override void CreateChildControls()
        {

            if (this.Field != null)
            {
                field = this.Field as RegularExpressionValidatorField;

                base.CreateChildControls();
                this.txtValidateRegex = (TextBox)TemplateContainer.FindControl("txtValidateRegex");
                
            }
            if (this.ControlMode != SPControlMode.Display)
            {
                if (!this.Page.IsPostBack)
                {
                    if (this.ControlMode == SPControlMode.New)
                    {


                    }
                }
            }
            else
            {
                if (this.ItemFieldValue != null)
                {
                   
                }

            }
        }
        public override object Value
        {
            get
            {
                EnsureChildControls();
                return txtValidateRegex.Text;
            }
            set
            {
                EnsureChildControls();
                this.txtValidateRegex.Text = (String)this.ItemFieldValue;

                }

            }
        
       
    }
}

