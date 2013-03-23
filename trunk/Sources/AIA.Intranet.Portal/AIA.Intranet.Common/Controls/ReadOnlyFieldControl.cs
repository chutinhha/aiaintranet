using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SharePoint.WebControls;
using System.Web.UI.WebControls;

namespace AIA.Intranet.Common.Controls
{
    public class ReadOnlyFieldControl : BaseFieldControl
    {
        public ReadOnlyFieldControl()
            : base()
        {

        }
        protected override void CreateChildControls()
        {
            this.Controls.Add(new Literal() { Text = "Hekkk" });
        }
    }
}
