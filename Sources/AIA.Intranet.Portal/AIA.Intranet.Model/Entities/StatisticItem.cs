using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SharePoint;

namespace AIA.Intranet.Model.Entities
{
    public class StatisticItem: BaseEntity
    {
        public StatisticItem(SPListItem item)
            : base(item)
        {

        }
        public string TaskStatistics { get; set; }

        [Field(Ignore=true)]
        public TaskStatistics Tasks { get; set; }
    }
}
