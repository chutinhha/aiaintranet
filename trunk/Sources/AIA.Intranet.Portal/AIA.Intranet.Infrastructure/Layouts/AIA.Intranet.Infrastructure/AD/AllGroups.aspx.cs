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
using Microsoft.SharePoint.Utilities;
using AIA.Intranet.Infrastructure.Utilities;

namespace AIA.Intranet.Infrastructure.Pages
{
    public partial class AllGroups : AdministrationPage
    {
        protected GridView grGroups;
        protected Button btnAdd;
        protected LinkButton btnManageUser;

        string editGroupPageUrl = SPContext.Current.Web.Url.TrimEnd('/') + "/_layouts/AIA.Intranet.Infrastructure/AD/EditGroup.aspx?Path=";
        string allUsersPageUrl = SPContext.Current.Web.Url.TrimEnd('/') + "/_layouts/AIA.Intranet.Infrastructure/AD/AllUsers.aspx";

        protected void Page_Load(object sender, EventArgs e)
        {
            //CheckPermission();
            //if (!SharepointHelper.HasPermissions(this.Web, new string[] { "Full Control", "Design" }))
            //{
            //    Response.Redirect(this.Web.Url + "/_layouts/accessdenied.aspx");
            //}

            if (!IsPostBack)
            {
                List<ADGroup> groups = new List<ADGroup>();
                //groups.Add(new ADGroup() {Name="Group 1", Description = "Group 1" });
                //grGroups.DataSource = groups;
                ADHelper adHepper = new ADHelper();
                try
                {
                    groups = adHepper.GetGroups();
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.Write(ex.Message);
                }
                grGroups.DataSource = groups;
                grGroups.DataBind();
            }

        }

        protected void gr_RowDataBound(object sender, GridViewRowEventArgs e)
        {

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Attributes.Add("onmouseover", "this.style.backgroundColor='#C2D69B'");
                e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor='white'");
                e.Row.Attributes.Add("style", "cursor:pointer;");
                e.Row.Attributes.Add("onclick", "location='EditGroup.aspx?Path=" + e.Row.Cells[2].Text + "'");
                //e.Row.Cells[2].Visible = false;
            }

        }  

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            Response.Redirect(editGroupPageUrl);
        }

        protected void btnManageUser_Click(object sender, EventArgs e)
        {
            Response.Redirect(allUsersPageUrl);
        }

    }
}