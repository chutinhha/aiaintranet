using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.SharePoint.WebControls;

using Microsoft.SharePoint;
using AIA.Intranet.Infrastructure.Models;
using System.Collections.ObjectModel;
using AIA.Intranet.Infrastructure.ActiveDirectory;
using AIA.Intranet.Infrastructure.Utilities;

namespace AIA.Intranet.Infrastructure.Pages
{
    public partial class AllUsers : AdministrationPage
    {
        protected GridView grUsers;
        protected Button btnAdd;
        protected LinkButton btnManageGroup;
        protected TextBox txtUserName;

        string editUserPageUrl = SPContext.Current.Web.Url.TrimEnd('/') + "/_layouts/AIA.Intranet.Infrastructure/AD/EditUser.aspx?Path=";
        string allGroupsPageUrl = SPContext.Current.Web.Url.TrimEnd('/') + "/_layouts/AIA.Intranet.Infrastructure/AD/AllGroups.aspx";

        protected void Page_Load(object sender, EventArgs e)
        {
            //if (!SharepointHelper.HasPermissions(this.Web, new string[] { "Full Control", "Design" }))
            //{
            //    Response.Redirect(this.Web.Url + "/_layouts/accessdenied.aspx");
            //}

            if (!IsPostBack)
            {
                List<ADUser> groups = new List<ADUser>();
                //groups.Add(new ADGroup() {Name="Group 1", Description = "Group 1" });
                //grGroups.DataSource = groups;
                //ADHelper adHepper = new ADHelper();
                //try
                //{
                //    groups = adHepper.GetUsers();
                //}
                //catch (Exception ex)
                //{
                //    System.Diagnostics.Debug.Write(ex.Message);
                //}
                grUsers.DataSource = groups;
                grUsers.DataBind();
            }

        }

        protected void gr_RowDataBound(object sender, GridViewRowEventArgs e)
        {

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Attributes.Add("onmouseover", "this.style.backgroundColor='#C2D69B'");
                e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor='white'");
                e.Row.Attributes.Add("style", "cursor:pointer;");
                e.Row.Attributes.Add("onclick", "location='EditUser.aspx?Path=" + e.Row.Cells[2].Text + "'");
                //e.Row.Cells[2].Visible = false;
            }

        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            List<ADUser> users = FindUsers();
            grUsers.DataSource = users;
            grUsers.DataBind();
        }

        private List<ADUser> FindUsers()
        {
            ADHelper helper = new ADHelper();
            List<ADUser> users = helper.GetUsers();
            List<ADUser> results = new List<ADUser>();
            string usertToFind = txtUserName.Text;
            if (string.IsNullOrEmpty(usertToFind))
            {
                results = users;

            }
            else
            {
                var matchUsers = from user in users
                                 where user.LoginName.ToLower().Contains(usertToFind.ToLower()) || user.DisplayName.ToLower().Contains(usertToFind.ToLower())
                                 select user;
                results = matchUsers.ToList();
            }
            return results;
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            Response.Redirect(editUserPageUrl);
        }

        protected void btnManageGroup_Click(object sender, EventArgs e)
        {
            Response.Redirect(allGroupsPageUrl);
        }
        
    }
}