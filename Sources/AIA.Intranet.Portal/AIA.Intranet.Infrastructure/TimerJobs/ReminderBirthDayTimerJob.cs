using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SharePoint.Administration;
using Microsoft.SharePoint;
using AIA.Intranet.Resources;
using AIA.Intranet.Model;
using AIA.Intranet.Model.Infrastructure;
using AIA.Intranet.Common.Extensions;
using AIA.Intranet.Model;

namespace AIA.Intranet.Infrastructure.TimerJobs
{
    public class ReminderBirthDayTimerJob : SPJobDefinition
    {
        #region [Contructors]
        public ReminderBirthDayTimerJob()
            : base()
        {
        }
        public ReminderBirthDayTimerJob(string jobName, SPService service, SPServer server, SPJobLockType targetType)
            : base(jobName, service, server, targetType)
        {
            this.Title = jobName;
        }
        public ReminderBirthDayTimerJob(string jobName, SPWebApplication webApplication)
            : base(jobName, webApplication, null, SPJobLockType.ContentDatabase)
        {
            this.Title = jobName;
        }
        #endregion

        #region [Methods]
        public override void Execute(Guid contentDbId)
        {
            SPSecurity.RunWithElevatedPrivileges(delegate()
            {
                try
                {
                    SPWebApplication webApplication = this.Parent as SPWebApplication;

                    foreach (SPSite obj in webApplication.Sites)
                    {
                        if (DepartmentProcessing.IsActionPlanFeatureActivated(obj.RootWeb))
                        {
                            using (SPSite site = new SPSite(obj.Url))
                            {
                                using (SPWeb web = site.RootWeb)
                                {
                                    SPList list = web.GetList(Constants.EMPLOYEE_LIST_URL);
                                    var setting = SPContext.Current.List.GetCustomSettings<RemindBirthDaySetting>(Model.IOfficeFeatures.Infrastructure);

                                    if (setting.IsEnableRemindBirthDay && list.Items.Count > 0)
                                    {
                                        foreach (SPListItem item in list.Items)
                                        {
                                            var birthDay = DateTime.Parse(item[IOfficeColumnId.Employees.BirthDay].ToString());
                                            var email = item[IOfficeColumnId.Employees.Email].ToString();
                                            var department = item[IOfficeColumnId.Employees.Department].ToString();

                                            if (setting.IsSendByMonth)
                                            {
                                                if (setting.IsSendMail)
                                                {
                                                    System.Globalization.CultureInfo myCultureInfo = System.Globalization.CultureInfo.CurrentCulture;
                                                    var sendMailDate = DateTime.ParseExact("01-" + DateTime.Today.Month.ToString() + "-" + DateTime.Today.Year.ToString(), "dd-MM-yyyy", myCultureInfo);
                                                    sendMailDate = DateTime.Today.AddMonths(1).AddDays(-1);

                                                    if (setting.IsAllEmployees)
                                                    {
                                                        
                                                    }
                                                    else if (setting.IsChosenDeppartment)
                                                    {

                                                    }
                                                }
                                                else if (setting.IsAnnoucement)
                                                {

                                                }
                                            }
                                            else if (setting.IsSendByWeek)
                                            {
                                                if (setting.IsSendMail)
                                                {
                                                    var sendMailDate = DateTime.Today.DayOfWeek;


                                                    if (setting.IsAllEmployees)
                                                    {

                                                    }
                                                    else if (setting.IsChosenDeppartment)
                                                    {

                                                    }
                                                }
                                                else if (setting.IsAnnoucement)
                                                {

                                                }
                                            }
                                            else if (setting.IsSendByDay)
                                            {
                                                if (setting.IsSendMail)
                                                {
                                                    if (setting.IsAllEmployees)
                                                    {

                                                    }
                                                    else if (setting.IsChosenDeppartment)
                                                    {

                                                    }
                                                }
                                                else if (setting.IsAnnoucement)
                                                {

                                                }
                                            }

                                            if (DateTime.Parse(DateTime.Now.ToShortDateString()).Equals(birthDay))
                                            {
                                                //SendEmail(web, item["Branch"].ToString(), item["Year"].ToString(), item["ReportBeforeDate"].ToString(), Constants.EmailReminderCode);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            });
        }
        #endregion
    }
}
