using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AIA.Intranet.Model.Entities
{
    public class OrgChartNode
    {
        public string Id { get; set; }
        public List<OrgChartNode> ChildNodes { get; set; }

        public string Title { get; set; }
        public OrgChartNodeTypes Type { get; set; }
        public object Data { get; set; }

        public virtual string GetHtml(){
            return Title;
        }
        public OrgChartNode()
        {
            ChildNodes = new List<OrgChartNode>();
        }
        public string Position { get; set; }
        public string ImageUrl { get; set; }
    }

    public class DepartmentOrgChartNode : OrgChartNode
    {        
        public override string GetHtml()
        {
            Department dept = Data as Department;
            return string.Format("<div class='department_orgchart'><a href='{0}'>{1}</a></div>", dept.Link, dept.Title);
        }
    }

    public class DeptHeadOrgChartNode : OrgChartNode
    {
        public override string GetHtml()
        {
            Employee emp = Data as Employee;
            return string.Format("<div class='depthead_orgchart'><a href='javascript:openDialog(\"{0}\")'>{1}</a></div>", emp.empoyeeUrl, emp.Title);
        }
    }

    public class DeputyDeptHeadOrgChartNode : OrgChartNode
    {
        public override string GetHtml()
        {
            Employee emp = Data as Employee;
            return string.Format("<div class='deputyhead_orgchart'><a href='javascript:openDialog(\"{0}\")'>{1}</a></div>", emp.empoyeeUrl, emp.Title);
        }
    }

    public class EmployeeOrgChartNode : OrgChartNode
    {
        public override string GetHtml()
        {
            Employee emp = Data as Employee;
            return string.Format("<div class='employeeorgchartnode_orgchart'><a href='javascript:openDialog(\"{0}\")'>{1}</a></div>", emp.empoyeeUrl, emp.Title);
        }        
    }

    public class EmployeeNode : OrgChartNode
    {
        public override string GetHtml()
        {
            return string.Format("<div class='employeenode_orgchart'>Nhân viên</div>");
        }
    }

    public enum OrgChartNodeTypes{
        PlaceHolder,
        Department,
        Employee

    }
}
