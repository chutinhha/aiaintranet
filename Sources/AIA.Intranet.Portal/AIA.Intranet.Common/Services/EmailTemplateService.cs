using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AIA.Intranet.Model.Entities;
using Microsoft.SharePoint;
using AIA.Intranet.Common.Utilities;
using AIA.Intranet.Common.Utilities.Camlex;

namespace AIA.Intranet.Common.Services
{
   public class EmailTemplateService
   {
       public static EmailTemplate GetTemplateByName(string url, string name)
       {
           var spList = CCIUtility.GetListFromURL(url);
           CAMLListQuery<EmailTemplate> queryer = new CAMLListQuery<EmailTemplate>(spList);
           string CAML = Camlex.Query()
                                          .Where(x => (string)x["Title"] == name).ToString();

           return queryer.ExecuteSingleQuery(CAML);
       }
   }
}
