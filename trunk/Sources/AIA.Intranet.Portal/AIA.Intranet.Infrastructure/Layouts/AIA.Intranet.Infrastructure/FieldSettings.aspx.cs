using System;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using System.Linq;
using AIA.Intranet.Common.Extensions;

using System.Web.UI.WebControls;
using AIA.Intranet.Model.Infrastructure;
using Microsoft.SharePoint.Utilities;
using System.Web;
using System.Collections.Generic;
using AIA.Intranet.Model;

namespace AIA.Intranet.Infrastructure.Layouts
{
    public partial class FieldSettings : LayoutsPageBase
    {
        private SPList currentList;
        protected FieldPermissionSettings settings;
        protected FieldPermissionSettings PermissionSettings
        {
            get
            {
                if(settings ==  null )
                    settings= CurrentList.GetCustomSettings<FieldPermissionSettings>(Model.IOfficeFeatures.IOfficeApp, false);
                if (settings == null) settings = new FieldPermissionSettings();
                return settings;
            }
        }
        protected SPList CurrentList
        {
            get
            {
                if (this.currentList == null)
                {
                    this.currentList = SPContext.Current.List;
                }
                return this.currentList;
            }
        }

        #region Event Handler
        protected override bool RequireSiteAdministrator
        {
            get
            {
                return true;
            }
        }
        protected override void OnInit(EventArgs e)
        {
            fieldRepeater.ItemDataBound += new System.Web.UI.WebControls.RepeaterItemEventHandler(fieldRepeater_ItemDataBound);
            btnSave.Click += new EventHandler(btnSave_Click);
            btnDelete.Click += new EventHandler(btnDelete_Click);
            btnCancel.Click += new EventHandler(btnCancel_Click);
            base.OnInit(e);
        }

        void btnCancel_Click(object sender, EventArgs e)
        {
            SPUtility.Redirect(SPContext.Current.Web.Url + "/_layouts/listedit.aspx?List=" + Request["List"], SPRedirectFlags.Default, HttpContext.Current);
        }

        void btnDelete_Click(object sender, EventArgs e)
        {
            CurrentList.RemoveCustomSettings<FieldPermissionSettings>(IOfficeFeatures.IOfficeApp);
            SPUtility.Redirect(SPContext.Current.Web.Url + "/_layouts/listedit.aspx?List=+" + Request["List"], SPRedirectFlags.Default, HttpContext.Current);
        }

        void btnSave_Click(object sender, EventArgs e)
        {
           // var settings = CurrentList.GetCustomSettings<FieldPermissionSettings>(Model.IOfficeFeatures.CCIappInfrastructure, false);
            FieldPermissionSettings settings = new FieldPermissionSettings();

            foreach (RepeaterItem item in fieldRepeater.Items)
            {
                var fieldIdHidden = item.FindControl("fieldIdHidden") as HiddenField;

                var permission = new FieldPermissions()
                {
                    FieldId = new Guid(fieldIdHidden.Value)
                };

                CheckBoxList addPermissionChecks = item.FindControl("addPermissionChecks") as CheckBoxList;

                foreach (ListItem item1 in addPermissionChecks.Items)
                {
                    if(item1.Selected)
                    permission.NewGroupPermissions.Add(item1.Text);
                }

                PeopleEditor newItemPP = item.FindControl("newItemPP") as PeopleEditor;
                permission.NewUserPermissions.AddRange(GetInputUsers(newItemPP));


                CheckBoxList editPermissionChecks = item.FindControl("editPermissionChecks") as CheckBoxList;

                foreach (ListItem item1 in editPermissionChecks.Items)
                {
                    if(item1.Selected)
                    permission.EditGroupPermissions.Add(item1.Text);
                }

                PeopleEditor editItemPP = item.FindControl("editItemPP") as PeopleEditor;
                permission.EditUserPermissions.AddRange(GetInputUsers(editItemPP));

                CheckBoxList viewPermissionChecks = item.FindControl("viewPermissionChecks") as CheckBoxList;

                foreach (ListItem item1 in viewPermissionChecks.Items)
                {
                    if (item1.Selected)
                        permission.ViewGroupPermission.Add(item1.Text);
                }

                PeopleEditor viewItemPP = item.FindControl("viewItemPP") as PeopleEditor;
                permission.ViewUserPermissions.AddRange(GetInputUsers(viewItemPP));

                settings.AddItem(permission);
            }
            
            CurrentList.SetCustomSettings<FieldPermissionSettings>(Model.IOfficeFeatures.IOfficeApp, settings);
            
            UpdateFormUrl();
            SPUtility.Redirect(SPContext.Current.Web.Url + "/_layouts/listedit.aspx?List="+ Request["List"], SPRedirectFlags.Default, HttpContext.Current);
        }

