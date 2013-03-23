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
using AIA.Intranet.Model;

namespace AIA.Intranet.Infrastructure.Pages
{
    public partial class EditUser : AdministrationPage
    {
        protected GridView grGroups;
        protected TextBox txtLoginName;
        protected TextBox txtFirstName;
        protected TextBox txtLastName;
        protected TextBox txtDisplayName;
        protected TextBox txtOffice;
        protected TextBox txtEmail;
        protected CheckBox cbRequireChangePass;
        protected CheckBox cbCannotChagnePass;
        protected CheckBox cbPassNeverExpired;
        protected CheckBox cbAccountIsDisable;
        protected TextBox txtPassword;
        protected TextBox txtConfirmPassword;
        protected Label lblErrorMessage;
        protected Label lblPassword;
        protected Label lblConfirmPassword;
        protected Panel pnEditMode;
        protected Label lblTitle;
        protected Panel pnGroups;
        protected Panel pnGroupButtons;
        protected Panel pnResetPassword;

        protected Button btnSave;
        protected LinkButton btnRemoveGroup;

        string allUsersPageUrl = SPContext.Current.Web.Url.TrimEnd('/') + "/_layouts/AIA.Intranet.Infrastructure/AD/AllUsers.aspx";
        string searchUserPageUrl = SPContext.Current.Web.Url.TrimEnd('/') + "/_layouts/AIA.Intranet.Infrastructure/AD//SearchUser.aspx?user=";


        protected void Page_Load(object sender, EventArgs e)
        {
            //if (!SharepointHelper.HasPermissions(this.Web, new string[] { "Full Control", "Design" }))
            //{
            //    Response.Redirect(this.Web.Url + "/_layouts/accessdenied.aspx");
            //}

            HandlePasswordField();
            if (!IsPostBack)
            {
                ResetSession();
                string source = Request.QueryString["Source"] != null ? Request.QueryString["Source"].ToString() : string.Empty;
                if (!string.IsNullOrEmpty(source))
                {
                    HideGroups();
                }
                string userDn = Request.QueryString["Path"].ToString();
                Session[Constants.UserDn] = userDn;

                if (!string.IsNullOrEmpty(userDn))
                {
                    ShowEditMode();
                    ADHelper helper = new ADHelper();
                    ADUser user = helper.GetUser(userDn);
                    Session[Constants.UserLogInName] = user.LoginName;
                    txtLoginName.Text = user.LoginName;
                    txtFirstName.Text = user.FirstName;
                    txtLastName.Text = user.LastName;
                    txtDisplayName.Text = user.DisplayName;
                    txtOffice.Text = user.Office;
                    txtEmail.Text = user.EmailAddress;

                    //Bind group
                    List<ADGroup> groups = helper.GetGroupsOfUser(user.Path);

                    grGroups.DataSource = groups;
                    grGroups.DataBind();
                }
                
            }
            List<ADGroup> groupsToAdd = (List<ADGroup>)Session[Constants.GroupsToAdd];
            if (groupsToAdd != null)
            {
                AddGroupsToGrid(groupsToAdd);
                Session[Constants.GroupsToAdd] = null;
            }
        }

        private void HandlePasswordField()
        {
            txtPassword.Attributes.Add("value", txtPassword.Text);
            txtConfirmPassword.Attributes.Add("value", txtConfirmPassword.Text);
        }

        private void ResetSession()
        {
            Session[Constants.UserDn] = string.Empty;
            Session[Constants.UserLogInName] = string.Empty;
        }

        private void ShowEditMode()
        {
            txtPassword.Visible = false;
            txtConfirmPassword.Visible = false;
            cbAccountIsDisable.Visible = false;
            cbCannotChagnePass.Visible = false;
            cbPassNeverExpired.Visible = false;
            cbRequireChangePass.Visible = false;
            lblConfirmPassword.Visible = false;
            lblPassword.Visible = false;
            pnEditMode.Visible = false;
            pnResetPassword.Visible = true;
            lblTitle.Text = Constants.EditUserPageTitle;
        }

        private void HideGroups()
        {
            pnGroups.Visible = false;
            pnGroupButtons.Visible = false;
        }

        private void AddGroupsToGrid(List<ADGroup> groupsToAdd)
        {
            List<ADGroup> groups = new List<ADGroup>();
            ADGroup group = null;

            foreach (GridViewRow gr in grGroups.Rows)
            {
                group = new ADGroup();
                group.Name = gr.Cells[0].Text;
                group.Description = gr.Cells[1].Text;
                group.Path = gr.Cells[2].Text;
                groups.Add(group);
            }
            groups = groups.Concat(groupsToAdd).ToList();

            grGroups.DataSource = groups;
            grGroups.DataBind();
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            lblErrorMessage.Text = string.Empty;

            ADHelper helper = new ADHelper();
            ADUser user = new ADUser();
            user.Path = Session[Constants.UserDn] != null ? Session[Constants.UserDn].ToString() : string.Empty;

            try
            {
                bool isEditMode = !string.IsNullOrEmpty(user.Path);
                ValidateUser(isEditMode);
            }
            catch (Exception ex)
            {
                lblErrorMessage.Text = ex.Message;
                return;
            }

            //group.Path = Request.QueryString["Path"].ToString();
            GetUserFromUI(user);
            try
            {
                if (!string.IsNullOrEmpty(user.Path))
                {
                    helper.UpdateUserAccount(user);
                }
                else
                {
                    if (string.IsNullOrEmpty(user.DisplayName))
                    {
                        user.DisplayName = user.FirstName + " " + user.LastName;
                    }
                    helper.CreateUserAccount(user);
                    string userDn = "CN=" + user.DisplayName + "," + helper.OuPath;
                    user = helper.GetUser(userDn);
                    Session[Constants.UserDn] = userDn;
                }

                //Update members
                UpdateGroups(helper, user);
                //Response.Redirect(allUsersPageUrl);
                Redirect(user.LoginName);
            }
            catch(Exception ex)
            {
                lblErrorMessage.Text += ex.Message;
            }
            
        }

