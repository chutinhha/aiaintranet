using System;

namespace AIA.Intranet.Model.Search.Settings
{
    [Serializable]
    public class SearchSettings
    {
        public SearchCriteriaEditorSettings SearchCriteriaSettings { get; set; }
        public SearchResultsEditorSettings SearchResultsSettings { get; set; }
        public SearchScope Scope { get; set; }
        public SearchListBaseType ListBaseType { get; set; }
        public string ResultPageURL { get; set; }
        public string SearchBoxPageURL { get; set; }
    }
}
