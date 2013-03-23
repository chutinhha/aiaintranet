using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using AIA.Intranet.Common;
using AIA.Intranet.Model.Security;
using AIA.Intranet.Common.Extensions;
using Microsoft.SharePoint;
using AIA.Intranet.Model.Security;
using AIA.Intranet.Model;


namespace AIA.Intranet.Infrastructure.Layouts
{

    public partial class RuleList : SecuredPageLayout
    {
        private List<string> clientIDs = new List<string>();
        private SPList m_list;
        private SecuritySettings settings;

        #region Consts
        private const int ORDER_COLUMN = 2;
        private const int RULE_ID_COLUMN = 1;
        #endregion

        #region Properties
        protected SPList CurrentList
        {
            get
            {
                if (this.m_list == null)
                {
                    string listQS = base.Request.QueryString["List"];
                    if (listQS != null)
                    {
                        this.m_list = base.Web.Lists[new Guid(listQS)];
                    }
                }
                return this.m_list;
            }
        }

        protected string SourceUrl
        {
            get
            {
                return base.Request.QueryString["Source"];
            }
        }

        protected string CurrentUrlWithoutSource
        {
            get
            {
                if (Request.QueryString["Source"] != null)
                    return Request.Url.Scheme + "://" + Request.Url.Authority + Request.RawUrl.Remove(Request.RawUrl.IndexOf("&Source"));
                else
                    return Request.Url.Scheme + "://" + Request.Url.Authority + Request.RawUrl;
            }
        }
        #endregion

        #region Event Handling

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                settings = CurrentList.GetCustomSettings<SecuritySettings>(IOfficeFeatures.IOfficeApp);
                tblError.Visible = false;

                if (settings == null || settings.Rules.Count == 0)
                    BtnSave.Enabled = false;

                if (!Page.IsPostBack)
                {
                    BindRuleList(settings);
                }
            }
            catch
            {
                tblMain.Visible = false;
                tblError.Visible = true;
            }
        }

        protected void RuleListGrid_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DropDownList order = e.Row.FindControl("Order") as DropDownList;
                if (order != null)
                {
                    order.DataSource = settings.Rules;
                    order.DataTextField = "Order";
                    order.SelectedValue = ((Rule)e.Row.DataItem).Order.ToString();
                    order.DataBind();
                    clientIDs.Add(order.ClientID);
                }
            }
        }

        protected void RuleListGrid_DataBound(object sender, EventArgs e)
        {
            //build script for reorder comboboxes
            string clientIdArrayScript = "var Ir_Default =  new Array(";
            foreach (string s in clientIDs)
            {
                string arrVar = string.Format("new Array(\"{0}\", 1)", s);
                clientIdArrayScript += arrVar + ",";
            }
            clientIdArrayScript = clientIdArrayScript.TrimEnd(',') + ");";
            ScriptPlaceHolder.Text = string.Format("<script type=\"text/javascript\">\r\n//<![CDATA[\r\n{0}\r\n//]]>\r\n</script>", clientIdArrayScript);
        }

        protected void BtnAdd_Click(object sender, EventArgs e)
        {
            Response.Redirect(CurrentUrlWithoutSource.Replace("RuleList", "EditRule") + "&Source=" + CurrentUrlWithoutSource);
        }

        protected void BtnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect(Request.RawUrl.Replace("AIA.Intranet.Security/RuleList", "listedit") + "&Source=" + CurrentUrlWithoutSource);
        }

        protected void BtnSave_Click(object sender, EventArgs e)
        {
            foreach (GridViewRow row in this.RuleListGrid.Rows)
            {

                Control orderDropdown = row.Cells[ORDER_COLUMN].FindControl("Order");
                string strRuleId = row.Cells[RULE_ID_COLUMN].Text;
                if (orderDropdown != null)
                {
                    DropDownList dropList = (DropDownList)orderDropdown;
                    Rule rule = settings.Rules.FirstOrDefault(r => r.ID == strRuleId);
                    if (rule == null) continue;
                    rule.Order = Convert.ToInt32(dropList.SelectedValue);
                }
            }
            this.CurrentList.SetCustomSettings<SecuritySettings>(IOfficeFeatures.IOfficeApp, settings);
            BindRuleList(settings);
        }

        protected void BtnClearSetting_Click(object sender, EventArgs e)
        {
            this.CurrentList.RemoveCustomSettings<SecuritySettings>(IOfficeFeatures.IOfficeApp);
            this.tblMain.Visible = true;
            this.tblError.Visible = false;
            BindRuleList(settings);
        }

        private void BindRuleList(SecuritySettings settings)
        {
            if (settings != null && settings.Rules.Count > 0)
            {
                settings.Rules.Sort(
                    delegate(Rule r1, Rule r2)
                    {
                        return r1.Order.CompareTo(r2.Order);
                    });
                RuleListGrid.DataSource = settings.Rules;
                RuleListGrid.DataBind();
            }
        }

        protected string BuildQSContentType(string strContentType)
        {
            if (!string.IsNullOrEmpty(strContentType))
                return "&ContentTypeId=" + strContentType;
            return null;
        }
        #endregion
    }
}

