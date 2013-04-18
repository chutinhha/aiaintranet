using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SharePoint.Administration;

using System.Reflection;
using System.ComponentModel;
using System.Diagnostics;
using AIA.Intranet.Model;

namespace AIA.Intranet.Common
{
    [System.Runtime.InteropServices.GuidAttribute("DBEEB5AB-C5A7-46B5-A2BB-5581F960C333")]
    public class DiagnosticsService : SPDiagnosticsServiceBase
    {
        private static string DiagnosticsAreaName = "Corridor .app";

        public DiagnosticsService()
        {
        }

        public DiagnosticsService(string name, SPFarm farm)
            : base(name, farm)
        {

        }
        public static string GetDescription(Enum en)
        {
            Type type = en.GetType();

            MemberInfo[] memInfo = type.GetMember(en.ToString());

            if (memInfo != null && memInfo.Length > 0)
            {
                object[] attrs = memInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);

                if (attrs != null && attrs.Length > 0)
                {
                    return ((DescriptionAttribute)attrs[0]).Description;
                }
            }

            return en.ToString();
        }

        protected override IEnumerable<SPDiagnosticsArea> ProvideAreas()
        {
            List<SPDiagnosticsCategory> categories = new List<SPDiagnosticsCategory>();
            foreach (string catName in Enum.GetNames(typeof(AIAPortalFeatures)))
            {
                AIAPortalFeatures entry = ( AIAPortalFeatures)Enum.Parse(typeof(AIAPortalFeatures), catName) ;
                uint catId = (uint)(int)Enum.Parse(typeof(AIAPortalFeatures), catName);
                string friendlyname = GetDescription(entry);
                categories.Add(new SPDiagnosticsCategory(friendlyname, TraceSeverity.Verbose, EventSeverity.Error, 0, catId));
            }
            yield return new SPDiagnosticsArea(DiagnosticsAreaName, categories);
        }

        public static DiagnosticsService Local
        {
            get
            {
                return SPDiagnosticsServiceBase.GetLocal<DiagnosticsService>();
            }
        }
        public SPDiagnosticsCategory this[string categoryName]
        {
            get
            {
                return Areas[DiagnosticsAreaName].Categories[categoryName];
            }
        }
        public SPDiagnosticsCategory this[AIAPortalFeatures id]
        {
            get
            {
                return Areas[DiagnosticsAreaName].Categories[(uint)id];
            }
        }

        /// <summary>
        /// Writes a Warning to the Windows Event Log, and to the SharePoint ULS logs
        /// </summary>
        public  void Warning(SPDiagnosticsCategory category, string message)
        {
            WriteLog(category, TraceSeverity.None, EventSeverity.Warning, message);
        }

        /// <summary>
        /// Writes an Information level entry to the Windows Event Log, and to the SharePoint ULS logs
        /// </summary>
        public  void Information(SPDiagnosticsCategory category, string message)
        {
            WriteLog(category, TraceSeverity.None, EventSeverity.Information, message);
        }

        /// <summary>
        /// Writes a High level message to the SharePoint ULS only
        /// </summary>
        public  void High(SPDiagnosticsCategory category, string message)
        {
            WriteLog(category, TraceSeverity.High, EventSeverity.None, message);
        }

        /// <summary>
        /// Writes a Medium level message to the SharePoint ULS only
        /// </summary>
        public  void Medium(SPDiagnosticsCategory category, string message)
        {
            WriteLog(category, TraceSeverity.Medium, EventSeverity.None, message);
        }

        /// <summary>
        /// Writes a Low level message to the SharePoint ULS only (i.e. Most Verbose, or most detailed)
        /// </summary>
        public  void Low(SPDiagnosticsCategory category, string message)
        {
            WriteLog(category, TraceSeverity.Verbose, EventSeverity.None, message);
        }


        /// <summary>
        /// Actually controls the writing. Calls to WriteEvent ALSO writes to the ULS log. That's why the public logging methods
        /// all use TraceSeverity.None.
        /// </summary>
        private  void WriteLog(SPDiagnosticsCategory category, TraceSeverity trcSeverity, EventSeverity evtSeverity, string message)
        {
            //uint catId = (uint)category;
            //SPDiagnosticsCategory diagCat = MyLogger.Local.Areas[DiagnosticsAreaName].Categories[catId];

            StackTrace stackTrace = new StackTrace();
            StackFrame stackFrame = stackTrace.GetFrame(2);
            MethodBase methodBase = stackFrame.GetMethod();

            message = string.Format("{0}.{1} : {2}",
                                        methodBase.DeclaringType.Name,
                                        methodBase.Name,
                                        message);


            if (evtSeverity != EventSeverity.None)
            {
                //Problem - event source might not be registered on Server, and the app pool might not be able to create it.
                //Therefore, catch the exception, and write it into the logs. Nothing gets written to the Windows log,
                //but at least it's somewhat handled.
                try
                {
                    base.WriteEvent(0, category, evtSeverity, message);
                }
                catch (Exception ex)
                {
                    base.WriteTrace(0, category, TraceSeverity.Unexpected, string.Format("Unable to write to event log {0} : {1}", ex.GetType().ToString(), ex.Message));

                    // If there was an error writing to the event log, make sure the tracelog is written to instead.
                    switch (evtSeverity)
                    {
                        case EventSeverity.Error:
                            trcSeverity = TraceSeverity.Unexpected;
                            break;
                        case EventSeverity.Warning:
                            trcSeverity = TraceSeverity.Monitorable;
                            break;
                        case EventSeverity.Information:
                            trcSeverity = TraceSeverity.High;
                            break;
                    }
                }
            }

            if (trcSeverity != TraceSeverity.None)
            {
                base.WriteTrace(0, category, trcSeverity, message);
            }
        }
    }
}
