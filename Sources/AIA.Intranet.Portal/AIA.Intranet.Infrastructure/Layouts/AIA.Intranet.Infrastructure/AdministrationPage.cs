using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using Microsoft.SharePoint.Utilities;
using System.Web;
using AIA.Intranet.Common.Extensions;


namespace AIA.Intranet.Infrastructure.Pages
{
   public class AdministrationPage : LayoutsPageBase
    {
        
        #region Properties
       private SPContentType _contentType;

       protected SPContentType CurrentContentType
       {
           get
           {
               if (_contentType == null)
               {
                   string contentTypeId = HttpContext.Current.Request.QueryString["ctype"];
                   if (this.CurrentList != null)
                   {
                       _contentType = CurrentList.ContentTypes[new SPContentTypeId(contentTypeId)];
                   }
                   else
                   {
                       _contentType = SPContext.Current.Web.FindContentType(new SPContentTypeId(contentTypeId));
                   }
               }

               return _contentType;
           }
       }


       protected override bool RequireSiteAdministrator
       {
           get
           {
               return true;
           }
       }
        protected SPList CurrentList
        {
            get
            {
                return SPContext.Current.List;
            }
        }

        protected SPWeb Web
        {
            get
            {
                return SPContext.Current.Web;
            }
        }
        protected string SourceUrl
        {
            get
            {
                return base.Request.QueryString["Source"];
            }
        }
        #endregion

        public virtual void GoToPreviousPage()
        {
            SPUtility.Redirect(this.SourceUrl, SPRedirectFlags.Default, this.Context);
        }
    }
}
