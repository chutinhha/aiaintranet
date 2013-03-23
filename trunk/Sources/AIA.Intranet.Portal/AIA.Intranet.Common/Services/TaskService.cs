using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AIA.Intranet.Model.Entities;
using AIA.Intranet.Common.Utilities;
using AIA.Intranet.Model;
using AIA.Intranet.Common.Utilities.Camlex;
using Microsoft.SharePoint;

namespace AIA.Intranet.Common.Services
{
    public class TaskService
    {
        public static TaskItem GetTaskById(int id)
        {
            var query = new CAMLListQuery<TaskItem>(Constants.TASK_LIST_URL);
            return query.GetItemById(id);
        }

        public static void IncreaseTaskStatistic(SPWeb spWeb, int userId, TypeStatistic type, string contentTypeId)
        {
            TaskStatistics taskStatistic = StatisticService.GetTaskStatistics(spWeb, userId);
            if (taskStatistic != null)
            {
                switch (type)
                {
                    case TypeStatistic.Task:
                        if (contentTypeId == Constants.TASK_CONTENT_TYPE_PRIVATE)
                            taskStatistic.Personal += 1;
                        else if (contentTypeId == Constants.TASK_CONTENT_TYPE_PROJECT)
                            taskStatistic.Projects += 1;
                        else
                            taskStatistic.Departments += 1;
                        break;
                    case TypeStatistic.TaskRate:
                        break;
                    case TypeStatistic.TaskStatus:
                        break;
                }
                StatisticService.UpdateTaskStatistics(spWeb, userId, taskStatistic);
            }
        }

        public static void DecreaseTaskStatistic(SPWeb web, int userId, TypeStatistic type, string contentTypeId)
        {
            TaskStatistics taskStatistic = StatisticService.GetTaskStatistics(web, userId);
            if (taskStatistic != null)
            {
                switch (type)
                {
                    case TypeStatistic.Task:
                        if (contentTypeId == Constants.TASK_CONTENT_TYPE_PRIVATE)
                            taskStatistic.Personal -= 1;
                        else if (contentTypeId == Constants.TASK_CONTENT_TYPE_PROJECT)
                            taskStatistic.Projects -= 1;
                        else
                            taskStatistic.Departments -= 1;
                        break;
                    case TypeStatistic.TaskRate:
                        break;
                    case TypeStatistic.TaskStatus:
                        break;
                }
                StatisticService.UpdateTaskStatistics(web, userId, taskStatistic);
            }
        }

    }
}
