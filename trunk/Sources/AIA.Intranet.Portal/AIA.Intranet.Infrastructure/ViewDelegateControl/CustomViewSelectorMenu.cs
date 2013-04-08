using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using System;

namespace AIA.Intranet.Infrastructure
{
    public class CustomViewSelectorMenu : ViewSelectorMenu
    {
        internal CustomViewSelectorMenu(SPContext renderContext)
        {
            base.RenderContext = renderContext;
        }
    }
}

