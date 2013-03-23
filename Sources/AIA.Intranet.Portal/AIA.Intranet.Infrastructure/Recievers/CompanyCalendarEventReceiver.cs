using System;
using System.Security.Permissions;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Security;
using Microsoft.SharePoint.Utilities;
using Microsoft.SharePoint.Workflow;
using AIA.Intranet.Model;
using AIA.Intranet.Model.Entities;
using AIA.Intranet.Common.Services;
using AIA.Intranet.Common.Helpers;
using AIA.Intranet.Common.Utilities;
using System.Text;
using AIA.Intranet.Resources;
using System.Collections.Generic;
using System.Linq;

namespace AIA.Intranet.Infrastructure.Recievers
{
    /// <summary>
    /// List Item Events
    /// </summary>
    public class CompanyCalendarEventReceiver : SPItemEventReceiver
    {
        /// <summary>
        /// An item was added.
        /// </summary>
        public override void ItemAdded(SPItemEventProperties properties)
        {
            base.ItemAdded(properties);
            SendEmailWhenCreate(properties);
        }

        /// <summary>
        /// An item was updated
        /// </summary>
       public override void ItemUpdating(SPItemEventProperties properties)
       {
           base.ItemUpdating(properties);
           SendEmailWhenEdit(properties);
       }

       /// <summary>
       /// An item is being deleted.
       /// </summary>
       public override void ItemDeleting(SPItemEventProperties properties)
       {
           base.ItemDeleting(properties);
           SendEmailWhenDelete(properties);
       }

        private void SendEmailWhenCreate(SPItemEventProperties properties)
        {
            string mailTo = string.Empty;

            SPWeb web = properties.Web;

            SPListItem item = properties.ListItem;
            SPFieldLookupValueCollection attendees = (SPFieldLookupValueCollection)item[IOfficeColumnId.ComCalendarAttendees];

            if (attendees != null && attendees.Count > 0)
            {
                foreach (var attendee in attendees)
                {
                    Employee employee = EmployeeService.GetEmployeeByItemId(attendee.LookupId, web);
                    mailTo += employee.UserEmail + ";";
                }
            }

            SPListItem emailTemplateItem = CCIUtility.GetEmailTemplate(Constants.EMAIL_TEMPLATES_LIST_URL, "Thông báo lịch làm việc công ty", web.Site.RootWeb);

            string variables = GetDefinedVariables(properties);

            SendEmailHelper.SendEmailbytemplate(item, emailTemplateItem, mailTo, "", variables);
        }

        private void SendEmailWhenEdit(SPItemEventProperties properties)
        {
            string mailTo = string.Empty;

            SPWeb web = properties.Web;

            SPListItem item = properties.ListItem;
            
            SPFieldLookupValueCollection originalAttendees = (SPFieldLookupValueCollection)item[IOfficeColumnId.ComCalendarAttendees];
            List<int> originalUserIDs = new List<int>();

            if (originalAttendees != null && originalAttendees.Count > 0)
            {
                foreach (var attendee in originalAttendees)
                {
                    originalUserIDs.Add(attendee.LookupId);
                }
            }

            string[] afterAttendees = properties.AfterProperties["Attendees"].ToString().Split(new string[] { ";#" }, StringSplitOptions.RemoveEmptyEntries);
            List<int> afterUserIDs = new List<int>();
            if (afterAttendees.Length > 1)
            {
                for (int i = 0; i < afterAttendees.Length; i = i + 2)
                {
                    afterUserIDs.Add(Convert.ToInt32(afterAttendees[i]));
                }
            }

            List<int> listRemovedUsers = originalUserIDs.Except(afterUserIDs).ToList();
            List<int> listNewUsers = afterUserIDs.Except(originalUserIDs).ToList();
            List<int> listSameUsers = originalUserIDs.Intersect(afterUserIDs).ToList();

            string removedUsers = string.Empty;
            string newUsers = string.Empty;
            string sameUsers = string.Empty;

            foreach (int userId in listRemovedUsers)
            {
                Employee employee = EmployeeService.GetEmployeeByItemId(userId, web);
                removedUsers += employee.UserEmail + ";";
            }

            foreach (int userId in listNewUsers)
            {
                Employee employee = EmployeeService.GetEmployeeByItemId(userId, web);
                newUsers += employee.UserEmail + ";";
            }

            foreach (int userId in listSameUsers)
            {
                Employee employee = EmployeeService.GetEmployeeByItemId(userId, web);
                sameUsers += employee.UserEmail + ";";
            }

            string variables = GetDefinedVariables(properties);

            SPListItem emailTemplateItemNew = CCIUtility.GetEmailTemplate(Constants.EMAIL_TEMPLATES_LIST_URL, "Thông báo lịch làm việc công ty", web.Site.RootWeb);
            SendEmailHelper.SendEmailbytemplate(item, emailTemplateItemNew, newUsers, "", variables);

            SPListItem emailTemplateItemEdit = CCIUtility.GetEmailTemplate(Constants.EMAIL_TEMPLATES_LIST_URL, "Thông báo thay đổi lịch làm việc công ty", web.Site.RootWeb);
            SendEmailHelper.SendEmailbytemplate(item, emailTemplateItemEdit, sameUsers, "", variables);

            SPListItem emailTemplateItemRemove = CCIUtility.GetEmailTemplate(Constants.EMAIL_TEMPLATES_LIST_URL, "Thông báo hủy lịch làm việc công ty", web.Site.RootWeb);
            SendEmailHelper.SendEmailbytemplate(item, emailTemplateItemRemove, removedUsers, "", variables);
        }

