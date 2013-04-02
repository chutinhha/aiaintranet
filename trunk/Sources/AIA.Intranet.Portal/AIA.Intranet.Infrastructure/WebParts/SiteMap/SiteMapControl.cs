using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using Microsoft.SharePoint;

namespace AIA.Intranet.Infrastructure.WebParts.SiteMap
{
    public class SiteMapControl: WebControl
    {
        public class SiteMapItem
        {
            public string Title { get; set; }
            public string Url { get; set; }
            public List<SiteMapItem> ChildItems { get; set; }

        }
        private SiteMapItem root;
        public string RootId { get; set; }

        protected override void CreateChildControls()
        {
            base.CreateChildControls();
            var rootWeb = SPContext.Current.Site.RootWeb;
            root = ReadSiteStructure(rootWeb);

            HtmlGenericControl control = new HtmlGenericControl("ul");

            control.Attributes.Add("id", RootId);

            control.Attributes.Add("class", "treeview");
            foreach (var item in root.ChildItems)
            {
                AddChildControl(control, item, 0, root.ChildItems[root.ChildItems.Count-1] == item);
            }
            
            this.Controls.Add(control);
            
        }

        private SiteMapItem ReadSiteStructure(SPWeb web)
        {
            SiteMapItem node = new SiteMapItem()
            {
               Title  = web.Title,
               Url = web.Url
            };

            if (web.Webs != null && web.Webs.Count > 0)
            {
                node.ChildItems = new List<SiteMapItem>();
                foreach (SPWeb subWeb in web.Webs)
                {
                    node.ChildItems.Add(ReadSiteStructure(subWeb));
                }
            }
            return node;
        }

        private void AddChildControl(HtmlGenericControl control, SiteMapItem root, int level, bool isLast)
        {
            HtmlGenericControl li = new HtmlGenericControl("li");
            control.Controls.Add(li);

            if (isLast) li.Attributes.Add("style", "li_last");
                
                HtmlGenericControl a = new HtmlGenericControl("a");
                a.Attributes.Add("class", "level_"+(level+1).ToString());
                

                a.Attributes.Add("href", root.Url);
                a.InnerText = root.Title;
                li.Controls.Add(a);

                if (root.ChildItems != null && root.ChildItems.Count > 0)
                {
                    li.Attributes.Add("class", "submenu");
                   // li.Attributes.Add("style", "background-image: url('/Style Library/images/open.gif');");
                    if (root.ChildItems != null)
                    {
                        HtmlGenericControl ul = new HtmlGenericControl("ul");
                        foreach (var item in root.ChildItems)
                        {
                            
                           
                            //ul.Attributes.Add("rel", "closed");
                            //ul.Attributes.Add("style", "display:block");
                           
                            li.Controls.Add(ul);
                            int childLevel = level + 1;

                            AddChildControl(ul, item, childLevel, item== root.ChildItems[root.ChildItems.Count-1]);
                        }
                    }
                }
        }
        public SiteMapControl()
        {
            
            
        }
    }
}
