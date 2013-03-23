using System;
using System.IO;
using System.Text;
using System.Xml;
using System.IO.Packaging;

namespace AIA.Intranet.Common.Helpers
{
    public class DocxFileReader : IDisposable

    {
        private Stream FileStream;
        private const string ContentTypeNamespace =
            @"http://schemas.openxmlformats.org/package/2006/content-types";

        private const string WordprocessingMlNamespace =
            @"http://schemas.openxmlformats.org/wordprocessingml/2006/main";

        private const string DocumentXmlXPath =
            "/t:Types/t:Override[@ContentType=\"application/vnd.openxmlformats-officedocument.wordprocessingml.document.main+xml\"]";

        private const string BodyXPath = "/w:document/w:body";

        private string docxFile = "";
        private string docxFileLocation = "";


        public DocxFileReader (string fileName)
        {
            docxFile = fileName;
        }

        public DocxFileReader(Stream fs)
        {
            FileStream = fs;
        }
        #region ExtractText()
        /// <summary>
        /// Extracts text from the Docx file.
        /// </summary>
        /// <returns>Extracted text.</returns>
        public string ExtractText()
        {
            if (string.IsNullOrEmpty(docxFile) && FileStream== null)
                throw new Exception("Input file not specified.");

            // Usually it is "/word/document.xml"


            String documentRelationshipType = "http://schemas.openxmlformats.org/officeDocument/2006/relationships/officeDocument";
            String dcPropertiesSchema = "http://purl.org/dc/elements/1.1/";
            StringBuilder sb = new StringBuilder();

            using (Package package = FileStream!= null?Package.Open(FileStream, FileMode.Open, FileAccess.Read) :Package.Open(docxFile, FileMode.Open, FileAccess.Read))
            {
                foreach (PackageRelationship relationship in package.GetRelationshipsByType(documentRelationshipType))
                {
                    //  There should be only one document part in the package. 
                    Uri documentUri = PackUriHelper.ResolvePartUri(new Uri("/", UriKind.Relative), relationship.TargetUri);
                    PackagePart documentPart = package.GetPart(documentUri);
                    Stream documentXml = documentPart.GetStream();

                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.PreserveWhitespace = true;
                    xmlDoc.Load(documentXml);
                    documentXml.Close();

                    XmlNamespaceManager nsmgr = new XmlNamespaceManager(xmlDoc.NameTable);
                    nsmgr.AddNamespace("w", WordprocessingMlNamespace);

                    XmlNode node = xmlDoc.DocumentElement.SelectSingleNode(BodyXPath, nsmgr);

                    if (node == null)
                        return string.Empty;

                    sb.Append(ReadNode(node));
                    documentXml.Close();
                }
            }

            return sb.ToString();

        }
        #endregion

        #region ReadNode()
        /// <summary>
        /// Reads content of the node and its nested childs.
        /// </summary>
        /// <param name="node">XmlNode.</param>
        /// <returns>Text containing in the node.</returns>
        private string ReadNode(XmlNode node)
        {
            if (node == null || node.NodeType != XmlNodeType.Element)
                return string.Empty;

            StringBuilder sb = new StringBuilder();
            foreach (XmlNode child in node.ChildNodes)
            {
                if (child.NodeType != XmlNodeType.Element) continue;

                switch (child.LocalName)
                {
                    case "t":                           // Text
                        sb.Append(child.InnerText.TrimEnd());

                        string space = ((XmlElement)child).GetAttribute("xml:space");
                        if (!string.IsNullOrEmpty(space) && space == "preserve")
                            sb.Append(' ');

                        break;

                    case "cr":                          // Carriage return
                    case "br":                          // Page break
                        sb.Append(Environment.NewLine);
                        break;

                    case "tab":                         // Tab
                        sb.Append("\t");
                        break;

                    case "p":                           // Paragraph
                        sb.Append(ReadNode(child));
                        sb.Append(Environment.NewLine);
                        sb.Append(Environment.NewLine);
                        break;

                    default:
                        sb.Append(ReadNode(child));
                        break;
                }
            }
            return sb.ToString();
        }
        #endregion

        public void Dispose()
        {
            FileStream.Close();
            FileStream.Dispose();
        }
    }
}
