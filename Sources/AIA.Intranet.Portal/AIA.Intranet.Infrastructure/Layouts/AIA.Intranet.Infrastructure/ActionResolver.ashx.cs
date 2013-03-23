using System;
using System.Web;
using System.Runtime.InteropServices;
using Microsoft.SharePoint;
using AIA.Intranet.Model;
using AIA.Intranet.Common.Extensions;

namespace AIA.Intranet.Infrastructure.Layouts
{
    public class ActionReslover : IHttpHandler
    {
        enum ActionLinkTypes
        {
            Discussion,
            View,
            Edit
        }

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
            string at = context.Request["type"];

            ActionLinkTypes type = (ActionLinkTypes)Enum.Parse(typeof(ActionLinkTypes), at);

            
            
            SPSecurity.RunWithElevatedPrivileges(delegate
            {
                string url = context.Request["item"];
                if (url.StartsWith("/"))
                {
                    string root = string.Format("{0}://{1}{2}{3}",
                                   context.Request.Url.Scheme,
                                   context.Request.Url.Host,
                                   context.Request.Url.Port == 80
                                       ? string.Empty
                                       : ":" + context.Request.Url.Port,
                                   context.Request.ApplicationPath);
                    url = root + url;
                }


                using (var site = new SPSite(url))
                {
                    using (SPWeb web = site.OpenWeb(site.RootWeb.ID))
                    {
                        var list = web.Lists.GetList(new Guid(context.Request["List"]), true);

                        int id = Convert.ToInt32(context.Request["ID"]);
                        
                        var item  = list.GetItemById(id);
                        switch (type)
                        {
                            case ActionLinkTypes.Discussion:
                                string discussionUrl = string.Format("{0}/_layouts/AIA.Intranet.Infrastructure/ItemDiscussionResolver.aspx?List={1}&Item={2}", web.Url, list.ID, id);

                                context.Response.Redirect(discussionUrl);
                                break;
                            case ActionLinkTypes.View:
                                string viewUrl = item.DisplayFormUrl();
                                context.Response.Redirect(viewUrl);
                                break;
                            case ActionLinkTypes.Edit:
                                break;
                            default:
                                break;
                        }


                    }
                }

            });
        }
        #endregion
    }
}