        private void Redirect(string userName)
        {
            string source = Request.QueryString["Source"] != null ? Request.QueryString["Source"].ToString() : string.Empty;
            if (!string.IsNullOrEmpty(source))
            {
                if (source.Equals("search",StringComparison.OrdinalIgnoreCase))
                {
                    Response.Redirect(searchUserPageUrl + userName + "&IsDlg=1");
                }
                
            }
            else
            {
                Response.Redirect(allUsersPageUrl);
            }
        }

        private void GetUserFromUI(ADUser user)
        {
            user.LoginName = txtLoginName.Text;
            user.FirstName = txtFirstName.Text;
            user.LastName = txtLastName.Text;
            user.DisplayName = txtDisplayName.Text;
            user.Office = txtOffice.Text;
            user.EmailAddress = txtEmail.Text;
            user.Password = txtPassword.Text;
            user.IsAccountIsDisable = cbAccountIsDisable.Checked;
            user.IsCannotChagnePass = cbCannotChagnePass.Checked;
            user.IsPassNeverExpired = cbPassNeverExpired.Checked;
            user.IsRequireChangePass = cbRequireChangePass.Checked;
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            if (Session[Constants.UserDn] != null && !string.IsNullOrEmpty(Session[Constants.UserDn].ToString()))
            {
                ADHelper adHepper = new ADHelper();
                adHepper.Delete(Session[Constants.UserDn].ToString());
            }
            //Response.Redirect(allUsersPageUrl);
            Redirect(string.Empty);
        }

        protected void btnRemoveGroup_Click(object sender, EventArgs e)
        {
            List<ADGroup> groups = new List<ADGroup>();
            ADGroup group = null;

            foreach (GridViewRow gr in grGroups.Rows)
            {
                CheckBox cb = (CheckBox)gr.Cells[3].FindControl("isSelect");
                if (!cb.Checked)
                {
                    group = new ADGroup();
                    group.Name = gr.Cells[0].Text;
                    group.Description = gr.Cells[1].Text;
                    group.Path = gr.Cells[2].Text;
                    groups.Add(group);
                }
            }

            grGroups.DataSource = groups;
            grGroups.DataBind();

        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            //Response.Redirect(allUsersPageUrl);
            Redirect(string.Empty);
        }

        protected void cbRequireChangePass_Changed(object sender, EventArgs e)
        {
            if (cbRequireChangePass.Checked)
            {
                cbCannotChagnePass.Enabled = false;
                cbPassNeverExpired.Enabled = false;
                cbCannotChagnePass.Checked = false;
                cbPassNeverExpired.Checked = false;
            }
            else
            {
                cbCannotChagnePass.Enabled = true;
                cbPassNeverExpired.Enabled = true;
            }
        }
        

        private void UpdateGroups(ADHelper helper, ADUser user)
        {
            List<ADGroup> oldGroups = helper.GetGroupsOfUser(user.Path);
            var  oldGroupsTemp = from adGroup in oldGroups
                                select new ADGroup() { Name = adGroup.Name, Path = adGroup.Path };

            List<ADGroup> newGroups = GetSelectedGroups();

            var addGroups = newGroups.Except(oldGroupsTemp);
            var removeGroups = oldGroupsTemp.Except(newGroups);

            foreach (ADGroup group in removeGroups)
            {
                try
                {
                    helper.RemoveUserFromGroup(user.Path, group.Path);
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.Write(ex.Message);
                    lblErrorMessage.Text += " Can not remove group: " + group.Name + " from list of groups of user " + user.LoginName + ".";
                }
            }

            foreach (ADGroup group in addGroups)
            {
                try
                {
                    helper.AddUserToGroup(user.Path, group.Path);
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.Write(ex.Message);
                    lblErrorMessage.Text += " Can not add group: " + group.Name + " to list of groups of user " + user.LoginName + ".";
                }
            }
        }

        private List<ADGroup> GetSelectedGroups()
        {
            List<ADGroup> groups = new List<ADGroup>();
            ADGroup newGroup = null;
            foreach (GridViewRow row in grGroups.Rows)
            {
                newGroup = new ADGroup();
                newGroup.Name = row.Cells[0].Text;
                newGroup.Path = row.Cells[2].Text;
                groups.Add(newGroup);
            }
            return groups;
        }

        private void ValidateUser(bool isEditMode)
        {
            if (string.IsNullOrEmpty(txtLoginName.Text))
            {
                throw new Exception("Please enter login name.");
            }
            if (!isEditMode)
            {
                if (string.IsNullOrEmpty(txtPassword.Text))
                {
                    throw new Exception("Please enter password.");
                }
                if (!string.Equals(txtPassword.Text, txtConfirmPassword.Text))
                {
                    throw new Exception("The passwords do not match.");
                }
            }
            
            if (string.IsNullOrEmpty(txtFirstName.Text)
                && string.IsNullOrEmpty(txtLastName.Text)
                && string.IsNullOrEmpty(txtDisplayName.Text))
            {
                throw new Exception("Please enter at least First Name or Last Name or Display Name.");
            }

        }

        
    }
}