using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using AIA.Intranet.Model.Entities;

namespace AIA.Intranet.Model
{
    public class EntityMapping : Hashtable
    {
        private static EntityMapping instance;
        private static object syncLock = new object();
        public static EntityMapping Instance(){
            lock (syncLock)
            {
                if (instance == null)
                {

                    instance = new EntityMapping();

                }
            }
            return instance;
        }

        public  EntityMapping()
        {
            base.Add(ListTemplateIds.EMAIL_TEMPLATE_ID, typeof(EmailTemplate));
            base.Add(ListTemplateIds.EMPLOYEE_TEMPLATE_ID, typeof(Employee));
            base.Add(ListTemplateIds.DEPARTMENT_TEMPLATE_ID, typeof(Department));
            base.Add(ListTemplateIds.PROJECT_TEMPLATE_ID, typeof(ProjectItem));
            base.Add(ListTemplateIds.TASKSCHEDULE_TEMPLATE_ID, typeof(TaskItem));
        }
    }
}
