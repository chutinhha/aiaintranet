using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AIA.Intranet.Model.Infrastructure
{
    [Serializable]
    public class EmailTemplateSettings
    {
        /// <summary>
        /// The URL to email template list. this Url can be relative part(ex :/Lists/email template)
        /// </summary>
        public string Url{ get; set; }
        /// <summary>
        /// Name of the template
        /// </summary>
        public string Name { get; set; }
    }
}
