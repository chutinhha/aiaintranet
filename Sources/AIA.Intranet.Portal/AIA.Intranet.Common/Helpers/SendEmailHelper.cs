using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net.Mail;
using System.Reflection;
using System.Text.RegularExpressions;
using AIA.Intranet.Common.Extensions;
using AIA.Intranet.Common.Utilities;

using Microsoft.SharePoint;
using Microsoft.SharePoint.Administration;
using Microsoft.SharePoint.Utilities;
using Microsoft.SharePoint.Workflow;
using SPDisposeCheck;
using AIA.Intranet.Model.Infrastructure;
using AIA.Intranet.Model;

using AIA.Intranet.Common.Services;
using AIA.Intranet.Model.Entities;
using AIA.Intranet.Resources;

namespace AIA.Intranet.Common.Helpers
{
    
	public class SendEmailHelper
	{
		public static void SendEmailbytemplate(SPListItem item, SPListItem itemTemplate, string to, string cc, string variables)
		{
			SendEmailbytemplate(item, null, itemTemplate, to, cc, variables, true);
		}

		public static void SendEmailbytemplate(SPWeb web, SPListItem itemTemplate, string to, string cc, string variables)
		{
			SendEmailbytemplate(web, null, itemTemplate, to, cc, variables, true);
		}

		
		public static void SendEmailbytemplate(SPListItem item, SPListItem itemTemplate, string to)
		{
			SendEmailbytemplate(item, null, itemTemplate, to, string.Empty, string.Empty, false);
		}

		public static void SendEmailbytemplate(SPListItem item, SPListItem taskItem, SPListItem itemTemplate, string to)
		{
			SendEmailbytemplate(item, taskItem, itemTemplate, to, string.Empty, string.Empty, true);
		}

		public static void SendEmailbytemplate(SPListItem item, SPListItem taskItem, SPListItem itemTemplate, string to, bool attachTaskLink)
		{
			SendEmailbytemplate(item, taskItem, itemTemplate, to, string.Empty, string.Empty, attachTaskLink);
		}

		
		public static void SendEmailbytemplate(SPListItem item, SPListItem taskItem, SPListItem itemTemplate, string to, string cc)
		{
			SendEmailbytemplate(item, taskItem, itemTemplate, to, cc, string.Empty, true);
		}

		private static string EnsureAbsoluteUrl(SPSite site, string content)
		{
			string imgPattern = "src=(?:\"|\')?(?<repalace>\\/[^>]*[^/].(?:jpg|bmp|gif|png))(?:\"|\')?";
			string hrefPattern = "href=\\\"(?<repalace>\\/[^\\\"]*)\\\"";

			string returnValue = EnsureAbsoluteUrl(site, content, imgPattern);
			returnValue = EnsureAbsoluteUrl(site, returnValue, hrefPattern);
			return SPHttpUtility.HtmlDecode(returnValue);
		}

		private static string EnsureAbsoluteUrl(SPSite site, string content, string pattern)
		{
			var matches = Regex.Matches(content, pattern, RegexOptions.IgnoreCase);
			if (matches.Count > 0)
			{
				Match math = matches[0];

				string url = math.Groups["repalace"].Value;
				if (SPUrlUtility.IsUrlRelative(url))
				{
					string siteUrl = site.WebApplication.GetResponseUri(SPUrlZone.Internet).AbsoluteUri.Trim('/');
					content = content.Replace(string.Format("\"{0}\"", url), string.Format("\"{0}\"", siteUrl + url));
				}
				if (matches.Count > 1)
				{
					content = EnsureAbsoluteUrl(site, content, pattern);
				}
			}
			return content;
		}


		public static void SendEmailbytemplate(SPListItem item, SPListItem taskItem, SPListItem itemTemplate, string to, string cc, string variables, bool attachTaskLink)
		{
			string subject = string.Empty;

			//string body =itemTemplate["Body"].ToString();
			string body = EnsureAbsoluteUrl(item.Web.Site, itemTemplate["Body"].ToString()); 

			bool sendAsPlainMode = false;

			if (itemTemplate.VerifyFieldAccess("SendAsPlainText")) //Fields.ContainsFieldWithStaticName("SendAsPlainText") && itemTemplate["SendAsPlainText"] != null)
			{
				sendAsPlainMode = Convert.ToBoolean(itemTemplate["SendAsPlainText"].ToString());
			}
			if (itemTemplate["Subject"] != null)
				subject = itemTemplate["Subject"].ToString();

			SendEmailbytemplate(item, taskItem, to, cc, variables, subject, body, sendAsPlainMode, attachTaskLink);
		}

