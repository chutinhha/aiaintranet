using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using AIA.Intranet.Common.Extensions;
using AIA.Intranet.Model;
//using AIA.Intranet.Model.Search;
//using AIA.Intranet.Model.Search.Settings;
using Microsoft.SharePoint;

namespace AIA.Intranet.Common.Helpers
{
    public class CAMLHelper
    {
        public static readonly string Where = @"<Where>{0}</Where>";
        public static readonly string Equal = @"<Eq><FieldRef Name='{0}' /><Value Type='{1}'>{2}</Value></Eq>";
        public static readonly string LookupId = @"<Eq><FieldRef Name='{0}' LookupId='TRUE' /><Value Type='{1}'>{2}</Value></Eq>";
        public static readonly string Contains = @"<Contains><FieldRef Name='{0}' /><Value Type='{1}'>{2}</Value></Contains>";
        public static readonly string IsNull = @"<IsNull><FieldRef Name='{0}' /></IsNull>";
        public static readonly string DateGeq = @"<Geq><FieldRef Name='{0}' /><Value Type='{1}'>{2}</Value></Geq>";
        public static readonly string DateLeq = @"<Leq><FieldRef Name='{0}' /><Value Type='{1}'>{2}</Value></Leq>";
        
    //    public static string BuildQueryFromSearchDefinition(SearchDefinition searchDefinition)
    //    {
    //        SearchDefinition definition = searchDefinition.Clone<SearchDefinition>();

    //        string strGlobalOperator = definition.UseGlobalOperatorOR ? "Or" : "And";
    //        List<SearchCriteria> criterias = definition.SearchCriteriaList.Where(p => !p.IsEmpty && !p.IgnoreBlank).ToList();
            
    //        if (criterias == null || criterias.Count == 0) 
    //            return string.Empty;
            
    //        if (criterias[0].IsContentTypeDropDown)
    //        {
    //            var ctCriteria = criterias[0];
    //            string[] cts = ctCriteria.Value.Split(';');
    //            ctCriteria.Value = string.IsNullOrEmpty(cts[0]) ? "nullable" : cts[0];
    //            //ctCriteria.Value = cts[0];
    //            ctCriteria.RelatedOperator = "Or";

    //            for (int i = 1; i < cts.Length; i++)
    //            {
    //                if(string.IsNullOrEmpty(cts[i])) continue;

    //                SearchCriteria cloned = new SearchCriteria() {
    //                    FieldId = ctCriteria.FieldId,
    //                    FieldType = ctCriteria.FieldType,
    //                    FieldTypeName = ctCriteria.FieldTypeName,
    //                    IsContentTypeDropDown = ctCriteria.IsContentTypeDropDown,
    //                    Operator = ctCriteria.Operator,
    //                    RelatedOperator = "Or",
    //                    Value = cts[i]
    //                };

    //                criterias.Insert(1, cloned);
    //            }
    //        }
    //        SearchCriteriaNode node = SearchCriteriaNode.ConvertToTree(criterias, strGlobalOperator);
    //        return node.GetCAMLQuery();
    //    }

    //    public static string BuildQueryFromSearchDefinition(List<SearchCriteria> searchCriteria, bool useGlobalOperatorOR)
    //    {
    //        if (searchCriteria == null || searchCriteria.Count==0) return string.Empty;
    //        string strReturn = string.Empty;
    //        string strANDCloseTag = string.Empty;
    //        string strTempReplace = string.Empty;
    //        strReturn += "<Where>";
    //        string strTemp = string.Empty;
    //        string strGlobalOperator = useGlobalOperatorOR ? "Or" : "And";
    //        for (int i = 0; i < searchCriteria.Count; i++)
    //        {
    //            switch (searchCriteria[i].FieldType)
    //            {
    //                case SPFieldType.Boolean:
    //                case SPFieldType.Currency:
    //                case SPFieldType.Integer:
    //                case SPFieldType.Number:
    //                case SPFieldType.Calculated:
    //                case SPFieldType.Computed:
    //                case SPFieldType.Note:
    //                case SPFieldType.Text:
    //                case SPFieldType.Choice:
    //                case SPFieldType.MultiChoice:
    //                case SPFieldType.File:
    //                    strTempReplace = "<{0}><FieldRef ID=\'"
    //                        + searchCriteria[i].FieldId + "' /><Value Type=\'"
    //                        + searchCriteria[i].FieldType.ToString() + "'>"
    //                        + searchCriteria[i].Value + "</Value></{0}>";

    //                    switch (searchCriteria[i].Operator)
    //                    {
    //                        case Operators.Equal:
    //                            strTemp = string.Format(strTempReplace, "Eq");
    //                            break;

    //                        case Operators.NotEqual:
    //                            strTemp = string.Format(strTempReplace, "Neq");
    //                            break;

    //                        case Operators.GreaterThan:
    //                            strTemp = string.Format(strTempReplace, "Gt");
    //                            break;

    //                        case Operators.LessThan:
    //                            strTemp = string.Format(strTempReplace, "Lt");
    //                            break;

    //                        case Operators.StartsWith:
    //                            strTemp = string.Format(strTempReplace, "BeginsWith");
    //                            break;

    //                        case Operators.Contains:
    //                            strTemp = string.Format(strTempReplace, "Contains");
    //                            break;
    //                    }
    //                    break;

    //                case SPFieldType.DateTime:
    //                    switch (searchCriteria[i].Operator)
    //                    {
    //                        case Operators.Equal:
    //                            strTempReplace = "<Eq><FieldRef ID=\"{0}\" /><Value Type=\"DateTime\" {1}>{2}</Value></Eq>";
    //                            break;
    //                        case Operators.NotEqual:
    //                            strTempReplace = "<Neq><FieldRef ID=\"{0}\" /><Value Type=\"DateTime\" {1}>{2}</Value></Neq>";
    //                            break;
    //                        case Operators.EarlierThan:
    //                            strTempReplace = "<Lt><FieldRef ID=\"{0}\" /><Value Type=\"DateTime\" {1}>{2}</Value></Lt>";
    //                            break;
    //                        case Operators.LaterThan:
    //                            strTempReplace = "<Gt><FieldRef ID=\"{0}\" /><Value Type=\"DateTime\" {1}>{2}</Value></Gt>";
    //                            break;
    //                    }
    //                    if (!searchCriteria[i].IsTimeIncluded)
    //                        strTemp = string.Format(strTempReplace, searchCriteria[i].FieldId, string.Empty, searchCriteria[i].Value);
    //                    else
    //                        strTemp = string.Format(strTempReplace, searchCriteria[i].FieldId, "IncludeTimeValue='TRUE'", searchCriteria[i].Value);
    //                    break;

    //                case SPFieldType.Lookup:
    //                    switch (searchCriteria[i].Operator)
    //                    {
    //                        case Operators.Equal:
    //                            strTempReplace = "<Eq><FieldRef ID=\"{0}\" /><Value Type=\"Lookup\" >{1}</Value></Eq>";
    //                            break;
    //                        case Operators.NotEqual:
    //                            strTempReplace = "<Neq><FieldRef ID=\"{0}\" /><Value Type=\"Lookup\" >{1}</Value></Neq>";
    //                            break;
    //                        case Operators.Contains:
    //                            strTempReplace = "<Contains><FieldRef ID=\"{0}\" /><Value Type=\"Lookup\" >{1}</Value></Contains>";
    //                            break;
    //                    }
    //                    strTemp = (string.Format(strTempReplace, searchCriteria[i].FieldId, searchCriteria[i].Value));
    //                    break;

