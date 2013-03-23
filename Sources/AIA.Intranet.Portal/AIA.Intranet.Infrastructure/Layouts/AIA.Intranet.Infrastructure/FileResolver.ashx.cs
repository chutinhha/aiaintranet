using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System;
using System.Web;
using Microsoft.SharePoint;

namespace AIA.Intranet.Infrastructure.Layouts
{
    public class FileResolver : IHttpHandler
    {
        /// <summary>
        /// You will need to configure this handler in the web.config file of your 
        /// web and register it with IIS before being able to use it. For more information
        /// see the following link: http://go.microsoft.com/?linkid=8101007
        /// </summary>
        #region IHttpHandler Members

        public bool IsReusable
        {
            // Return false in case your Managed Handler cannot be reused for another request.
            // Usually this would be false in case you have some state information preserved per request.
            get { return true; }
        }

        public void ProcessRequest(HttpContext context)
        {
            string fileUrl = context.Request["File"];
            string targetUrl = string.Empty;
            using (SPSite site = new SPSite(fileUrl))
            using (SPWeb web = site.OpenWeb())
            {
                var file = web.GetFile(fileUrl);
                var listItem= file.Item;
                var list = file.Item.ParentList;
                //var view = list.Views.Cast<SPView>().FirstOrDefault(prop=>prop.Title == "View");
                targetUrl = string.Format("{0}/_layouts/AIA.Intranet.Infrastructure/PictureViewer.aspx", list.ParentWeb.Url);
                targetUrl = string.Format("{0}?List={1}&ID={2}", targetUrl, list.ID, listItem.ID);
                if (!String.IsNullOrEmpty(context.Request["IsDlg"])) targetUrl += "&IsDlg=1";


            }
            context.Response.Redirect(targetUrl);
        }

        #endregion
    }
}