		public static void SendEmailbytemplate(SPWeb web, SPListItem taskItem, SPListItem itemTemplate, string to, string cc, string variables, bool attachTaskLink)
		{
			string subject = string.Empty;

			//string body = itemTemplate["Body"].ToString();
			string body = EnsureAbsoluteUrl(web.Site, itemTemplate["Body"].ToString()); 
			bool sendAsPlainMode = false;

			if (itemTemplate.VerifyFieldAccess("SendAsPlainText")) //Fields.ContainsFieldWithStaticName("SendAsPlainText") && itemTemplate["SendAsPlainText"] != null)
			{
				sendAsPlainMode = Convert.ToBoolean(itemTemplate["SendAsPlainText"].ToString());
			}
			if (itemTemplate["Subject"] != null)
				subject = itemTemplate["Subject"].ToString();

			SendEmailbytemplate(web, taskItem, to, cc, variables, subject, body, sendAsPlainMode);
		}

		//public static void SendEmailbytemplate(SPListItem item, string to, string cc, string subject, string body)
		//{
		//    SendEmailbytemplate(item, null, null, to, cc, string.Empty, subject, body );
		//}

		public static void SendEmailByTemplateWithAttachments(SPListItem item, MailAddress fromAddress, string to, string cc, string subject, string body, List<Attachment> attachments, SPUser currentUser)
		{
			SendEmailByTemplateWithAttachments(item, fromAddress, to, string.Empty, cc, subject, body, attachments, currentUser);
		}

		public static void SendEmailByTemplateWithAttachments(SPListItem item, MailAddress fromAddress, string to, string cc, SPListItem itemTemplate, List<Attachment> attachments, SPUser currentUser)
		{
		   
			SendEmailByTemplateWithAttachments(item, fromAddress, to, string.Empty, cc, itemTemplate, attachments, currentUser);
		}