    //                case SPFieldType.User:
    //                    switch (searchCriteria[i].Operator)
    //                    {
    //                        case Operators.Equal:
    //                            strTempReplace = "<Eq><FieldRef ID=\"{0}\"  LookupId=\"TRUE\"/><Value Type=\"Integer\" >{1}</Value></Eq>";
    //                            break;
    //                        case Operators.NotEqual:
    //                            strTempReplace = "<Neq><FieldRef ID=\"{0}\" LookupId=\"TRUE\"/><Value Type=\"Integer\" >{1}</Value></Neq>";
    //                            break;
    //                        case Operators.Contains:
    //                            strTempReplace = "<Contains><FieldRef ID=\"{0}\" LookupId=\"TRUE\"/><Value Type=\"Integer\" >{1}</Value></Contains>";
    //                            break;
    //                    }
    //                    strTemp = string.Format(strTempReplace, searchCriteria[i].FieldId, searchCriteria[i].Value.Split(";#".ToCharArray())[0]);
    //                    break;

    //                case SPFieldType.Invalid:
    //                    if (string.Compare(searchCriteria[i].FieldTypeName,
    //                        Constants.LOOKUP_WITH_PICKER_TYPE_NAME, true) == 0)
    //                    {
    //                        switch (searchCriteria[i].Operator)
    //                        {
    //                            case Operators.Equal:
    //                                strTempReplace = "<Eq><FieldRef ID=\"{0}\" /><Value Type=\"{1}\" >{2}</Value></Eq>";
    //                                break;
    //                            case Operators.NotEqual:
    //                                strTempReplace = "<Neq><FieldRef ID=\"{0}\" /><Value Type=\"{1}\" >{2}</Value></Neq>";
    //                                break;
    //                            case Operators.Contains:
    //                                strTempReplace = "<Contains><FieldRef ID=\"{0}\" /><Value Type=\"{1}\" >{2}</Value></Contains>";
    //                                break;
    //                        }
    //                        strTemp = string.Format(strTempReplace, searchCriteria[i].FieldId, Constants.LOOKUP_WITH_PICKER_TYPE_NAME, searchCriteria[i].Value);
    //                    }
    //                    break;
    //            }

    //            if (i < searchCriteria.Count - 1)
    //            {
    //                strReturn += "<" + strGlobalOperator + ">";
    //                strReturn += strTemp;
    //                strANDCloseTag += "</" + strGlobalOperator + ">";
    //            }
    //        }
    //        strReturn += strTemp + strANDCloseTag;
    //        strReturn += "</Where>";

    //        return strReturn;
    //    }

    //    public static string BuildViewFieldsXml(SPList list, List<FieldSetting> resultFields)
    //    {
    //        string strFirstFields = "<FieldRef Name=\"EncodedAbsUrl\" Nullable=\"TRUE\"/>";
    //        string strLastFields = string.Empty;
    //        strFirstFields += "<FieldRef Name=\"FileRef\" Nullable=\"TRUE\"/>";
    //        strFirstFields += "<FieldRef Name=\"" + Constants.LINK_TO_FILE_COLUMN + "\" Nullable=\"TRUE\"/>";
    //        strFirstFields += "<FieldRef Name=\"" + Constants.LINK_VIEW_COLUMN + "\" Nullable=\"TRUE\"/>";
    //        strFirstFields += "<FieldRef Name=\"" + Constants.LINK_EDIT_COLUMN + "\" Nullable=\"TRUE\"/>";

    //        strFirstFields += "<FieldRef Name=\"" + Constants.LINK_WORKFLOW_COLUMN + "\" Nullable=\"TRUE\"/>";
    //        strFirstFields += "<FieldRef Name=\"" + Constants.LINK_SEND_ESIGN_COLUMN + "\" Nullable=\"TRUE\"/>";
    //        strFirstFields += "<FieldRef Name=\"" + Constants.LINK_ESIGN_HISTORY_COLUMN + "\" Nullable=\"TRUE\"/>";
    //        strFirstFields += "<FieldRef Name=\"" + Constants.LINK_VERSION_HISTORY_COLUMN + "\" Nullable=\"TRUE\"/>";
    //        strFirstFields += "<FieldRef Name=\"" + Constants.LINK_MANAGE_PERMISSIONS_COLUMN + "\" Nullable=\"TRUE\"/>";
    //        strFirstFields += "<FieldRef Name=\"" + Constants.LINK_DOCUMENT_DISCUSSIONS_COLUMN + "\" Nullable=\"TRUE\"/>";
    //        strFirstFields += "<FieldRef Name=\"" + Constants.LINK_UPLOAD_EXECUTED_COLUMN + "\" Nullable=\"TRUE\"/>";
    //        strFirstFields += "<FieldRef Name=\"" + Constants.LINK_ALERT_ME_COLUMN + "\" Nullable=\"TRUE\"/>";
    //        strFirstFields += "<FieldRef Name=\"" + Constants.LINK_SEND_TO_OTHER_LOCATION_COLUMN + "\" Nullable=\"TRUE\"/>";
    //        strFirstFields += "<FieldRef Name=\"" + Constants.LINK_SEND_TO_WORKSPACE_COLUMN + "\" Nullable=\"TRUE\"/>";
    //        strFirstFields += "<FieldRef Name=\"" + Constants.LINK_SEND_TO_EMAIL_COLUMN + "\" Nullable=\"TRUE\"/>";
    //        strFirstFields += "<FieldRef Name=\"" + Constants.LINK_SEND_TO_DOWNLOAD_COPY_COLUMN + "\" Nullable=\"TRUE\"/>";
    //        strFirstFields += "<FieldRef Name=\"" + Constants.LINK_SEND_TO_DOWNLOAD_PDF_COPY_COLUMN + "\" Nullable=\"TRUE\"/>";
    //        strFirstFields += "<FieldRef Name=\"" + Constants.LINK_RELATED_METRICS_COLUMN + "\" Nullable=\"TRUE\"/>";
    //        strFirstFields += "<FieldRef Name=\"" + Constants.LINK_RELATED_DOCUMENT_COLUMN + "\" Nullable=\"TRUE\"/>";
    //        strFirstFields += "<FieldRef Name=\"" + Constants.LINK_CREATE_RELATED_DOCUMENT_COLUMN + "\" Nullable=\"TRUE\"/>";
    //        strFirstFields += "<FieldRef Name=\"" + Constants.LINK_SEND_EEC_COLUMN + "\" Nullable=\"TRUE\"/>";
    //        strFirstFields += "<FieldRef Name=\"" + Constants.LINK_EMAIL_ATTACHMENT_COLUMN + "\" Nullable=\"TRUE\"/>";
    //        strFirstFields += "<FieldRef Name=\"" + Constants.LINK_CREATE_PDF_COLUMN + "\" Nullable=\"TRUE\"/>";
    //        strFirstFields += "<FieldRef Name=\"" + Constants.LINK_RELATED_RISKS_COLUMN + "\" Nullable=\"TRUE\"/>";

    //        for (int i = 0; i < resultFields.Count; i++)
    //        {
    //            SPField field = null;
    //            if (list != null && list.Fields.ContainFieldId(new Guid(resultFields[i].FieldId)))
    //                field = list.Fields[new Guid(resultFields[i].FieldId)];

