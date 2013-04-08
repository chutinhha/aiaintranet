using System;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Security;
using System.Reflection;
using AIA.Intranet.Common.Helpers;
using AIA.Intranet.Model.Infrastructure;
using System.Collections.Generic;
using AIA.Intranet.Common.Extensions;
using AIA.Intranet.Infrastructure.Resources;
using AIA.Intranet.Common.Utilities;
using AIA.Intranet.Model;
using System.Linq;

namespace AIA.Intranet.Infrastructure.Features.AIA.Intranet.Infrastructure.News
{
    /// <summary>
    /// This class handles events raised during feature activation, deactivation, installation, uninstallation, and upgrade.
    /// </summary>
    /// <remarks>
    /// The GUID attached to this class may be used during packaging and should not be modified.
    /// </remarks>

    [Guid("009541f9-7808-49c9-990e-59a5d6e3e96f")]
    public class AIAIntranetInfrastructureEventReceiver : SPFeatureReceiver
    {
        // Uncomment the method below to handle the event raised after a feature has been activated.

        public override void FeatureActivated(SPFeatureReceiverProperties properties)
        {
            var web = properties.Feature.Parent as SPWeb;
            //UpdateNewsCategoroySettings(web);
            //CreateFirstCategory(web);
            var listNews = CCIUtility.GetListFromURL(Constants.NEWS_LIST_URL, web);
            if (listNews != null)
            {
                CreateDetailNewsPage(web, listNews);
                UpdateImageField(web, listNews);
                CreateNewsListView(web, listNews);
            }
        }


        // Uncomment the method below to handle the event raised before a feature is deactivated.

        //public override void FeatureDeactivating(SPFeatureReceiverProperties properties)
        //{
        //}


        // Uncomment the method below to handle the event raised after a feature has been installed.

        //public override void FeatureInstalled(SPFeatureReceiverProperties properties)
        //{
        //}


        // Uncomment the method below to handle the event raised before a feature is uninstalled.

        //public override void FeatureUninstalling(SPFeatureReceiverProperties properties)
        //{
        //}

        // Uncomment the method below to handle the event raised when a feature is upgrading.

        //public override void FeatureUpgrading(SPFeatureReceiverProperties properties, string upgradeActionName, System.Collections.Generic.IDictionary<string, string> parameters)
        //{
        //}

        #region Private Functions
        private void UpdateNewsCategoroySettings(SPWeb web)
        {
            try
            {
                Assembly assembly = Assembly.GetExecutingAssembly();
                string xml = assembly.GetResourceTextFile("AIA.Intranet.Infrastructure.XMLCustomSettings.NewsCustomSettings.xml");

                var settings = SerializationHelper.DeserializeFromXml<List<CustomSettingDefinition>>(xml);

                web.UpdateCustomSetting(settings);

            }
            catch (Exception)
            {

            }
        }

        private void CreateFirstCategory(SPWeb web)
        {
            try
            {
                web.AllowUnsafeUpdates = true;

                SPList listNewsCategory = CCIUtility.GetListFromURL(Constants.NEWS_CATEGORY_LIST_URL, web);

                if (listNewsCategory != null)
                {
                    SPList listNews = CCIUtility.GetListFromURL(Constants.NEWS_DEFAULT_LISTS_URL, web);
                    if (listNews == null)
                    {
                        SPListItem newFolder = listNewsCategory.Items.Add(listNewsCategory.RootFolder.ServerRelativeUrl, SPFileSystemObjectType.Folder, null);
                        if (newFolder != null)
                        {
                            newFolder[SPBuiltInFieldId.Title] = Constants.NEWS_DEFAULT_CATEGORY;
                            newFolder.Update();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                CCIUtility.LogError("News Feature Event Receiver", ex.Message);
            }
            finally
            {
                web.AllowUnsafeUpdates = false;
            }
        }

        private void CreateDetailNewsPage(SPWeb web, SPList list)
        {
            var rootFolder = list.RootFolder;

            var dispFormUrl = string.Format("{0}/{1}/{2}.aspx", web.ServerRelativeUrl.TrimEnd('/'), rootFolder.Url, Constants.NEWS_DISPLAYPAGE);
            var dispForm = web.GetFile(dispFormUrl);
            if (dispForm != null && dispForm.Exists)
                dispForm.Delete();	// delete & recreate our display form

            // create a new DispForm
            dispForm = rootFolder.Files.Add(dispFormUrl, SPTemplateFileType.FormPage);

            WebPartHelper.ProvisionWebpart(web, new WebpartPageDefinitionCollection()
            {
                new WebpartPageDefinition() {
                PageUrl = dispForm.Url,
                Title = list.Title,
                Webparts = new System.Collections.Generic.List<WebpartDefinition>() {
                        new DefaultWP(){
                            Index = 0,
                            ZoneId = "Main",
                            WebpartName = "NewsDetailView.webpart",
                            Properties = new System.Collections.Generic.List<Property>(){
                                new Property(){
                                    Name = "Title",
                                    Value = list.Title
                                },
                                new Property(){
                                    Name="ChromeType",
                                    Type="chrometype",
                                    Value="2"
                                }
                            }
                        }
                    }
                },
                new WebpartPageDefinition() {
                PageUrl = dispForm.Url,
                Title = "Other news",
                Webparts = new System.Collections.Generic.List<WebpartDefinition>() {
                        new DefaultWP(){
                            Index = 2,
                            ZoneId = "Main",
                            WebpartName = "OtherNewsListView.webpart",
                            Properties = new System.Collections.Generic.List<Property>(){
                                new Property(){
                                    Name = "Title",
                                    Value = "Other news"
                                }
                            }
                        }
                    }
                }
            });

            dispForm.Update();
            //list.Update();
        }

        private void UpdateImageField(SPWeb web, SPList list)
        {
            try
            {
                var fields = list.Fields.Cast<SPField>().ToList();
                var imageFields = fields.Where(p => p.TypeAsString == Constants.IMAGE_FIELD_TYPE_NAME).ToList();
                foreach (var item in imageFields)
                {
                    //var currentWeb = item.ParentList.ParentWeb;
                    var newsImageLibrary = web.Lists[web.Folders["NewsImages"].ParentListId];

                    item.UpdateImageField(web, newsImageLibrary);
                }
            }
            catch (Exception ex)
            {
            }
        }

        private void CreateNewsListView(SPWeb web, SPList list)
        {
            try
            {
                // create new view with custom webpart
                SPViewCollection allviews = list.Views;
                string viewName = Constants.NEWS_LISTPAGE;

                System.Collections.Specialized.StringCollection viewFields = new System.Collections.Specialized.StringCollection();

                var view = allviews.Add(viewName, viewFields, string.Empty, 1, true, true);
                WebPartHelper.HideXsltListViewWebParts(web, view.Url);
                WebPartHelper.ProvisionWebpart(web, new WebpartPageDefinitionCollection()
                {
                    new WebpartPageDefinition() {
                    PageUrl = view.Url,
                    Title = list.Title,
                    Webparts = new System.Collections.Generic.List<WebpartDefinition>() {
                            new DefaultWP(){
                                Index = 0,
                                ZoneId = "Main",
                                WebpartName = "NewsListView.webpart"
                            }
                        }
                    }
                });
                WebPartHelper.MoveWebPart(web, view.Url, "NewsListView.webpart", "Main", 0);
                view.Title = "Approved Items";
                view.Update();
                //list.Update();
            }
            catch (Exception ex)
            {
                
                throw;
            }
        }

        #endregion Private Functions
    }
}
