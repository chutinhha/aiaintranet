using Microsoft.SharePoint;
using Microsoft.SharePoint.Security;
using Microsoft.SharePoint.WebControls;
using Microsoft.SharePoint.WebPartPages;
using System;
using System.Reflection;
using System.Security.Permissions;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

namespace AIA.Intranet.Infrastructure
{
    [AspNetHostingPermission(SecurityAction.InheritanceDemand, Level=AspNetHostingPermissionLevel.Minimal), SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel=true), AspNetHostingPermission(SecurityAction.LinkDemand, Level=AspNetHostingPermissionLevel.Minimal), SharePointPermission(SecurityAction.LinkDemand, ObjectModel=true)]
    public class PageHeadDelegateControl : WebControl
    {
        private bool _incompatibleUIVersion;
        private bool _ltvsmAlreadyPresent;
        private string _trace = string.Empty;
        private bool _visible;

        private Control findControl(Control parent, string id)
        {
            if (parent.HasControls())
            {
                Control control = parent.FindControl(id);
                if (control != null)
                {
                    return control;
                }
                foreach (Control control2 in parent.Controls)
                {
                    control = this.findControl(control2, id);
                    if (control != null)
                    {
                        return control;
                    }
                }
            }
            return null;
        }

        private XsltListViewWebPart getFirstListViewWebPart()
        {
            WebPartManager currentWebPartManager = WebPartManager.GetCurrentWebPartManager(this.Page);
            if (currentWebPartManager != null)
            {
                foreach (System.Web.UI.WebControls.WebParts.WebPart part in currentWebPartManager.WebParts)
                {
                    XsltListViewWebPart part2 = part as XsltListViewWebPart;
                    if (part2 != null)
                    {
                        return part2;
                    }
                }
            }
            return null;
        }

        [SharePointPermission(SecurityAction.Demand, ObjectModel=true)]
        protected override void OnLoad(EventArgs e)
        {
            try
            {
                SPWeb contextWeb = SPControl.GetContextWeb(this.Context);
                if ((contextWeb != null) && (contextWeb.UIVersion <= 3))
                {
                    this._incompatibleUIVersion = true;
                    return;
                }
                Type baseType = this.Page.GetType().BaseType;
                if ((baseType == typeof(WebPartPage)) || (baseType == typeof(Page)))
                {
                    Control control = this.findControl(this.Page, "LTViewSelectorMenu");
                    if (control != null)
                    {
                        if (!control.Visible)
                        {
                            typeof(ListTitleViewSelectorMenu).GetField("m_wpSingleInit", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(control, true);
                            typeof(ListTitleViewSelectorMenu).GetField("m_wpSingle", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(control, true);
                        }
                        this._ltvsmAlreadyPresent = true;
                    }
                    else
                    {
                        this._visible = true;
                    }
                }
                else if (baseType == typeof(WikiEditPage))
                {
                    this._visible = true;
                }
            }
            catch (Exception exception)
            {
                this._trace = this._trace + exception.ToString();
            }
            base.OnLoad(e);
        }

        [SharePointPermission(SecurityAction.Demand, ObjectModel=true)]
        protected override void Render(HtmlTextWriter writer)
        {
            if (!string.IsNullOrEmpty(this._trace))
            {
                writer.Write(this._trace);
            }
            base.Render(writer);
        }

        public override bool Visible
        {
            [SharePointPermission(SecurityAction.Demand, ObjectModel=true)]
            get
            {
                return this._visible;
            }
        }
    }
}