    //            if (field == null & list != null)
    //                field = list.ParentWeb.Site.RootWeb.FindField(resultFields[i].FieldId);

    //            if (field == null & list != null)
    //                field = list.ParentWeb.FindField(resultFields[i].FieldId);

    //            if (field == null)
    //                field = SPContext.Current.Web.FindField(resultFields[i].FieldId);
                    
    //            string strTemp = "<FieldRef ID=\"{0}\"  Nullable=\"TRUE\" />";
    //            //ignore fields can't show
    //            switch (field.Type)
    //            {
    //                case SPFieldType.Lookup:
    //                //case SPFieldType.MultiChoice:
    //                case SPFieldType.User:
    //                case SPFieldType.Invalid:
    //                    continue;
    //                case SPFieldType.Computed:
    //                    SPFieldComputed computedField = (SPFieldComputed)field;
    //                    foreach (string f in computedField.FieldReferences)
    //                    {
    //                        if (string.Compare(f, "Description", true) == 0) continue;
    //                        strFirstFields += string.Format("<FieldRef Name=\"{0}\"  Nullable=\"TRUE\" />", f);
    //                    }
    //                    break;
    //                case SPFieldType.Integer:
    //                case SPFieldType.Number:
    //                case SPFieldType.DateTime:
    //                case SPFieldType.Currency:
    //                case SPFieldType.MultiChoice:
    //                case SPFieldType.Boolean:
    //                    break;
    //            }
    //            strLastFields += string.Format(strTemp, field.Id.ToString());

    //        }
    //        return strFirstFields + strLastFields;
    //    }

    //    public static string BuildViewFieldsXml(SPList list, List<FieldSetting> resultFields, List<SearchCriteria> searchCriterias)
    //    {
    //        var q = from f in searchCriterias
    //                select new FieldSetting() { FieldId = f.FieldId };
    //        List<FieldSetting> searchFields = q.ToList<FieldSetting>();

    //        return BuildViewFieldsXml(list, resultFields, searchFields);
    //    }

    //    public static string BuildViewFieldsXml(SPContentType ct, List<FieldSetting> resultFields, List<SearchCriteria> searchCriterias)
    //    {
    //        var q = from f in searchCriterias
    //                select new FieldSetting() { FieldId = f.FieldId };
    //        List<FieldSetting> searchFields = q.ToList<FieldSetting>();

    //        return BuildViewFieldsXml(ct, resultFields, searchFields);
    //    }

    //    private static string BuildViewFieldsXml(SPContentType ct, List<FieldSetting> resultFields, List<FieldSetting> searchFields)
    //    {
    //        List<FieldSetting> fields = new List<FieldSetting>();
    //        fields.AddRange(resultFields);
    //        fields.AddRange(searchFields);
    //        var mergeFields = from e in fields group e by e.FieldId into g select g.First();

    //        return BuildViewFieldsXml(ct, mergeFields.ToList());
    //    }

    //    private static string BuildViewFieldsXml(SPContentType ct, List<FieldSetting> resultFields)
    //    {
    //        string strFirstFields = "<FieldRef Name=\"EncodedAbsUrl\" Nullable=\"TRUE\"/>";
    //        string strLastFields = string.Empty;
    //        strFirstFields += "<FieldRef Name=\"FileRef\" Nullable=\"TRUE\"/>";
    //        strFirstFields += "<FieldRef Name=\"" + Constants.LINK_TO_FILE_COLUMN + "\" Nullable=\"TRUE\"/>";
    //        strFirstFields += "<FieldRef Name=\"" + Constants.LINK_VIEW_COLUMN + "\" Nullable=\"TRUE\"/>";
    //        strFirstFields += "<FieldRef Name=\"" + Constants.LINK_EDIT_COLUMN + "\" Nullable=\"TRUE\"/>";

    //        strFirstFields += "<FieldRef Name=\"" + Constants.LINK_WORKFLOW_COLUMN + "\" Nullable=\"TRUE\"/>";
    //        strFirstFields += "<FieldRef Name=\"" + Constants.LINK_SEND_ESIGN_COLUMN + "\" Nullable=\"TRUE\"/>";
    //        strFirstFields += "<FieldRef Name=\"" + Constants.LINK_ESIGN_HISTORY_COLUMN + "\" Nullable=\"TRUE\"/>";
    //        strFirstFields += "<FieldRef Name=\"" + Constants.LINK_VERSION_HISTORY_COLUMN + "\" Nullable=\"TRUE\"/>";
    //        strFirstFields += "<FieldRef Name=\"" + Constants.LINK_MANAGE_PERMISSIONS_COLUMN + "\" Nullable=\"TRUE\"/>";
    //        strFirstFields += "<FieldRef Name=\"" + Constants.LINK_DOCUMENT_DISCUSSIONS_COLUMN + "\" Nullable=\"TRUE\"/>";
    //        strFirstFields += "<FieldRef Name=\"" + Constants.LINK_UPLOAD_EXECUTED_COLUMN + "\" Nullable=\"TRUE\"/>";
    //        strFirstFields += "<FieldRef Name=\"" + Constants.LINK_ALERT_ME_COLUMN + "\" Nullable=\"TRUE\"/>";
    //        strFirstFields += "<FieldRef Name=\"" + Constants.LINK_SEND_TO_OTHER_LOCATION_COLUMN + "\" Nullable=\"TRUE\"/>";
    //        strFirstFields += "<FieldRef Name=\"" + Constants.LINK_SEND_TO_WORKSPACE_COLUMN + "\" Nullable=\"TRUE\"/>";
    //        strFirstFields += "<FieldRef Name=\"" + Constants.LINK_SEND_TO_EMAIL_COLUMN + "\" Nullable=\"TRUE\"/>";
    //        strFirstFields += "<FieldRef Name=\"" + Constants.LINK_SEND_TO_DOWNLOAD_COPY_COLUMN + "\" Nullable=\"TRUE\"/>";
    //        strFirstFields += "<FieldRef Name=\"" + Constants.LINK_SEND_TO_DOWNLOAD_PDF_COPY_COLUMN + "\" Nullable=\"TRUE\"/>";
    //        strFirstFields += "<FieldRef Name=\"" + Constants.LINK_RELATED_METRICS_COLUMN + "\" Nullable=\"TRUE\"/>";
    //        strFirstFields += "<FieldRef Name=\"" + Constants.LINK_RELATED_DOCUMENT_COLUMN + "\" Nullable=\"TRUE\"/>";
    //        strFirstFields += "<FieldRef Name=\"" + Constants.LINK_CREATE_RELATED_DOCUMENT_COLUMN + "\" Nullable=\"TRUE\"/>";
    //        strFirstFields += "<FieldRef Name=\"" + Constants.LINK_SEND_EEC_COLUMN + "\" Nullable=\"TRUE\"/>";
    //        strFirstFields += "<FieldRef Name=\"" + Constants.LINK_EMAIL_ATTACHMENT_COLUMN + "\" Nullable=\"TRUE\"/>";
    //        strFirstFields += "<FieldRef Name=\"" + Constants.LINK_CREATE_PDF_COLUMN + "\" Nullable=\"TRUE\"/>";
    //        strFirstFields += "<FieldRef Name=\"" + Constants.LINK_RELATED_RISKS_COLUMN + "\" Nullable=\"TRUE\"/>";

