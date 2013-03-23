using System;
using System.Collections.Generic;
using AIA.Intranet.Model.Search;

namespace AIA.Intranet.Model.Workflow
{
    [Serializable]
    public class TaskRuleSettings
    {
        public TaskRuleSettings()
        {
            ApprovedCriteriaList = new List<Criteria>();
            RejectedCriteriaList = new List<Criteria>();
            TerminationCriteriaList = new List<Criteria>();
            ReassignCriteriaList = new List<Criteria>();
            RequestInformationCriteriaList = new List<Criteria>();
            FinishedCriteriaList = new List<Criteria>();
            SignatureVerificationCriteriaList = new List<Criteria>();
            DataQualityCompletedCriteriaList = new List<Criteria>();
        }

        public List<Criteria> ApprovedCriteriaList { get; set; }
        public List<Criteria> RejectedCriteriaList { get; set; }
        public List<Criteria> TerminationCriteriaList { get; set; }
        public List<Criteria> ReassignCriteriaList { get; set; }
        public List<Criteria> RequestInformationCriteriaList { get; set; }
        public List<Criteria> FinishedCriteriaList { get; set; }
        public List<Criteria> SignatureVerificationCriteriaList { get; set; }
        public List<Criteria> DataQualityCompletedCriteriaList { get; set; }
    }
}
