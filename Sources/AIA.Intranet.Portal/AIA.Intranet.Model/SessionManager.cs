using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace AIA.Intranet.Model
{
    public class SessionManager
    {
        public static string Theme
        {
            get
            {
                var theme = HttpContext.Current.Session["Theme"] as string;

             
                return theme;
            }
            set
            {
                HttpContext.Current.Session["Theme"] = value;
            }
        }

        public static DateTime? LastVisited
        {
            get
            {
                var lastVisited = HttpContext.Current.Session["LastVisited"] as DateTime?;

                //if (lastVisited == null)
                //{
                //    lastVisited = DateTime.Now;
                //    //HttpContext.Current.Session["LastVisited"] = DateTime.Now;
                //}
                return lastVisited;
            }
            set
            {
                HttpContext.Current.Session["LastVisited"] = value;
            }
        }
    }
}
