using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using AIA.Intranet.Common.Helpers;
using System.Collections;
using AIA.Intranet.Common.Utilities;
using AIA.Intranet.Common.NVelocityTemplateEngine;
using System.Xml.Linq;
using Microsoft.SharePoint;

namespace AIA.Intranet.Common.Extensions
{
    public static class StringExtensions
    {
        public static bool IsAbsoluteUrl(this string url)
        {
            Uri result;
            return Uri.TryCreate(url, UriKind.Absolute, out result);
        }

        public static string TrimQueryString(this string source)
        {
            string result = source;
            if (string.IsNullOrEmpty(source)) return string.Empty;
            string[] removeTokens = {"&IsDlg=1"};

            foreach (var item in removeTokens)
            {
                result = result.Replace(item, string.Empty);
            }
            return result;

         }
        public static string ToMD5(this string name)
        {
            return SecurityHelper.HashPassword(name);
        }
        public static bool IsDocumentExtension(this string name)
        {
            string nameExt = Path.GetExtension(name).ToLower();
            if (nameExt == ".doc" || nameExt == ".docx")
                return true;
            return false;
        }

        public static string EncodeBase64(this string sInput)
        {
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(sInput));
        }

        public static string DecodeBase64(this string sInput)
        {
            return Encoding.UTF8.GetString(Convert.FromBase64String(sInput));
        }

        public static string EncodeAlertWord(this string sInput)
        {
            return "=?utf-8?B?" + sInput.EncodeBase64() + "?=";
        }

        public static bool IsValidEmailAddress(this string s)
        {
            s = s.Trim();
            Regex regex = new Regex(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$");

            if (s.IndexOf(';') != -1)
            {
                var array = s.Split(';');
                foreach (var str in array)
                {
                    if (!str.IsValidEmailAddress()) return false;
                }
            }
            else if (s.IndexOf(',') != -1)
            {
                var array = s.Split(',');
                foreach (var str in array)
                {
                    if (!str.IsValidEmailAddress()) return false;
                }
            }
            else { 
                return regex.IsMatch(s);
            }

            return true;
        }

        //public static bool IsValidEmailAddress(this string s, Regex regex)
        //{
        //    return regex.IsMatch(s);
        //}

        public static string ConvertToValidSharePointFileName(this string fileName)
        {
            char[] invalidFilenameCharacters = new[] { '\\', '/', ':', '*', '?', '"', '<', '>', '|', '#', '{', '}', '%', '~', '&' };
            string[] invalidFileNameStrings = new[] { ".files", "_files",
	                "-Dateien", "_fichiers", "_bestanden","_file",
	                "_archivos", "-filer", "_tiedostot", "_pliki",
	                "_soubory","_elemei", "_ficheiros", "_arquivos",
	                "_dosyalar", "_datoteke","_fitxers", "_failid",
	                "_fails", "_bylos", "_fajlovi", "_fitxategiak"};

            string validSharePointFileName = fileName;

            //Remove Invalid characters
            int pos;
            while ((pos = validSharePointFileName.IndexOfAny(invalidFilenameCharacters)) >= 0)
            {
                validSharePointFileName = validSharePointFileName.Remove(pos, 1);
            }

            //Remove consecutive periods(..)
            while (validSharePointFileName.Contains(".."))
            {
                validSharePointFileName = validSharePointFileName.Replace("..", ".");
            }


            //Remove reserved words from end
            bool done = false;
            while (!done)
            {
                bool removed = false;
                foreach (string s in invalidFileNameStrings)
                {
                    if (validSharePointFileName.EndsWith(s))
                    {
                        if (validSharePointFileName.Length > s.Length)
                        {
                            validSharePointFileName = validSharePointFileName.Substring(0, validSharePointFileName.Length - s.Length);
                            removed = true;
                        }
                    }
                }
                done = !removed;
            }

            //Remove period (.) at end
            if (validSharePointFileName.EndsWith("."))
            {
                validSharePointFileName = validSharePointFileName.Remove(validSharePointFileName.Length - 1, 1);
            }

            //Remove period (.) at end
            if (validSharePointFileName.StartsWith("."))
            {
                validSharePointFileName = validSharePointFileName.Remove(0, 1);
            }
            string name = Path.GetFileNameWithoutExtension(validSharePointFileName);
            string ext = Path.GetExtension(validSharePointFileName);
            name = name.TrimBy(123);

            return name+ext;
        }

        public static string ToPlainText(this string source)
        {
            HtmlToTextConverter converter = new HtmlToTextConverter();
            return converter.Convert(source);

        }

        public static string TrimBy(this string source, int characters)
        {
            if (source == null) return string.Empty;
            if (source.Length < characters || characters <= 0) return source;
            return source.Substring(0, characters);

        }

        public static string PopulateTemplate(this string template, SPListItem item)
        {
            try
            {
                var obj = item.ToDictionary().ToAnonymos();

                var memoryEngine = NVelocityEngineFactory.CreateNVelocityMemoryEngine(true);
                var Parameters = new Hashtable();
                Parameters.Add("Item", obj);
                template = memoryEngine.Process(Parameters, template);
            }
            catch (Exception ex)
            {

                CCIUtility.LogError(ex.Message + ex.StackTrace, Model.IOfficeFeatures.IOfficeApp);
            }
            return template;
        }

        public static string CustomFunctionPopulate(this string template)
        {
            try
            {
                var memoryEngine = NVelocityEngineFactory.CreateNVelocityMemoryEngine(true);
                var Parameters = new Hashtable();
                Parameters.Add("Helper", new StringHelper());
                template= memoryEngine.Process(Parameters, template);
            }
            catch (Exception ex)
            {

                CCIUtility.LogError(ex.Message+ ex.StackTrace, Model.IOfficeFeatures.IOfficeApp);
            }
           return template;
        }

        
        public static string DoStripDiacritics(this string accented)
        {
            Regex regex = new Regex(@"\p{IsCombiningDiacriticalMarks}+");

            string strFormD = accented.Normalize(NormalizationForm.FormD);
            return regex.Replace(strFormD, String.Empty).Replace('\u0111', 'd').Replace('\u0110', 'D');
        }


        public static string Simplyfied(this string source)
        {
            Regex regex = new Regex("\\s");
            return regex.Replace(source.DoStripDiacritics(), "");
        }

        public static string ConvertToValidSharePointFieldName(this string source)
        {
            System.Globalization.TextInfo UsaTextInfo = new System.Globalization.CultureInfo("en-US", false).TextInfo;
            string staticName = UsaTextInfo.ToTitleCase(source);
            Regex regex = new Regex(@"\W");
            return regex.Replace(staticName.DoStripDiacritics(), string.Empty);
        }

        public static bool IsGuid(this string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return false;
            }

            const string pattern = @"^(\{{0,1}([0-9a-fA-F]){8}-([0-9a-fA-F]){4}-([0-9a-fA-F]){4}-([0-9a-fA-F]){4}-([0-9a-fA-F]){12}\}{0,1})$";
            return Regex.IsMatch(value, pattern);
        }