        private static List<string> GetInputUsers(PeopleEditor newItemPP)
        {
            List<string> results = new List<string>();
            foreach (PickerEntity entity in newItemPP.ResolvedEntities)
            {
                switch ((string)entity.EntityData["PrincipalType"])
                {
                    case "User":
                        results.Add(entity.Key);
                        break;
                    case "SharePointGroup":
                        // … do stuff here …
                        break;
                    //permission.NewUserPermissions.Add(item1.);
                }
            }
            return results;
        }

        private void UpdateFormUrl()
        {
            foreach (SPContentType ctype in currentList.ContentTypes.Cast<SPContentType>().Where(p=>!p.Sealed))
            {
                ctype.NewFormTemplateName = "FieldPermissionTemplate";
                ctype.EditFormTemplateName = "FieldPermissionTemplate";
                ctype.DisplayFormTemplateName = "FieldPermissionTemplate";
                ctype.Update();
            }
        }

        void fieldRepeater_ItemDataBound(object sender, System.Web.UI.WebControls.RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.Item)
            {
                SPField field = e.Item.DataItem as SPField;

                Literal titleLiteral = e.Item.FindControl("titleLiteral") as Literal;
                titleLiteral.Text = field.Title;

                CheckBoxList addPermissionChecks = e.Item.FindControl("addPermissionChecks") as CheckBoxList;

                FieldPermissions permission = PermissionSettings[field.Id];
                if (permission == null) permission = new FieldPermissions();

                BindGroupCheckBoxList(addPermissionChecks, permission.NewGroupPermissions);

                PeopleEditor newItemPP = e.Item.FindControl("newItemPP") as PeopleEditor;
                BindPeoplePicker(newItemPP, permission.NewUserPermissions);

                CheckBoxList editPermissionChecks = e.Item.FindControl("editPermissionChecks") as CheckBoxList;
                BindGroupCheckBoxList(editPermissionChecks, permission.EditGroupPermissions);

                PeopleEditor editItemPP = e.Item.FindControl("editItemPP") as PeopleEditor;
                BindPeoplePicker(editItemPP, permission.EditUserPermissions);

                CheckBoxList viewPermissionChecks = e.Item.FindControl("viewPermissionChecks") as CheckBoxList;
                BindGroupCheckBoxList(viewPermissionChecks, permission.ViewGroupPermission);

                PeopleEditor viewItemPP = e.Item.FindControl("viewItemPP") as PeopleEditor;
                BindPeoplePicker(viewItemPP, permission.ViewUserPermissions);

                HiddenField fieldIdHidden = e.Item.FindControl("fieldIdHidden") as HiddenField;
                fieldIdHidden.Value = field.Id.ToString();
            }
        }

        private void BindPeoplePicker(PeopleEditor ppEditor,List<string> list)
        {
            string users = "";
 	        System.Collections.ArrayList entityArrayList = new System.Collections.ArrayList();
              PickerEntity entity = new PickerEntity();
            foreach (var item in list)
	        {
		        entity.Key = item;
              // this can be omitted if you're sure of what you are doing
              entity = ppEditor.ValidateEntity(entity);
              entityArrayList.Add(entity);
              users += item + ",";
	        }
              
              //ppEditor.UpdateEntities(entityArrayList);
            if(list.Count>0)
            ppEditor.CommaSeparatedAccounts = users.Remove(users.LastIndexOf(","), 1); 
        }

        private void BindGroupCheckBoxList(CheckBoxList cbList, List<String> selectedItems)
        {
            var datasource = this.Web.Groups.Cast<SPGroup>().ToList();
            //var setting = PermissionSettings;

            foreach (var item in datasource)
            {
                cbList.Items.Add(new ListItem() { 
                    Text = item.Name,
                    Value = item.Name,
                    Selected = selectedItems.Contains(item.Name)
                });
            }
            
        }
        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
            DisplayFieldList();
        }

        private void DisplayFieldList()
        {
            var fields = CurrentList.Fields.Cast<SPField>()
                                     .Where(p => !p.Hidden && !p.ReadOnlyField && !p.IsSystemField()).ToList();
            fieldRepeater.DataSource = fields;
            fieldRepeater.DataBind();


        }
    }
}
