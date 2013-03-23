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
using AIA.Intranet.Common.Extensions;
using AIA.Intranet.Common.Utilities;

namespace AIA.Intranet.Infrastructure.CustomFields
{
    public partial class ImageFieldBaseFieldControl : BaseFieldControl
    {
        /// <summary>
        /// Private class used to parse the field value
        /// </summary>
        class FieldContent
        {
            public int ID { get; private set; }
            public string Thumbnail { get; private set; }
            public string FullUrl { get; private set; }

            public FieldContent(string fieldValue)
            {
                this.ID = int.Parse(fieldValue.Substring(0, fieldValue.IndexOf(";#")));
                this.FullUrl = fieldValue.Substring(fieldValue.IndexOf(";#") + 2, fieldValue.LastIndexOf(";#") - fieldValue.IndexOf(";#") - 2);
                this.Thumbnail = fieldValue.Substring(fieldValue.LastIndexOf(";#") + 2);
            }
        }

        /// <summary>
        /// Field properties
        /// </summary>
        private ImageFieldProperties fieldProperties = null;

        /// <summary>
        /// Control template
        /// </summary>
        protected override string DefaultTemplateName { get { return "ImageFieldTemplate"; } }

        /// <summary>
        /// Existing image drop down list change
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        protected void ddlExistingPicture_OnSelectedIndexChanged(object sender, EventArgs args)
        {
            if (ddlExistingPicture.SelectedValue != "-1")
            {
                // Display the image control
                imgExistingPicture.Visible = true;

                // Get the SPListItem corresponding to the picture
                using (SPWeb web = SPContext.Current.Site.OpenWeb(new Guid(fieldProperties.WebId)))
                {
                    try
                    {
                        SPList pictureLibrary = web.Lists[new Guid(fieldProperties.PictureLibraryId)];
                        SPListItem picture = pictureLibrary.GetItemById(int.Parse(ddlExistingPicture.SelectedValue));

                        // Display the thumbnail in the picture
                        imgExistingPicture.ImageUrl = CCIUtility.GetRelativeUrl(picture["EncodedAbsThumbnailUrl"].ToString());
                    }
                    catch (Exception ex) {
                        CCIUtility.LogError(ex);
                    }
                }
            }
            else
            {
                imgExistingPicture.Visible = false;
            }
        }

        /// <summary>
        /// Code used to display the field value in the DisplayForm
        /// </summary>
        /// <param name="output"></param>
        protected override void RenderFieldForDisplay(System.Web.UI.HtmlTextWriter output)
        {
            if (this.ItemFieldValue != null)
            {
                FieldContent picture = new FieldContent(this.ItemFieldValue.ToString());
                output.WriteLine(string.Format("<a href='{0}' target='_blank'><img src='{1}' style='border-width:0px;' /></a>", picture.FullUrl, picture.Thumbnail));
            }
        }

        /// <summary>
        /// Create child controls
        /// </summary>
        protected override void CreateChildControls()
        {
            // Retrieve field properties
            
            fieldProperties = FieldManagement<ImageFieldProperties>.GetFieldProperties(this.Field);
            if (string.IsNullOrEmpty(fieldProperties.WebId))
            {
                fieldProperties.WebId = SPContext.Current.Web.ID.ToString();
            }
            if (ControlMode != SPControlMode.Display)
            {
                base.CreateChildControls();
                BindControls();

                if (ddlExistingPicture != null)
                {

                    if (!Page.IsPostBack)
                    {

                        // Retrieve all pictures in order to add them in the drop down list
                        using (SPWeb web = SPContext.Current.Site.OpenWeb(new Guid(fieldProperties.WebId)))
                        {
                            if (!fieldProperties.PictureLibraryId.IsGuid())
                            {
                                var folder = web.Folders[fieldProperties.PictureLibraryId];
                                var list = web.Lists[folder.ParentListId];
                                fieldProperties.PictureLibraryId = list.ID.ToString();
                            }
                            SPList pictureLibrary = web.Lists[new Guid(fieldProperties.PictureLibraryId)];
                            SPListItemCollection pictures = pictureLibrary.GetItems(new SPQuery { Query = "<Query />" });

                            if (fieldProperties.DefaultPictureId != -1)
                            {
                                ddlExistingPicture.Items.Add(new ListItem(SPUtility.GetLocalizedString("$Resources:Default", "ImageField", (uint)SPContext.Current.Web.Locale.LCID), fieldProperties.DefaultPictureId.ToString()));
                            }

                            ddlExistingPicture.Items.Add(new ListItem(SPUtility.GetLocalizedString("$Resources:None", "ImageField", (uint)SPContext.Current.Web.Locale.LCID), "-1"));
                            imgExistingPicture.Visible = false;

                            foreach (SPListItem picture in pictures)
                            {
                                if (!picture.Name.StartsWith("dv_") || picture.Name.Length != 39)
                                {
                                    string pictureUrl = CCIUtility.GetRelativeUrl(picture["EncodedAbsThumbnailUrl"].ToString());
                                    ListItem pictureItem = new ListItem(picture.Name, picture.ID.ToString());

                                    // If the picture is the one referenced by the field
                                    if (this.ControlMode == SPControlMode.Edit && this.ItemFieldValue != null && new FieldContent(this.ItemFieldValue.ToString()).Thumbnail == pictureUrl)
                                    {
                                        pictureItem.Selected = true;
                                        imgExistingPicture.Visible = true;
                                        imgExistingPicture.ImageUrl = pictureUrl;
                                    }

                                    ddlExistingPicture.Items.Add(pictureItem);
                                }
                            }
                            if (imgExistingPicture.Visible == false)
                            {
                                ddlExistingPicture_OnSelectedIndexChanged(null, null);
                            }
                        }
                    }

                    ddlExistingPicture.SelectedIndexChanged += ddlExistingPicture_OnSelectedIndexChanged;
                }

            }

        }

