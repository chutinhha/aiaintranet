using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SharePoint;

using SPDisposeCheck;
using AIA.Intranet.Model.Infrastructure;
using AIA.Intranet.Common.Utilities;
using AIA.Intranet.Model;

namespace AIA.Intranet.Common
{
    
    public class CoreFeatureReceiver : SPFeatureReceiver
    {
        private List<ListIntanceDefinition> instances;
        public CoreFeatureReceiver()
        {
            instances = new List<ListIntanceDefinition>();
        }
        public void AddListIntance(string title, string description, string url, int templateId,  bool showOnQuickLaunch, bool hidden)
        {
            instances.Add(new ListIntanceDefinition()
            { 
                Title = title,
                Description = description,
                Url = url, 
                TemplateId = templateId,
                ShowOnQuickLaunch = showOnQuickLaunch,
                Hidden = hidden,
                RootWebOnly = false
            });
        }
      
        public void AddListIntance(string title, string description, string url, string featureId, int templateId, bool showOnQuickLaunch, bool hidden)
        {
            instances.Add(new ListIntanceDefinition()
            {
                Title = title,
                Description = description,
                Url = url,
                FeatureId = featureId,
                TemplateId = templateId,
                ShowOnQuickLaunch = showOnQuickLaunch,
                Hidden = hidden,
                RootWebOnly = false
            });
        }

        public void AddListIntance(string title, string description, string url, int templateId, bool showOnQuickLaunch, bool hidden, bool rootWebOnly)
        {
            instances.Add(new ListIntanceDefinition()
            {
                Title = title,
                Description = description,
                Url = url,
                TemplateId = templateId,
                ShowOnQuickLaunch = showOnQuickLaunch,
                Hidden = hidden,
                RootWebOnly = rootWebOnly
            });
        }

        [SPDisposeCheckIgnore(SPDisposeCheckID.SPDisposeCheckID_999, "Don't need dispose Root web")]
        public override void FeatureActivated(SPFeatureReceiverProperties properties)
        {
            base.FeatureActivated(properties);
            SPSecurity.RunWithElevatedPrivileges(delegate()
            {
                foreach (var item in instances)
                {
                    SPWeb web = null;
                    try
                    {
                        if (properties.Feature.Parent.GetType() == typeof(SPSite))
                        {
                            SPSite site = properties.Feature.Parent as SPSite;
                            web = site.RootWeb;
                        }
                        else
                        {
                            web = properties.Feature.Parent as SPWeb;
                            if (item.RootWebOnly == true)
                                web = web.Site.RootWeb;
                        }

                        if (web == null) continue;

                        CreateListInstance(item, web);
                    }
                    finally
                    {
                        //if (web != null) web.Dispose();
                    }
                   
                }
            });
           
        }
        private void CreateListInstance(ListIntanceDefinition item, SPWeb web)
        {
            try
            {
                if ( CCIUtility.GetListFromURL(web.Url+"/"+item.Url) != null)
                {
                   
                    CCIUtility.LogError(item.Url + " has already existed on " + web.Url, IOfficeFeatures.Infrastructure);
                    return;
                }
                //SPListTemplate template = web.ListTemplates[item.TemplateId];
                var template = web.ListTemplates.Cast<SPListTemplate>().Where(p => p.Type_Client == item.TemplateId).FirstOrDefault();
                if (!string.IsNullOrEmpty(item.FeatureId))
                {
                    template = web.ListTemplates.Cast<SPListTemplate>().Where(p => p.Type_Client == item.TemplateId && p.FeatureId == new Guid(item.FeatureId)).FirstOrDefault();
                }

                if (template != null)
                {

                    //Guid listId = web.Lists.Add(item.Title, item.Description, template);
                    Guid listId=web.Lists.Add(item.Title, item.Description, item.Url,template.FeatureId.ToString(), item.TemplateId,"100");
                    web.Update();

                    SPList list = web.Lists[listId];
                    list.OnQuickLaunch = item.ShowOnQuickLaunch;
                    list.Hidden = item.Hidden;
                    list.RootWebOnly = item.RootWebOnly;
                    list.Update();
                }
            }
            catch (Exception ex)
            {
                CCIUtility.LogError(ex.Message + ex.StackTrace, IOfficeFeatures.Infrastructure);
                //throw;
            }
            
        }

    }
}