    //        for (int i = 0; i < resultFields.Count; i++)
    //        {
    //            SPField field = null;
    //            if (!ct.Fields.ContainFieldId(new Guid(resultFields[i].FieldId))) continue;

    //            if (ct != null) field = ct.Fields[new Guid(resultFields[i].FieldId)];
    //            if (field == null && ct != null)
    //            {
    //                if (ct.ParentList != null)
    //                    field = ct.ParentList.ParentWeb.Site.RootWeb.FindField(resultFields[i].FieldId);
    //                else
    //                    field = ct.ParentWeb.Site.RootWeb.FindField(resultFields[i].FieldId);
    //            }

    //            if (field == null)
    //                field = SPContext.Current.Web.FindField(resultFields[i].FieldId);

    //            string strTemp = "<FieldRef ID=\"{0}\"  Nullable=\"TRUE\" />";
    //            //ignore fields can't show
    //            switch (field.Type)
    //            {
    //                case SPFieldType.Lookup:
    //                //case SPFieldType.MultiChoice:
    //                case SPFieldType.User:
    //                case SPFieldType.Invalid:
    //                    continue;
    //                case SPFieldType.Computed:
    //                    SPFieldComputed computedField = (SPFieldComputed)field;
    //                    foreach (string f in computedField.FieldReferences)
    //                    {
    //                        strFirstFields += string.Format("<FieldRef Name=\"{0}\"  Nullable=\"TRUE\" />", f);
    //                    }
    //                    break;
    //                case SPFieldType.Integer:
    //                case SPFieldType.Number:
    //                case SPFieldType.DateTime:
    //                case SPFieldType.Currency:
    //                case SPFieldType.MultiChoice:
    //                case SPFieldType.Boolean:
    //                    break;
    //            }
    //            strLastFields += string.Format(strTemp, field.Id.ToString());

    //        }
    //        return strFirstFields + strLastFields;
    //    }

    //    public static string BuildViewFieldsXml(SPList list, List<FieldSetting> resultFields, List<FieldSetting> searchFields)
    //    {
    //        //return BuildViewFieldsXml(list, resultFields) + BuildViewFieldsXml(list, searchFields.GetRange(1, 10));
    //        return BuildViewFieldsXml(list, resultFields) + BuildViewFieldsXml(list, searchFields);
    //    }

    //    public static string BuildScopeSearchXml(int intType)
    //    {
    //        if (intType == 0) return string.Empty;
    //        if (intType == 1)
    //            return "<Webs Scope=\"Recursive\"/>";
    //        else
    //            return "<Webs Scope=\"SiteCollection\"/>";
    //    }

    //    public static string GetCAMLListBaseTypeSearch(SearchListBaseType listBaseType)
    //    {
    //        return string.Format("<Lists BaseType=\"{0}\" /></Lists>", ((int)listBaseType).ToString());
    //    }

    //    public static string GetCAMLQueryFromSearchDefinition(SearchDefinition searchDefinition)
    //    {
    //        string strReturn = string.Empty;
    //        string strANDCloseTag = string.Empty;
    //        string strTempReplace = string.Empty;
    //        strReturn += "<Where>";
    //        string strTemp = string.Empty;
    //        string strGlobalOperator = searchDefinition.UseGlobalOperatorOR ? "Or" : "And";
    //        for (int i = 0; i < searchDefinition.SearchCriteriaList.Count; i++)
    //        {
    //            switch (searchDefinition.SearchCriteriaList[i].FieldType)
    //            {
    //                case SPFieldType.Boolean:
    //                case SPFieldType.Currency:
    //                case SPFieldType.Integer:
    //                case SPFieldType.Number:
    //                case SPFieldType.Calculated:
    //                case SPFieldType.Computed:
    //                case SPFieldType.Note:
    //                case SPFieldType.Text:
    //                case SPFieldType.File:
    //                case SPFieldType.Choice:
    //                case SPFieldType.MultiChoice:
    //                    strTempReplace = "<{0}><FieldRef ID=\'"
    //                        + searchDefinition.SearchCriteriaList[i].FieldId + "' /><Value Type=\'"
    //                        + searchDefinition.SearchCriteriaList[i].FieldType.ToString() + "'>"
    //                        + searchDefinition.SearchCriteriaList[i].Value + "</Value></{0}>";

    //                    switch (searchDefinition.SearchCriteriaList[i].Operator)
    //                    {
    //                        case Operators.Equal:
    //                            strTemp = string.Format(strTempReplace, "Eq");
    //                            break;

    //                        case Operators.NotEqual:
    //                            strTemp = string.Format(strTempReplace, "Neq");
    //                            break;

    //                        case Operators.GreaterThan:
    //                            strTemp = string.Format(strTempReplace, "Gt");
    //                            break;

    //                        case Operators.LessThan:
    //                            strTemp = string.Format(strTempReplace, "Lt");
    //                            break;

    //                        case Operators.StartsWith:
    //                            strTemp = string.Format(strTempReplace, "BeginsWith");
    //                            break;

    //                        case Operators.Contains:
    //                            strTemp = string.Format(strTempReplace, "Contains");
    //                            break;
    //                    }
    //                    break;

    //                case SPFieldType.DateTime:
    //                    switch (searchDefinition.SearchCriteriaList[i].Operator)
    //                    {
    //                        case Operators.Equal:
    //                            strTempReplace = "<Eq><FieldRef ID=\"{0}\" /><Value Type=\"DateTime\" {1}>{2}</Value></Eq>";
    //                            break;
    //                        case Operators.NotEqual:
    //                            strTempReplace = "<Neq><FieldRef ID=\"{0}\" /><Value Type=\"DateTime\" {1}>{2}</Value></Neq>";
    //                            break;
    //                        case Operators.EarlierThan:
    //                            strTempReplace = "<Lt><FieldRef ID=\"{0}\" /><Value Type=\"DateTime\" {1}>{2}</Value></Lt>";
    //                            break;
    //                        case Operators.LaterThan:
    //                            strTempReplace = "<Gt><FieldRef ID=\"{0}\" /><Value Type=\"DateTime\" {1}>{2}</Value></Gt>";
    //                            break;
    //                    }
    //                    if (!searchDefinition.SearchCriteriaList[i].IsTimeIncluded)
    //                        strTemp = (string.Format(strTempReplace, searchDefinition.SearchCriteriaList[i].FieldId, string.Empty, searchDefinition.SearchCriteriaList[i].Value));
    //                    else
    //                        strTemp = (string.Format(strTempReplace, searchDefinition.SearchCriteriaList[i].FieldId, "IncludeTimeValue='TRUE'", searchDefinition.SearchCriteriaList[i].Value));
    //                    break;

    //                case SPFieldType.Lookup:
    //                    switch (searchDefinition.SearchCriteriaList[i].Operator)
    //                    {
    //                        case Operators.Equal:
    //                            strTempReplace = "<Eq><FieldRef ID=\"{0}\" /><Value Type=\"Lookup\" >{1}</Value></Eq>";
    //                            break;
    //                        case Operators.NotEqual:
    //                            strTempReplace = "<Neq><FieldRef ID=\"{0}\" /><Value Type=\"Lookup\" >{1}</Value></Neq>";
    //                            break;
    //                        case Operators.Contains:
    //                            strTempReplace = "<Contains><FieldRef ID=\"{0}\" /><Value Type=\"Lookup\" >{1}</Value></Contains>";
    //                            break;
    //                    }
    //                    strTemp = (string.Format(strTempReplace, searchDefinition.SearchCriteriaList[i].FieldId, searchDefinition.SearchCriteriaList[i].Value));
    //                    break;

