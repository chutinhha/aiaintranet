using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebPartPages;
using System.Globalization;
using System.Xml;
using System.Web.UI;
using SP = Microsoft.SharePoint.WebPartPages;
using AIA.Intranet.Model.Infrastructure;
using System.IO;
using AIA.Intranet.Common.Extensions;
using Microsoft.SharePoint.Utilities;

namespace AIA.Intranet.Common.Helpers
{
    public class WebPartHelper
    {
        /// <summary>
        /// Add the web part to page.
        /// </summary>
        /// <param name="web">The web.</param>
        /// <param name="pageUrl">The page URL.</param>
        /// <param name="webPartName">Name of the web part.</param>
        /// <param name="zoneID">The zone ID.</param>
        /// <param name="zoneIndex">Index of the zone.</param>
        /// <returns></returns>
        public static string AddWebPartToPage(SPWeb web, string pageUrl, string webPartName, string zoneID, int zoneIndex, bool isHidden)
        {
            using (SPLimitedWebPartManager webPartManager = web.GetLimitedWebPartManager(
                    pageUrl, System.Web.UI.WebControls.WebParts.PersonalizationScope.Shared))
            {
                using (System.Web.UI.WebControls.WebParts.WebPart webPart = CreateWebPart(web, webPartName, webPartManager, isHidden))
                {
                    bool isExists = false;
                    foreach (System.Web.UI.WebControls.WebParts.WebPart wp in webPartManager.WebParts)
                    {
                        if (wp.Title.Equals(webPartName.Replace(".webpart", "")))
                        {
                            isExists = true;
                            break;
                        }
                        else
                        {
                            isExists = false;
                        }
                    }
                    if (!isExists)
                    {
                        webPartManager.AddWebPart(webPart, zoneID, zoneIndex);
                    }

                    return webPart.ID;
                }
            }
        }

        /// <summary>
        /// Creates the web part.
        /// </summary>
        /// <param name="web">The web.</param>
        /// <param nme="webPartName">Name of the web part.</param>
        /// <param name="webPartManager">The web part manager.</param>
        /// <returns></returns>
        public static System.Web.UI.WebControls.WebParts.WebPart CreateWebPart(SPWeb web, string webPartName, SPLimitedWebPartManager webPartManager, bool isHidden)
        {
            SPQuery qry = new SPQuery();
            qry.Query = String.Format(CultureInfo.CurrentCulture,
                "<Where><Eq><FieldRef Name='FileLeafRef'/><Value Type='File'>{0}</Value></Eq></Where>",
                webPartName);
            SPList webPartGallery = null;

            if (web.ParentWeb == null)
            {
                webPartGallery = web.GetCatalog(
                   SPListTemplateType.WebPartCatalog);
            }
            else
            {
                webPartGallery = web.Site.RootWeb.GetCatalog(
                   SPListTemplateType.WebPartCatalog);
            }

            SPListItemCollection webParts = webPartGallery.GetItems(qry);

            XmlReader xmlReader = new XmlTextReader(webParts[0].File.OpenBinaryStream());
            string errorMsg;
            System.Web.UI.WebControls.WebParts.WebPart webPart = webPartManager.ImportWebPart(xmlReader, out errorMsg);
            webPart.Hidden = isHidden;

            return webPart;
        }

        public static void AddXsltViewWebPart(SPWeb web, SPList list, string pageUrl, string webPartName, string zoneID,
                   int zoneIndex, bool isHidden, string viewTitle)
        {
            using (SPLimitedWebPartManager webPartManager = web.GetLimitedWebPartManager(
                    pageUrl, System.Web.UI.WebControls.WebParts.PersonalizationScope.Shared))
            {
                bool isExists = false;
                foreach (System.Web.UI.WebControls.WebParts.WebPart wp in webPartManager.WebParts)
                {
                    if (wp.Title.Equals(webPartName))
                    {
                        isExists = true;
                        break;
                    }
                    else
                    {
                        isExists = false;
                    }
                }
                if (!isExists)
                {
                    XsltListViewWebPart webPart = new XsltListViewWebPart();
                    webPart.ListId = list.ID;
                    webPart.Title = webPartName;
                    webPart.ChromeType = System.Web.UI.WebControls.WebParts.PartChromeType.TitleAndBorder;
                    SPView view = list.Views[viewTitle];
                    webPart.ViewGuid = view.ID.ToString();
                    webPart.XmlDefinition = view.GetViewXml();
                    webPartManager.AddWebPart(webPart, zoneID, zoneIndex);
                }
            }
        }

