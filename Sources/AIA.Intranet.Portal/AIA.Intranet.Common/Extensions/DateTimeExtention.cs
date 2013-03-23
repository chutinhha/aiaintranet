using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;

namespace AIA.Intranet.Common.Extensions
{
    public static class DateTimeExtention
    {
        public static int GetWeekNumber(this DateTime date)
        {
            DateTimeFormatInfo dfi = DateTimeFormatInfo.CurrentInfo;            
            Calendar cal = dfi.Calendar;

            return cal.GetWeekOfYear(date, dfi.CalendarWeekRule, dfi.FirstDayOfWeek) ;      
        }
        public static string  GetBlogPostFormat(this DateTime date)
        {
            TimeSpan timespan = DateTime.Now - date;
            if (timespan.TotalSeconds < 60) return "Just few second ago";
            if (timespan.TotalMinutes < 60) return string.Format("{0} minutes {1} second ago", timespan.Minutes, timespan.Seconds);
            if (timespan.TotalHours < 24) return string.Format("{0} hours {1} minutes {2} second ago", timespan.Hours, timespan.Minutes, timespan.Seconds);

             return string.Format("{0} days {1} hours {2} minutes {3} second ago",timespan.Days, timespan.Hours, timespan.Minutes, timespan.Seconds);
           
        }

    }
}
