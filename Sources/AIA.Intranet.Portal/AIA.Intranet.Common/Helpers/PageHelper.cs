using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using Microsoft.SharePoint;
using Microsoft.SharePoint.WebPartPages;
using System.Globalization;
using System.Xml;

namespace AIA.Intranet.Common.Helpers
{
    public class PageHelper
    {
        public static string AddWebPartToPage(SPWeb web, string pageUrl, string webPartName, string zoneID, int zoneIndex)
        {
            try
            {
                web.AllowUnsafeUpdates = false;
                using (SPLimitedWebPartManager webPartManager = web.GetLimitedWebPartManager(pageUrl, System.Web.UI.WebControls.WebParts.PersonalizationScope.Shared))
                {
                    using (var webPart = CreateWebPart(web, webPartName, webPartManager))
                    {
                        if (webPart != null)
                        {
                            webPartManager.AddWebPart(webPart, zoneID, zoneIndex);
                            return webPart.ID;
                        }
                    }
                }

            }
            catch (Exception ex)
            {

                // throw;
            }
            finally
            {
                web.AllowUnsafeUpdates = false;
            }
           
            return string.Empty;
        }

        public static System.Web.UI.WebControls.WebParts.WebPart CreateWebPart(SPWeb web, string webPartName, SPLimitedWebPartManager webPartManager)
        {
            SPQuery qry = new SPQuery();
            qry.Query = String.Format(CultureInfo.CurrentCulture,
                "<Where><Eq><FieldRef Name='FileLeafRef'/><Value Type='File'>{0}</Value></Eq></Where>",
                webPartName);

            SPList webPartGallery = null;

            if (null == web.ParentWeb)
            {
                webPartGallery = web.GetCatalog(SPListTemplateType.WebPartCatalog);
            }
            else
            {
                webPartGallery = web.Site.RootWeb.GetCatalog(SPListTemplateType.WebPartCatalog);
            }

            SPListItemCollection webParts = webPartGallery.GetItems(qry);

            if (webParts == null || webParts.Count == 0) return null;

            XmlReader xmlReader = new XmlTextReader(webParts[0].File.OpenBinaryStream());
            string errorMsg;
            var webPart = webPartManager.ImportWebPart(xmlReader, out errorMsg);

            return webPart;
        }
    }
}