        /// <summary>
        /// Update list id in a webpart
        /// </summary>
        /// <param name="web">Current web</param>
        /// <param name="pageUrl">Url of page contains web part that you want to change list id</param>
        /// <param name="webPartName">The name of web part that you want to change list id</param>
        /// <param name="list">Updated list</param>
        /// <param name="viewTitle">Updated list view title</param>
        public static void UpdateListId(SPWeb web, string pageUrl, string webPartName, SPList list, string viewTitle)
        {
            using (SPLimitedWebPartManager webPartManager = web.GetLimitedWebPartManager(
                    pageUrl, System.Web.UI.WebControls.WebParts.PersonalizationScope.Shared))
            {
                foreach (System.Web.UI.WebControls.WebParts.WebPart wp in webPartManager.WebParts)
                {
                    if (wp.Title.Equals(webPartName))
                    {
                        (wp as XsltListViewWebPart).ListId = list.ID;
                        wp.TitleUrl = list.RootFolder.Url;
                        SPView view = list.DefaultView;
                        (wp as XsltListViewWebPart).ViewGuid = view.ID.ToString();
                        (wp as XsltListViewWebPart).ViewId = int.Parse(view.BaseViewID);
                        (wp as XsltListViewWebPart).XmlDefinition = view.GetViewXml();

                        webPartManager.SaveChanges(wp);

                        break;
                    }
                }
            }
        }

        /// <summary>
        /// Find xslt list view webpart
        /// </summary>
        /// <param name="control">Control contains webpart</param>
        /// <returns></returns>
        public static SP.XsltListViewWebPart FindListViewWebPart(Control control)
        {
            SP.XsltListViewWebPart listview = null;
            if (control is SP.XsltListViewWebPart)
            {
                listview = control as SP.XsltListViewWebPart;
            }
            else
            {
                foreach (Control child in control.Controls)
                {
                    listview = FindListViewWebPart(child);
                    if (listview != null)
                    {
                        break;
                    }
                }
            }
            return listview;
        }

        public static void MoveWebPart(SPWeb web, string pageUrl, string webPartName, string zoneID, int zoneIndex)
        {
            try
            {
                string fullPageUrl = string.Format("{0}/{1}", web.Url.TrimEnd('/'), pageUrl.TrimStart('/'));
                

                SPLimitedWebPartManager mgr = web.GetLimitedWebPartManager(
                        fullPageUrl,
                        System.Web.UI.WebControls.WebParts.PersonalizationScope.Shared);
                foreach (System.Web.UI.WebControls.WebParts.WebPart wp in mgr.WebParts)
                {
                    if (wp.Title.Equals(webPartName.Replace(".webpart", "")))
                    {
                        mgr.MoveWebPart(wp, zoneID, zoneIndex);
                        mgr.SaveChanges(wp);
                        break;
                    }
                }
            }
            catch (Exception) { }
        }

        public static void HideXsltListViewWebParts(SPWeb web, string pageUrl)
        {
            try
            {
                string fullPageUrl = string.Format("{0}/{1}", web.Url.TrimEnd('/'), pageUrl.TrimStart('/'));

                SPLimitedWebPartManager mgr = web.GetLimitedWebPartManager(
                        fullPageUrl,
                        System.Web.UI.WebControls.WebParts.PersonalizationScope.Shared);
                foreach (System.Web.UI.WebControls.WebParts.WebPart wp in mgr.WebParts)
                {
                    if (wp is SP.XsltListViewWebPart)
                    {
                        wp.Hidden = true;
                        mgr.SaveChanges(wp);
                    }
                }
            }
            catch(Exception){}
        }

