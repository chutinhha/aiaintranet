using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AIA.Intranet.Model.Entities;
using AIA.Intranet.Common.Utilities;
using AIA.Intranet.Model;
using AIA.Intranet.Common.Utilities.Camlex;

namespace AIA.Intranet.Common.Services
{
    public class ProjectServices
    {
        public static ProjectItem GetProjectByItemID(int itemID)
        {
            var query = new CAMLListQuery<ProjectItem>(Constants.PROJECT_LIST_URL);
            return query.GetItemById(itemID);
        }
    }
}
