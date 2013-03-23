using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AIA.Intranet.Model.Entities;
using Microsoft.SharePoint;
using AIA.Intranet.Common.Utilities;
using AIA.Intranet.Common.Extensions;

namespace AIA.Intranet.Common.Helpers
{
    public class TemplateHelper
    {
        public static EmailTemplate PopulateTemplate(string url, string name, SPListItem item)
        {
            var templateItem = CCIUtility.GetEmailTemplate(url, name);
            if (templateItem == null) return null;

           

            var result = new EmailTemplate(templateItem);
             var bodyVariables = Parser.GetAndSetVariablesFromTemplate(result.Body, item, null);
            var subjectVariables = Parser.GetAndSetVariablesFromTemplate(result.Subject, item, null);
            var designerVariales = Parser.GetVariablesFromDesigner(result.Body);
            result.Subject = Parser.SetValuesToContent(result.Subject, subjectVariables, designerVariales);
            result.Body = Parser.SetValuesToContent(result.Body, bodyVariables, designerVariales);
            result.Body = result.Body.PopulateTemplate(item);
            result.Subject = result.Subject.PopulateTemplate(item);
            return result;

        }

        public static EmailTemplate PopulateTemplate(string url, SPWeb web, string name, SPListItem item)
        {
            var templateItem = CCIUtility.GetEmailTemplate(url, web, name);
            if (templateItem == null) return null;
            var result = new EmailTemplate(templateItem);
            var bodyVariables = Parser.GetAndSetVariablesFromTemplate(result.Body, item, null);
            var subjectVariables = Parser.GetAndSetVariablesFromTemplate(result.Subject, item, null);
            var designerVariales = Parser.GetVariablesFromDesigner(result.Body);
            result.Subject = Parser.SetValuesToContent(result.Subject, subjectVariables, designerVariales);
            result.Body = Parser.SetValuesToContent(result.Body, bodyVariables, designerVariales);
            result.Body = result.Body.PopulateTemplate(item);
            result.Subject = result.Subject.PopulateTemplate(item);

            return result;

        }
    }
}