    //                case SPFieldType.User:
    //                    switch (searchDefinition.SearchCriteriaList[i].Operator)
    //                    {
    //                        case Operators.Equal:
    //                            strTempReplace = "<Eq><FieldRef ID=\"{0}\"  LookupId=\"TRUE\"/><Value Type=\"Integer\" >{1}</Value></Eq>";
    //                            break;
    //                        case Operators.NotEqual:
    //                            strTempReplace = "<Neq><FieldRef ID=\"{0}\" LookupId=\"TRUE\"/><Value Type=\"Integer\" >{1}</Value></Neq>";
    //                            break;
    //                        case Operators.Contains:
    //                            strTempReplace = "<Contains><FieldRef ID=\"{0}\" LookupId=\"TRUE\"/><Value Type=\"Integer\" >{1}</Value></Contains>";
    //                            break;
    //                    }
    //                    strTemp = (string.Format(strTempReplace, searchDefinition.SearchCriteriaList[i].FieldId, searchDefinition.SearchCriteriaList[i].Value.Split(";#".ToCharArray())[0]));
    //                    break;
    //            }

    //            if (i < searchDefinition.SearchCriteriaList.Count - 1)
    //            {
    //                strReturn += "<" + strGlobalOperator + ">";
    //                strReturn += strTemp;
    //                strANDCloseTag += "</" + strGlobalOperator + ">";
    //            }
    //        }
    //        strReturn += strTemp + strANDCloseTag;
    //        strReturn += "</Where>";

    //        return strReturn;
    //    }

    //    public static string GetCAMLScopeSearch(int intType)
    //    {
    //        if (intType == 0) return string.Empty;
    //        if (intType == 1)
    //            return "<Webs Scope=\"Recursive\"/>";
    //        else
    //            return "<Webs Scope=\"SiteCollection\"/>";
    //    }

    //    public static string GetCAMLFieldsResult(SPWeb web, SPList list, List<FieldSetting> ListResultFields)
    //    {
    //        string strFirstFields = "<FieldRef Name=\"EncodedAbsUrl\" Nullable=\"TRUE\"/>";
    //        string strLastFields = string.Empty;
    //        strFirstFields += "<FieldRef Name=\"FileRef\" Nullable=\"TRUE\"/>";
    //        strFirstFields += "<FieldRef Name=\"" + Constants.LINK_TO_FILE_COLUMN + "\" Nullable=\"TRUE\"/>";
    //        strFirstFields += "<FieldRef Name=\"" + Constants.LINK_VIEW_COLUMN + "\" Nullable=\"TRUE\"/>";
    //        strFirstFields += "<FieldRef Name=\"" + Constants.LINK_EDIT_COLUMN + "\" Nullable=\"TRUE\"/>";
    //        for (int i = 0; i < ListResultFields.Count; i++)
    //        {
    //            SPField field = null;
    //            if (list != null) field = list.Fields[new Guid(ListResultFields[i].FieldId)];
    //            if (field == null)
    //                field = web.FindField(ListResultFields[i].FieldId);
    //            string strTemp = "<FieldRef ID=\"{0}\"  Nullable=\"TRUE\" />";
    //            //ignore fields can't show
    //            switch (field.Type)
    //            {
    //                case SPFieldType.Lookup:
    //                //case SPFieldType.MultiChoice:
    //                case SPFieldType.User:
    //                    continue;
    //                case SPFieldType.Computed:
    //                    SPFieldComputed computedField = (SPFieldComputed)field;
    //                    foreach (string f in computedField.FieldReferences)
    //                    {
    //                        strFirstFields += (string.Format("<FieldRef Name=\"{0}\"  Nullable=\"TRUE\" />", f));
    //                    }
    //                    break;
    //                case SPFieldType.Integer:
    //                case SPFieldType.Number:
    //                case SPFieldType.DateTime:
    //                case SPFieldType.Currency:
    //                case SPFieldType.MultiChoice:
    //                case SPFieldType.Boolean:
    //                    break;
    //            }
    //            strLastFields += (string.Format(strTemp, field.Id.ToString()));

    //        }
    //        return strFirstFields + strLastFields;
    //    }

    //    private static bool isValidKeyToday(string keyInput, ref int daysNumber)
    //    {
    //        keyInput = keyInput.Trim().ToLower();
    //        if (string.Compare(keyInput, Constants.KEY_TODAY, true) == 0)
    //            return true;

    //        if (keyInput.Contains(Constants.KEY_TODAY) == false)
    //            return false;

    //        keyInput = keyInput.Replace(" ", "");
    //        Regex regex = new Regex(@"^\[(?<key>[a-z]*)\](?<calc>[+-]+)(?<number>[\d]+$)");
    //        MatchCollection mats = regex.Matches(keyInput);
    //        if (mats.Count == 0)
    //            return false;
    //        string value = mats[0].Groups["calc"].Value + mats[0].Groups["number"].Value;
    //        int.TryParse(value, out daysNumber);

    //        return true;
    //    }

    //    public static string combineQuery(string query1, string query2, bool useGlobalOperatorOR)
    //    {
    //        if (string.IsNullOrEmpty(query1.Trim()))
    //            return query2;

    //        if (string.IsNullOrEmpty(query2.Trim()))
    //            return query1;

    //        string strGlobalOperator = useGlobalOperatorOR ? "Or" : "And";
    //        string temp1,temp2;
    //        if (query1.Trim().StartsWith("<Where>"))
    //            temp1 = query1.Remove(query1.LastIndexOf("</Where>"));
    //        else
    //            temp1 = "<Where>" + query1;

    //        if (query2.Trim().EndsWith("</Where>"))
    //            temp2 = query2.Substring(query1.IndexOf("<Where>") + 7);
    //        else
    //            temp2 = query2 + "</Where>";

    //        string returnString = temp1.Replace("<Where>", "<Where> <{0}>") + temp2.Replace("</Where>", "</{0}> </Where>");

    //        return string.Format(returnString, strGlobalOperator); 
    //    }
    //}


    //class SearchCriteriaNode
    //{
    //    public bool IsLeaf { get; set; }
    //    public SearchCriteria Criteria { get; set; }
    //    public string CAML
    //    {
    //        get
    //        {
    //            return GetCamlForSearchCriteria(this.Criteria);
    //        }
    //    }
    //    public string Operator { get; set; }
    //    public List<SearchCriteriaNode> Nodes { get; set; }
    //    public SearchCriteriaNode()
    //    {
    //        Nodes = new List<SearchCriteriaNode>();
    //    }
    //    private static bool isValidKeyToday(string keyInput, ref int daysNumber)
    //    {
    //        keyInput = keyInput.Trim().ToLower();
    //        if (string.Compare(keyInput, Constants.KEY_TODAY, true) == 0)
    //            return true;

    //        if (keyInput.Contains(Constants.KEY_TODAY) == false)
    //            return false;

    //        keyInput = keyInput.Replace(" ", "");
    //        Regex regex = new Regex(@"^\[(?<key>[a-z]*)\](?<calc>[+-]+)(?<number>[\d]+$)");
    //        MatchCollection mats = regex.Matches(keyInput);
    //        if (mats.Count == 0)
    //            return false;
    //        string value = mats[0].Groups["calc"].Value + mats[0].Groups["number"].Value;
    //        int.TryParse(value, out daysNumber);

