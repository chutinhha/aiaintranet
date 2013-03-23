using System;
using System.Collections.Generic;
using Microsoft.SharePoint;

namespace AIA.Intranet.Model.Search.Settings
{
    [Serializable]
    public class FieldSetting
    {
        public string FieldId { get; set; }
        public int Order { get; set; }
        public bool UseKeyword { get; set; }
        public SortType Sort { get; set; }
        public OperatorDisplayType DisplayType { get; set; }
        public List<Operators> Operators { get; set; }
        //public string HiddenOperators { get; set; }
        public FieldSetting()
        {
            Operators = new List<Operators>();
        }
    }

    public static class FieldSettingExtensions
    {
        public static SPField ToSPField(this FieldSetting fieldSetting)
        {
            SPField field = null;
            if (fieldSetting == null || string.IsNullOrEmpty(fieldSetting.FieldId))
                return null;
            try
            {
                field = SPContext.Current.Web.AvailableFields[new Guid(fieldSetting.FieldId)];
            }
            catch
            {
                field = null;
            }
            return field;
        }
    }
}