		public static void SendEmailByTemplateWithAttachments(SPListItem item, MailAddress fromAddress, string to, string reply, string cc, SPListItem itemTemplate, List<Attachment> attachments, SPUser currentUser)
		{
			List<EmailVariable> bodyVariables = null;
			List<EmailVariable> subjectVariables = null;
			List<EmailVariable> designerVariables = new List<EmailVariable>();
			StringDictionary header;

			string subject = string.Empty;
			bool sendAsPlainMode = false;
			//string body = itemTemplate["Body"].ToString();
			string body = EnsureAbsoluteUrl(item.Web.Site, itemTemplate["Body"].ToString()); 
			if (itemTemplate["Subject"] != null)
				subject = itemTemplate["Subject"].ToString();

			bodyVariables = Parser.GetAndSetVariablesFromTemplate(body, item, null, null);
            subjectVariables = Parser.GetAndSetVariablesFromTemplate(subject, item, null, null);
            subject = Parser.SetValuesToContent(subject, subjectVariables, designerVariables);
            body = Parser.SetValuesToContent(body, bodyVariables, designerVariables);

			if (itemTemplate.VerifyFieldAccess("SendAsPlainText"))
			{
				sendAsPlainMode = Convert.ToBoolean(itemTemplate[new Guid("1BCADBD1-CC15-40A0-AAF7-6DE222412337")].ToString());
				if (sendAsPlainMode)                
					body = body.ToPlainText();
			}

			MailMessage message = new MailMessage
			{
				Subject = subject,
				Body = body,
				IsBodyHtml = !sendAsPlainMode,                
			};

			
			message.From = fromAddress;


			// To mail
			string[] arrTo = to.Split(";".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
			foreach (string t in arrTo)
				message.To.Add(t);

			// CC mail
			if (!String.IsNullOrEmpty(cc))
			{
				string[] arrCC = cc.Split(";".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
				foreach (string c in arrCC)
					message.CC.Add(c);
			}

			// Reply and body
			if (!string.IsNullOrEmpty(reply))
			{
				message.ReplyTo = new MailAddress(reply);
			}
			else
			{

				if (currentUser != null && !string.IsNullOrEmpty(currentUser.Email))
					message.ReplyTo = new MailAddress(currentUser.Email);
				else

					message.Body += "<br><br><i> System generated email, please do not reply to this message</i>";
			}

			foreach (Attachment a in attachments)
				message.Attachments.Add(a);

			// Send message
			//string server = SPContext.Current.Site.WebApplication.OutboundMailServiceInstance.Server.Address;
			//SmtpClient client = new SmtpClient(server);
			SmtpClient client = getSmtpInformation();
			client.Send(message);
		}

		public static void SendEmailByTemplateWithAttachments(SPListItem item, MailAddress fromAddress, string to, string reply, string cc, string subject, string body, List<Attachment> attachments, SPUser currentUser)
		{
			List<EmailVariable> bodyVariables = null;
			List<EmailVariable> subjectVariables = null;
			List<EmailVariable> designerVariables = new List<EmailVariable>();

            bodyVariables = Parser.GetAndSetVariablesFromTemplate(body, item, null, null);
            subjectVariables = Parser.GetAndSetVariablesFromTemplate(subject, item, null, null);
            subject = Parser.SetValuesToContent(subject, subjectVariables, designerVariables);
            body = Parser.SetValuesToContent(body, bodyVariables, designerVariables);

			MailMessage message = new MailMessage
			{
				Subject = subject,
				Body = body,
				IsBodyHtml = true,
			};
 
			message.From = fromAddress;

			
			// To mail
			string[] arrTo = to.Split(";".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
			foreach (string t in arrTo)
				message.To.Add(t);
			
			// CC mail
			if (!String.IsNullOrEmpty(cc))
			{
				string[] arrCC = cc.Split(";".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
				foreach (string c in arrCC)
					message.CC.Add(c);
			}

			// Reply and body
			if (!string.IsNullOrEmpty(reply))
			{
				message.ReplyTo = new MailAddress(reply);
			}
			else
			{

				if (currentUser != null && !string.IsNullOrEmpty(currentUser.Email))
					message.ReplyTo = new MailAddress(currentUser.Email);
				else

					message.Body += "<br><br><i> System generated email, please do not reply to this message</i>";
			}

			 foreach (Attachment a in attachments)
				message.Attachments.Add(a);

			// Send message
			//string server = SPContext.Current.Site.WebApplication.OutboundMailServiceInstance.Server.Address;
			//SmtpClient client = new SmtpClient(server);
			 SmtpClient client = getSmtpInformation();
			client.Send(message);
		}

		public static void SendEmailByTemplateWithAttachments(SPListItem item, MailAddress fromAddress, string to, string reply, string cc, string subject, string body, bool isSendPlainText, List<Attachment> attachments, SPUser currentUser)
		{
			List<EmailVariable> bodyVariables = null;
			List<EmailVariable> subjectVariables = null;
			List<EmailVariable> designerVariables = new List<EmailVariable>();

            bodyVariables = Parser.GetAndSetVariablesFromTemplate(body, item, null, null);
            subjectVariables = Parser.GetAndSetVariablesFromTemplate(subject, item, null, null);
            subject = Parser.SetValuesToContent(subject, subjectVariables, designerVariables);
            body = Parser.SetValuesToContent(body, bodyVariables, designerVariables);
			body = isSendPlainText ? body.ToPlainText() : body;

			MailMessage message = new MailMessage
			{
				Subject = subject,
				Body = body,
				IsBodyHtml = !isSendPlainText,
			};

			message.From = fromAddress;


			// To mail
			string[] arrTo = to.Split(";".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
			foreach (string t in arrTo)
				message.To.Add(t);

			// CC mail
			if (!String.IsNullOrEmpty(cc))
			{
				string[] arrCC = cc.Split(";".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
				foreach (string c in arrCC)
					message.CC.Add(c);
			}

			// Reply and body
			if (!string.IsNullOrEmpty(reply))
			{
				message.ReplyTo = new MailAddress(reply);
			}
			else
			{

				if (currentUser != null && !string.IsNullOrEmpty(currentUser.Email))
					message.ReplyTo = new MailAddress(currentUser.Email);
				else

					message.Body += "<br><br><i> System generated email, please do not reply to this message</i>";
			}

			foreach (Attachment a in attachments)
				message.Attachments.Add(a);
			
			SmtpClient client = getSmtpInformation();
			client.Send(message);
		}

		private static void SendEmailbytemplate(SPListItem item, SPListItem taskItem, string to, string cc, string variables, string subject, string body, bool sendAsPlainText, bool attachTaskLink)
		{
			List<EmailVariable> designerVariables = null;
			List<EmailVariable> bodyVariables = null;
			List<EmailVariable> subjectVariables = null;

            bodyVariables = Parser.GetAndSetVariablesFromTemplate(body, item, taskItem);
            subjectVariables = Parser.GetAndSetVariablesFromTemplate(subject, item, taskItem);
            designerVariables = Parser.GetVariablesFromDesigner(variables);
            subject = Parser.SetValuesToContent(subject, subjectVariables, designerVariables);
            body = Parser.SetValuesToContent(body, bodyVariables, designerVariables);

            if (sendAsPlainText)
				body = body.ToPlainText();

			SPSecurity.RunWithElevatedPrivileges(delegate()
			{
				using (SPSite site = new SPSite(item.Web.Site.ID))
				{
					using (SPWeb web = site.OpenWeb(item.Web.ID))
					{
						StringDictionary headers = new StringDictionary();
						if (attachTaskLink)
							headers = buidEmailHeaders(taskItem, to, cc, subject, sendAsPlainText);
						else
							headers = buidEmailHeaders(null, to, cc, subject, sendAsPlainText);
						SPUtility.SendEmail(web, headers, body);
					}
				}
			}
			);
		}

		//TODO : Should refactore this method to avoid duplicate code.

		private static void SendEmailbytemplate(SPWeb web, SPListItem taskItem,  string to, string cc, string variables, string subject, string body, bool sendAsPlainText)
		{
			List<EmailVariable> designerVariables = null;
			List<EmailVariable> bodyVariables = null;
			List<EmailVariable> subjectVariables = null;

            bodyVariables = Parser.GetAndSetVariablesFromTemplate(body, null, taskItem);
            subjectVariables = Parser.GetAndSetVariablesFromTemplate(subject, null, taskItem);
            designerVariables = Parser.GetVariablesFromDesigner(variables);
            subject = Parser.SetValuesToContent(subject, subjectVariables, designerVariables);
            body = Parser.SetValuesToContent(body, bodyVariables, designerVariables);

			if (sendAsPlainText)
				body = body.ToPlainText();


			StringDictionary headers = new StringDictionary();
			headers = buidEmailHeaders(taskItem, to, cc, subject, sendAsPlainText);

			SPUtility.SendEmail(web, headers, body);

		}

		private static StringDictionary buidEmailHeaders(SPListItem taskItem, string to, string cc, string subject, bool isPlainText)
		{
			StringDictionary headers = new StringDictionary();
			headers.Add("to", to);
			
			if (!string.IsNullOrEmpty(cc))
				headers.Add("cc", cc);
			headers.Add("subject", subject);
			if (isPlainText)
			{
				headers["Content-Type"] = "text/plain; charset=utf-8";
			}
			else
			{
				headers["Content-Type"] = "text/html; charset=utf-8";
			}

			if (taskItem == null) return headers;
			headers["X-Sharing-Title"] = taskItem.Title.EncodeAlertWord();
			headers["Content-Class"] = "MSWorkflowTask";
			headers["Content-Transfer-Encoding"] = "8bit ";
			headers["X-Sharing-Remote-Uid"] = taskItem.ParentList.ID.ToString();
			headers["X-Sharing-ItemID"] = taskItem.ID.ToString().EncodeAlertWord();
			
			headers["X-Sharing-WSSBaseUrl"] = taskItem.Web.Url.EncodeAlertWord();
			return headers;
		}

		public static string ParseTemplateContent(string content, SPListItem item)
		{
			return ParseTemplateContent(content, item, null);
		}

		public static string ParseTemplateContent(string content, SPListItem item, SPUser user)
		{
			string outputStr = content;
			SPSecurity.RunWithElevatedPrivileges(delegate()
			{
				try
				{
					//SPUser user = (SPContext.Current != null && SPContext.Current.Web != null) ? SPContext.Current.Web.CurrentUser : null;

					List<EmailVariable> bodyVariables = new List<EmailVariable>();
                    bodyVariables = Parser.GetAndSetVariablesFromTemplate(content, item, null, user);
                    outputStr = Parser.SetValuesToContent(content, bodyVariables, new List<EmailVariable>());
				}
				catch
				{
					
				}
			});
			return outputStr;
		}

		public static string GetEmailFieldFromTemplate(SPListItem sourceListItem, SPListItem emailListItem, string fieldName)
		{
			return GetEmailFieldFromTemplate(sourceListItem, emailListItem, fieldName, null);
		}

		public static string GetEmailFieldFromTemplate(SPListItem sourceListItem, SPListItem emailListItem, string fieldName, SPUser user)
		{
			string fieldValue = string.Empty;
			SPSecurity.RunWithElevatedPrivileges(delegate()
			{
				try
				{
					fieldValue = emailListItem[fieldName].ToString();
					List<EmailVariable> bodyVariables = new List<EmailVariable>();
                    bodyVariables = Parser.GetAndSetVariablesFromTemplate(fieldValue, sourceListItem, null, user);
                    fieldValue = Parser.SetValuesToContent(fieldValue, bodyVariables, new List<EmailVariable>());
				}
				catch
				{
					CCIUtility.LogInfo("Email template could not be located", "AIA.Intranet.Workflow");
				}
			});
			return fieldValue;
		}
	   
		

		
		

		private static SmtpClient getSmtpInformation()
		{
			string smtpServer = SPAdministrationWebApplication.Local.OutboundMailServiceInstance.Server.Address;
			SmtpClient client = new SmtpClient(smtpServer);
			return client;
		}

		public static string GetEmailFromFieldValue(SPListItem item, string fieldId)
		{
			string emails = string.Empty;
			try
			{
                
                if (item[new Guid(fieldId)] != null)
				{
                    var field = item.ParentList.Fields[new Guid(fieldId)];

                    if (field.Type == SPFieldType.User)
                    {
                        SPFieldUserValueCollection userValues = new SPFieldUserValueCollection(item.Web.Site.RootWeb, item[new Guid(fieldId)].ToString());
                        foreach (SPFieldUserValue userValue in userValues)
                        {
                            if (userValue.User == null)
                            {
                                SPGroup group = item.Web.Site.RootWeb.SiteGroups.GetByID(userValue.LookupId);
                                foreach (SPUser user in group.Users)
                                {
                                    if (!string.IsNullOrEmpty(user.Email))
                                        emails += userValue.User.Email + ";";
                                }
                            }
                            else
                            {
                                if (!string.IsNullOrEmpty(userValue.User.Email))
                                    emails += userValue.User.Email + ";";
                            }
                        }
                    }
                    else if (field.TypeAsString == "LookupFieldWithPicker")
                    {
                        SPWeb rootWeb = item.ParentList.ParentWeb.Site.RootWeb;
                        SPFieldLookupValueCollection attendees = (SPFieldLookupValueCollection)item[new Guid(fieldId)];

                        if (attendees != null && attendees.Count > 0)
                        {
                            foreach (var attendee in attendees)
                            {
                                Employee employee = EmployeeService.GetEmployeeByItemId(attendee.LookupId, rootWeb);
                                emails += employee.UserEmail + ";";
                            }
                        }
                    }

					
				}
				emails.Trim(';');
			}
			catch { }
			return emails;
		}
		
		public static SPListItem GetEmailTemplateItem(SPWorkflowActivationProperties workflowPros, string templateListURL, string templateName)
		{
			SPList emailTemplateList = workflowPros.GetListFromURL(templateListURL.Split(',')[0]);
			if (emailTemplateList == null) return null;

			SPListItemCollection emailListItems = emailTemplateList.FindItems("Title", templateName);
			if (emailListItems.Count == 0) return null;
			return emailListItems[0];
		}

		public static SPListItem GetEmailTemplateItem(string templateListURL, string templateName)
		{
			SPList emailTemplateList = SPContext.Current.Site.GetListFromURL(templateListURL.Split(',')[0]);
			if (emailTemplateList == null) return null;

			SPListItemCollection emailListItems = emailTemplateList.FindItems("Title", templateName);
			if (emailListItems.Count == 0) return null;
			return emailListItems[0];
		}

		private static string ParseContentWithVariables(string body, List<NameValue> variables)
		{
			string results = body;
			if (variables != null && variables.Count > 0)
			{
				foreach (var varriable in variables)
				{
					results = results.Replace(varriable.Name, varriable.Value);
				}
			}
			return results;
		}

		public static void SendEmailbytemplateEx(SPListItem item, SPListItem taskItem, List<NameValue> variables, SPListItem itemTemplate, string to)
		{
			SendEmailbytemplateEx(item, taskItem, variables, itemTemplate, to, string.Empty, string.Empty);
		}

		public static void SendEmailbytemplateEx(SPListItem item, SPListItem taskItem, List<NameValue> xvariables, SPListItem itemTemplate, string to, string cc, string variables)
		{

			List<EmailVariable> designerVariables = null;
			List<EmailVariable> bodyVariables = null;
			List<EmailVariable> subjectVariables = null;
			string subject = string.Empty;
			bool sendAsPlainMode = false;
			string body = EnsureAbsoluteUrl(item.Web.Site, itemTemplate["Body"].ToString()); 
			//string body = itemTemplate["Body"].ToString();

			if (itemTemplate["Subject"] != null)
				subject = itemTemplate["Subject"].ToString();

            bodyVariables = Parser.GetAndSetVariablesFromTemplate(body, item, taskItem);
            subjectVariables = Parser.GetAndSetVariablesFromTemplate(subject, item, taskItem);
            designerVariables = Parser.GetVariablesFromDesigner(variables);
            subject = Parser.SetValuesToContent(subject, subjectVariables, designerVariables);
            body = Parser.SetValuesToContent(body, bodyVariables, designerVariables);
			subject = ParseContentWithVariables(subject, xvariables);
			body = ParseContentWithVariables(body, xvariables);

			if (itemTemplate.VerifyFieldAccess("SendAsPlainText"))
			{
				sendAsPlainMode = Convert.ToBoolean(itemTemplate["SendAsPlainText"].ToString());
				if (sendAsPlainMode)
					body = body.ToPlainText();
			}

			SPSecurity.RunWithElevatedPrivileges(delegate()
			{
				using (SPSite site = new SPSite(item.Web.Site.ID))
				{
					using (SPWeb web = site.OpenWeb(item.Web.ID))
					{
						StringDictionary headers = new StringDictionary();
						headers = buidEmailHeaders(taskItem, to, cc, subject, sendAsPlainMode);
						SPUtility.SendEmail(web, headers, body);
					}
				}
			}
			);
		}


        //public static void SendEmailbytemplate(SPListItem sourceListItem, SPListItem taskListItem,string emailTo, string EmailTitle, string EmailBody, bool sendAsPlainText, bool attachTaskLink)
        //{
        //    SendEmailbytemplate(sourceListItem, taskListItem, emailTo, EmailTitle, EmailBody, false, true);
        //}

        public static void SendEmailbytemplate(SPListItem sourceListItem, SPListItem taskListItem,string emailTo, string EmailTitle, string EmailBody, bool sendAsPlainText, bool attachTaskLink)
        {
            List<EmailVariable> designerVariables = new List<EmailVariable>(); 
            List<EmailVariable> bodyVariables = new List<EmailVariable>(); 
            List<EmailVariable> subjectVariables = new List<EmailVariable> ();

            bodyVariables = Parser.GetAndSetVariablesFromTemplate(EmailBody, sourceListItem, taskListItem);
            subjectVariables = Parser.GetAndSetVariablesFromTemplate(EmailTitle, sourceListItem, taskListItem);
            //designerVariables = Parser.GetVariablesFromDesigner(variables);
            var subject = Parser.SetValuesToContent(EmailTitle, subjectVariables, designerVariables);
            var body = Parser.SetValuesToContent(EmailBody, bodyVariables, designerVariables);

            if (sendAsPlainText)
                body = body.ToPlainText();

            SPSecurity.RunWithElevatedPrivileges(delegate()
            {
                using (SPSite site = new SPSite(sourceListItem.Web.Site.ID))
                {
                    using (SPWeb web = site.OpenWeb(sourceListItem.Web.ID))
                    {
                        StringDictionary headers = new StringDictionary();
                        if (attachTaskLink)
                            headers = buidEmailHeaders(taskListItem, emailTo, string.Empty, subject, sendAsPlainText);
                        else
                            headers = buidEmailHeaders(null, emailTo, string.Empty, subject, sendAsPlainText);
                        SPUtility.SendEmail(web, headers, body);
                    }
                }
            }
            );
        }
    }
    public class Parser {
        public static List<string> extractVariables(string text)
        {
            List<string> results = new List<string>();
            if (string.IsNullOrEmpty(text)) return results;

            int index = 0;
            while (index < text.Length && text.IndexOf('%', index) > 0)
            {
                int first = text.IndexOf('%', index);

                if (first > 0)
                {
                    index = first + 1;
                    int last = text.IndexOfAny(new char[] { '%', ',', '#', '\r', '\n' }, index);

                    if (last > 0)
                    {
                        index = last + 1;
                        string temp = text.Substring(first + 1, last - first - 1);
                        var match = Regex.Match(temp, @"[\w\d:_\s]*", RegexOptions.Singleline);

                        if (match != null && match.Value == temp)
                        {
                            if (!temp.StartsWith("20"))
                                results.Add(temp);
                        }
                    }
                }

            }
            return results;
        }
        public static List<EmailVariable> GetAndSetVariablesFromTemplate(string strInput, SPListItem sourceListItem, SPListItem taskItem)
        {
            return GetAndSetVariablesFromTemplate(strInput, sourceListItem, taskItem, null);
        }

        public static List<EmailVariable> GetAndSetVariablesFromTemplate(string strInput, SPListItem sourceListItem, SPListItem taskItem, SPUser user)
        {
            List<EmailVariable> list = new List<EmailVariable>();
            List<string> variables = new List<string>();
            variables = extractVariables(strInput);

            foreach (string variable in variables)
            {
                EmailVariable var = new EmailVariable(variable);
                SetVariableValuesFromItem(var, sourceListItem, taskItem, user);
                list.Add(var);
            }

            return list;
        }

        public static void SetVariableValuesFromItem(EmailVariable variable, SPListItem sourceListItem, SPListItem taskItem)
        {
            SetVariableValuesFromItem(variable, sourceListItem, taskItem, null);
        }
        [SPDisposeCheckIgnore(SPDisposeCheckID.SPDisposeCheckID_999, "")]
        public static void SetVariableValuesFromItem(EmailVariable variable, SPListItem sourceListItem, SPListItem taskItem, SPUser user)
        {
            PropertyInfo property;
            switch (variable.Type.ToLower())
            {
                case Constants.Workflow.ITEM:
                    SetVariableValues(variable, sourceListItem);
                    break;

                case Constants.Workflow.TASK:
                    if (taskItem == null) break;
                    SetVariableValues(variable, taskItem);

                    if (string.IsNullOrEmpty(variable.Value)
                        && string.Compare("Comment", variable.Name, true) == 0)
                    {
                        Hashtable properties = SPWorkflowTask.GetExtendedPropertiesAsHashtable(taskItem);
                        if (properties.ContainsKey(Constants.Workflow.CCI_COMMENT))
                        {
                            variable.Value = properties[Constants.Workflow.CCI_COMMENT].ToString();
                        }
                    }
                    break;

                case Constants.Workflow.TASK_LIST:
                    if (taskItem == null) break;
                    SetVariableValues(variable, taskItem.ParentList);
                    break;



                case Constants.Workflow.CURRENT_USER:
                    if (user == null) break;
                    Type type = user.GetType();
                    property = type.GetProperties().FirstOrDefault(p => string.Compare(p.Name, variable.Name, true) == 0);
                    if (property == null)
                        break;
                    object value1 = property.GetValue(user, null);
                    variable.Value = value1 == null ? string.Empty : value1.ToString();
                    break;

                case Constants.Workflow.GLOBAL:
                    ProcessGlobalVariables(variable, sourceListItem, taskItem);
                    break;

                case Constants.Workflow.NONE:
                    break;
            }
        }

        [SPDisposeCheckIgnore(SPDisposeCheckID.SPDisposeCheckID_999, "")]
        [SPDisposeCheckIgnore(SPDisposeCheckID.SPDisposeCheckID_635, "")]
        public static void ProcessGlobalVariables(EmailVariable variable, SPListItem sourceListItem, SPListItem taskItem)
        {
            SPSecurity.RunWithElevatedPrivileges(delegate()
            {
                using (SPWeb web = sourceListItem.ParentList.ParentWeb)
                {
                    SPList globalVariableList = CCIUtility.GetListFromURL(web.Site.RootWeb.Url + "/Lists/CMappGlobalVariables");
                    if (globalVariableList == null)
                        return;
                    SPQuery query = new SPQuery(globalVariableList.DefaultView);
                    query.Query = "<Where><And><Eq><FieldRef Name='Title' /><Value Type='Text'>" + variable.Name + "</Value></Eq>";
                    query.Query += "<Eq><FieldRef Name='Type' /><Value Type='Text'>Global Variable</Value></Eq></And></Where>";
                    SPListItemCollection items = globalVariableList.GetItems(query);
                    if (items != null && items.Count > 0)
                    {
                        SPListItem item = items[0];
                        if (item[new Guid("23250978-D4C9-4677-AEE5-C9A318740B1C")] != null)
                            variable.Value = item[new Guid("23250978-D4C9-4677-AEE5-C9A318740B1C")].ToString();
                        if (!string.IsNullOrEmpty(variable.Value) && !variable.Value.Contains("%" + variable.Type + ":" + variable.Name + "%"))
                        {
                            List<EmailVariable> newVariable = GetAndSetVariablesFromTemplate(variable.Value, item, taskItem);
                            variable.Value = SetValuesToContent(variable.Value, newVariable, new List<EmailVariable>());
                        }
                        else
                        {
                            if (item[new Guid("40BB5B9B-B29A-42e0-98BF-B0255441C53F")] != null)
                                variable.Value = item[new Guid("40BB5B9B-B29A-42e0-98BF-B0255441C53F")].ToString();
                        }
                    }
                }
            });
        }

        public static void SetVariableValues(EmailVariable variable, SPList list)
        {
            if (list == null)
            {
                variable.Value = string.Empty;
                return;
            }

            if (string.Compare(variable.Name, "name", true) == 0)
            {
                variable.Value = list.Title;
            }

            if (string.Compare(variable.Name, "id", true) == 0)
            {
                variable.Value = list.ID.ToString();
            }
        }

        public static void SetVariableValues(EmailVariable variable, SPListItem item)
        {
            if (item == null)
            {
                variable.Value = string.Empty;
                return;
            }
            SPField varField = item.Fields.Cast<SPField>().
                FirstOrDefault(p => string.Compare(p.Title, variable.Name, true) == 0 || string.Compare(p.InternalName, variable.Name, true) == 0);

            if (varField != null)
            {
                switch (varField.Type)
                {
                    case SPFieldType.User:
                    case SPFieldType.Lookup:
                        SetLookupValueFieldType(variable, item, varField);
                        break;
                    case SPFieldType.Invalid:
                        if (string.Compare(varField.TypeAsString, Constants.LOOKUP_WITH_PICKER_TYPE_NAME, true) == 0)
                        {
                            SetLookupValueFieldType(variable, item, varField);
                        }

                        if (string.Compare(varField.TypeAsString, Constants.LINK_TO_ITEM_TYPE_NAME, true) == 0)
                        {
                            //SetLookupValueFieldType(variable, item, varField);
                            variable.Value = item[varField.Id] != null ? item[varField.Id].ToString() : string.Empty;
                        }

                        break;
                    case SPFieldType.DateTime:
                        variable.Value = item[varField.Id] != null ? Convert.ToDateTime(item[varField.Id]).ToString(CommonResources.VNDateTimeFormat) : string.Empty;
                        break;
                    default:
                        variable.Value = item[varField.Id] != null ? item[varField.Id].ToString() : string.Empty;
                        break;
                }
            }
        }

        public static void SetLookupValueFieldType(EmailVariable variable, SPListItem item, SPField varField)
        {
            if (item[varField.Id] == null) return;
            SPFieldLookup lookupField = (SPFieldLookup)varField;
            SPFieldLookupValue itemFieldValue = (SPFieldLookupValue)lookupField.GetFieldValue(item[varField.Id].ToString());
            variable.Value = itemFieldValue.LookupValue;
        }

        public static string SetValuesToContent(string strContent, List<EmailVariable> templateVariables, List<EmailVariable> designerVariables)
        {
            if (string.IsNullOrEmpty(strContent)) return string.Empty;
            foreach (EmailVariable var in templateVariables)
            {
                EmailVariable inputVar = designerVariables.
                    FirstOrDefault(v => string.Compare(v.Name, var.Name, true) == 0 && v.Type == var.Type);

                if (inputVar != null)
                    var.Value = inputVar.Value;

                if (var.Type == Constants.Workflow.NONE)
                    strContent = strContent.Replace("%" + var.Name + "%", var.Value);
                else
                    strContent = strContent.Replace("%" + var.Type + ":" + var.Name + "%", var.Value);
            }
            if (!string.IsNullOrEmpty(strContent))
                strContent = strContent.CustomFunctionPopulate();

            return strContent;
        }
        public static List<EmailVariable> GetVariablesFromDesigner(string strInput)
        {
            List<EmailVariable> list = new List<EmailVariable>();

            return list;

            if (string.IsNullOrEmpty(strInput)) 
                return list;

            Regex regex = new Regex("%(?<NAME>[^%\r\n]+)*%:(?<VALUE>[^%]+)");
            MatchCollection mats = regex.Matches(strInput);
            foreach (Match mat in mats)
            {
                EmailVariable var = new EmailVariable(mat.Groups["NAME"].Value, mat.Groups["VALUE"].Value);
                list.Add(var);
            }
            return list;
        }

    }
}
