using System;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using AIA.Intranet.Common.Extensions;
using System.Collections.Generic;
using AIA.Intranet.Infrastructure.CustomFields;
using AIA.Intranet.Common.Utilities;
using AIA.Intranet.Model;
using AIA.Intranet.Common.Services;
using AIA.Intranet.Model.Entities;

namespace AIA.Intranet.Infrastructure.Layouts
{
    public partial class ShareItem : LayoutsPageBase
    {
        #region Properties
        protected SPList CurrentList
        {
            get
            {
                return SPContext.Current.List;
            }
        }
        #endregion

        protected override void OnInit(EventArgs e)
        {
            btnShare.Click += new EventHandler(btnShare_Click);
            base.OnInit(e);
        }
        protected override void OnLoad(EventArgs e)
        {
            SPList list = CCIUtility.GetListFromURL(Constants.DEPARTMENT_LIST_URL);


            this.lkDepartments = new LookupFieldWithPickerEntityEditor();
            this.lkDepartments.ID = "lkDepartments";
            this.lkDepartments.CustomProperty = new LookupFieldWithPickerPropertyBag(list.ParentWeb.ID, list.ID,
                list.Fields[SPBuiltInFieldId.Title].Id, new List<string>() { "Title" }, 30, 1).ToString();

            this.lkDepartments.MultiSelect = true;
            this.pnlDepartment.Controls.Add(this.lkDepartments);


            base.OnLoad(e);
        }
        protected LookupFieldWithPickerEntityEditor lkDepartments;
      
        void btnShare_Click(object sender, EventArgs e)
        {
            SPListItem item = SPContext.Current.ListItem;

            List<string> loginNames = GetSharedUsersOrGroups();

            SPSecurity.RunWithElevatedPrivileges(delegate()
            {
                
                using (var site = new SPSite(SPContext.Current.Site.ID))
                using (var web = site.OpenWeb(SPContext.Current.Web.ID))
                {
                    try
                    {
                        web.AllowUnsafeUpdates = true;
                        var list = web.Lists[SPContext.Current.ListId];
                        var currentItem = list.GetItemById(item.ID);

                        SPRoleDefinition contributerDef = SPContext.Current.Web.RoleDefinitions.GetByType(SPRoleType.Reader);
                        currentItem.BreakRoleInheritance(true);
                        currentItem.SystemUpdate();
                        currentItem.SetPermissions(contributerDef.Name, loginNames);
                    }
                    catch (Exception ex)
                    {

                        CCIUtility.LogError(ex);
                    }
                    finally
                    {
                        web.AllowUnsafeUpdates = false;
                    }
                }
            });
            


            Context.Response.Write("<script type='text/javascript'>window.frameElement.commitPopup();</script>");
            Context.Response.Flush();
            Context.Response.End();


        }

        private List<string> GetSharedUsersOrGroups()
        {
            List<string> loginNames = new List<string>();
            foreach (PickerEntity entity in ppUsers.ResolvedEntities)
            {
                loginNames.Add(entity.Key);
            }

            foreach (PickerEntity item in lkDepartments.ResolvedEntities)
            {
                int id = Convert.ToInt32(item.Key.Split(";#".ToCharArray(), StringSplitOptions.RemoveEmptyEntries));
                Department dept = DepartmentService.GetDepartment(id);
                loginNames.Add(dept.DepartmentSiteGroup);
                //TODO - Get department groups name and add to list.
            }
            //Share to my department
            if (chkMyDept.Checked)
            {
                var myDept = IOfficeContext.Department;

                //TODO - Get department groups name and add to list.
                loginNames.Add(myDept.DepartmentSiteGroup);
            }

            return loginNames;
            
        }
        protected void Page_Load(object sender, EventArgs e)
        {
        }
    }
}
