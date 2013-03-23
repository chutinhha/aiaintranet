using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.SharePoint.WebControls;

using Microsoft.SharePoint;
using System.Collections.ObjectModel;
using AIA.Intranet.Infrastructure.Models;
using AIA.Intranet.Infrastructure.ActiveDirectory;
using AIA.Intranet.Infrastructure.Utilities;
using Microsoft.SharePoint.Utilities;
using AIA.Intranet.Model;

namespace AIA.Intranet.Infrastructure.Pages
{
    public partial class EditGroup : AdministrationPage
    {
        protected GridView grMember;
        protected TextBox txtGroupName;
        protected TextBox txtDescription;
        protected Button btnSave;
        protected LinkButton btnRemoveMember;
        protected Label lblTitle;
        protected Label lblErrorMessage;

        string allGroupUrl = SPContext.Current.Web.Url.TrimEnd('/') + "/_layouts/AIA.Intranet.Infrastructure/AD/AllGroups.aspx";
        
        protected void Page_Load(object sender, EventArgs e)
        {
            //if (!SharepointHelper.HasPermissions(this.Web, new string[] { "Full Control", "Design" }))
            //{
            //    Response.Redirect(this.Web.Url + "/_layouts/accessdenied.aspx");
            //}

            lblErrorMessage.Text = string.Empty;

            if (!IsPostBack)
            {
                ResetSession();

                string groupDn = Request.QueryString["Path"].ToString();
                Session[Constants.GroupDn] = groupDn;

                if (!string.IsNullOrEmpty(groupDn))
                {
                    lblTitle.Text = Constants.EditGroupPageTitle;
                    ADHelper helper = new ADHelper();
                    ADGroup group = helper.GetGroup(groupDn);
                    Session[Constants.GroupLogInName] = group.LoginName;
                    txtGroupName.Text = group.Name;
                    txtDescription.Text = group.Description;

                    //Bind members
                    List<ADUser> users = helper.GetUserFromGroup(group.LoginName);
                    
                    grMember.DataSource = users;
                    grMember.DataBind();
                }
                
            }
            List<ADUser> usersToAdd = (List<ADUser>)Session[Constants.UsersToAdd];
            if (usersToAdd != null)
            {
                AddUsersToGrid(usersToAdd);
                Session[Constants.UsersToAdd] = null;
            }
        }

        private void ResetSession()
        {
            Session[Constants.GroupLogInName] = string.Empty;
            Session[Constants.GroupDn] = string.Empty;
        }

        private void AddUsersToGrid(List<ADUser> usersToAdd)
        {
            List<ADUser> users = new List<ADUser>();
            ADUser user = null;

            foreach (GridViewRow gr in grMember.Rows)
            {
                user = new ADUser();
                user.LoginName = gr.Cells[0].Text;
                user.DisplayName = gr.Cells[1].Text;
                user.Path = gr.Cells[2].Text;
                users.Add(user);
            }
            users = users.Concat(usersToAdd).ToList();

            grMember.DataSource = users;
            grMember.DataBind();
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            lblErrorMessage.Text = string.Empty;
            try
            {
                ADHelper helper = new ADHelper();
                ADGroup group = new ADGroup();
                group.Path = Session["groupDn"] != null ? Session["groupDn"].ToString() : string.Empty;
                //group.Path = Request.QueryString["Path"].ToString();
                group.Name = txtGroupName.Text;
                group.Description = txtDescription.Text;
                group.LoginName = Session[Constants.GroupLogInName] != null ? Session[Constants.GroupLogInName].ToString() : string.Empty;
                if (!string.IsNullOrEmpty(group.Path))
                {
                    helper.UpdateGroup(group);
                }
                else
                {
                    helper.CreateGroup(group);
                    string groupDn = "CN=" + group.Name + "," + helper.OuPath;
                    group = helper.GetGroup(groupDn);
                    Session["groupDn"] = groupDn;
                }

                //Update members
                UpdateMembers(helper, group);
                Response.Redirect(allGroupUrl);
            }
            catch (Exception ex)
            {
                lblErrorMessage.Text += ex.Message;
            }
            
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            if (Session["groupDn"] != null && !string.IsNullOrEmpty(Session["groupDn"].ToString()))
            {
                ADHelper adHepper = new ADHelper();
                adHepper.Delete(Session["groupDn"].ToString());
            }
            Response.Redirect(allGroupUrl);
        }

        protected void btnRemove_Click(object sender, EventArgs e)
        {
            List<ADUser> users = new List<ADUser>();
            ADUser user = null;

            foreach (GridViewRow gr in grMember.Rows)
            {
                CheckBox cb = (CheckBox)gr.Cells[3].FindControl("isSelect");
                if (!cb.Checked)
                {
                    user = new ADUser();
                    user.LoginName = gr.Cells[0].Text;
                    user.DisplayName = gr.Cells[1].Text;
                    user.Path = gr.Cells[2].Text;
                    users.Add(user);
                }
            }

            grMember.DataSource = users;
            grMember.DataBind();

        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            
            Response.Redirect(allGroupUrl);
        }
        

        private void UpdateMembers(ADHelper helper, ADGroup group)
        {
            List<ADUser> oldUsers = helper.GetUserFromGroup(group.LoginName);
            var  oldusersTemp = from user in oldUsers
                                select new ADUser(){ LoginName = user.LoginName, Path=user.Path};

            List<ADUser> newUsers = GetSelectedUsers();

            var addUsers = newUsers.Except(oldusersTemp);
            var removeUsers = oldusersTemp.Except(newUsers);

            foreach (ADUser user in removeUsers)
            {
                try
                {
                    helper.RemoveUserFromGroup(user.Path, group.Path);
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.Write(ex.Message);
                    lblErrorMessage.Text += " Remove user: " + user.LoginName + " failed.";
                }
            }

            foreach (ADUser user in addUsers)
            {
                try
                {
                    helper.AddUserToGroup(user.Path, group.Path);
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.Write(ex.Message);
                    lblErrorMessage.Text += " Add user: " + user.LoginName + " failed.";
                }
            }
        }

        private List<ADUser> GetSelectedUsers()
        {
            List<ADUser> users = new List<ADUser>();
            ADUser newUser = null;
            foreach (GridViewRow row in grMember.Rows)
            {
                newUser = new ADUser();
                newUser.LoginName = row.Cells[0].Text;
                newUser.Path = row.Cells[2].Text;
                users.Add(newUser);
            }
            return users;
        }

        private void CheckPermission()
        {
            using (SPWeb site = this.Web)
            {
                // Get a reference to the roles that 
                // are bound to the user and the role 
                // definition against which we need to 
                // verify the user. 
                SPRoleDefinitionBindingCollection usersRoles =
                  site.AllRolesForCurrentUser;
                SPRoleDefinitionCollection siteRoleCollection =
                  site.RoleDefinitions;
                SPRoleDefinition fullRoleDefinition =
                  siteRoleCollection["Full Control"];

                SPRoleDefinition designRoleDefinition = siteRoleCollection["Design"];
                // Determine whether the user is in the role. If 
                // not, redirect the user to the access-denied page 
                if (!(usersRoles.Contains(fullRoleDefinition) || usersRoles.Contains(designRoleDefinition)))
                {
                    Response.Redirect(this.Web.Url + "/_layouts/accessdenied.aspx");
                }
            }

        }
             
    }
}