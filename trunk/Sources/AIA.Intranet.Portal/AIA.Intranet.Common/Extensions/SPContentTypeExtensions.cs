using System.Xml;
using AIA.Intranet.Common.Helpers;
using AIA.Intranet.Common.Utilities;
using AIA.Intranet.Common.Extensions;
using Microsoft.SharePoint;
using AIA.Intranet.Model;
using System.Linq;
using Microsoft.SharePoint.Workflow;
using System;

namespace AIA.Intranet.Common.Extensions
{
    public static class SPContentTypeExtensions
    {
        public static void AssociateWokflow(this SPContentType ctype, string wokflowTemplateName, string workflowName, string association)
        {
            SPWorkflowTemplate workflowTemplate = null;                // Workflow template
            SPWorkflowAssociation workflowAssociation = null;         //Workflow association
            SPList historyList = null;                                                     // Workflow history list
            SPList taskList = null;                                                        // Workflow tasks list
            SPList list = null;                                                               //Sharepoint List       


            workflowTemplate = ctype.ParentWeb.Site.RootWeb.WorkflowTemplates.GetTemplateByName(wokflowTemplateName, System.Globalization.CultureInfo.CurrentCulture);

            //List which we is going to associate the workflow           
            historyList = ctype.ParentWeb.Lists["Workflow History"];  //The list to which to log workflow history events.

            taskList = ctype.ParentWeb.Lists["Workflow Task"];      //The task list on which to create workflow tasks for this workflow


            try
            {
                ctype.ParentWeb.AllowUnsafeUpdates = false;
                // Create workflow association
                workflowAssociation = SPWorkflowAssociation.CreateWebContentTypeAssociation(workflowTemplate, workflowName, "Workflow Task", "Workflow History");

                // Set workflow parameters 
                workflowAssociation.AllowManual = false;
                workflowAssociation.AutoStartCreate = true;
                workflowAssociation.AutoStartChange = false;

                workflowAssociation.AssociationData = association;

                // Add workflow association to my list
                //list.AddWorkflowAssociation(workflowAssociation);
                ctype.WorkflowAssociations.Add(workflowAssociation);
                // Enable workflow
                workflowAssociation.Enabled = true;
            }
            finally
            {
                ctype.ParentWeb.AllowUnsafeUpdates = false;
            }
        }

        public static void EnsureEventReciever(this SPContentType contenttype, string recieverClass, string assembly, params SPEventReceiverType[] recieverTypes)
        {
            if (contenttype == null || contenttype.Sealed) return;

            
            //if (contenttype.EventReceivers == null) contenttype.EventReceivers;

            foreach (var item in recieverTypes)
            {
                if (!contenttype.EventReceivers.Cast<SPEventReceiverDefinition>().Any(P => P.Class == recieverClass &&
                    P.Assembly == assembly &&
                    P.Type == item))
                {
                    contenttype.EventReceivers.Add(item, assembly, recieverClass);
                }
            }
            contenttype.Update(true);
        }

        public static void EnsureEventReciever(this SPContentType contenttype, System.Type recieverClass, params SPEventReceiverType[] recieverTypes)
        {
            if (contenttype == null || contenttype.Sealed) return;

            string assembly = recieverClass.Assembly.FullName;
            //if (contenttype.EventReceivers == null) contenttype.EventReceivers;

            foreach (var item in recieverTypes)
            {
                if (!contenttype.EventReceivers.Cast<SPEventReceiverDefinition>().Any(P => P.Class == recieverClass.FullName &&
                    P.Assembly == assembly &&
                    P.Type == item))
                {
                    contenttype.EventReceivers.Add(item, assembly, recieverClass.FullName);
                }
            }
            contenttype.Update();
        }

        public static void SetCustomSettings<T>(this SPContentType contentType, IOfficeFeatures featureName, T settingsObject)
        {
            contentType.SetCustomSettings<T>(featureName, settingsObject, false);
        }

        public static void RemoveCustomSettings<T>(this SPContentType ctype, IOfficeFeatures featureName)
        {
            ctype.SetCustomSettings<T>(featureName, default(T));
        }

        public static void RemoveCustomSettings<T>(this SPContentType ctype, IOfficeFeatures featureName, bool applyToChilds)
        {
            ctype.SetCustomSettings<T>(featureName, default(T), applyToChilds);
        }

        public static void SetCustomSettings<T>(this SPContentType contentType, IOfficeFeatures featureName, T settingsObject, bool applyToChilds)
        {
            string strKey = CCIUtility.BuildKey<T>(featureName);
            string settingsXml = SerializationHelper.SerializeToXml<T>(settingsObject, strKey);
            SPXmlDocumentCollection xmlDocCollection = contentType.XmlDocuments;
            if (!string.IsNullOrEmpty(xmlDocCollection[strKey]))
                xmlDocCollection.Delete(strKey);

            XmlDocument customXmlDocSettings = new XmlDocument();
            customXmlDocSettings.LoadXml(settingsXml);
            xmlDocCollection.Add(customXmlDocSettings);

            //try
            //{
            SPSecurity.RunWithElevatedPrivileges(delegate()
            {
                contentType.ParentWeb.AllowUnsafeUpdates = true;
                contentType.XmlDocuments.Delete("http://schemas.microsoft.com/sharepoint/v3/contenttype/forms/url");
                if (contentType.ParentList == null)
                {
                    contentType.Update(applyToChilds, false);
                }
                else
                {
                    contentType.Update();
                }
                contentType.ParentWeb.AllowUnsafeUpdates = false;
            });
            //}
            //catch
            //{
            //    CCIUtility.LogInfo("Do not throw message if using option apply to child but current content type doen't have any child", "AIA.Intranet.Common");
            //}
        }

        public static T GetCustomSettings<T>(this SPContentType contentType, IOfficeFeatures featureName)
        {
            return contentType.GetCustomSettings<T>(featureName, true);
        }

        public static T GetCustomSettings<T>(this SPContentType contentType, IOfficeFeatures featureName, bool lookupInParent)
        {
            string strKey = CCIUtility.BuildKey<T>(featureName);
            string settingsXml = contentType.XmlDocuments[strKey];

            if (!string.IsNullOrEmpty(settingsXml))
                return (T)SerializationHelper.DeserializeFromXml<T>(settingsXml, strKey);

            if (!lookupInParent) return default(T);

            T objReturn = default(T);
            if (contentType.Parent.Sealed == false)
                objReturn = contentType.Parent.GetCustomSettings<T>(featureName);

            if (objReturn == null && contentType.ParentList != null)
                objReturn = contentType.ParentList.GetCustomSettings<T>(featureName);

            if (objReturn == null && contentType.ParentWeb != null)
                objReturn = contentType.ParentWeb.GetCustomSettings<T>(featureName);

            return objReturn;
        }

    //    public static SPContentTypeId FindContentType(this SPList list, string contentTypeId)
    //    {
    //        SPContentTypeId ctIdReturn = SPContentTypeId.Empty;

    //        SPContentTypeId sourceCTId = new SPContentTypeId(contentTypeId);
    //        SPContentTypeId foundCTId = list.ContentTypes.BestMatch(sourceCTId);
    //        bool found = (foundCTId.Parent.CompareTo(sourceCTId) == 0);
    //        SPContentType ct = list.ParentWeb.FindContentType(sourceCTId);

    //        if (found)
    //        {
    //            ctIdReturn = list.ContentTypes[ct.Name].Id;
    //        }
    //        return ctIdReturn;
    //    }
    }
}
