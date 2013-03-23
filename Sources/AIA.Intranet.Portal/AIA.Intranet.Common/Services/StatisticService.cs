using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AIA.Intranet.Model.Entities;
using AIA.Intranet.Common.Utilities;
using AIA.Intranet.Model;
using AIA.Intranet.Common.Utilities.Camlex;
using AIA.Intranet.Common.Helpers;
using Microsoft.SharePoint;

namespace AIA.Intranet.Common.Services
{
    public class StatisticService
    {
        public static TaskStatistics GetTaskStatistics(int userID)
        {
            var list = CCIUtility.GetListFromURL(Constants.STATISTIC_LIST_URL);

            string caml = Camlex.Query().Where(p => p["Employee"] == (DataTypes.UserId)userID.ToString()).ToString();
            CAMLListQuery<StatisticItem> query = new CAMLListQuery<StatisticItem>(list);

            var item = query.ExecuteSingleQuery(caml);
            if (item != null && !string.IsNullOrEmpty(item.TaskStatistics))
                return SerializationHelper.DeserializeFromXml<TaskStatistics>(item.TaskStatistics);
            
            return null;
        }

        public static TaskStatistics GetTaskStatistics(SPWeb web, int userID)
        {
            var list = CCIUtility.GetListFromURL(Constants.STATISTIC_LIST_URL, web);

            string caml = Camlex.Query().Where(p => p["Employee"] == (DataTypes.UserId)userID.ToString()).ToString();
            CAMLListQuery<StatisticItem> query = new CAMLListQuery<StatisticItem>(list);

            var item = query.ExecuteSingleQuery(caml);
            if (item != null && !string.IsNullOrEmpty(item.TaskStatistics))
            {
                return SerializationHelper.DeserializeFromXml<TaskStatistics>(item.TaskStatistics);
            }
            else
            {
                if (userID != list.ParentWeb.Site.SystemAccount.ID)
                {
                    if (!list.ParentWeb.AllowUnsafeUpdates)
                        list.ParentWeb.AllowUnsafeUpdates = true;
                    SPListItem spListItem = list.Items.Add();
                    SPUser user = list.ParentWeb.Users.GetByID(userID);
                    spListItem["Title"] = user.Name;
                    spListItem["Employee"] = new SPFieldUserValue(list.ParentWeb, user.ID, user.LoginName);
                    TaskStatistics task = new TaskStatistics()
                    {
                        Personal = 1,
                        Projects = 1,
                        Departments = 1,

                        Rate_1 = 1,
                        Rate_2 = 1,
                        Rate_3 = 1,
                        Rate_4 = 1,
                        Rate_5 = 1,

                        Assigned = 1,
                        InProcessing = 1,
                        Completed = 1,
                        Canceled = 1,
                        OnHold = 1
                    };
                    string taskStatistic = SerializationHelper.SerializeToXml<TaskStatistics>(task);
                    spListItem["TaskStatistics"] = taskStatistic;
                    spListItem.Update();
                    return SerializationHelper.DeserializeFromXml<TaskStatistics>(taskStatistic);
                }
            }
            return null;
        }

        public static TaskStatistics GetTaskStatistics(string Login)
        {
            var list = CCIUtility.GetListFromURL(Constants.STATISTIC_LIST_URL);

            string caml = Camlex.Query().Where(p => p["Employee"] == (DataTypes.User)Login.ToString()).ToString();
            CAMLListQuery<StatisticItem> query = new CAMLListQuery<StatisticItem>(list);

            var item = query.ExecuteSingleQuery(caml);

            if (item != null && !string.IsNullOrEmpty(item.TaskStatistics))
                return SerializationHelper.DeserializeFromXml<TaskStatistics>(item.TaskStatistics);
            return null;
        }

        public static void UpdateTaskStatistics(SPWeb web, int userID, TaskStatistics value)
        {
            var list = CCIUtility.GetListFromURL(Constants.STATISTIC_LIST_URL, web);
            string caml = Camlex.Query().Where(p => p["Employee"] == (DataTypes.UserId)userID.ToString()).ToString();
            CAMLListQuery<StatisticItem> query = new CAMLListQuery<StatisticItem>(list);
            SPListItem item = query.GetItem(caml);
            if (item != null)
            {
                if (!web.AllowUnsafeUpdates)
                    web.AllowUnsafeUpdates = true;
                item["TaskStatistics"] = SerializationHelper.SerializeToXml<TaskStatistics>(value);
                item.Update();
            }
        }
    }
}