        public static int GetLatestWebPartIndex(SPWeb web, string pageUrl)
        { 
            return GetLatestWebPartIndex(web, pageUrl, string.Empty);
        }

        public static int GetLatestWebPartIndex(SPWeb web, string pageUrl, string zoneId)
        {
            int idx = 0;

            try
            {
                string fullPageUrl = string.Format("{0}/{1}", web.Url.TrimEnd('/'), pageUrl.TrimStart('/'));

                SPLimitedWebPartManager mgr = web.GetLimitedWebPartManager(
                        fullPageUrl,
                        System.Web.UI.WebControls.WebParts.PersonalizationScope.Shared);

                foreach (System.Web.UI.WebControls.WebParts.WebPart wp in mgr.WebParts)
                {
                    if (string.IsNullOrEmpty(zoneId) || (!string.IsNullOrEmpty(zoneId) && mgr.GetZoneID(wp) == zoneId))
                    {
                        if (idx < wp.ZoneIndex) idx = wp.ZoneIndex;
                    }
                }
            }
            catch (Exception) { }

            return idx;
        }

        private static bool IsAbsoluteUrl(string url)
        {
            Uri result;
            return Uri.TryCreate(url, UriKind.Absolute, out result);
        }

        public static void ProvisionWebpart(SPWeb web, WebpartPageDefinitionCollection collection)
        {
            foreach (var item in collection)
            {
                try
                {

                    string pageUrl = item.PageUrl;
                    if (!string.IsNullOrEmpty(item.RootPath) && !string.IsNullOrEmpty(item.FileName))
                    {
                        if (item.RootPath.EndsWith(".aspx"))
                        {
                            item.PageUrl = item.RootPath;
                            pageUrl = item.RootPath;
                        }

                        else
                        {
                            pageUrl = string.Format("{0}/{1}", item.RootPath.TrimEnd('/'), SPEncode.UrlEncode(item.FileName.TrimStart('/')));
                            item.PageUrl = pageUrl;
                        }
                    }

                    if (!IsAbsoluteUrl(pageUrl))
                    {
                        pageUrl = string.Format("{0}/{1}", web.Url.TrimEnd('/'), pageUrl.TrimStart('/'));
                    }

                    web.EnsureWebpartPage(item.PageUrl, item.Title, item.Overwrite, item.UseFormFolder);
                    
                using (SPLimitedWebPartManager webPartManager = web.GetLimitedWebPartManager(
                    pageUrl, System.Web.UI.WebControls.WebParts.PersonalizationScope.Shared))
                {

                    foreach (var wp in item.Webparts)
                    {
                        try
                        {
                            using (var concerateWP = wp.CreateWebPart(web, webPartManager))
                            {
                                
                                if (concerateWP == null) continue;
                                try
                                {
                                    if (!wp.AllowDuplicate && webPartManager.WebParts.Cast<System.Web.UI.WebControls.WebParts.WebPart>().Any(p => p.Title == concerateWP.Title))
                                    {
                                        continue;
                                    }
                                }
                                catch (Exception)
                                {
                                    
                                    //throw;
                                }
                               

                                webPartManager.AddWebPart(concerateWP, wp.ZoneId, wp.Index ==0 ?webPartManager.WebParts.Count+1:  wp.Index);

                                CreateDefaultWebPart(web, webPartManager, wp, concerateWP);
                            }
                        }
                        catch (Exception ex)
                        {
                            
                            
                        }

                    }
                }
                }
                catch (Exception ex)
                {
                    
                }
            }
        }


