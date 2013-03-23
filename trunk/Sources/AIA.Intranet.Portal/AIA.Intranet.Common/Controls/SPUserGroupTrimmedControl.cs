using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SharePoint.WebControls;
using System.Web.UI;
using Microsoft.SharePoint;

namespace AIA.Intranet.Common.Controls
{
    public class SPUserGroupTrimmedControl : SPSecurityTrimmedControl
    {
        private List<string> groups = new List<string>();

        public string GroupsString
        {
            get
            {
                return string.Join(",", groups.ToArray());
            }
            set
            {
                groups.AddRange(
                    value.Split(new char[] { ',' },
                   System.StringSplitOptions.RemoveEmptyEntries)
                    );
            }
        }

        protected override void Render(HtmlTextWriter output)
        {
            if (!string.IsNullOrEmpty(GroupsString) && IsMember())
            {
                base.Render(output);
            }
        }

        private bool IsMember()
        {
            using (SPWeb web = new SPSite(SPContext.Current.Web.Url).OpenWeb())
            {
                bool isMember = false;
                foreach (string group in groups)
                {
                    isMember = web.IsCurrentUserMemberOfGroup(web.Groups[group].ID);
                }
                return isMember;
            }
        }
    }
}
