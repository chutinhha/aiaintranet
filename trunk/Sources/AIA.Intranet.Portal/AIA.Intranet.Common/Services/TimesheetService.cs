using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AIA.Intranet.Model.Entities;
using AIA.Intranet.Common.Utilities;
using Microsoft.SharePoint;
using AIA.Intranet.Common.Utilities.Camlex;
using AIA.Intranet.Model;

namespace AIA.Intranet.Common.Services
{
    public class TimesheetService
    {
        public static List<TimesheetItem> GetOnDateTimesheets(SPList list, DateTime date)
        {
            var query = new CAMLListQuery<TimesheetItem>(list);
            DateTime endDate = new DateTime(date.Year, date.Month, date.Day + 1, 0, 0, 1);
            string caml = Camlex.Query()
                              .Where(x => (DateTime)x["EventDate"] > date.IncludeTimeValue()
                              && (DateTime)x["EventDate"] < endDate.IncludeTimeValue())
                              .OrderBy(x => new[] { x["EventDate"] as Camlex.Asc })
                              .ToString();

            return query.ExecuteListQuery(caml);
        }

        public static TimesheetItem GetOnDateTimesheet(SPList list, DateTime date)
        {
            var query = new CAMLListQuery<TimesheetItem>(list);
            string caml = string.Format(@"<Where>
                                            <And>
                                                <Geq>
                                                    <FieldRef Name='EventDate' />
                                                    <Value Type='DateTime'>{0}</Value>
                                                </Geq>
                                                <Leq>
                                                    <FieldRef Name='EventDate' />
                                                    <Value Type='DateTime'>{1}</Value>
                                                </Leq>
                                            </And>
                                        </Where>
                                        <OrderBy>
                                            <FieldRef Name='EventDate' Ascending='False' />
                                        </OrderBy>", date.ToString("yyyy-MM-dd"), date.ToString("yyyy-MM-dd"));

            return query.ExecuteSingleQuery(caml);
        }

        public static TimesheetItem GetOnDateTimesheet(SPList list, DateTime date, int id)
        {
            var query = new CAMLListQuery<TimesheetItem>(list);
            string caml = string.Format(@"<Where>
                                            <And>
                                                <Neq>
                                                    <FieldRef Name='ID' />
                                                    <Value Type='Counter'>{0}</Value>
                                                </Neq>
                                                <And>
                                                    <Geq>
                                                        <FieldRef Name='EventDate'/>
                                                        <Value Type='DateTime'>{1}</Value>
                                                    </Geq>
                                                    <Leq>
                                                        <FieldRef Name='EventDate'/>
                                                        <Value Type='DateTime'>{2}</Value>
                                                    </Leq>
                                                </And>
                                            </And>
                                        </Where>
                                        <OrderBy>
                                            <FieldRef Name='EventDate' Ascending='False' />
                                        </OrderBy>", id, date.ToString("yyyy-MM-dd"), date.ToString("yyyy-MM-dd"));

            return query.ExecuteSingleQuery(caml);
        }

        public static void UpdateTimesheet(SPList list, int itemId, DateTime startTime, DateTime endDate)
        {
            using (DisableItemEvent disableItemEvent = new DisableItemEvent())
            {
                var item = list.GetItemById(itemId);
                item[SPBuiltInFieldId.StartDate] = startTime;
                item[SPBuiltInFieldId.EndDate] = endDate;
                item.SystemUpdate();
            }
        }

        public static void UpdateTimesheet(SPList list, int itemId)
        {
            using (DisableItemEvent disableItemEvent = new DisableItemEvent())
            {
                var item = list.GetItemById(itemId);
                item[IOfficeColumnId.Timesheet.IsSync] = true.ToString();
                item.SystemUpdate();
            }
        }

    }
}
