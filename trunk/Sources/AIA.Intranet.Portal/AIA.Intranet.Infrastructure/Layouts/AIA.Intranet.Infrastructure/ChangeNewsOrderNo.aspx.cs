using System;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using AIA.Intranet.Model;
using System.Collections.Generic;
using System.Linq.Expressions;
using AIA.Intranet.Common.Utilities.Camlex;
using AIA.Intranet.Common.Utilities;

namespace AIA.Intranet.Infrastructure.Layouts
{
    public partial class ChangeNewsOrderNo : LayoutsPageBase
    {
        protected override void OnInit(EventArgs e)
        {
            btnCancel.Click += new EventHandler(btnCancel_Click);
            btnSave.Click += new EventHandler(btnSave_Click);

            base.OnInit(e);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                string listId = Request.QueryString["ListId"];
                string itemId = Request.QueryString["ID"];
                string sourceUrl = Request.QueryString["Source"];

                SPSecurity.RunWithElevatedPrivileges(delegate
                {
                    using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                    {
                        using (SPWeb web = site.OpenWeb(SPContext.Current.Web.ID))
                        {
                            SPList list = web.Lists[new Guid(listId)];
                            SPListItem item = list.Items.GetItemById(Convert.ToInt32(itemId));

                            txtOrderNo.Text = item[Constants.ORDER_NUMBER_COLUMN] != null ? item[Constants.ORDER_NUMBER_COLUMN].ToString() : string.Empty;
                        }
                    }
                });
            }
        }


        private void btnCancel_Click(object sender, EventArgs e)
        {
            ClosePopup();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            int orderNo = 0;
            int.TryParse(txtOrderNo.Text, out orderNo);

            if (orderNo != 0)
            {
                string listId = Request.QueryString["ListId"];
                string itemId = Request.QueryString["ID"];
                string sourceUrl = Request.QueryString["Source"];

                SPSecurity.RunWithElevatedPrivileges(delegate
                {
                    using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                    {
                        using (SPWeb web = site.OpenWeb(SPContext.Current.Web.ID))
                        {
                            web.AllowUnsafeUpdates = true;

                            try
                            {
                                SPList curList = web.Lists[new Guid(listId)];
                                SPListItem curItem = curList.Items.GetItemById(Convert.ToInt32(itemId));

                                string caml = string.Empty;
                                var expressionsAnd = new List<Expression<Func<SPListItem, bool>>>();

                                expressionsAnd.Add(x => ((int)x[Constants.ORDER_NUMBER_COLUMN]) >= orderNo);
                                expressionsAnd.Add(x => (x["ID"]) != (DataTypes.Counter)itemId);

                                caml = Camlex.Query().WhereAll(expressionsAnd).OrderBy(x => x[Constants.ORDER_NUMBER_COLUMN] as Camlex.Asc).ToString();

                                SPQuery spQuery = new SPQuery();
                                spQuery.Query = caml;

                                SPListItemCollection items = curList.GetItems(spQuery);

                                using (DisableItemEvent scope = new DisableItemEvent())
                                {
                                    curItem[Constants.ORDER_NUMBER_COLUMN] = orderNo;
                                    curItem.SystemUpdate(false);
                                }

                                foreach (SPListItem item in items)
                                {
                                    orderNo++;

                                    using (DisableItemEvent scope = new DisableItemEvent())
                                    {
                                        item[Constants.ORDER_NUMBER_COLUMN] = orderNo;
                                        item.SystemUpdate(false);
                                    }
                                }
                            }
                            catch (Exception ex) { }
                            finally
                            {
                                web.AllowUnsafeUpdates = false;
                            }    
                        }
                    }
                });
            }

            ClosePopup();
        }

        private void ClosePopup()
        {
            Context.Response.Clear();
            Context.Response.Write("<script type='text/javascript'>window.frameElement.commitPopup();</script>");
            Context.Response.Flush();
            Context.Response.End();
        }
    }
}
