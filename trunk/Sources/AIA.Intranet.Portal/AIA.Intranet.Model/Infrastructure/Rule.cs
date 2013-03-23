using System;
using System.Collections.Generic;

using AIA.Intranet.Model.Search;

namespace AIA.Intranet.Model.Security
{
    [Serializable]
    public class Rule
    {

        public Rule()
        {
            CriteriaList = new List<Criteria>();
            PermissionAssignments = new List<PermissionAssigment>();
        }
        
        public string ID { get; set; }
        public List<Criteria> CriteriaList { get; set; }
        public List<PermissionAssigment> PermissionAssignments { get; set; }
        public int Order { get; set; }
        public string ContentTypeId { get; set; }
        public string DocumentTypes { get; set; }
        public string Name { get; set; }
        public string OwnerPermission { get; set; }

        public bool RunOnAdded { get; set; }
        public bool RunOnFirstUpdate { get; set; }
        public bool RunOnAnyUpdate { get; set; }
        public bool PreserveExistingSecurity { get; set; }
    }
}
