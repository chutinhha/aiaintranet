using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using Microsoft.SharePoint;
using System.Globalization;
using System.Xml;
using System.Xml.Serialization;
using System.Web.UI.WebControls.WebParts;

namespace AIA.Intranet.Model.Infrastructure
{
    [Serializable]
    [XmlRoot("Page")]
    
    public class WebpartPageDefinition
    {
        public string RootPath { get; set; }
        public string FileName { get; set; }

        public string PageUrl { get; set; }
        public bool Overwrite { get; set; }
        [XmlElement("Webparts")]
        public List<WebpartDefinition> Webparts { get; set; }
        public WebpartPageDefinition() {
            Webparts = new List<WebpartDefinition>();
    }


        public string Title { get; set; }

        

        public bool UseFormFolder { get; set; }
    }

    public class WebpartPageDefinitionCollection : List<WebpartPageDefinition>
    {

    }

    [Serializable]
    [XmlInclude(typeof(XSLTListViewWP))]
    [XmlInclude(typeof(DefaultWP))]
    [XmlInclude(typeof(ListViewWP))]
    [XmlInclude(typeof(ContenEditorWP))]
    [XmlRoot("Webpart")]
    public class WebpartDefinition
    {
        public string Title { get; set; }
        public string ZoneId { get; set; }
        public int Index { get; set; }
        public bool Hidden { get; set; }
        public bool AllowDuplicate { get; set; }
        [XmlArray("Properties")]
        [XmlArrayItem("Add")]
        public List<Property> Properties{ get; set; }
        public virtual WebPart CreateWebPart(SPWeb web, Microsoft.SharePoint.WebPartPages.SPLimitedWebPartManager webPartManager)
        {
            return default(WebPart);
        }

        //public void SetProperty(WebPart webpart, string property, object value)
        //{
        //    try
        //    {
        //        var type = webpart.GetType();
        //    var pi = type.GetProperty(property);
        //    if (pi != null)
        //    {
        //        pi.SetValue(webpart, value, null);
        //    }
        //    }
        //    catch (Exception)
        //    {
        //    }
        //}

        //public void SetProperty(WebPart webpart, string property, object value)
        //{

        //    var type = webpart.GetType();
        //    var pi = type.GetProperty(property);
        //    if (pi != null)
        //    {
        //        Object objValue = Convert.ChangeType(value, pi.PropertyType);

        //        pi.SetValue(webpart, objValue, null);
        //    }
        //}

        public void SetProperty(WebPart webpart, string property, object value)
        {

            var type = webpart.GetType();
            System.Reflection.PropertyInfo pi = type.GetProperty(property);
            if (pi != null)
            {

                Object objValue = null;

                if (System.Reflection.PropertyInfo.Equals(pi.PropertyType.ToString(), "System.Web.UI.WebControls.WebParts.PartChromeType"))
                {
                    objValue = System.Web.UI.WebControls.WebParts.PartChromeType.None;
                }
                else
                {
                    objValue = Convert.ChangeType(value, pi.PropertyType);
                }

                pi.SetValue(webpart, objValue, null);
            }
        }

        protected void UpdateProperties(WebPart webpart)
        {
            foreach (var item in Properties)
            {
                this.SetProperty(webpart,item.Name, item.Value);
            }
        }
        public WebpartDefinition()
        {
            Properties = new List<Property>();
        }

    }
    
    [Serializable]
    public class Property
    {
        [XmlAttribute]
        public string Name { get; set; }
        [XmlAttribute]
        public string Value { get; set; }
        [XmlAttribute]
        public string Type { get; set; }
    }
    [Serializable]

    public class XSLTListViewWP : WebpartDefinition
    {
        public string SiteUrl { get; set; }
        public string ListName { get; set; }
        public string ListUrl { get; set; }
        public string ViewName { get; set; }
        public bool CreateDefaultWP { get; set; }
        public override WebPart CreateWebPart(SPWeb web, Microsoft.SharePoint.WebPartPages.SPLimitedWebPartManager webPartManager)
        {
            SPList list = GetList(web, ListUrl, ListName);
            
            if (list == null) 
                return null;
            
            Microsoft.SharePoint.WebPartPages.XsltListViewWebPart webPart = new Microsoft.SharePoint.WebPartPages.XsltListViewWebPart();
            webPart.ListId = list.ID;
            webPart.Title = Title;
            webPart.WebId = list.ParentWeb.ID;

            
            //webPart.ChromeType = System.Web.UI.WebControls.WebParts.PartChromeType.TitleAndBorder;
            SPView view = GetView(list, ViewName);
            webPart.ViewGuid = view.ID.ToString();
            webPart.XmlDefinition = view.GetViewXml();

            webPart.ExportMode = WebPartExportMode.All;
            base.UpdateProperties(webPart);
            return webPart;
        }


