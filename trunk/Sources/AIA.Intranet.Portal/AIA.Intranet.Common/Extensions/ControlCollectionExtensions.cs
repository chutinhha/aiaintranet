using System.Collections.Generic;
using System.Web.UI;

namespace AIA.Intranet.Common.Extensions
{
    public static class ControlCollectionExtensions
    {
        public static IEnumerable<T> OfTypeRecursive<T>(this ControlCollection controls) where T : Control
        {
            foreach (Control c in controls)
            {
                T ct = c as T;

                if (ct != null)
                    yield return ct;

                foreach (T cc in OfTypeRecursive<T>(c.Controls))
                    yield return cc;
            }
        }
    }
}