        /// <summary>
        /// Retrieve the value to store in the field (upload the file is necessary)
        /// </summary>
        /// <returns>File name</returns>
        private string GetFieldValue()
        {

            string filename = null;

            // Check if the page is postack
            if (Page.IsPostBack)
            {
                string postbackControlName = Page.Request.Params.Get("__EVENTTARGET");

                if (!string.IsNullOrEmpty(postbackControlName))
                {
                    Button postbackControl = this.Page.FindControl(postbackControlName) as Button;

                    // And if the button clicked sent the command Save Item
                    if (postbackControl != null && postbackControl.CommandName == "SaveItem")
                    {
                        this.Page.Validate();

                        // And check if the page is valid. ALl this stuff just to know if the form is valid and the save button is clicked
                        if (this.Page.IsValid)
                        {

                            // If the user has selected a file
                            if (imageFieldPicture.HasFile)
                            {
                                // Get the name of the posted file
                                var imageFieldUpload = this.Context.Request.Files.AllKeys.SingleOrDefault(k => k == imageFieldPicture.UniqueID);

                                if (!string.IsNullOrEmpty(imageFieldUpload))
                                {
                                    // Get the posted file
                                    HttpPostedFile file = this.Context.Request.Files[imageFieldUpload];

                                    if (file.ContentLength > 0)
                                    {
                                        using (SPWeb web = SPContext.Current.Site.OpenWeb(new Guid(fieldProperties.WebId)))
                                        {
                                            SPList pictureLibrary = web.Lists[new Guid(fieldProperties.PictureLibraryId)];

                                            string uploadedFileName = file.FileName.Substring(file.FileName.LastIndexOf("\\") + 1, file.FileName.LastIndexOf(".") - file.FileName.LastIndexOf("\\") - 1);
                                            string uploadedFileExtension = file.FileName.Substring(file.FileName.LastIndexOf(".") + 1);
                                            string pictureFileName = string.Empty;

                                            if (fieldProperties == null || string.IsNullOrEmpty(fieldProperties.FormatName))
                                            {
                                                pictureFileName = string.Format("{0}.{1}", uploadedFileName, uploadedFileExtension);
                                            }
                                            else
                                            {
                                                pictureFileName = string.Format("{0}.{1}", fieldProperties.FormatName.Replace("[Name]", uploadedFileName).Replace("[Guid]", Guid.NewGuid().ToString("N")).Replace("[Date]", DateTime.Now.ToString("yyyyMMddHHmmssfff")), uploadedFileExtension);
                                            }

                                            string fileUrl = SPUrlUtility.CombineUrl(pictureLibrary.RootFolder.ToString(), pictureFileName);

                                            // Add the selected picture in the list
                                            SPListItem pictureItem = web.Files.Add(fileUrl, file.InputStream, fieldProperties != null ? fieldProperties.Overwrite : false).Item;

                                            //VuCA: changed pictureItem.ParentList.ParentWeb.Site.Url (make wrong url when using in subsite) to pictureItem.ParentList.ParentWeb.ServerRelativeUrl (also change from absolute url to relative url when saving)
                                            filename = string.Format("{0};#{1};#{2}", pictureItem.ID, SPUrlUtility.CombineUrl(pictureItem.ParentList.ParentWeb.ServerRelativeUrl, pictureItem.Url), CCIUtility.GetRelativeUrl(pictureItem["EncodedAbsThumbnailUrl"].ToString()));
                                        }
                                    }

                                    // These lines are used to remove the posted file from the Request.Files collection. Otherwise, SharePoint will automatically add it as an item attachment...
                                    MethodInfo baseRemove = this.Context.Request.Files.GetType().GetMethod("BaseRemove", BindingFlags.NonPublic | BindingFlags.Instance, null, new Type[] { typeof(string) }, null);

                                    if (baseRemove != null)
                                    {
                                        baseRemove.Invoke(this.Context.Request.Files, new object[] { imageFieldUpload });
                                    }

                                }

                            }
                            else
                            {
                                if (int.Parse(ddlExistingPicture.SelectedValue) != -1)
                                {
                                    // Retrieve the SPListItem in order to retrive its full url
                                    using (SPWeb web = SPContext.Current.Site.OpenWeb(new Guid(fieldProperties.WebId)))
                                    {
                                        SPList pictureLibrary = web.Lists[new Guid(fieldProperties.PictureLibraryId)];
                                        SPListItem picture = pictureLibrary.GetItemById(int.Parse(ddlExistingPicture.SelectedValue));

                                        //VuCA: changed web.Site.Url (make wrong url when using in subsite) to web.ServerRelativeUrl (also change from absolute url to relative url when saving)
                                        filename = string.Format(

                                            "{0};#{1};#{2}",
                                            ddlExistingPicture.SelectedValue,
                                            SPUrlUtility.CombineUrl(web.ServerRelativeUrl, picture.Url),
                                            imgExistingPicture.Visible ? imgExistingPicture.ImageUrl : CCIUtility.GetRelativeUrl(picture["EncodedAbsThumbnailUrl"].ToString())
                                        );
                                    }
                                }
                                else
                                {
                                    //raise validatation
                                    return "";
                                }
                            }
                        }
                    }
                }
            }

            return filename;
        }

        /// <summary>
        /// Field value
        /// </summary>
        public override object Value
        {
            get
            {
                EnsureChildControls();
                return GetFieldValue();
            }
        }

    }
}