    //        return true;
    //    }
    //    public static SearchCriteriaNode ConvertToTree(List<SearchCriteria> searchCriterias, string globalOpt)
    //    {
    //        List<SearchCriteriaNode> nodes = new List<SearchCriteriaNode>();
    //        var grouped = searchCriterias.GroupBy(p => p.FieldId);
    //        int count = grouped.Count();
    //        foreach (var item in grouped)
    //        {
    //            if (item.Count() == 1)
    //            {
    //                var temp = item.ElementAt(0);
    //                if(temp.SubCriterias == null || temp.SubCriterias.Count==0)
    //                temp.RelatedOperator = string.Empty;
    //            }

    //            //List

    //            List<SearchCriteriaNode> subNode = CreateSearchCriteriaNodeForList(item.ToList(), "Or", globalOpt);
    //            if (subNode.Count == 1) nodes.Add(subNode[0]);
    //            else
    //            {
    //                SearchCriteriaNode n = new SearchCriteriaNode()
    //                {
    //                    Operator = globalOpt
    //                };
    //                n.Nodes.AddRange(subNode.ToArray());
    //                nodes.Add(n);
    //            }
    //        }

    //        List<SearchCriteriaNode> results = ConvertNodeList(nodes, globalOpt);

    //        if (results.Count == 1)
    //        {
                
    //            return results[0];
    //        }
    //        SearchCriteriaNode parent = new SearchCriteriaNode()
    //        {
    //            Operator = globalOpt
    //        };

    //        parent.Nodes.AddRange(results.ToArray());
    //        return parent;

    //    }
    //    //TODO : Cleanup this function
    //    private static List<SearchCriteriaNode> CreateSearchCriteriaNodeForList(List<SearchCriteria> list, string op, string global)
    //    {
    //        List<SearchCriteriaNode> nodes = new List<SearchCriteriaNode>();
    //        global = list[0].RelatedOperator;

    //        foreach (var n in list)
    //        {
    //             List<SearchCriteriaNode>  temp = new List<SearchCriteriaNode>();
    //            temp.Add(new SearchCriteriaNode() { Criteria = n, IsLeaf=true });
    //            if (n.SubCriterias != null && n.SubCriterias.Count>0)
    //            {
    //                //global = n.RelatedOperator;
    //                n.RelatedOperator = "Or";
    //                foreach (var item in n.SubCriterias)
    //                {
    //                    //item.RelatedOperator = "Or";

    //                    temp.Add(new SearchCriteriaNode() { Criteria = item , IsLeaf=true});
    //                }
    //            }
    //            List<SearchCriteriaNode> temp1 = ConvertNodeListEx(temp, "Or");
    //            //if(temp.Count>1)
    //            //temp1[0].Operator = global;
    //            nodes.AddRange(temp1);

    //        }
    //        List<SearchCriteriaNode> results = ConvertNodeListEx(nodes, global);
            
    //        return results;
          
    //    }

    //    private static List<SearchCriteriaNode> ConvertNodeListEx(List<SearchCriteriaNode> nodes, string op)
    //    {
    //        if (nodes.Count == 0) return null;

    //        SearchCriteriaNode newNode = nodes[0];
    //        SearchCriteriaNode previousNode = null;

    //        int i = 1;
    //        string previousOpt = "";
    //        while (i < nodes.Count)
    //        {
    //            previousNode = nodes[i++];

    //            SearchCriteriaNode temp = new SearchCriteriaNode()
    //            {
    //               Operator = newNode.Criteria != null ? newNode.Criteria.RelatedOperator :previousNode.Criteria!=null? previousNode.Criteria.RelatedOperator: op
    //               //Operator = op
    //            };

    //            if (!string.IsNullOrEmpty(previousOpt)) temp.Operator = previousOpt;
    //            else
    //            if (!newNode.IsLeaf && previousNode.IsLeaf || !newNode.IsLeaf && !previousNode.IsLeaf)
    //                temp.Operator = op;
                
                
    //            temp.Nodes.Add(newNode);
    //            temp.Nodes.Add(previousNode);
    //            newNode = temp;
    //            previousOpt =previousNode.Criteria!=null? previousNode.Criteria.RelatedOperator:string.Empty;
                
    //        };

    //        return new List<SearchCriteriaNode>() { newNode };
    //    }
    //    private static List<SearchCriteriaNode> ConvertNodeList(List<SearchCriteriaNode> nodes, string op)
    //    {
    //        if (nodes.Count ==1) return  nodes;
    //        if (nodes.Count == 2)
    //        {
    //            if (nodes[0].Nodes.Count == 2 && nodes[1].Nodes.Count == 1)  nodes.Reverse();
    //            return nodes;

    //        }

    //        List<SearchCriteriaNode> result = new List<SearchCriteriaNode>();
    //        int i = 0;
    //        do
    //        {
    //            SearchCriteriaNode n1 = nodes[i++];
    //            SearchCriteriaNode n2 = null;
    //            if (i < nodes.Count) n2 = nodes[i++];

    //            if (n1 != null && n2 != null)
    //            {
    //                string applyOperator = GetApplyOperator(n1, n2, op);
    //                SearchCriteriaNode newNode = new SearchCriteriaNode() { Operator = applyOperator, IsLeaf = true };
    //                newNode.Nodes.Add(n1);
    //                newNode.Nodes.Add(n2);
    //                result.Add(newNode);
    //            }
    //            else
    //                result.Add(n1);

    //        }
    //        while (i < nodes.Count);
    //        return ConvertNodeList(result, op);
    //    }
        

    //    private static string GetApplyOperator(SearchCriteriaNode n1, SearchCriteriaNode n2, string op)
    //    {
    //        if (n1.Criteria != null && n2.Criteria != null)
    //        {
    //            if ((n1.Criteria.RelatedOperator == "" && n2.Criteria.RelatedOperator == "") || n1.Criteria.IsContentTypeDropDown){
    //                return op;
    //            }

    //            if ((n1.Criteria.RelatedOperator == "Or" || n1.Criteria.RelatedOperator == "") && (string.IsNullOrEmpty(n2.Criteria.RelatedOperator) || n2.Criteria.RelatedOperator == "Or"))
    //                return "Or";
    //            else
    //                return "And";
    //        }
    //        if (n1.Criteria == null && n2.Criteria == null)
    //        {
    //            if (n1.Operator == "Or" && n1.Operator == "Or") return "Or";
    //            else
    //                return "And";
    //        }


    //        return op;


    //    }

    //    private static SearchCriteriaNode AddNode(SearchCriteriaNode root, SearchCriteriaNode child, int count)
    //    {
    //        if (root.Nodes.Count < 2)
    //        {
    //            root.Nodes.Add(child);
    //            return root;
    //        }
    //        SearchCriteriaNode newNode = new SearchCriteriaNode();
    //        newNode.Nodes.Add(root);
    //        newNode.Nodes.Add(child);

    //        return newNode;

    //    }

    //    private static SearchCriteriaNode CreateNewNode(IGrouping<string, SearchCriteria> items)
    //    {
    //        SearchCriteriaNode result = new SearchCriteriaNode();
    //        int count = items.Count();
    //        foreach (var item in items)
    //        {

    //            SearchCriteriaNode newNode = new SearchCriteriaNode()
    //            {
    //                Criteria = item,
    //            };
    //            count--;
    //            result = AddNode(result, newNode, count);