        public static string ReplaceXmlAttributeValue(this string xml, string attributeName, string value)
        {
            if (string.IsNullOrEmpty(xml))
            {
                throw new ArgumentNullException("xml");
            }

            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentNullException("value");
            }


            int indexOfAttributeName = xml.IndexOf(string.Format(" {0}", attributeName), StringComparison.CurrentCultureIgnoreCase);

            if (indexOfAttributeName == -1)
            {
                throw new ArgumentOutOfRangeException("attributeName", string.Format("Attribute {0} not found in source xml", attributeName));
            }

            int indexOfAttibuteValueBegin = xml.IndexOf('"', indexOfAttributeName + 1);
            int indexOfAttributeValueEnd = xml.IndexOf('"', indexOfAttibuteValueBegin + 1);

            return xml.Substring(0, indexOfAttibuteValueBegin + 1) + value + xml.Substring(indexOfAttributeValueEnd);
        }
        public static string EnsureXmlAttribute(this string xml, string name, string value)
        {
            System.Xml.Linq.XElement ele = XElement.Parse(xml);
            if (ele == null) return xml;
            var att = ele.Attribute(name);
            if (att == null)
            {
                att = new XAttribute(name, value);
                ele.Add(att);

            }
            else
            {
                ele.SetAttributeValue(name, value);
            }
            return ele.ToString();
        }
        public static string EnsureXmlAttribute(this string xml, string name, object value)
        {
            System.Xml.Linq.XElement ele = XElement.Parse(xml);
            if (ele == null) return xml;
            var att = ele.Attribute(name);
            if (att == null)
            {
                att = new XAttribute(name, value);
                ele.Add(att);

            }
            else
            {
                ele.SetAttributeValue(name, value);
            }
            return ele.ToString();
        }

    }
}

