using System;
using System.Linq;
using AIA.Intranet.Common.Extensions;
using AIA.Intranet.Model;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Utilities;
using Microsoft.SharePoint.WebControls;

namespace AIA.Intranet.Infrastructure.Layouts
{
    public partial class CloneList : LayoutsPageBase
    {
        #region Properties
        protected SPList CurrentList
        {
            get
            {
                return SPContext.Current.List;
            }
        }

        protected string SourceUrl
        {
            get
            {
                return base.Request.QueryString["Source"];
            }
        }
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void OnSubmit(object o, EventArgs e)
        {
            this.Page.Validate();
            try
            {
                SPList newList;
                if ((int)CurrentList.BaseTemplate == 101166 || 10000 == (int)CurrentList.BaseTemplate || CurrentList.BaseType == SPBaseType.DocumentLibrary )
                {
                    newList = CloneDocumentLibrary();
                }
                else
                {
                    newList = CloneGenericList();
                }
                
                SPUtility.Redirect(newList.DefaultViewUrl, SPRedirectFlags.Default, this.Context);

                //SPLongOperation.BeginOperation beginOperation = null;

                //if (beginOperation == null)
                //{
                //    beginOperation = delegate(SPLongOperation longOperation)
                //    {
                //        SPList newList;
                //        if ((int)CurrentList.BaseTemplate == 101166)
                //        {
                //            newList = cloneDocumentLibrary();
                //        }
                //        else
                //        {
                //            newList = cloneGenericList();
                //        }

                //        longOperation.End(newList.DefaultViewUrl);
                //    };
                //}
                //SPLongOperation.Begin("Please wait...", string.Empty, beginOperation);

              
            }
            catch (Exception ex)
            {
                SPUtility.TransferToErrorPage("Error occur: " + ex.Message);
            }
        }

        private SPList CloneGenericList()
        {
            SPList list = CurrentList;
            SPList newList = CreateList(list);

            foreach (SPListItem item in list.Items)
            {
                SPListItem newDestItem = newList.Items.Add();

                foreach (string fileName in item.Attachments)
                {
                    SPFile file = item.ParentList.ParentWeb.GetFile(item.Attachments.UrlPrefix + fileName);
                    byte[] imageData = file.OpenBinary();
                    newDestItem.Attachments.Add(fileName, imageData);
                }

                foreach (SPField field in item.Fields)
                    if ((!field.ReadOnlyField) && (field.InternalName != "Attachments"))
                        newDestItem[field.InternalName] = item[field.InternalName];

                newDestItem.Update();
            }
            newList.Update();
            return newList;
        }

        private SPList CreateList(SPList list)
        {
            SPList newList = null;
            SPListTemplate template = SPContext.Current.Web.Site.RootWeb.ListTemplates.Cast<SPListTemplate>().FirstOrDefault(l => l.Type_Client == (int)list.BaseTemplate && l.FeatureId == list.TemplateFeatureId);
            Guid newListID = SPContext.Current.Web.Lists.Add(txtNewName.Text, string.Empty, template);
            //Guid newListID = SPContext.Current.Web.Lists.Add(txtNewName.Text, string.Empty, txtNewName.Text, template.FeatureId.ToString(), template.Type_Client, "100");
            newList = SPContext.Current.Web.Lists[newListID];

            list.CopyAllFieldsToList(newList);
            list.CopyAllContentTypesToList(newList);
            list.CopyAllViewsToList(newList);
            if (template.Type_Client == 2004 || 20116 == template.Type_Client)
            {
                string settingsXml = list.GetCustomProperty(Constants.SEARCH_SETTING_PROPERTY_NAME);
                newList.SetCustomProperty(Constants.SEARCH_SETTING_PROPERTY_NAME, settingsXml);
                newList.Update();
            }
            return newList;
        }

        private SPList CloneDocumentLibrary()
        {
            SPList list = CurrentList;
            SPList newList = CreateList(list);
            //RecursiveCopy(list, newList, list.RootFolder, newList.RootFolder);
            newList.Update();
            return newList;
        }
        
        private SPListItemCollection getItemInFolder(SPList list, SPFolder folder)
        {
            SPQuery qry = new SPQuery();
            qry.Folder = folder;
            SPListItemCollection ic = list.GetItems(qry);
            return ic;
        }

        private void RecursiveCopy(SPList objSourceList, SPList newlist, SPFolder objSourceFolder, SPFolder objDestinationFolder)
        {
            SPListItemCollection objItems = getItemInFolder(objSourceList, objSourceFolder);

            foreach (SPListItem objItem in objItems)
            {
                //If it's a file copy it.
                if (objItem.FileSystemObjectType == SPFileSystemObjectType.File)
                {
                    if (objItem.File != null)
                    {
                        byte[] fileBytes = objItem.File.OpenBinary();
                        string DestinationURL = string.Format(@"{0}/{1}", objDestinationFolder.Url, objItem.File.Name);

                        //Copy the file.
                        SPFile objDestinationFile = objDestinationFolder.Files.Add(DestinationURL, fileBytes, true);
                        objDestinationFile.Update();
                    }
                    else
                    {
                    }
                }
                else
                {
                    string dirURL = string.Format(@"{0}/{1}", objDestinationFolder.Url, objItem.Folder.Name);
                    SPFolder objNewFolder = objSourceList.ParentWeb.Site.RootWeb.GetFolder(dirURL);
                    if (!objNewFolder.Exists)
                    {
                        //SPFolder objNewFolder = objDestinationFolder.SubFolders.Add(dirURL);
                        objNewFolder = objDestinationFolder.SubFolders.Add(dirURL);
                        objNewFolder.Update();
                    }

                    //Copy all the files in the sub folder
                    RecursiveCopy(objSourceList, newlist, objItem.Folder, objNewFolder);
                }
            }
        }
    }
}
