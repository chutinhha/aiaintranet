using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Microsoft.SharePoint;
using AIA.Intranet.Common.Utilities;
using AIA.Intranet.Model;

namespace AIA.Intranet.Infrastructure.WebParts.BannerSlideShow
{
    public partial class BannerSlideShowUserControl : UserControl
    {
        public string BannerFolder { get; set; }

        private string liSlideShow = @"<li><a href={4}{0}{4}>
                                            <img src={4}/Style Library/images/space.png{4} 
                                                style={4}background: url('{0}') no-repeat center center; -webkit-background-size: cover;
                                                -moz-background-size: cover; -o-background-size: cover;
                                                background-size: cover; filter: progid:DXImageTransform.Microsoft.AlphaImageLoader(src='{0}', sizingMethod='scale');
                                                -ms-filter: progid:DXImageTransform.Microsoft.AlphaImageLoader(src='{0}', sizingMethod='scale');
                                                width: 52px; height: 35px{4} title='{1}' alt='{2}' longdesc='{3}' />
                                        </a></li>";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadBanner();
            }
        }

        private void LoadBanner()
        {
            SPSecurity.RunWithElevatedPrivileges(delegate()
            {
                using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                {
                    using (SPWeb web = site.OpenWeb(SPContext.Current.Site.RootWeb.ID))
                    {
                        SPList spList = Utility.GetListFromURL(Constants.BANNER_LIBRARY_URL, web);
                        string banners = string.Empty;
                        if (spList != null)
                        {
                            SPFolder spFolder = spList.RootFolder;

                            if (!string.IsNullOrEmpty(BannerFolder))
                            {
                                SPFolderCollection folderCollection = spFolder.SubFolders;
                                
                                foreach (SPFolder folder in folderCollection)
                                {
                                    if (spFolder.Name == "Attachments" || spFolder.Item == null) continue;
                                    if (spFolder.Item.Name == BannerFolder)
                                    {
                                        spFolder = folder;
                                        break;
                                    }
                                }
                            }

                            SPQuery spQuery = new SPQuery();
                            spQuery.Query = "<OrderBy><FieldRef Name='OrderNumber'/></OrderBy>";
                            spQuery.Folder = spFolder;

                            SPListItemCollection spItemCollection = spList.GetItems(spQuery);

                            foreach (SPItem spItem in spItemCollection)
                            {
                                if (Convert.ToBoolean(spItem["Active"]))
                                {
                                    banners += string.Format(liSlideShow, string.Format("{0}/{1}", site.MakeFullUrl(spFolder.ServerRelativeUrl), spItem["Name"]), spItem["Title"], spItem["Description"], spItem["RelatedLink"], '"');
                                }
                            }
                        }
                        ulThumbList.InnerHtml = banners;
                    }
                }
            });
            
        }
    }
}
