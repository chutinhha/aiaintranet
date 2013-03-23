using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.SharePoint;
using System.Collections.Generic;
using System.Linq;

using System;
using AIA.Intranet.Common.Utilities;

namespace AIA.Intranet.Infrastructure.Pages
{
    public partial class VariableEditor : Page
    {
        #region Controls
        protected DropDownList ddlType;
        protected ListBox lsbColumn;
        protected DropDownList ddlGroup;
        protected Panel pnlGroup;
        protected Label lbColumn;
        protected Label lbDescription;
        #endregion
        #region Handler
        protected override void OnInit(EventArgs e)
        {
            ddlType.SelectedIndexChanged += new EventHandler(ddlType_SelectedIndexChanged);
            ddlGroup.SelectedIndexChanged += new EventHandler(ddlGroup_SelectedIndexChanged);
            base.OnInit(e);
        }
        protected override void OnLoad(EventArgs e)
        {
            if(!IsPostBack)
                loadGroup();
        }
        protected void ddlType_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (ddlType.SelectedValue)
            {
                case "Global":
                    pnlGroup.Visible = false;
                    loadGlobalVariable();
                    lbColumn.Text = "Available variables:";
                    lbDescription.Text = "Variables";
                    break;

                case "Item":
                case "Task":
                    pnlGroup.Visible = true;
                    loadGroup();
                    lbColumn.Text = "Available site columns:";
                    lbDescription.Text = "Site Columns";
                    break;

                case "TaskList":
                    pnlGroup.Visible = false;
                    loadTaskListVariable();
                    lbColumn.Text = "Available columns:";
                    lbDescription.Text = "Columns";
                    break;
            }
        }

        protected void ddlGroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            loadSiteColumns(ddlGroup.SelectedValue);
        }
        #endregion
        #region Methods
        protected void loadGlobalVariable()
        {
            try
            {
                SPSecurity.RunWithElevatedPrivileges(delegate()
                {
                    using (SPSite oSite = new SPSite(SPContext.Current.Site.ID))
                    {
                        lsbColumn.Items.Clear();
                        SPList GlobalList = oSite.RootWeb.GetList(oSite.RootWeb.Url + "/Lists/GlobalConfigurations");
                        foreach (SPListItem i in GlobalList.Items)
                        {
                            if (string.Compare(i["Type"].ToString(), "Global Variable") == 0)
                                lsbColumn.Items.Add(new ListItem() { Text = i.Title, Value = i.Title });
                        }
                        if (lsbColumn.Items.Count > 0)
                            lsbColumn.Items[0].Selected = true;
                    }
                });
            }
            catch (Exception ex)
            {
                CCIUtility.LogError(ex.Message, "AIA.Intranet.Infrastructure.VariableEditor");
            }
        }

        protected void loadTaskListVariable()
        {
            try
            {
                lsbColumn.Items.Clear();
                lsbColumn.Items.Add(new ListItem() { Text = "ID", Value = "ID" });
                lsbColumn.Items.Add(new ListItem() { Text = "Name", Value = "Name" });
                if (lsbColumn.Items.Count > 0)
                    lsbColumn.Items[0].Selected = true;
            }
            catch (Exception ex)
            {
                CCIUtility.LogError(ex.Message, "AIA.Intranet.Infrastructure.VariableEditor");
            }
        }

        protected void loadGroup()
        {
            ddlGroup.Items.Clear();
            List<SPField> fields = SPContext.Current.Web.AvailableFields.Cast<SPField>().ToList();
            ddlGroup.Items.Add(new ListItem() { Text = "All Groups", Value = "All Groups" });
            fields = fields.Where(p => p.Group != "_Hidden").OrderBy(p => p.Group).ToList();
            while(fields != null && fields.Count > 0)
            {
                ddlGroup.Items.Add(new ListItem() { Text = fields[0].Group, Value = fields[0].Group });
                fields = fields.Where(p => p.Group != fields[0].Group).ToList();
            }
            ddlGroup.SelectedIndex = 0;
            loadSiteColumns("All Groups");
        }
        protected void loadSiteColumns(string GroupName)
        {
            lsbColumn.Items.Clear();
            IEnumerable<SPField> fields = SPContext.Current.Web.AvailableFields.Cast<SPField>();
            List<SPField> results = fields.Where(p => !p.Hidden && (p.Group == GroupName || GroupName == "All Groups"))
                 .OrderBy(p => p.Title)
                 .ToList();
            foreach (SPField field in results)
            {
                lsbColumn.Items.Add(new ListItem() { Text = field.Title, Value = field.Title });
            }
            if (lsbColumn.Items.Count > 0)
                lsbColumn.Items[0].Selected = true;
        }
        #endregion
    }
}
