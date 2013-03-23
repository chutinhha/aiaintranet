using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SharePoint;

namespace AIA.Intranet.Model.Entities
{
    public class UserInfomation : BaseEntity
    {
        public UserInfomation(SPListItem item ): base (item){}
        public string ThemeName { get; set; }
        public string Name { get; set; }
        public string Picture { get; set; }

    }
}
