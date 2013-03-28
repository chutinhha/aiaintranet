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
            
        }
        #endregion
    }
}
