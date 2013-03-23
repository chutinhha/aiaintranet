using System;
using Microsoft.SharePoint;

namespace AIA.Intranet.Common.Utilities
{
    public class DisableItemEvent : SPItemEventReceiver, IDisposable
    {
        bool oldValue;

        public DisableItemEvent()
        {
            this.oldValue = base.EventFiringEnabled;
            base.EventFiringEnabled = false;
        }

        public void Dispose()
        {
            base.EventFiringEnabled = oldValue;
        }
    }
}
