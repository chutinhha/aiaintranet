using System; 
using System.Collections.Generic;

namespace AIA.Intranet.Model.Search.Settings
{
    [Serializable]
    public class SearchCriteriaEditorSettings
    {
        public SearchCriteriaEditorSettings()
        {
            SearchFields = new List<FieldSetting>();
            ContentTypeOperators = new List<Operators>();
        }
        public List<FieldSetting> SearchFields { get; set; }
        public bool UseGlobalOperatorOR { get; set; }
        public bool UseContentTypeDropDown { get; set; }
        public string ContentTypeGroupName { get; set; }
        public bool UseFullTextSearch { get; set; }
        public OperatorDisplayType ContentTypeOperatorDisplayType { get; set; }
        public List<Operators> ContentTypeOperators { get; set; }

    }
}