    //        }
    //        return result;
    //    }
    //    private static void PrintNode(SearchCriteriaNode node, StringWriter writer)
    //    {

    //        string begin = node.Criteria == null ? string.Format("<{0}>", node.Operator) :  node.CAML;

    //        writer.Write(begin);

    //        foreach (SearchCriteriaNode n in node.Nodes)
    //        {
    //            PrintNode(n, writer);
    //        }

    //        if (node.Criteria == null)
    //            writer.Write("</{0}>", node.Operator);
    //    }

    //    private static void PrintNode(SearchCriteriaNode node, int p, StringWriter writer)
    //    {
    //        string tab = "";
    //        for (int i = 0; i < p; i++) tab += "  ";
    //        string begin = node.Criteria == null ? string.Format("{1}<{0}>", node.Operator, tab) : tab + node.CAML;


    //        writer.WriteLine(begin);

    //        foreach (SearchCriteriaNode n in node.Nodes)
    //        {
    //            PrintNode(n, p + 1, writer);
    //        }

    //        if (node.Criteria == null)
    //            writer.WriteLine("{1}</{0}>", node.Operator, tab);
    //    }

    //    public string GetCAMLQuery()
    //    {
    //        StringBuilder sb = new StringBuilder();
    //        StringWriter writer = new StringWriter(sb);
    //        writer.Write("<Where>");
    //        SearchCriteriaNode.PrintNode(this, writer);
    //        writer.Write("</Where>");
    //        writer.Close();
    //        return sb.ToString();
    //    }

    //    private string GetCamlForSearchCriteria(SearchCriteria criteria)
    //    {
    //       string strTempReplace = "";
    //        string strTemp = "";
    //        string strTempNullPattern = "<{0}><FieldRef ID=\'" + criteria.FieldId + "' /></{0}>";
    //        if (criteria.UseKeyword)
    //        {
    //            // fieldtype is user and keyword is [Me]
    //            if (string.Compare(criteria.Value.Trim(), Constants.KEY_ME, true) == 0 &&
    //                criteria.FieldType == SPFieldType.User)
    //            {
    //                switch (criteria.Operator)
    //                {
    //                    case Operators.Equal:
    //                        strTempReplace = "<Eq><FieldRef ID=\"{0}\" /><Value Type=\"Integer\" ><UserID /></Value></Eq>";
    //                        break;
    //                    case Operators.NotEqual:
    //                        strTempReplace = "<Neq><FieldRef ID=\"{0}\"/><Value Type=\"Integer\" ><UserID /></Value></Neq>";
    //                        break;
    //                    case Operators.Contains:
    //                        strTempReplace = "<Contains><FieldRef ID=\"{0}\" /><Value Type=\"Integer\" ><UserID /></Value></Contains>";
    //                        break;
    //                }
    //                strTemp = string.Format(strTempReplace, criteria.FieldId);
    //            }
    //            else
    //            {
    //                // fieldtype is user and keyword is [Today]
    //                int daysNumber = 0;
    //                if (isValidKeyToday(criteria.Value, ref daysNumber) &&
    //                    criteria.FieldType == SPFieldType.DateTime)
    //                {

    //                    switch (criteria.Operator)
    //                    {
    //                        case Operators.Equal:
    //                            strTempReplace = "<Eq><FieldRef ID=\"{0}\" /><Value Type=\"DateTime\" > <Today {1} /></Value></Eq>";
    //                            break;
    //                        case Operators.NotEqual:
    //                            strTempReplace = "<Neq><FieldRef ID=\"{0}\" /><Value Type=\"DateTime\" ><Today {1} /></Value></Neq>";
    //                            break;
    //                        case Operators.EarlierThan:
    //                            strTempReplace = "<Lt><FieldRef ID=\"{0}\" /><Value Type=\"DateTime\" ><Today {1} /></Value></Lt>";
    //                            break;
    //                        case Operators.LaterThan:
    //                            strTempReplace = "<Gt><FieldRef ID=\"{0}\" /><Value Type=\"DateTime\" ><Today {1} /></Value></Gt>";
    //                            break;
    //                    }

    //                    if (daysNumber == 0)
    //                        strTemp = string.Format(strTempReplace, criteria.FieldId, string.Empty);
    //                    else
    //                        strTemp = string.Format(strTempReplace, criteria.FieldId, "OffsetDays=\"" + daysNumber.ToString() + "\"");
    //                }
    //                else
    //                {
    //                    strTempReplace = "<{0}><FieldRef ID=\'"
    //                        + criteria.FieldId + "' /><Value Type=\'"
    //                        + criteria.FieldType.ToString() + "'>"
    //                        + criteria.Value + "</Value></{0}>";

    //                    switch (criteria.Operator)
    //                    {
    //                        case Operators.Equal:
    //                            strTemp = string.Format(strTempReplace, "Eq");
    //                            break;

    //                        case Operators.NotEqual:
    //                            strTemp = string.Format(strTempReplace, "Neq");
    //                            break;

    //                        case Operators.GreaterThan:
    //                            strTemp = string.Format(strTempReplace, "Gt");
    //                            break;

    //                        case Operators.LessThan:
    //                            strTemp = string.Format(strTempReplace, "Lt");
    //                            break;

    //                        case Operators.StartsWith:
    //                            strTemp = string.Format(strTempReplace, "BeginsWith");
    //                            break;

    //                        case Operators.Contains:
    //                            strTemp = string.Format(strTempReplace, "Contains");
    //                            break;
                            
    //                        case Operators.IsNull:
    //                            strTemp = string.Format(strTempNullPattern, "IsNull");
    //                            break;
    //                    }
    //                }
    //            }
    //        }
    //        // not use keyword
    //        else
    //        {
    //            switch (criteria.FieldType)
    //            {
    //                case SPFieldType.Calculated:
    //                    strTemp = GetCAMLOfCalculatedField(criteria);
    //                    break;

    //                case SPFieldType.Boolean:
    //                case SPFieldType.Currency:
    //                case SPFieldType.Integer:
    //                case SPFieldType.Number:
    //                //case SPFieldType.Calculated:
    //                case SPFieldType.Computed:
    //                case SPFieldType.Note:
    //                case SPFieldType.Text:
    //                case SPFieldType.File:
    //                case SPFieldType.Choice:
    //                case SPFieldType.MultiChoice:
    //                    strTempReplace = "<{0}><FieldRef ID=\'"
    //                        + criteria.FieldId + "' /><Value Type=\'"
    //                        + criteria.FieldType.ToString() + "'>"
    //                        + criteria.Value + "</Value></{0}>";

    //                    switch (criteria.Operator)
    //                    {
    //                        case Operators.Equal:
    //                            strTemp = string.Format(strTempReplace, "Eq");
    //                            break;

    //                        case Operators.NotEqual:
    //                            strTemp = string.Format(strTempReplace, "Neq");
    //                            break;

    //                        case Operators.GreaterThan:
    //                            strTemp = string.Format(strTempReplace, "Gt");
    //                            break;

    //                        case Operators.LessThan:
    //                            strTemp = string.Format(strTempReplace, "Lt");
    //                            break;

    //                        case Operators.StartsWith:
    //                            strTemp = string.Format(strTempReplace, "BeginsWith");
    //                            break;

    //                        case Operators.Contains:
    //                            strTemp = string.Format(strTempReplace, "Contains");
    //                            break;

