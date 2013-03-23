using System;
using System.Web;
using System.Runtime.InteropServices;
using Microsoft.SharePoint;
using AIA.Intranet.Model;

namespace AIA.Intranet.Infrastructure.Pages
{
    [Guid("18b80488-f1ca-4fbd-b441-a246a9eca448")]
    public partial class DisplayAttachments : IHttpHandler
    {
        #region IHttpHandler Members

        /// <summary>
        /// Gets a value indicating whether another request can use the <see cref="T:System.Web.IHttpHandler"/> instance.
        /// </summary>
        /// <value></value>
        /// <returns>true if the <see cref="T:System.Web.IHttpHandler"/> instance is reusable; otherwise, false.
        /// </returns>
        public bool IsReusable
        {
            get { return false; }
        }

        /// <summary>
        /// Enables processing of HTTP Web requests by a custom HttpHandler that implements the <see cref="T:System.Web.IHttpHandler"/> interface.
        /// </summary>
        /// <param name="context">An <see cref="T:System.Web.HttpContext"/> object that provides references to the intrinsic server objects (for example, Request, Response, Session, and Server) used to service HTTP requests.</param>
        public void ProcessRequest(HttpContext context)
        {
            SPSecurity.RunWithElevatedPrivileges(delegate
            {
                string url = context.Request["item"];
                if(url.StartsWith("/")){
                     string root = string.Format("{0}://{1}{2}{3}",
                                    context.Request.Url.Scheme,
                                    context.Request.Url.Host,
                                    context.Request.Url.Port == 80
                                        ? string.Empty
                                        : ":" + context.Request.Url.Port,
                                    context.Request.ApplicationPath);
                    url = root+url;
                }
                

                using (var site = new SPSite(url))
                {
                    using(SPWeb web = site.OpenWeb(site.RootWeb.ID))
                    {
                        var list = web.Lists.GetList(new Guid(context.Request["List"]), true);

                        int id = Convert.ToInt32(context.Request["ID"]);
                        var templateID = list.RootFolder.Properties["vti_listservertemplate"].ToString();
                        SPListItem item = null;

                        if (templateID == ListTemplateIds.TOP_EMPLOYEE_LIST)
                        {

                            var temp = list.GetItemById(id);
                            var fieldValue = temp["EmployeeName"] as SPFieldLookupValue;
                            //var field = list.Fields["EmployeeName"] as SPFieldLookup;
                            
                            //if (field != null && fieldValue != null)
                            {

                                list = web.GetList("/Lists/Employees");
                                id = fieldValue.LookupId;

                            }
                        }
                        
                            item = list.GetItemById(id);
                        


                        if (item.Attachments.Count > 0)
                        {
                            var file = item.Attachments[0];

                            //SPUrlUtility.CombineUrl(item.Attachments.UrlPrefix, fileName);

                            SPFolder attachments = item.ParentList.RootFolder.SubFolders["Attachments"].SubFolders[item.ID.ToString()];
                            var attachmentFile = attachments.Files[file];
                            var stream = attachmentFile.OpenBinaryStream();

                            byte[] data = new byte[stream.Length];
                            stream.Read(data, 0, data.Length);
                            stream.Close();

                            context.Response.ContentType = "image/jpeg";
                            context.Response.Cache.SetCacheability(HttpCacheability.Public);
                            context.Response.Cache.SetExpires(DateTime.Now.AddMinutes(10));
                            context.Response.Cache.SetMaxAge(new TimeSpan(0, 10, 0));
                            context.Response.AddHeader("Last-Modified", DateTime.Now.ToString());

                            // Send back the file content
                            context.Response.BinaryWrite(data);
                            context.Response.End();
                        }
                        else
                        {
                            //TODO - Change this code to show the not found image
                            context.Response.Redirect(Model.Constants.NO_IMAGE_URL);
                            context.Response.End();
                        }
                    }
                }

            });
        }

        #endregion
    }
}