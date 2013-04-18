using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using AIA.Intranet.Common.Utilities;
using Microsoft.SharePoint.Utilities;

namespace AIA.Intranet.Infrastructure.CustomFields
{
    public class ImageField : SPFieldText
    {
        public ImageField(SPFieldCollection fields, string fieldName) : base(fields, fieldName) { }
        public ImageField(SPFieldCollection fields, string typeName, string displayName) : base(fields, typeName, displayName) { }

        public override BaseFieldControl FieldRenderingControl
        {
            get
            {
                BaseFieldControl control = new ImageFieldBaseFieldControl();
                control.FieldName = InternalName;
                return control;
            }
        }

        public override void OnUpdated()
        {
            try
            {
                FieldManagement<ImageFieldProperties>.SetFieldProperties(this);
            }
            catch (Exception)
            {
                
                
            }
            
        }

        public override void OnAdded(SPAddFieldOptions op)
        {
            
            //var data = FieldManagement<ImageFieldProperties>.GetFieldProperties(this);


            //if(string.IsNullOrEmpty(data.WebId)){
            //    data.WebId = SPContext.Current.Site.RootWeb.ID.ToString();
            //}
            try
            {
                FieldManagement<ImageFieldProperties>.SetFieldProperties(this);

            }
            catch (Exception ex)
            {
                Utility.LogError(ex);
                
            }

        }

        public override string GetValidatedString(object value)
        {
            if (!base.Required)
            {


                return base.GetValidatedString(value);
            }


            if (value == null || String.IsNullOrEmpty(value.ToString()))
            {


                throw new SPFieldValidationException(base.Title + " " + SPUtility.GetLocalizedString("$Resources:IsRequired", "ImageField", (uint)SPContext.Current.Web.Locale.LCID));
            }

            return base.GetValidatedString(value);
        }

        public Guid PictureLibrary { get { return new Guid(FieldManagement<ImageFieldProperties>.GetFieldProperties(this).PictureLibraryId); } }
        public Guid Web { get { return new Guid(FieldManagement<ImageFieldProperties>.GetFieldProperties(this).WebId); } }
        public bool Overwrite { get { return FieldManagement<ImageFieldProperties>.GetFieldProperties(this).Overwrite; } }
        public string FormatName { get { return FieldManagement<ImageFieldProperties>.GetFieldProperties(this).FormatName; } }

    }
}
