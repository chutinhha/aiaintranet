using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SharePoint.WebControls;
using System.Reflection;
using System.Web;
using Microsoft.SharePoint;
using System.Web.UI.WebControls;
using Microsoft.SharePoint.Utilities;
using System.Web.UI;

namespace AIA.Intranet.Infrastructure.CustomFields
{
    public partial class ImageFieldBaseFieldControl : BaseFieldControl
    {

        protected DropDownList ddlExistingPicture;
        protected FileUpload imageFieldPicture;
        protected Image imgExistingPicture;

        protected Label lbExistingPicture;
        protected Label lbOrNewPicture;
        protected RegularExpressionValidator revUploadPicture;

        private void BindControls()
        {
            imageFieldPicture = TemplateContainer.FindControl("imageFieldPicture") as FileUpload;
            ddlExistingPicture = TemplateContainer.FindControl("ddlExistingPicture") as DropDownList;
            imgExistingPicture = TemplateContainer.FindControl("imgExistingPicture") as Image;

            lbExistingPicture = TemplateContainer.FindControl("lbExistingPicture") as Label;
            lbExistingPicture.Text = SPUtility.GetLocalizedString("$Resources:SelectExistingPicture", "ImageField", (uint)SPContext.Current.Web.Locale.LCID);

            lbOrNewPicture = TemplateContainer.FindControl("lbOrNewPicture") as Label;
            lbOrNewPicture.Text = SPUtility.GetLocalizedString("$Resources:UploadNewPicture", "ImageField", (uint)SPContext.Current.Web.Locale.LCID);

            revUploadPicture = TemplateContainer.FindControl("revUploadPicture") as RegularExpressionValidator;
            revUploadPicture.ErrorMessage = SPUtility.GetLocalizedString("$Resources:InvalidPicture", "ImageField", (uint)SPContext.Current.Web.Locale.LCID);
        }

    }
}