        //public virtual void ExportWebPart(WebPart webPart, XmlWriter writer)
        //{
        //    if (webPart == null)
        //    {
        //        throw new ArgumentNullException("webPart");
        //    }
        //    if (!this.Controls.Contains(webPart))
        //    {
        //        throw new ArgumentException(SR.GetString("UnknownWebPart"), "webPart");
        //    }
        //    if (writer == null)
        //    {
        //        throw new ArgumentNullException("writer");
        //    }
        //    if (webPart.ExportMode == WebPartExportMode.None)
        //    {
        //        throw new ArgumentException(SR.GetString("WebPartManager_PartNotExportable"), "webPart");
        //    }
        //    bool arg_79_0 = (webPart.ExportMode != WebPartExportMode.NonSensitiveData) ? false : (this.Personalization.Scope != PersonalizationScope.Shared);
        //    bool flag = arg_79_0;
        //    writer.WriteStartElement("webParts");
        //    writer.WriteStartElement("webPart");
        //    writer.WriteAttributeString("xmlns", "http://schemas.microsoft.com/WebPart/v3");
        //    writer.WriteStartElement("metaData");
        //    writer.WriteStartElement("type");
        //    Control control = webPart.ToControl();
        //    UserControl userControl = control as UserControl;
        //    if (userControl == null)
        //    {
        //        writer.WriteAttributeString("name", WebPartUtil.SerializeType(control.GetType()));
        //    }
        //    else
        //    {
        //        writer.WriteAttributeString("src", userControl.AppRelativeVirtualPath);
        //    }
        //    writer.WriteEndElement();
        //    writer.WriteElementString("importErrorMessage", webPart.ImportErrorMessage);
        //    writer.WriteEndElement();
        //    writer.WriteStartElement("data");
        //    IDictionary personalizablePropertyValues = PersonalizableAttribute.GetPersonalizablePropertyValues(webPart, PersonalizationScope.Shared, flag);
        //    writer.WriteStartElement("properties");
        //    if (!(webPart is GenericWebPart))
        //    {
        //        this.ExportIPersonalizable(writer, webPart, flag);
        //        this.ExportToWriter(personalizablePropertyValues, writer);
        //    }
        //    else
        //    {
        //        this.ExportIPersonalizable(writer, control, flag);
        //        IDictionary personalizablePropertyValues2 = PersonalizableAttribute.GetPersonalizablePropertyValues(control, PersonalizationScope.Shared, flag);
        //        this.ExportToWriter(personalizablePropertyValues2, writer);
        //        writer.WriteEndElement();
        //        writer.WriteStartElement("genericWebPartProperties");
        //        this.ExportIPersonalizable(writer, webPart, flag);
        //        this.ExportToWriter(personalizablePropertyValues, writer);
        //    }
        //    writer.WriteEndElement();
        //    writer.WriteEndElement();
        //    writer.WriteEndElement();
        //    writer.WriteEndElement();
        //}

        private static void CreateDefaultWebPart(SPWeb web, SPLimitedWebPartManager webPartManager, WebpartDefinition wp, System.Web.UI.WebControls.WebParts.WebPart concerateWP)
        {
            //TODO : find a solution to create default webpart late
            return;
            if (wp is XSLTListViewWP)
            {
                XSLTListViewWP xstlWP = wp as XSLTListViewWP;
                if (xstlWP.CreateDefaultWP)
                {
                    using (MemoryStream ms = new MemoryStream())
                    {


                        XmlWriter writer = new XmlTextWriter(ms, Encoding.UTF8);
                        concerateWP.ExportMode = System.Web.UI.WebControls.WebParts.WebPartExportMode.All;
                        
                        webPartManager.ExportWebPart(concerateWP, writer);


                        var webPartGallery = web.GetCatalog(SPListTemplateType.WebPartCatalog);

                        //var fileStream
                        SPFile spfile = webPartGallery.RootFolder.Files.Add(wp.Title + ".webpart", ms.GetBuffer(), true);

                        // Commit 
                        webPartGallery.RootFolder.Update();
                        webPartGallery.Update();

                    }


                }
            }
        }
    }
}
