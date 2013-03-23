//--------------------------------------------------------------------------------
// This file is a "Sample" as part of the MICROSOFT SDK SAMPLES FOR SHAREPOINT
// PRODUCTS AND TECHNOLOGIES
//
// (c) 2008 Microsoft Corporation.  All rights reserved.  
//
// This source code is intended only as a supplement to Microsoft
// Development Tools and/or on-line documentation.  See these other
// materials for detailed information regarding Microsoft code samples.
// 
// THIS CODE AND INFORMATION ARE PROVIDED AS IS WITHOUT WARRANTY OF ANY
// KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE
// IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//--------------------------------------------------------------------------------

using System;

namespace SPDisposeCheck
{

    public enum SPDisposeCheckID
    {
        // SPDisposeCheckIDs.
        SPDisposeCheckID_000 = 0,   //UNDEFINED
        SPDisposeCheckID_100 = 100, //Microsoft.SharePoint.SPList.BreakRoleInheritance() method
        SPDisposeCheckID_110 = 110, //Microsoft.SharePoint.SPSite new() operator
        SPDisposeCheckID_120 = 120, //Microsoft.SharePoint.SPSite.OpenWeb()
        SPDisposeCheckID_130 = 130, //Microsoft.SharePoint.SPSite.AllWebs[] indexer
        SPDisposeCheckID_140 = 140, //Microsoft.SharePoint.SPSite.RootWeb, LockIssue, Owner, and SecondaryContact properties
        SPDisposeCheckID_150 = 150, //Microsoft.SharePoint.SPSite.AllWebs.Add() method
        SPDisposeCheckID_160 = 160, //Microsoft.SharePoint.SPWeb.GetLimitedWebPartManager() method
        SPDisposeCheckID_170 = 170, //Microsoft.SharePoint.SPWeb.ParentWeb property
        SPDisposeCheckID_180 = 180, //Microsoft.SharePoint.SPWeb.Webs property
        SPDisposeCheckID_190 = 190, //Microsoft.SharePoint.SPWeb.Webs.Add() method
        SPDisposeCheckID_200 = 200, //Microsoft.SharePoint.SPWebCollection.Add() method 
        SPDisposeCheckID_210 = 210, //Microsoft.SharePoint.WebControls.SPControl GetContextSite() and GetContextWeb() methods
        SPDisposeCheckID_220 = 220, //Microsoft.SharePoint.SPContext Current.Site / SPContext.Site and SPContext.Current.Web / SPContext.Web properties
        SPDisposeCheckID_230 = 230, //Microsoft.SharePoint.Administration.SPSiteCollection[] indexer
        SPDisposeCheckID_240 = 240, //Microsoft.SharePoint.Administration.Add() method
        SPDisposeCheckID_300 = 300, //Microsoft.SharePoint.Publishing.GetPublishingWebs() method
        SPDisposeCheckID_310 = 310, //Microsoft.SharePoint.Publishing.PublishingWebCollection.Add() method
        SPDisposeCheckID_320 = 320, //Microsoft.SharePoint.Publishing.PublishingWeb.GetVariation() method 
        SPDisposeCheckID_400 = 400, //Microsoft.Office.Server.UserProfiles.PersonalSite property 
        SPDisposeCheckID_500 = 500, //Microsoft.SharePoint.Portal.SiteData.AreaManager.GetArea() method
        SPDisposeCheckID_999 = 999,  //All
        SPDisposeCheckID_635 = 635,
    }

    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Assembly, Inherited = false, AllowMultiple = true)]
    public class SPDisposeCheckIgnoreAttribute : Attribute
    {
        public SPDisposeCheckIgnoreAttribute(SPDisposeCheckID Id, string Reason)
        {
            _id = Id;
            _reason = Reason;
        }

        protected SPDisposeCheckID _id;
        protected string _reason;

        public SPDisposeCheckID Id
        {
            get { return _id; }
            set { _id = Id; }
        }

        public string Reason
        {
            get { return _reason; }
            set { _reason = Reason; }
        }
    }

}