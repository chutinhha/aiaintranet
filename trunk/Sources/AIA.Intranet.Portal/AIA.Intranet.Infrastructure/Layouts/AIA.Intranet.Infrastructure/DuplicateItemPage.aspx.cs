using System;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using System.IO;

using System.Collections;
using Microsoft.SharePoint.Utilities;
using AIA.Intranet.Common.Utilities;

namespace AIA.Intranet.Infrastructure.Layouts
{
    public partial class DuplicateItemPage : LayoutsPageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected override void OnLoad(EventArgs e)
        {
            SPLongOperation.Begin(
                delegate(SPLongOperation longOperation)
                {
                    try
                    {
                        using (SPLongOperation BeginOperation = new SPLongOperation(this.Page))
                        {
                            if (Request.QueryString["Source"] == null || Request.QueryString["List"] == null || Request.QueryString["ID"] == null)
                                Response.Redirect(SPContext.Current.Site.Url);
                            SPWeb web = SPContext.Current.Web;
                            web.AllowUnsafeUpdates = true;

                            SPList CurrentList = web.Lists.GetList(new Guid(Request.QueryString["List"]), true);
                            SPListItem SourceItem = CurrentList.GetItemById(int.Parse(Request.QueryString["ID"]));
                            SPListItem newDestItem;

                            if (SourceItem.File == null)
                            {
                                newDestItem = CurrentList.Items.Add();

                                foreach (string fileName in SourceItem.Attachments)
                                {
                                    SPFile file = SourceItem.ParentList.ParentWeb.GetFile(SourceItem.Attachments.UrlPrefix + fileName);
                                    byte[] imageData = file.OpenBinary();
                                    newDestItem.Attachments.Add(fileName, imageData);
                                }

                                foreach (SPField field in SourceItem.Fields)
                                    if ((!field.ReadOnlyField) && (field.InternalName != "Attachments"))
                                        newDestItem[field.InternalName] = SourceItem[field.InternalName];

                                newDestItem["Title"] = "Copy of " + newDestItem["Title"];
                            }
                            else
                            {
                                SPFolder currentFolder = SourceItem.Folder == null ? CurrentList.RootFolder : SourceItem.Folder;
                                SPFile newFile = currentFolder.Files.Add(Path.GetFileNameWithoutExtension(SourceItem.File.Name) + DateTime.Now.ToString("yyyy-MM-dd_H-mm-ss") + Path.GetExtension(SourceItem.File.Name), SourceItem.File.OpenBinary());
                                currentFolder.Update();
                                newDestItem = newFile.Item;

                                foreach (SPField field in SourceItem.Fields)
                                    if ((!field.ReadOnlyField) && (field.InternalName != "Attachments") && (field.InternalName != "FileLeafRef") && (field.InternalName != "Title"))
                                        newDestItem[field.InternalName] = SourceItem[field.InternalName];
                            }

                            newDestItem.Update();
                            Hashtable tbl = SourceItem.Properties;
                            foreach (DictionaryEntry item in tbl)
                            {
                                newDestItem.Properties[item.Key] = SourceItem.Properties[item.Key];
                            }
                            newDestItem.Update();
                            web.AllowUnsafeUpdates = false;

                            //ClientScript.RegisterClientScriptBlock(this.GetType(), "NavigateToEditPage", "<script language='javascript' type='text/javascript'> SP.UI.ModalDialog.commonModalDialogClose('OK', '" + url + "');</script>");                           
                        }
                    }
                    catch (Exception ex)
                    {
                        CCIUtility.LogError(ex.Message, "CCI.Infrastructure.DuplicateItemPage");
                    }              
      
                    base.OnLoad(e);
                    longOperation.End(SPEncode.UrlDecodeAsUrl(Request.QueryString["Source"]));
                }
            );
            
        }
    }
}
