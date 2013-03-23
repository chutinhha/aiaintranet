using System;
using System.Collections.Generic;
using System.Workflow.Activities;

namespace AIA.Intranet.Model.Workflow
{
    [Serializable]
    public class ApprovalConfiguration
    {
        public string TaskConfigurationName { get; set; }

        public string ContentTypeId { get; set; }

        public List<string> Approvers { get; set; }

        public bool ExpandGroup { get; set; }

        public ExecutionType AssignmentType { get; set; }

        public string AssignmentEmailTemplate { get; set; }

        public double DueDateDuration { get; set; }

        public double ReminderDuration { get; set; }

        public string ReminderEmailTemplate { get; set; }

        public double EscalationDuration { get; set; }

        public string EscalationPartyEmail { get; set; }

        public string EscalationEmailTemplate { get; set; }

        public bool UseNumberRequired { get; set; }

        public int NumberRequired { get; set; }

        public string URLEmailTemplate { get; set; }

        public List<string> TaskContributors { get; set; }

        public List<string> TaskObservers { get; set; }

        public string TaskInstruction { get; set; }

        public string TaskTitlePrefix { get; set; }

        public TaskRuleSettings TaskRuleConfiguration { get; set; }

        public TaskEventSettings TaskEventConfiguration { get; set; }

        public bool AllowReassign { get; set; }

        public bool AllowDueDateChangeOnReassignment { get; set; }

        public bool AlloRequestInfomation { get; set; }

        public bool AllowDueDateChangeOnRequestInformation { get; set; }

        public bool AllowPlaceOnHold { get; set; }

        public bool AllowSendEEC { get; set; }

        public bool UseMetadataAssignment { get; set; }

        public bool ByPassTask { get; set; }

        public bool IgnoreIfNoParticipant { get; set; }
    }
}