    //                        case Operators.IsNull:
    //                            strTemp = string.Format(strTempNullPattern, "IsNull");
    //                            break;
    //                    }
    //                    break;

    //                case SPFieldType.DateTime:
    //                    if (criteria.Operator == Operators.IsNull)
    //                    {
    //                        strTemp = string.Format(strTempNullPattern, "IsNull");
    //                        break;
    //                    }

    //                    switch (criteria.Operator)
    //                    {
    //                        case Operators.Equal:
    //                            strTempReplace = "<Eq><FieldRef ID=\"{0}\" /><Value Type=\"DateTime\" {1}>{2}</Value></Eq>";
    //                            break;
    //                        case Operators.NotEqual:
    //                            strTempReplace = "<Neq><FieldRef ID=\"{0}\" /><Value Type=\"DateTime\" {1}>{2}</Value></Neq>";
    //                            break;
    //                        case Operators.EarlierThan:
    //                            strTempReplace = "<Lt><FieldRef ID=\"{0}\" /><Value Type=\"DateTime\" {1}>{2}</Value></Lt>";
    //                            break;
    //                        case Operators.LaterThan:
    //                            strTempReplace = "<Gt><FieldRef ID=\"{0}\" /><Value Type=\"DateTime\" {1}>{2}</Value></Gt>";
    //                            break;
    //                    }
    //                    if (!criteria.IsTimeIncluded)
    //                        strTemp = string.Format(strTempReplace, criteria.FieldId, string.Empty, criteria.Value);
    //                    else
    //                        strTemp = string.Format(strTempReplace, criteria.FieldId, "IncludeTimeValue='TRUE'", criteria.Value);
    //                    break;

    //                case SPFieldType.Lookup:
    //                    if (criteria.Operator == Operators.IsNull)
    //                    {
    //                        strTemp = string.Format(strTempNullPattern, "IsNull");
    //                        break;
    //                    }

    //                    switch (criteria.Operator)
    //                    {
    //                        case Operators.Equal:
    //                            strTempReplace = "<Eq><FieldRef ID=\"{0}\" /><Value Type=\"Lookup\" >{1}</Value></Eq>";
    //                            break;
    //                        case Operators.NotEqual:
    //                            strTempReplace = "<Neq><FieldRef ID=\"{0}\" /><Value Type=\"Lookup\" >{1}</Value></Neq>";
    //                            break;
    //                        case Operators.Contains:
    //                            strTempReplace = "<Contains><FieldRef ID=\"{0}\" /><Value Type=\"Lookup\" >{1}</Value></Contains>";
    //                            break;
    //                    }
    //                    strTemp = (string.Format(strTempReplace, criteria.FieldId, criteria.Value));
    //                    break;

    //                case SPFieldType.User:
    //                    if (criteria.Operator == Operators.IsNull)
    //                    {
    //                        strTemp = string.Format(strTempNullPattern, "IsNull");
    //                        break;
    //                    }
    //                    switch (criteria.Operator)
    //                    {
    //                        case Operators.Equal:
    //                            strTempReplace = "<Eq><FieldRef ID=\"{0}\"  LookupId=\"TRUE\"/><Value Type=\"Integer\" >{1}</Value></Eq>";
    //                            break;
    //                        case Operators.NotEqual:
    //                            strTempReplace = "<Neq><FieldRef ID=\"{0}\" LookupId=\"TRUE\"/><Value Type=\"Integer\" >{1}</Value></Neq>";
    //                            break;
    //                        case Operators.Contains:
    //                            strTempReplace = "<Contains><FieldRef ID=\"{0}\" LookupId=\"TRUE\"/><Value Type=\"Integer\" >{1}</Value></Contains>";
    //                            break;
    //                    }
    //                    strTemp = string.Format(strTempReplace, criteria.FieldId, string.IsNullOrEmpty(criteria.Value) ? "" : criteria.Value.Split(";#".ToCharArray())[0]);
    //                    break;

    //                case SPFieldType.Invalid:
    //                    if (string.Compare(criteria.FieldTypeName,
    //                        Constants.LOOKUP_WITH_PICKER_TYPE_NAME, true) == 0)
    //                    {
    //                        strTempReplace = "<{0}><FieldRef ID=\'"
    //                       + criteria.FieldId + "' /><Value Type=\'"
    //                       + criteria.FieldType.ToString() + "'>"
    //                       + criteria.Value + "</Value></{0}>";

    //                        switch (criteria.Operator)
    //                        {
    //                            case Operators.Equal:
    //                                strTemp = string.Format(strTempReplace, "Eq");
    //                                break;

    //                            case Operators.NotEqual:
    //                                strTemp = string.Format(strTempReplace, "Neq");
    //                                break;

    //                            case Operators.GreaterThan:
    //                                strTemp = string.Format(strTempReplace, "Gt");
    //                                break;

    //                            case Operators.LessThan:
    //                                strTemp = string.Format(strTempReplace, "Lt");
    //                                break;

    //                            case Operators.StartsWith:
    //                                strTemp = string.Format(strTempReplace, "BeginsWith");
    //                                break;

    //                            case Operators.Contains:
    //                                strTemp = string.Format(strTempReplace, "Contains");
    //                                break;

    //                            case Operators.IsNull:
    //                                strTemp = string.Format(strTempNullPattern, "IsNull");
    //                                break;
    //                        }
    //                    }
    //                    break;
    //            }
    //        }
    //        return strTemp;
    //    }

    //    public static string GetCAMLOfCalculatedField(SearchCriteria searchCriteria)
    //    {
    //        string caml = string.Empty;
    //        string strTempNullPattern = "<{0}><FieldRef ID=\'" + searchCriteria.FieldId + "' /></{0}>";

    //        string strTempCommonPattern = "<{0}><FieldRef ID=\'"
    //                      + searchCriteria.FieldId + "' /><Value Type=\'"
    //                      + searchCriteria.CalculatedOutputType.ToString() + "'>"
    //                      + searchCriteria.Value + "</Value></{0}>";

    //        //string strTempDateTypePattern = string.Empty;
    //        if (searchCriteria.CalculatedOutputType == SPFieldType.DateTime && searchCriteria.IsTimeIncluded)
    //        {
    //            strTempCommonPattern = "<{0}><FieldRef ID=\'"
    //                      + searchCriteria.FieldId + "' /><Value Type=\'"
    //                      + searchCriteria.FieldType.ToString() + "' IncludeTimeValue='TRUE'>"
    //                      + searchCriteria.Value + "</Value></{0}>";
    //        }

    //        switch (searchCriteria.Operator)
    //        {
    //            case Operators.Equal:
    //                caml = string.Format(strTempCommonPattern, "Eq");
    //                break;

    //            case Operators.NotEqual:
    //                caml = string.Format(strTempCommonPattern, "Neq");
    //                break;

    //            case Operators.GreaterThan:
    //                caml = string.Format(strTempCommonPattern, "Gt");
    //                break;

    //            case Operators.LessThan:
    //                caml = string.Format(strTempCommonPattern, "Lt");
    //                break;

    //            case Operators.StartsWith:
    //                caml = string.Format(strTempCommonPattern, "BeginsWith");
    //                break;

    //            case Operators.Contains:
    //                caml = string.Format(strTempCommonPattern, "Contains");
    //                break;

    //            case Operators.IsNull:
    //                caml = string.Format(strTempNullPattern, "IsNull");
    //                break;
    //        }
           

    //        return caml;
    //    }
    }
}
