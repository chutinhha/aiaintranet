using System.Linq;

namespace AIA.Intranet.Model.Workflow
{
	public class EmailVariable
	{
        public string Name { get; set; }
        private string _value;
        public string Type { get; set; }

        public string Value
        {
            get
            {
                return _value;
            }
            set
            {
                trimmingHtmlValue(value);
            }
        }

        public EmailVariable(string Name, string Value)
        {
            this.Name = Name;
            this.Value = Value;
            this.Type = Constants.Workflow.NONE;
            if (!Name.Contains(':')) return;
            string[] arrVarName = Name.Split(':');
            if (arrVarName.Count<string>() > 0)
            {
                this.Type = arrVarName[0];
                this.Name = arrVarName[1];
            }
        }

        public EmailVariable(string name)
        {
            this.Name = name;
            this.Value = string.Empty;
            this.Type = Constants.Workflow.NONE;
            if (!name.Contains(':'))
                return;

            string[] arrVarName = Name.Split(':');
            if (arrVarName.Count<string>() > 0)
            {
                this.Type = arrVarName[0];
                this.Name = arrVarName[1];
            }
        }

        private void trimmingHtmlValue(string s)
        {
            if (string.IsNullOrEmpty(s)) return;

            while (s.ToUpper().StartsWith("<DIV") || s.ToUpper().StartsWith("<P "))
            {
                if (s.ToUpper().StartsWith("<DIV"))
                {
                    s = s.Remove(0, s.IndexOf(">") + 1);
                    s = s.Remove(s.Length - 6, 6);
                }
                if (s.ToUpper().StartsWith("<P "))
                {   
                    s = s.Remove(0, 2);
                    s = s.Remove(s.Length - 4, 4);
                    s = "<SPAN " + s;
                    s += "</SPAN>";
                }
                if (s.ToUpper().StartsWith("<P>"))
                {
                    s = s.Remove(0, 3);
                    s = s.Remove(s.Length - 4, 4);
                }
            }
            this._value = s;
        }
	}
}
