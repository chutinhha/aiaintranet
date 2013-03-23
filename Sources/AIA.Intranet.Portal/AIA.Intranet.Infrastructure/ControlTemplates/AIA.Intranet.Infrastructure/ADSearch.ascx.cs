using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Collections.ObjectModel;
using AIA.Intranet.Infrastructure.Models;
using AIA.Intranet.Infrastructure.ActiveDirectory;
using System.Collections.Generic;
using System.Linq;
using AIA.Intranet.Infrastructure.Utilities;
using System.Web;
using Microsoft.SharePoint;
using AIA.Intranet.Model;

namespace AIA.Intranet.Infrastructure.ControlTemplates
{
    public partial class Search : UserControl
    {
        string editUserPageUrl = SPContext.Current.Web.Url.TrimEnd('/') + "/_layouts/AIA.Intranet.Infrastructure/EditUser.aspx?Path=";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string user  = Request.QueryString["user"] != null? Request.QueryString["user"].ToString():string.Empty;

                if (!string.IsNullOrEmpty(user))
                {
                    List<ADUser> users = FindUsers(user);
                    grMember.DataSource = users;
                    grMember.DataBind();
                    txtUserName.Text = user;
                }
                
            }

        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            string usertToFind = txtUserName.Text;
            List<ADUser> users = FindUsers(usertToFind);
            grMember.DataSource = users;
            grMember.DataBind();
        }

        private List<ADUser> FindUsers(string usertToFind)
        {
            ADHelper helper = new ADHelper();
            List<ADUser> users = helper.GetUsers();
            List<ADUser> results = new List<ADUser>();
            
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

        protected void btnSelect_Click(object sender, EventArgs e)
        {
            List<ADUser> users = new List<ADUser>();
            ADUser user = null;
            foreach (GridViewRow gr in grMember.Rows)
            {
                CheckBox cb = (CheckBox)gr.Cells[3].FindControl("isSelect");
                if (cb.Checked)
                {
                    user = new ADUser();
                    user.LoginName = gr.Cells[0].Text;
                    user.DisplayName = HttpUtility.HtmlDecode(gr.Cells[1].Text);
                    user.Path = gr.Cells[2].Text;
                    users.Add(user);
                }
            }

            Session[Constants.UsersToAdd] = users;

            Context.Response.Write("<script type='text/javascript'>window.frameElement.commitPopup();</script>");
            Context.Response.Flush();
            Context.Response.End();


        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Session[Constants.UsersToAdd] = null;

            Context.Response.Write("<script type='text/javascript'>window.frameElement.commitPopup();</script>");
            Context.Response.Flush();
            Context.Response.End();
        }

        protected void btnAddNewUser_Click(object sender, EventArgs e)
        {
            Response.Redirect(editUserPageUrl + "&Source=search&IsDlg=1");
        }
        
    }
}
