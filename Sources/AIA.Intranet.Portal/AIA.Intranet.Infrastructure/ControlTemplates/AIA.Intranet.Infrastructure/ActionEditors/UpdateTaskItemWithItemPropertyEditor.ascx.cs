﻿using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using AIA.Intranet.Infrastructure.Controls;

using Microsoft.SharePoint;
using System.Collections.Generic;
using AIA.Intranet.Model.Workflow;
using AIA.Intranet.Common.Utilities;
using System.Reflection;
using AIA.Intranet.Common.Helpers;

namespace AIA.Intranet.Infrastructure.CONTROLTEMPLATES.AIA.Intranet.Infrastructure
{
    public partial class UpdateTaskItemWithItemPropertyEditor : ActionEditorControl
    {
        protected override void OnInit(EventArgs e)
        {
            chkRemoveAction.Attributes.Add("onclick", string.Format("{0}_click(this);", chkRemoveAction.ClientID));
            base.OnInit(e);
        }
        protected override void OnPreRender(EventArgs e)
        {
            if (chkRemoveAction.Checked)
            {
                string script = string.Empty;
                script += "<script type='text/javascript'>";
                script += string.Format("_spBodyOnLoadFunctionNames.push('{0}_click(document.getElementById(\"{1}\"),{2})');", chkRemoveAction.ClientID, chkRemoveAction.ClientID, ddlItemColumnValidator.ClientID);
                script += "</script>";
                Page.ClientScript.RegisterClientScriptBlock(GetType(), "ActiveValidate_" + ddlItemColumnValidator.ClientID, script);
            }
            base.OnPreRender(e);
        }

        protected override void OnLoad(EventArgs e)
        {
            if (IsFirstLoad)
            {
                List<SPField> fields = getAvailableColumns();
                ControlHelper.LoadFieldsToDropdown(ddlItemColumn, fields);
                List<SPField> taskListFields = fields.Where(p => p.Id != SPBuiltInFieldId.AssignedTo).ToList();
                ControlHelper.LoadFieldsToDropdown(ddlTaskColumn, taskListFields);                
            }
            if (!IsPostBack)
            {
                displayActionData();
            }
            base.OnLoad(e);
        }

        //private void showESignMetadataProperties()
        //{
        //    ddlMetadataProperty.Items.Add(new ListItem() { Text = "Select a property", Value = string.Empty });

        //    Type type = typeof(ESignEventMetadata);
        //    List<PropertyInfo> properties = type.GetProperties().Cast<PropertyInfo>().OrderBy(p => p.Name).ToList();

        //    SPQuery postsQuery = new SPQuery();
        //    postsQuery.Query = string.Format(
        //        "<Query>" +
        //           "<OrderBy>" +
        //              "<FieldRef Name='Title' />" +
        //           "</OrderBy>" +
        //        "</Query>");


        //    string list = Request["listSettings"];
        //    if (string.IsNullOrEmpty(list)) return;

        //    SPList dataList = CCIUtility.GetListFromURL(list);
        //    SPListItemCollection data = dataList.GetItems(postsQuery);
        //    IEnumerable<SPListItem> items = data.Cast<SPListItem>();
        //    foreach (var item in items)
        //    {
        //        ddlMetadataProperty.Items.Add(new ListItem() { Text = item["Title"].ToString(), Value = item["Title"].ToString() });
        //    }

        //}

        protected List<SPField> getAvailableColumns()
        {
            IEnumerable<SPField> fields = null;

            if (!string.IsNullOrEmpty(base.ContentTypeId))
            {
                //Load user colum from content type.
                SPContentType ct = base.GetContentType();
                if (ct != null)
                {
                    fields = ct.Fields.Cast<SPField>();
                }
            }
            else
            {
                fields = SPContext.Current.Web.AvailableFields.Cast<SPField>();
            }
            List<SPField> results = fields.Where(p => !p.Hidden && p.Title != "Predecessors" && p.Title != "Priority"
                    && p.Id != SPBuiltInFieldId.Predecessors
                    && p.Id != SPBuiltInFieldId.RelatedIssues
                    && p.Id != SPBuiltInFieldId.AssignedTo
                    && p.Id != SPBuiltInFieldId.Priority
                    && p.Id != SPBuiltInFieldId.TaskDueDate
                    && p.Id != SPBuiltInFieldId.StartDate
                    && p.Id != SPBuiltInFieldId.WorkflowName
                    && p.Id != SPBuiltInFieldId.TaskStatus
                    && p.Id != SPBuiltInFieldId.Body
                    && p.Id != SPBuiltInFieldId.Completed
                    && p.Id != SPBuiltInFieldId.PercentComplete
                    && p.Id != SPBuiltInFieldId.Outcome
                    && p.Id != SPBuiltInFieldId.TaskType
                    )
                .OrderBy(p => p.Title)
                .ToList();
            return results;
        }
       
        public override TaskActionSettings GetAction()
        {
            EnsureChildControls();
            if (chkRemoveAction.Checked) return null;
            return new UpdateTaskItemWithItemPropertySettings()
            {
                ItemFieldId = ddlItemColumn.SelectedValue,
                TaskFieldId = ddlTaskColumn.SelectedValue
            };
        }
        private void displayActionData()
        {
            List<SPField> fields = getAvailableColumns();
            ControlHelper.LoadFieldsToDropdown(ddlItemColumn, fields);
            ControlHelper.LoadFieldsToDropdown(ddlTaskColumn, fields);           

            if (Action == null) return;
            UpdateTaskItemWithItemPropertySettings savedAction = (UpdateTaskItemWithItemPropertySettings)Action;
            ddlItemColumn.SelectedValue = savedAction.ItemFieldId;
            ddlTaskColumn.SelectedValue = savedAction.TaskFieldId;
        }
    }
}