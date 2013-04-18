using System;
using System.Security.Permissions;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Security;
using Microsoft.SharePoint.Utilities;
using Microsoft.SharePoint.Workflow;
using System.Reflection;
using AIA.Intranet.Common.Helpers;
using System.Collections.Generic;
using AIA.Intranet.Model.Entities;
using AIA.Intranet.Model;
using AIA.Intranet.Common.Utilities.Camlex;
using AIA.Intranet.Common.Utilities;
using AIA.Intranet.Model.Infrastructure;
using AIA.Intranet.Resources;
using AIA.Intranet.Common.Extensions;

namespace AIA.Intranet.Infrastructure.Receivers.NewsEventReceiver
{
    /// <summary>
    /// List Events
    /// </summary>
    public class NewsEventReceiver : SPListEventReceiver
    {
       /// <summary>
       /// A list is being added.
       /// </summary>
       public override void ListAdding(SPListEventProperties properties)
       {
           base.ListAdding(properties);
       }

       /// <summary>
       /// A list is being deleted.
       /// </summary>
       public override void ListDeleting(SPListEventProperties properties)
       {
           base.ListDeleting(properties);
       }

       /// <summary>
       /// A list was added.
       /// </summary>
       public override void ListAdded(SPListEventProperties properties)
       {
           base.ListAdded(properties);

           if (properties.TemplateId.ToString() == ListTemplateIds.NEWS_TEMPLATE_ID)
           {
               SPSecurity.RunWithElevatedPrivileges(delegate()
               {
                   using (SPSite site = new SPSite(properties.SiteId))
                   {
                       using (SPWeb web = site.OpenWeb(properties.Web.ID))
                       {
                           try
                           {
                               EventFiringEnabled = false;
                               web.AllowUnsafeUpdates = true;

                               var list = web.Lists[properties.ListId];

                               // create new view with custom webpart
                               CreateNewsListView(web, list);

                               // create new Display form with custom webpart
                               CreateDisplayForm(web, list);

                               // add webpart to News's homepage
                               CreateWebpartInNewsHomepage(web, list);
                           }
                           catch (Exception ex)
                           {
                               Utility.LogError(ex.Message + ex.StackTrace, "News Event Receiver");
                           }
                           finally
                           {
                               web.AllowUnsafeUpdates = false;
                               EventFiringEnabled = true;
                           }
                       }
                   }
               });
           }
       }

       private void CreateNewsListView(SPWeb web, SPList list)
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

           view.Update();
           //list.Update();
       }

       private void CreateDisplayForm(SPWeb web, SPList list)
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
                Title = "Tin cùng chuyên mục",
                Webparts = new System.Collections.Generic.List<WebpartDefinition>() {
                        new DefaultWP(){
                            Index = 2,
                            ZoneId = "Main",
                            WebpartName = "OtherNewsListView.webpart",
                            Properties = new System.Collections.Generic.List<Property>(){
                                new Property(){
                                    Name = "Title",
                                    Value = "Tin cùng chuyên mục"
                                }
                            }
                        }
                    }
                }
            });

           dispForm.Update();
           //list.Update();
       }

       private void CreateWebpartInNewsHomepage(SPWeb web, SPList list)
       {
           string pageUrl = Constants.NEWS_HOME_PAGE;
           string zoneId = "Header";

           int latestIdx = WebPartHelper.GetLatestWebPartIndex(web, pageUrl, zoneId);

           WebPartHelper.ProvisionWebpart(web, new WebpartPageDefinitionCollection()
            {
                new WebpartPageDefinition() {
                PageUrl = pageUrl,
                Title = list.Title,
                Webparts = new System.Collections.Generic.List<WebpartDefinition>() {
                        new DefaultWP(){
                            AllowDuplicate = true,
                            Index = latestIdx + 1,
                            ZoneId = zoneId,
                            Title = list.Title,
                            WebpartName = "ViewNewsCategoryWebPart.webpart",
                            Properties = new System.Collections.Generic.List<Property>(){
                                new Property(){
                                    Name = "WebID",
                                    Value = web.ID.ToString(),
                                    Type = "string"
                                },
                                new Property(){
                                    Name = "ListID",
                                    Value = list.ID.ToString(),
                                    Type = "string"
                                },
                                new Property(){
                                    Name = "Title",
                                    Value = list.Title
                                },
                                new Property(){
                                    Name = "TitleUrl",
                                    Value = list.DefaultViewUrl
                                },
                                new Property(){
                                    Name = "Description",
                                    Value = ""
                                }
                            }
                        }
                    }
                }
            });
       }


       /// <summary>
       /// A list was deleted.
       /// </summary>
       public override void ListDeleted(SPListEventProperties properties)
       {
           base.ListDeleted(properties);
       }


    }
}
