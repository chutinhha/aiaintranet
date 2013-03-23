using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Xml.Linq;
using AIA.Intranet.Common.Utilities;
using Microsoft.SharePoint;
//using Word = Microsoft.Office.Interop.Word;

namespace AIA.Intranet.Common.Helpers
{
    public class HashHelper
    {
        public static byte[] Create(SPFile file, bool includeMetadata)
        {
            HashAlgorithm hash = HashAlgorithm.Create();
            if (includeMetadata)
                return hash.ComputeHash(file.OpenBinary());

            string fileExt = Path.GetExtension(file.Name).ToLower();
            if (fileExt == ".doc" || fileExt == ".docx")
                return hash.ComputeHash(Encoding.ASCII.GetBytes(getMainDocumentPart(file)));

            return hash.ComputeHash(file.OpenBinary());
        }
       
        private static string getMainDocumentPart(SPFile file)
        {
            if (file == null) return null;

            string mainDocumentPartXml = string.Empty;
            string tempFile = Path.ChangeExtension(Path.GetTempPath() + Guid.NewGuid().ToString(), Path.GetExtension(file.Name).ToLower());
            
            //Word.Application app = null;
            //Word.Documents docs = null;
            //Word._Document doc = null;
            //try
            //{
            //    byte[] content = file.OpenBinary();
            //    using (FileStream f1 = new FileStream(tempFile, FileMode.OpenOrCreate))
            //        f1.Write(content, 0, content.Length);

            //    app = new Word.ApplicationClass();
            //    docs = app.Documents;
            //    object readOnly = true;
            //    object isVisible = true;
            //    object isRevert = false;
            //    object missing = System.Reflection.Missing.Value;
            //    object tempFileName = tempFile;
            //    doc = docs.Open(ref tempFileName, ref missing, ref readOnly,
            //            ref missing, ref missing, ref missing, ref isRevert, ref missing, ref missing,
            //            ref missing, ref missing, ref isVisible, ref missing, ref missing, ref missing, ref missing);

            //    if (doc != null)
            //    {
            //        XElement xe = XElement.Parse(doc.WordOpenXML);
            //        IEnumerable<XElement> awElements =
            //            from el in xe.Descendants()
            //            where el.Name.Namespace == "http://schemas.openxmlformats.org/wordprocessingml/2006/main" && el.Name.LocalName == "body"
            //            select el;
            //        mainDocumentPartXml = awElements.FirstOrDefault().Parent.FirstNode.ToString();

            //        object isSave = false;
            //        doc.Close(ref isSave, ref missing, ref missing);
            //        System.Runtime.InteropServices.Marshal.ReleaseComObject(doc);
            //        doc = null;
            //    }
                
            //    System.Runtime.InteropServices.Marshal.ReleaseComObject(docs);
            //    docs = null;

            //    app.Quit(ref missing, ref missing, ref missing);
            //    System.Runtime.InteropServices.Marshal.ReleaseComObject(app);
            //    app = null;
            //    Thread.Sleep(1000);
            //}
            //catch (Exception ex)
            //{
            //    CCIUtility.LogError(ex.Message, "CCI.Common");
            //}
            //finally
            //{
            //    try
            //    {
            //        object missing = System.Reflection.Missing.Value;
            //        if (doc != null)
            //        {
            //            object isSave = false;
            //            doc.Close(ref isSave, ref missing, ref missing);
            //            System.Runtime.InteropServices.Marshal.ReleaseComObject(doc);
            //            doc = null;
            //        }

            //        if (docs != null)
            //        {
            //            System.Runtime.InteropServices.Marshal.ReleaseComObject(docs);
            //            docs = null;
            //        }

            //        if (app != null)
            //        {
            //            app.Quit(ref missing, ref missing, ref missing);
            //            System.Runtime.InteropServices.Marshal.ReleaseComObject(app);
            //            app = null;
            //            Thread.Sleep(1000);
            //        }
            //        if (File.Exists(tempFile))
            //            File.Delete(tempFile);
            //    }
            //    catch { }
            //}

            return mainDocumentPartXml;
        }

        public static bool Compare(byte[] byteA, byte[] byteB)
        {
            if (byteA == null || byteB == null)
                return false;

            return BitConverter.ToString(byteA) == BitConverter.ToString(byteB);
        }
    }
}
