using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SharePoint;
using System.Reflection;
using System.Xml.Serialization;

namespace AIA.Intranet.Model.Entities
{
    public interface IBaseEntity
    {
    }
    public class BaseEntity : IBaseEntity
    {
        public string Title { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        [Field(Ignore=true)]
        public int CreateByID { get; set; }
        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }

        [Field(Ignore = true)]
        public string ApprovalStatus { get; set; }
        
        [Field(FieldName="Author")]
        public string CreatedBy { get; set; }
         [Field(FieldName = "Editor")]
        public string ModifiedBy { get; set; }

        [Field(Ignore = true)]
        public int ID { get; set; }
        public BaseEntity(){}

        [Field(Ignore=true)]
        [XmlIgnore]
        public SPListItem OrginalItem { get; set; }

        public BaseEntity(SPListItem listItem)
        {
            OrginalItem = listItem;
            Type type = this.GetType();
            var properties = type.GetProperties(System.Reflection.BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetProperty);

            foreach (var item in properties)
            {
                var attrbutes = item.GetCustomAttributes(typeof(FieldAttribute), true);
                string staticName = item.Name;
                if (attrbutes != null && attrbutes.Length > 0)
                {
                    if (((FieldAttribute)attrbutes[0]).Ignore) continue;
                    staticName = ((FieldAttribute)attrbutes[0]).FieldName;

                }
                if (listItem.Fields.ContainsFieldWithStaticName(staticName) && listItem[staticName] != null)
                {
                    try
                    {
                        var obj = listItem[staticName];
                        var field = listItem.ParentList.Fields.Cast<SPField>().Where(p => p.StaticName == staticName).FirstOrDefault();
                        //var field = listItem.ParentList.Fields[item.Name];
                        object data = null;
                        switch (field.Type)
                        {
                            //case SPFieldType.AllDayEvent:
                            //    break;
                            //case SPFieldType.Attachments:
                            //    break;
                            //case SPFieldType.Boolean:
                            //    break;
                            //case SPFieldType.Calculated:
                            //    break;
                            //case SPFieldType.Choice:
                            //    break;
                            //case SPFieldType.Computed:
                            //    break;
                            //case SPFieldType.ContentTypeId:
                            //    break;
                            //case SPFieldType.Counter:
                            //    break;
                            //case SPFieldType.CrossProjectLink:
                            //    break;
                            //case SPFieldType.Currency:
                            //    break;
                            //case SPFieldType.DateTime:
                            //    break;
                            //case SPFieldType.Error:
                            //    break;
                            //case SPFieldType.File:
                            //    break;
                            //case SPFieldType.GridChoice:
                            //    break;
                            //case SPFieldType.Guid:
                            //    break;
                            //case SPFieldType.Integer:
                            //    break;
                            //case SPFieldType.Invalid:
                            //    break;
                            //case SPFieldType.Lookup:
                            //    break;
                            //case SPFieldType.MaxItems:
                            //    break;
                            //case SPFieldType.ModStat:
                            //    break;
                            //case SPFieldType.MultiChoice:
                            //    break;
                            //case SPFieldType.Note:
                            //    break;
                            case SPFieldType.Number:
                                data = Convert.ToDecimal(obj);
                                break;
                            //case SPFieldType.PageSeparator:
                            //    break;
                            //case SPFieldType.Recurrence:
                            //    break;
                            //case SPFieldType.Text:
                            //    break;
                            //case SPFieldType.ThreadIndex:
                            //    break;
                            //case SPFieldType.Threading:
                            //    break;
                            case SPFieldType.URL:
                                data = obj.ToString().Split(new char[] { ',' })[0];

                                break;
                            //case SPFieldType.User:
                            //    break;
                            //case SPFieldType.WorkflowEventType:
                            //    break;
                            //case SPFieldType.WorkflowStatus:
                            //    break;
                            default:
                                data = obj;
                                break;
                        }
                        Object objValue = Convert.ChangeType(data, item.PropertyType);
                        item.SetValue(this, objValue, null);
                    }
                    catch (Exception ex)
                    {

                    }

                }
            }
            this.CreatedDate = Convert.ToDateTime(listItem[SPBuiltInFieldId.Created]);
            this.ModifiedDate = Convert.ToDateTime(listItem[SPBuiltInFieldId.Modified]);
            this.ApprovalStatus = listItem[SPBuiltInFieldId._ModerationStatus].ToString();
            this.CreateByID = Convert.ToInt32(this.CreatedBy.Split(";#".ToCharArray())[0]);
            this.ID = listItem.ID;
            if (listItem.ContentTypeId.IsChildOf(SPBuiltInContentTypeId.Document))
            {
                FileUrl = listItem["EncodedAbsUrl"].ToString();//+ listItem["FileRef"].ToString().Substring(listItem["FileRef"].ToString().IndexOf("#") + 1);
            }
        }

        [Field(Ignore=true)]
        public string FileUrl { get; set; }
        public BaseEntity(System.Data.DataRow row)
        {
            Type type = this.GetType();
            var properties = type.GetProperties(System.Reflection.BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetProperty);

            foreach (var item in properties)
            {
                var attrs = item.GetCustomAttributes(typeof(FieldAttribute), true);
                string fieldName = item.Name;
                string fieldType = "Text";
                
                if (attrs != null && attrs.Length > 0)
                {
                    FieldAttribute fieldAttr = attrs[0] as FieldAttribute;
                    if (fieldAttr.Ignore) continue;
                    fieldType = fieldAttr.Type;
                    fieldName = fieldAttr.FieldName;
                }
                try
                {
                    if (row[fieldName] != null)
                    {

                        var obj = row[fieldName];
                        item.SetValue(this, obj, null);
                    }
                }
                    catch(Exception ex){}

            }

            this.CreatedDate = Convert.ToDateTime(row["Created"]);

            string relativeUrl =row["FileRef"].ToString().Substring(
            row["FileRef"].ToString().IndexOf("#") + 1);
            this.FileUrl = relativeUrl;
        }


        
    }

}
