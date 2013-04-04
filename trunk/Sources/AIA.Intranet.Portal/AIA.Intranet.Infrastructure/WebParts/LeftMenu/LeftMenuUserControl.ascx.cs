using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Microsoft.SharePoint;
using System.Collections.Generic;
using System.Linq.Expressions;
using AIA.Intranet.Model;
using AIA.Intranet.Common.Utilities.Camlex;
using System.Text;
using System.Web;

namespace AIA.Intranet.Infrastructure.WebParts.LeftMenu
{
    public partial class LeftMenuUserControl : UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                LoadData();
            }
        }

        private void LoadData()
        {
            SPWeb currentWeb = SPContext.Current.Web;

            SPList leftMenuList = currentWeb.GetList(currentWeb.ServerRelativeUrl.TrimEnd('/') + "/" + Constants.LEFT_MENU_LIST_URL.TrimStart('/'));

            if (leftMenuList != null)
            {
                StringBuilder htmlBuilder = new StringBuilder();

                string caml = string.Empty;
                var expressionsAnd = new List<Expression<Func<SPListItem, bool>>>();

                expressionsAnd.Add(x => ((bool)x[Constants.ACTIVE_COLUMN]) == true);

                caml = Camlex.Query().WhereAll(expressionsAnd).OrderBy(x => x[Constants.ORDER_NUMBER_COLUMN] as Camlex.Asc).ToString();

                SPQuery spQuery = new SPQuery();
                spQuery.Query = caml;

                SPListItemCollection items = leftMenuList.GetItems(spQuery);

                if (items != null && items.Count > 0)
                {
                    for (int i = 0; i < items.Count; i++ )
                    {
                        SPListItem item = items[i];

                        string classNoBorder = string.Empty;
                        if (i == items.Count - 1) classNoBorder = "class='noBorder'";

                        if (item["MenuKeywords"] != null && HttpContext.Current.Request.Url.AbsolutePath.ToLower().Contains(item["MenuKeywords"].ToString().ToLower()))
                        {
                            if (item["URL"] != null)
                            {
                                SPFieldUrlValue urlValue = new SPFieldUrlValue(item["URL"].ToString());

                                htmlBuilder.AppendFormat("<li {0}><a href='{1}'>{2}</a></li>", classNoBorder, urlValue.Url, item.Title);
                            }
                            else
                            {
                                htmlBuilder.AppendFormat("<li {0}><a href='#'>{1}</a></li>", classNoBorder, item.Title);
                            }
                        }
                    }

                    if (htmlBuilder.Length > 0)
                        ltLeftMenu.Text = "<ul class='child'>" + htmlBuilder.ToString() + "</ul>";
                }
            }

            //hide left-panel if no left menu
            if (string.IsNullOrEmpty(ltLeftMenu.Text))
            {
                string htmlHideLeftPanel = @"
                                                <style type='text/css'>
                                                    #s4-leftpanel{display: none;}
                                                    .s4-ca{margin-left: 0;}
                                                </style>
                                            ";
                ltLeftMenu.Text = htmlHideLeftPanel;
            }
        }
    }
}
