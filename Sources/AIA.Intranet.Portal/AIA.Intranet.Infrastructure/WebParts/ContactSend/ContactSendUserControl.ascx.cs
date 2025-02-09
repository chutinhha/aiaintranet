﻿using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Microsoft.SharePoint;
using AIA.Intranet.Common.Utilities;
using AIA.Intranet.Model;
using AIA.Intranet.Common.Extensions;
using Microsoft.SharePoint.Utilities;

namespace AIA.Intranet.Infrastructure.WebParts.ContactSend
{
    public partial class ContactSendUserControl : UserControl
    {
        public string WebPartTitle { get; set; }
        public string WebPartDescription { get; set; }
        public string WebPartMessage { get; set; }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            literalWebPartTitle.Text = this.WebPartTitle;
            literalWebPartDescription.Text = this.WebPartDescription;
            divMessages.InnerText = this.WebPartMessage;
            btnSubmit.Click += new EventHandler(btnSubmit_Click);
        }

        private void SetFormField()
        {
            SPSecurity.RunWithElevatedPrivileges(delegate()
            {
                using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                {
                    using (SPWeb web = site.OpenWeb(SPContext.Current.Web.ID))
                    {
                        var opinionList = Utility.GetListFromURL(Constants.OPINION_LIST_URL, web);
                        if (opinionList != null)
                        {
                            //ffContent.ListId = opinionList.ID;
                        }
                    }
                }
            });
        }

        void btnSubmit_Click(object sender, EventArgs e)
        {
            SendEmail();
            divContent.Style.Add("display", "none");
            divMessages.Style.Remove("display");
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadEnquiry();
            }
        }

        private void SendEmail()
        {
            try
            {
                SPSecurity.RunWithElevatedPrivileges(delegate()
                {
                    using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                    {
                        using (SPWeb web = site.OpenWeb(SPContext.Current.Web.ID))
                        {
                            var opinionList = Utility.GetListFromURL(Constants.OPINION_LIST_URL, web);
                            var enquiryList = Utility.GetListFromURL(Constants.TYPE_OF_ENQUIRY_LIST_URL, web);
                            if (opinionList != null && enquiryList != null)
                            {
                                string emailAddress = GetEmailByEnquiry(web, int.Parse(ddlTypeOfEnquiry.SelectedValue));
                                if (string.IsNullOrEmpty(emailAddress))
                                {
                                    string internalEmail = enquiryList.GetCustomProperty(Constants.CONTACT_INTERNAL_EMAIL_PROPERTY);
                                    if (!string.IsNullOrEmpty(internalEmail))
                                    {
                                        string[] internalEmailArray = internalEmail.Split(',');
                                        foreach (string item in internalEmailArray)
                                        {
                                            SPUser user = web.EnsureUser(item);
                                            emailAddress += user.Email + ";";
                                        }
                                    }
                                    string externalEmail = enquiryList.GetCustomProperty(Constants.CONTACT_EXTERNAL_EMAIL_PROPERTY);
                                    if (!string.IsNullOrEmpty(externalEmail))
                                    {
                                        emailAddress += externalEmail;
                                    }
                                }

                                string emailTitle = enquiryList.GetCustomProperty(Constants.CONTACT_TITLE_EMAIL_PROPERTY);
                                if (string.IsNullOrEmpty(emailTitle))
                                {
                                    emailTitle = opinionList.Title;
                                }

                                string isAddDate = enquiryList.GetCustomProperty(Constants.CONTACT_ADD_DATE_EMAIL_PROPERTY);
                                if (!string.IsNullOrEmpty(isAddDate) && bool.Parse(isAddDate))
                                {
                                    emailTitle = string.Format("[{0}] - {1}", ddlTypeOfEnquiry.SelectedItem.Text, emailTitle);
                                }

                                //string emailHeader = enquiryList.GetCustomProperty(Constants.CONTACT_HEADER_EMAIL_PROPERTY);

                                string emailBodySetting = enquiryList.GetCustomProperty(Constants.CONTACT_BODY_HTML_EMAIL_PROPERTY);
                                string emailBody = txtContent.Text.Replace(System.Environment.NewLine, "<br />");

                                if (!string.IsNullOrEmpty(emailBodySetting))
                                {
                                    try
                                    {
                                        emailBody = string.Format(emailBodySetting, emailBody);
                                    }
                                    catch
                                    {
                                        emailBody = txtContent.Text.Replace(System.Environment.NewLine, "<br />"); ;
                                    }
                                    
                                }

                                web.AllowUnsafeUpdates = true;
                                SPListItem spListItem = opinionList.Items.Add();
                                spListItem["Title"] = emailTitle;
                                spListItem["Content"] = txtContent.Text;
                                spListItem["TypeOfEnquiry"] = ddlTypeOfEnquiry.SelectedValue;
                                spListItem["Author"] = SPContext.Current.Web.CurrentUser;
                                spListItem["Editor"] = SPContext.Current.Web.CurrentUser;
                                spListItem.SystemUpdate();
                                web.AllowUnsafeUpdates = false;
                                SPUtility.SendEmail(web, true, false, emailAddress, emailTitle, emailBody);
                            }
                        }
                    }
                });
            }
            catch (Exception ex)
            {
                Utility.LogError(ex.Message, AIAPortalFeatures.Infrastructure);
            }
        }

        private string GetEmailByEnquiry(SPWeb web, int id)
        {
            string email = string.Empty;
            try
            {
                var enquiryList = Utility.GetListFromURL(Constants.TYPE_OF_ENQUIRY_LIST_URL, web);
                if (enquiryList != null)
                {
                    SPListItem item = enquiryList.GetItemById(id);
                    if (item["InternalEmail"] != null)
                    {
                        SPFieldUserValueCollection userValueCollection = new SPFieldUserValueCollection(web, item["InternalEmail"].ToString());
                        if (userValueCollection != null && userValueCollection.Count > 0)
                        {
                            foreach (SPFieldUserValue us in userValueCollection)
                            {
                                email += us.User.Email + ";";
                            }
                        }
                    }
                                        
                    email += (item["ExternalEmail"] == null ? string.Empty : item["ExternalEmail"].ToString());
                }
            }
            catch (Exception ex)
            {
                Utility.LogError(ex.Message, AIAPortalFeatures.Infrastructure);
            }
            return email;
        }

        private void LoadEnquiry()
        {
            SPSecurity.RunWithElevatedPrivileges(delegate()
            {
                using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                {
                    using (SPWeb web = site.OpenWeb(SPContext.Current.Web.ID))
                    {
                        var enquiryList = Utility.GetListFromURL(Constants.TYPE_OF_ENQUIRY_LIST_URL, web);
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
