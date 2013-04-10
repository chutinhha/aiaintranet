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

namespace AIA.Intranet.Infrastructure.WebParts.FooterMenu
{
    public partial class FooterMenuUserControl : UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //if (!Page.IsPostBack)
            //{
                
            //}
            
            LoadData();
        }

        private void LoadData()
        {
            SPWeb rootWeb = SPContext.Current.Site.RootWeb;

            SPList footerMenuList = rootWeb.GetList(rootWeb.ServerRelativeUrl.TrimEnd('/') + "/" + Constants.FOOTER_MENU_LIST_URL.TrimStart('/'));

            if (footerMenuList != null)
            {
                StringBuilder htmlBuilder = new StringBuilder();

                string caml = string.Empty;
                var expressionsAnd = new List<Expression<Func<SPListItem, bool>>>();

                expressionsAnd.Add(x => ((bool)x[Constants.ACTIVE_COLUMN]) == true);

                caml = Camlex.Query().WhereAll(expressionsAnd).OrderBy(x => x[Constants.ORDER_NUMBER_COLUMN] as Camlex.Asc).ToString();

                SPQuery spQuery = new SPQuery();
                spQuery.Query = caml;

                SPListItemCollection items = footerMenuList.GetItems(spQuery);

                foreach (SPListItem item in items)
                {
                    if (item["URL"] != null)
                    {
                        SPFieldUrlValue urlValue = new SPFieldUrlValue(item["URL"].ToString());

                        htmlBuilder.AppendFormat("<a href='{0}'>{1}</a>", urlValue.Url, item.Title);
                    }
                    else
                    {
                        htmlBuilder.AppendFormat("<a href='#'>{0}</a>", item.Title);
                    }
                }

                ltFooterMenu.Text = htmlBuilder.ToString();
            }

        }
    }
}
