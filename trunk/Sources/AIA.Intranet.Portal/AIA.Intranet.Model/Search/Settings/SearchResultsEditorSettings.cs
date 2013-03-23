using System;
using System.Collections.Generic;


namespace AIA.Intranet.Model.Search.Settings
{
    [Serializable]
    public class SearchResultsEditorSettings
    {
        public List<FieldSetting> ResultFields { get; set; }
        public bool ECBMenu { get; set; }
        public int PageSize { get; set; }
        public uint MaxNumberOfRecord { get; set; }
        public int MaxPageDisplayed { get; set; }
        public string ECBFieldID { get; set; }
    }
}
