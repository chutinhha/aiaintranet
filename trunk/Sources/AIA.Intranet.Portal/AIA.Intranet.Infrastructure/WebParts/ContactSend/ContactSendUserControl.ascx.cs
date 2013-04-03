using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Microsoft.SharePoint;
using AIA.Intranet.Common.Utilities;
using AIA.Intranet.Model;

namespace AIA.Intranet.Infrastructure.WebParts.ContactSend
{
    public partial class ContactSendUserControl : UserControl
    {
        public string WebPartTitle { get; set; }
        public string WebPartDescription { get; set; }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            literalWebPartTitle.Text = this.WebPartTitle;
            literalWebPartDescription.Text = this.WebPartDescription;
            btnSubmit.Click += new EventHandler(btnSubmit_Click);
        }

        void btnSubmit_Click(object sender, EventArgs e)
        {
            
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadEnquiry();
            }
        }

        private void LoadEnquiry()
        {
            SPSecurity.RunWithElevatedPrivileges(delegate(){
                using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                {
                    using (SPWeb web = site.OpenWeb(SPContext.Current.Web.ID))
                    {
                        var enquiryList = CCIUtility.GetListFromURL(Constants.TYPE_OF_ENQUIRY_LIST_URL, web);
                        if (enquiryList != null)
                        {
                            var spListItemCollection = enquiryList.GetItems();
                            ddlTypeOfEnquiry.DataSource = spListItemCollection;
                            ddlTypeOfEnquiry.DataTextField = "Title";
                            ddlTypeOfEnquiry.DataValueField = "ID";
                            ddlTypeOfEnquiry.DataBind();
                        }
                    }
                }
            });
        }


    }
}