        private void SendEmailWhenDelete(SPItemEventProperties properties)
        {
            string mailTo = string.Empty;

            SPWeb web = properties.Web;

            SPListItem item = properties.ListItem;
            if (Convert.ToDateTime(item[IOfficeColumnId.ComCalendarEventDate]) >= DateTime.Now)
            {
                SPFieldLookupValueCollection attendees = (SPFieldLookupValueCollection)item[IOfficeColumnId.ComCalendarAttendees];

                if (attendees != null && attendees.Count > 0)
                {
                    foreach (var attendee in attendees)
                    {
                        Employee employee = EmployeeService.GetEmployeeByItemId(attendee.LookupId, web);
                        mailTo += employee.UserEmail + ";";
                    }
                }

                SPListItem emailTemplateItem = CCIUtility.GetEmailTemplate(Constants.EMAIL_TEMPLATES_LIST_URL, "Thông báo hủy lịch làm việc công ty", web.Site.RootWeb);

                string variables = GetDefinedVariables(properties);

                SendEmailHelper.SendEmailbytemplate(item, emailTemplateItem, mailTo, "", variables);
            }

        }

        private string GetDefinedVariables(SPItemEventProperties properties)
        { 
            StringBuilder variables = new StringBuilder();
            variables.AppendFormat("%{0}%:{1}", "beforeTitle", properties.BeforeProperties["Title"]);
            variables.AppendFormat("%{0}%:{1}", "afterTitle", properties.AfterProperties["Title"]);
            variables.AppendFormat("%{0}%:{1}", "beforeLocation", properties.BeforeProperties["Location"]);
            variables.AppendFormat("%{0}%:{1}", "afterLocation", properties.AfterProperties["Location"]);
            variables.AppendFormat("%{0}%:{1}", "beforeEventDate", properties.BeforeProperties["EventDate"] != null ? Convert.ToDateTime(properties.BeforeProperties["EventDate"]).ToUniversalTime().ToString(CommonResources.VNDateTimeFormat) : string.Empty);
            variables.AppendFormat("%{0}%:{1}", "afterEventDate", properties.AfterProperties["EventDate"] != null ? Convert.ToDateTime(properties.AfterProperties["EventDate"]).ToUniversalTime().ToString(CommonResources.VNDateTimeFormat) : string.Empty);
            variables.AppendFormat("%{0}%:{1}", "beforeEndDate", properties.BeforeProperties["EndDate"] != null ? Convert.ToDateTime(properties.BeforeProperties["EndDate"]).ToUniversalTime().ToString(CommonResources.VNDateTimeFormat) : string.Empty);
            variables.AppendFormat("%{0}%:{1}", "afterEndDate", properties.AfterProperties["EndDate"] != null ? Convert.ToDateTime(properties.AfterProperties["EndDate"]).ToUniversalTime().ToString(CommonResources.VNDateTimeFormat) : string.Empty);
            
            return variables.ToString();
        }

    }
}