        private SPView GetView(SPList list, string ViewName)
        {
            if (string.IsNullOrEmpty(ViewName)) return list.DefaultView;

            return list.Views[ViewName];
        }

        private SPList GetList(SPWeb web, string listUrl, string listName)
        {
            SPList currentList = null;
                if (!string.IsNullOrEmpty(listName))
                {
                    currentList = web.Lists.Cast<SPList>().FirstOrDefault(p => p.Title == listName);
                }

                if (currentList == null && !string.IsNullOrEmpty(listUrl))
                {
                    string url = string.Format("{0}/{1}", web.ServerRelativeUrl.TrimEnd('/'), listUrl.TrimStart('/'));
                    try
                    {
                        currentList = web.GetList(url);
                    }
                    catch
                    {
                        currentList = null;
                    }
                    
                }

                if (currentList == null)
                {
                    string url = string.Format("{0}/{1}", web.Site.RootWeb.ServerRelativeUrl.TrimEnd('/'), listUrl.TrimStart('/'));
                    currentList = web.Site.RootWeb.GetList(url);
                }

                return currentList;
            
        }

        private SPList GetList(string siteUrl, string listUrl, string listName)
        {
            SPList currentList = null;
            try
            {
                using (SPSite site = new SPSite(siteUrl))
                {
                    using (SPWeb web = site.OpenWeb())
                    {
                        if (!string.IsNullOrEmpty(listName))
                        {
                            currentList = web.Lists.Cast<SPList>().FirstOrDefault(p => p.Title == listName);
                        }

                        if (currentList == null && !string.IsNullOrEmpty(listUrl))
                        {
                            listUrl = string.Format("{0}/{1}", web.ServerRelativeUrl.TrimEnd('/'), listUrl.TrimStart('/'));
                            currentList = web.GetList(listUrl);
                        }
                        return currentList;
                    }
                }
            }
            catch
            {
                return null;
            }
        }

    }       
    
    [Serializable]
    public class DefaultWP : WebpartDefinition
    {
        public string WebpartName { get; set; }
        public override WebPart CreateWebPart(SPWeb web,Microsoft.SharePoint.WebPartPages.SPLimitedWebPartManager webPartManager)
        {
            SPQuery qry = new SPQuery();
            qry.Query = String.Format(CultureInfo.CurrentCulture,
                "<Where><Eq><FieldRef Name='FileLeafRef'/><Value Type='File'>{0}</Value></Eq></Where>",
                this.WebpartName);
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
            WebPart webPart = webPartManager.ImportWebPart(xmlReader, out errorMsg);
            //webPart.Hidden = isHidden;
            base.UpdateProperties(webPart);
            return webPart;
        }

    }

    [Serializable]
    public class ListViewWP : WebpartDefinition
    {
        public string SiteUrl { get; set; }
        public string ListName { get; set; }
        public string ListUrl { get; set; }
        public string ViewName { get; set; }
        public bool CreateDefaultWP { get; set; }
        public override WebPart CreateWebPart(SPWeb web, Microsoft.SharePoint.WebPartPages.SPLimitedWebPartManager webPartManager)
        {
            SPList list = GetList(web, ListUrl, ListName);

            if (list == null)
                return null;

            Microsoft.SharePoint.WebPartPages.ListViewWebPart webPart = new Microsoft.SharePoint.WebPartPages.ListViewWebPart();
            webPart.ListId = list.ID;
            webPart.Title = Title;
            webPart.WebId = list.ParentWeb.ID;


            //webPart.ChromeType = System.Web.UI.WebControls.WebParts.PartChromeType.TitleAndBorder;
            SPView view = GetView(list, ViewName);
            webPart.ViewGuid = view.ID.ToString();
            webPart.ListViewXml = view.GetViewXml();

            webPart.ExportMode = WebPartExportMode.All;
            base.UpdateProperties(webPart);
            return webPart;
        }


        private SPView GetView(SPList list, string ViewName)
        {
            if (string.IsNullOrEmpty(ViewName)) return list.DefaultView;

            return list.Views[ViewName];
        }

