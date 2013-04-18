using System;
using System.Web.UI;
using System.Linq;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Microsoft.SharePoint.WebControls;

using Microsoft.SharePoint;
using System.Web;
using Microsoft.SharePoint.Utilities;
using AIA.Intranet.Infrastructure.CustomFields;
using AIA.Intranet.Common.Utilities;

namespace AIA.Intranet.Infrastructure.Controls
{
    public partial class ImageFieldControl : UserControl, IFieldEditor
    {
        // Field properties
        private ImageFieldProperties fieldProperties = null;

        public bool DisplayAsNewSection { get { return true; } }

        protected void Page_Load(object sender, EventArgs e)
        {
            LocalizeControls();

            if (!Page.IsPostBack)
            {
                SPSecurity.RunWithElevatedPrivileges(delegate()
                {
                    using (SPSite currentSite = new SPSite(SPContext.Current.Site.ID))
                    {
                        // Load all webs in drop down
                        foreach (SPWeb web in currentSite.AllWebs)
                        {
                            ddlWebs.Items.Add(new ListItem(web.Url, web.ID.ToString()));
                            web.Dispose();
                        }

                        // Select the good web site
                        if (fieldProperties != null)
                        {
                            ListItem item = ddlWebs.Items.FindByValue(fieldProperties.WebId);

                            if (item != null)
                                item.Selected = true;
                        }
                        else
                        {
                            ddlWebs.Items.FindByValue(SPContext.Current.Web.ID.ToString()).Selected = true;
                        }

                        // Load picture libraries of the site web
                        LoadPictureLibraries(currentSite);

                        // Init controls with field properties
                        if (fieldProperties != null)
                        {
                            ListItem item = ddlPictureLibrary.Items.FindByValue(fieldProperties.PictureLibraryId);

                            if (item != null)
                                item.Selected = true;

                            cbOverwrite.Checked = fieldProperties.Overwrite;
                            tbFormatName.Text = fieldProperties.FormatName;
                        }

                        // Load the default picture
                        if (fieldProperties != null && fieldProperties.DefaultPictureId != -1)
                        {
                            try
                            {
                                using (SPWeb spWeb = currentSite.OpenWeb(new Guid(fieldProperties.WebId)))
                                {
                                    SPList pictureLibrary = spWeb.Lists[new Guid(fieldProperties.PictureLibraryId)];
                                    SPListItem picture = pictureLibrary.GetItemById(fieldProperties.DefaultPictureId);

                                    // Display the thumbnail in the picture
                                    imgDefaultPicture.ImageUrl = Utility.GetRelativeUrl(picture["EncodedAbsThumbnailUrl"].ToString());
                                    
                                }
                                imgDefaultPicture.Visible = true;
                            }
                            catch {/* ignore incase picture library or default picture is not found */}
                        }
                    }
                });
            }
        }

        /// <summary>
        /// Add picture libraries name of the site web selected in the first dropdown list
        /// </summary>
        private void LoadPictureLibraries(SPSite spSite)
        {
            ddlPictureLibrary.Items.Clear();

            using (SPWeb web = spSite.OpenWeb(new Guid(ddlWebs.SelectedValue)))
            {
                var pictureLibraries = web.Lists.Cast<SPList>().Where(l => l.BaseTemplate == SPListTemplateType.PictureLibrary);

                foreach (SPList pl in pictureLibraries)
                {
                    ddlPictureLibrary.Items.Add(new ListItem(pl.Title, pl.ID.ToString()));
                }
            }
        }

        protected void ddlWebs_OnSelectedIndexChanged(object sender, EventArgs args)
        {
            SPSecurity.RunWithElevatedPrivileges(delegate()
            {
                using (SPSite currentSite = new SPSite(SPContext.Current.Site.ID))
                {
                    // Load libraries of the new selected web
                    LoadPictureLibraries(currentSite);
                }
            });
        }

        public void InitializeWithField(SPField field)
        {
            // Retrieves field properties
            fieldProperties = FieldManagement<ImageFieldProperties>.GetFieldProperties(field);
        }

        public void OnSaveChange(SPField field, bool isNewField)
        {
            SPSecurity.RunWithElevatedPrivileges(delegate()
            {
                using (SPSite currentSite = new SPSite(SPContext.Current.Site.ID))
                {
                    // Save field properties
                    ImageFieldProperties properties = new ImageFieldProperties
                    {
                        WebId = ddlWebs.SelectedValue,
                        PictureLibraryId = ddlPictureLibrary.SelectedValue,
                        FormatName = tbFormatName.Text,
                        Overwrite = cbOverwrite.Checked,
                        DefaultPictureId = fieldProperties == null ? -1 : fieldProperties.DefaultPictureId
                    };

                    if (!cbClearDefaultPicture.Checked)
                    {
                        if (fuDefaultPicture.HasFile)
                        {
                            // Get the posted file
                            HttpPostedFile file = fuDefaultPicture.PostedFile;

                            if (file.ContentLength > 0)
                            {
                                using (SPWeb web = currentSite.OpenWeb(new Guid(properties.WebId)))
                                {
                                    SPList pictureLibrary = web.Lists[new Guid(properties.PictureLibraryId)];

                                    string uploadedFileName = file.FileName.Substring(file.FileName.LastIndexOf("\\") + 1, file.FileName.LastIndexOf(".") - file.FileName.LastIndexOf("\\") - 1);
                                    string uploadedFileExtension = file.FileName.Substring(file.FileName.LastIndexOf(".") + 1);
                                    string pictureFileName = string.Format("dv_{0}.{1}", Guid.NewGuid().ToString("N"), uploadedFileExtension);
                                    string fileUrl = SPUrlUtility.CombineUrl(pictureLibrary.RootFolder.ToString(), pictureFileName);

                                    // Add the selected picture in the list
                                    SPListItem pictureItem = web.Files.Add(fileUrl, file.InputStream, true).Item;

                                    properties.DefaultPictureId = pictureItem.ID;
                                }
                            }
                        }
                    }
                    else
                    {
                        properties.DefaultPictureId = -1;
                    }

                    FieldManagement<ImageFieldProperties>.SaveProperties(properties);
                }
            });
        }

    }
}
