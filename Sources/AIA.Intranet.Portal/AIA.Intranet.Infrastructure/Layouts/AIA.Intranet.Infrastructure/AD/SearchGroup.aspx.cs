using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.SharePoint.WebControls;

using Microsoft.SharePoint;
using AIA.Intranet.Infrastructure.Models;
using AIA.Intranet.Infrastructure.ActiveDirectory;
using AIA.Intranet.Infrastructure.Utilities;
using AIA.Intranet.Model;

namespace AIA.Intranet.Infrastructure.Pages
{
    public partial class SearchGroup : AdministrationPage
    {
        protected TextBox txtGroupName;
        protected Button btnSearch;
        protected GridView grGroup;
        protected Button btnSelect;
        protected Button btnCancel;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                
            }

        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            List<ADGroup> users = FindGroups();
            grGroup.DataSource = users;
            grGroup.DataBind();
        }

        private List<ADGroup> FindGroups()
        {
            ADHelper helper = new ADHelper();
            List<ADGroup> groups = helper.GetGroups();
            List<ADGroup> results = new List<ADGroup>();
            string groupToFind = txtGroupName.Text;
            if (string.IsNullOrEmpty(groupToFind))
            {
                results = groups;

            }
            else
            {
                var matchGroups = from adGroup in groups
                                  where adGroup.Name.ToLower().Contains(groupToFind.ToLower()) || adGroup.LoginName.ToLower().Contains(groupToFind.ToLower())
                                  select adGroup;
                results = matchGroups.ToList();
            }
            return results;
        }

        protected void btnSelect_Click(object sender, EventArgs e)
        {
            List<ADGroup> groups = new List<ADGroup>();
            ADGroup group = null;
            foreach (GridViewRow gr in grGroup.Rows)
            {
                CheckBox cb = (CheckBox)gr.Cells[3].FindControl("isSelect");
                if (cb.Checked)
                {
                    group = new ADGroup();
                    group.Name = gr.Cells[0].Text;
                    group.Description = HttpUtility.HtmlDecode(gr.Cells[1].Text);
                    
                    group.Path = gr.Cells[2].Text;
                    groups.Add(group);
                }
            }

            Session[Constants.GroupsToAdd] = groups;

            Context.Response.Write("<script type='text/javascript'>window.frameElement.commitPopup();</script>");
            Context.Response.Flush();
            Context.Response.End();


        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Session[Constants.GroupsToAdd] = null;

            Context.Response.Write("<script type='text/javascript'>window.frameElement.commitPopup();</script>");
            Context.Response.Flush();
            Context.Response.End();
        }
    }
}