using System;

namespace AIA.Intranet.Model.Workflow
{
    [Serializable]
    public class Parameter
    {
        public string ListId { get; set; }
        public int ListItem { get; set; }
        public ApprovalConfiguration ApprovalConfiguation { get; set; }
        public int TaskItem { get; set; }
        public string Comment { get; set; }
        public string Submitter { get; set; }
        public string TaskItemName { get; set; }
    }
}
