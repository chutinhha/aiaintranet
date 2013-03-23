using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SharePoint.WebControls;
using System.Web.UI.WebControls;
using Microsoft.SharePoint;
using AIA.Intranet.Common.Utilities;

namespace AIA.Intranet.Infrastructure.CustomFields
{
    public class AssignmentFieldControl : BaseFieldControl
    {

        protected override string DefaultTemplateName
        {
            get
            {
                if (this.ControlMode == SPControlMode.Display)
                {
                    return this.DisplayTemplateName;
                }
                else
                {
                    return "AssignmentFieldControlTemplate";
                }
            }
        }
        public override string DisplayTemplateName
        {
            get
            {
                return "AssignmentFieldControlForDisplayTemplate";
            }
            set
            {
                base.DisplayTemplateName = value;
            }
        }
        protected RadioButton radAll;
        protected RadioButton radCustom;
        protected DropDownList ddlWebs;
        protected DropDownList ddlLists;
        protected DropDownList ddlColumns;
        AssignmentField field = null;
        Panel panelAssignment = null;
        protected Label lblDisplay;
        protected RadioButton radDefault;
        protected LookupFieldWithPickerEntityEditor lookupEditor = null;

        protected override void CreateChildControls()
        {
            
            if (this.Field != null)
            {
                field = this.Field as AssignmentField;

                base.CreateChildControls();
                this.radDefault = (RadioButton)TemplateContainer.FindControl("radDefault");
                this.radAll = (RadioButton)TemplateContainer.FindControl("radAll");
                this.radCustom = (RadioButton)TemplateContainer.FindControl("radCustom");
                this.lblDisplay = (Label)TemplateContainer.FindControl("lblDisplay");
            }

            if (this.ControlMode != SPControlMode.Display)
            {
                if (this.ControlMode == SPControlMode.New || this.ControlMode == SPControlMode.Edit)
                {
                    this.panelAssignment = (Panel)TemplateContainer.FindControl("panelAssignment");

                    this.lookupEditor = new LookupFieldWithPickerEntityEditor();
                    this.lookupEditor.ID = "lkDepartments";

                    Guid webID = Guid.Empty;
                    Guid listID = Guid.Empty;
                    Guid columnName = Guid.Empty;

                    string webUrl = !string.IsNullOrEmpty(field.GetProperty("WebId")) ? field.GetProperty("WebId") : string.Empty;
                    string list = field.GetProperty("List");
                    string showField = field.GetProperty("ShowField");

                    if (!string.IsNullOrEmpty(field.WebId) && !string.IsNullOrEmpty(field.ListId))
                    {
                        webID = new Guid(field.WebId);
                        listID = new Guid(field.ListId);
                        columnName = new Guid(field.ColumnName);
                    }
                    else
                    {
                        using (SPSite spSite = new SPSite(Web.Site.ID))
                        {
                            using (SPWeb spWeb = spSite.OpenWeb(webUrl))
                            {
                                webID = spWeb.ID;

                                var lkList = CCIUtility.GetListFromURL(list, spWeb);
                                listID = lkList.ID;

                                columnName = lkList.Fields[showField].Id;
                            }
                        }
                    }

                    this.lookupEditor.CustomProperty = new LookupFieldWithPickerPropertyBag(Web.ID, listID ,
                        columnName, new List<string>() { "Title" }, 30, 1).ToString();

                    this.lookupEditor.MultiSelect = true;
                    this.panelAssignment.Controls.Add(this.lookupEditor);
                }
            }
            else
            { 
                if (this.ItemFieldValue != null)
                {
                    string data = this.ItemFieldValue.ToString();
                    string value = string.Empty;

                    string[] users = data.Split(new string[] { ";#" }, StringSplitOptions.RemoveEmptyEntries);
                    if (users.Length > 1)
                    {
                        for (int i = 1; i < users.Length; i = i + 2)
                        {
                            value += users[i] + "; ";
                        }
                     
                        value = value.Trim();
                        value = value.TrimEnd(';');
                    }

                    if (String.IsNullOrEmpty(value))
                    {
                        value = data;
                    }

                    lblDisplay.Text = value;
                }
             
            }
        }

        public override object Value
        {
            get
            {
                
                EnsureChildControls();
                string results = "";
                if (radDefault.Checked)
                {
                    results = "Default";
                }
                else
                
                if (radAll.Checked)
                    results = "All";
                else
                {
                    foreach (PickerEntity item in this.lookupEditor.ResolvedEntities)
                    {
                        results += item.Key + ";#" + item.DisplayText + ";#";
                    }

                    results = results.TrimEnd('#');
                    results = results.TrimEnd(';');
                }
                return results;
            }
            set
            {
                EnsureChildControls();
                if (this.ItemFieldValue != null)
                {
                    string data = this.ItemFieldValue.ToString();
                    if (data.StartsWith("All"))
                    {
                        radAll.Checked = true;
                        radCustom.Checked = false;
                        radDefault.Checked = false;
                    }

                    else
                    {
                        if (data.StartsWith("Default"))
                        {
                            radAll.Checked = false;
                            radCustom.Checked = false;
                            radDefault.Checked = true;
                        }
                        else
                        {
                            BindPeoplePicker(this.lookupEditor, data.Split(new string[] { ";#" }, StringSplitOptions.RemoveEmptyEntries).ToList());
                            radCustom.Checked = true;
                            radAll.Checked = false;
                            radDefault.Checked = false;
                        }
                    }
                }
            }
        }
        private void BindPeoplePicker(LookupFieldWithPickerEntityEditor lookupEditor, List<string> list)
        {
            System.Collections.ArrayList entityArrayList = new System.Collections.ArrayList();
            PickerEntity entity = new PickerEntity();
            foreach (var item in list)
            {
                try
                {
                    entity = lookupEditor.GetEntityById(int.Parse(item));
                    if (entity != null)
                    {
                        entityArrayList.Add(entity);
                    }
                }
                catch { }
            }

            if (list.Count > 0)
            {
                lookupEditor.UpdateEntities(entityArrayList);
            }
        }


    }
}
