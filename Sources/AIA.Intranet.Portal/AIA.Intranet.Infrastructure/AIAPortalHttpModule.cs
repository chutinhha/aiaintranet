using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using Microsoft.SharePoint.Utilities;
using System.Collections;

namespace AIA.Intranet.Infrastructure
{
    public class AIAPortalHttpModule : IHttpModule
    {
        public void Init(HttpApplication context)
        {
            context.PreRequestHandlerExecute += new EventHandler(context_PreRequestHandlerExecute);
        }
        void context_PreRequestHandlerExecute(object sender, EventArgs e)
        {
            Page page = HttpContext.Current.CurrentHandler as Page;
            if (page != null)
            {
                // register handler for PreInit event
                page.PreInit += new EventHandler(page_PreInit);
                page.PreRender += new EventHandler(page_PreRender);
            }
        }

        void page_PreRender(object sender, EventArgs e)
        {

        }
        
        void page_PreInit(object sender, EventArgs e)
        {
            Page page = sender as Page;
            
            if (page != null)
            {
                var url =  page.Request.Url.ToString().ToLower();
                if (url.Contains("_vti_bin") || (url.Contains("_layouts") && !url.Contains("/_layouts/searchresults.aspx")) || url.Contains("_catalogs")) return;

                string masterpage = SPContext.Current.Site.ServerRelativeUrl.TrimEnd('/') + "/_catalogs/masterpage/AIAPortal.master";

                var file = SPContext.Current.Site.RootWeb.GetFile(masterpage);

                if (file.Exists)
                {
                    page.MasterPageFile = masterpage;
                }
            }
        }
        public void Dispose()
        {
        }
    }
}
