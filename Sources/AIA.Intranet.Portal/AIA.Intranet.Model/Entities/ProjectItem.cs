using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SharePoint;

namespace AIA.Intranet.Model.Entities
{
    public class ProjectItem : BaseEntity
    {
        public ProjectItem(SPListItem item)
            : base(item)
        {
            try
            {
                SPFieldLookupValue valueProjectManager = item[IOfficeColumnId.Project.ProjectManager] as SPFieldLookupValue;
                if (valueProjectManager != null)
                {
                    ProjectManagerID = valueProjectManager.LookupId;
                }

                SPFieldLookupValue valueProjectClerk = item[IOfficeColumnId.Project.ProjectClerk] as SPFieldLookupValue;
                if (valueProjectClerk != null)
                {
                    ProjectClerkID = valueProjectClerk.LookupId;
                }

                SPFieldLookupValueCollection valueProjectMembers = item[IOfficeColumnId.Project.ProjectMember] as SPFieldLookupValueCollection;
                if (valueProjectMembers != null)
                {
                    foreach (var member in valueProjectMembers)
                    {
                        ProjectMemberID += member.LookupId + "|";
                    }
                }
            }
            catch 
            { 
                
            }
        }

        [Field(Ignore = true)]
        public int ProjectManagerID { get; set; }
        [Field(Ignore = true)]
        public int ProjectClerkID { get; set; }
        [Field(Ignore = true)]
        public string ProjectMemberID { get; set; }
    }
}