        private SPList GetList(SPWeb web, string listUrl, string listName)
        {
            SPList currentList = null;
            if (!string.IsNullOrEmpty(listName))
            {
                currentList = web.Lists.Cast<SPList>().FirstOrDefault(p => p.Title == listName);
            }

            if (currentList == null && !string.IsNullOrEmpty(listUrl))
            {
                string url = string.Format("{0}/{1}", web.ServerRelativeUrl.TrimEnd('/'), listUrl.TrimStart('/'));
                try
                {
                    currentList = web.GetList(url);
                }
                catch
                {
                    currentList = null;
                }

            }

            if (currentList == null)
            {
                string url = string.Format("{0}/{1}", web.Site.RootWeb.ServerRelativeUrl.TrimEnd('/'), listUrl.TrimStart('/'));
                currentList = web.Site.RootWeb.GetList(url);
            }

            return currentList;

        }

        private SPList GetList(string siteUrl, string listUrl, string listName)
        {
            SPList currentList = null;
            try
            {
                using (SPSite site = new SPSite(siteUrl))
                {
                    using (SPWeb web = site.OpenWeb())
                    {
                        if (!string.IsNullOrEmpty(listName))
                        {
                            currentList = web.Lists.Cast<SPList>().FirstOrDefault(p => p.Title == listName);
                        }

                        if (currentList == null && !string.IsNullOrEmpty(listUrl))
                        {
                            listUrl = string.Format("{0}/{1}", web.ServerRelativeUrl.TrimEnd('/'), listUrl.TrimStart('/'));
                            currentList = web.GetList(listUrl);
                        }
                        return currentList;
                    }
                }
            }
            catch
            {
                return null;
            }
        }

    }


    [Serializable]
    public class ContenEditorWP : WebpartDefinition
    {
        public string SiteUrl { get; set; }
        public string ListName { get; set; }
        public string ListUrl { get; set; }
        public string ViewName { get; set; }
        public bool CreateDefaultWP { get; set; }
        public string Content { get; set; }
        public override WebPart CreateWebPart(SPWeb web, Microsoft.SharePoint.WebPartPages.SPLimitedWebPartManager webPartManager)
        {           
            Microsoft.SharePoint.WebPartPages.ContentEditorWebPart webPart = new Microsoft.SharePoint.WebPartPages.ContentEditorWebPart();
            // Create an XmlElement to hold the value of the Content property.
            XmlDocument xmlDoc = new XmlDocument();
            XmlElement xmlElement = xmlDoc.CreateElement("MyElement");
            xmlElement.InnerText = Content;
            // Set the Content property to the XmlElement.
            webPart.Content = xmlElement;
            base.UpdateProperties(webPart);
            return webPart;
        }


        private SPView GetView(SPList list, string ViewName)
        {
            if (string.IsNullOrEmpty(ViewName)) return list.DefaultView;

            return list.Views[ViewName];
        }

        private SPList GetList(SPWeb web, string listUrl, string listName)
        {
            SPList currentList = null;
            if (!string.IsNullOrEmpty(listName))
            {
                currentList = web.Lists.Cast<SPList>().FirstOrDefault(p => p.Title == listName);
            }

            if (currentList == null && !string.IsNullOrEmpty(listUrl))
            {
                string url = string.Format("{0}/{1}", web.ServerRelativeUrl.TrimEnd('/'), listUrl.TrimStart('/'));
                try
                {
                    currentList = web.GetList(url);
                }
                catch
                {
                    currentList = null;
                }

            }

            if (currentList == null)
            {
                string url = string.Format("{0}/{1}", web.Site.RootWeb.ServerRelativeUrl.TrimEnd('/'), listUrl.TrimStart('/'));
                currentList = web.Site.RootWeb.GetList(url);
            }

            return currentList;

        }

        private SPList GetList(string siteUrl, string listUrl, string listName)
        {
            SPList currentList = null;
            try
            {
                using (SPSite site = new SPSite(siteUrl))
                {
                    using (SPWeb web = site.OpenWeb())
                    {
                        if (!string.IsNullOrEmpty(listName))
                        {
                            currentList = web.Lists.Cast<SPList>().FirstOrDefault(p => p.Title == listName);
                        }

                        if (currentList == null && !string.IsNullOrEmpty(listUrl))
                        {
                            listUrl = string.Format("{0}/{1}", web.ServerRelativeUrl.TrimEnd('/'), listUrl.TrimStart('/'));
                            currentList = web.GetList(listUrl);
                        }
                        return currentList;
                    }
                }
            }
            catch
            {
                return null;
            }
        }

    }       
}
