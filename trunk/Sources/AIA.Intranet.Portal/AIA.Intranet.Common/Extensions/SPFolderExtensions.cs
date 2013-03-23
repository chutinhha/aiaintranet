using System;
using System.IO;
using Microsoft.SharePoint;

namespace AIA.Intranet.Common.Extensions
{
    public static class SPFolderExtensions
    {
        public static bool FileExists(this SPFolder folder, string fileName)
        {
            bool exists = false;
            SPWeb web = folder.ParentWeb;
            String testFilePath = Path.Combine(folder.Url, fileName);

            if (web.GetFile(testFilePath).Exists)
            {
                exists = true;
            }

            return exists;
        }

        public static SPFile GetFile(this SPFolder folder, string fileName)
        {
            SPFile file = null;
            SPWeb web = folder.ParentWeb;
            String testFilePath = Path.Combine(folder.Url, fileName);
            
            file = web.GetFile(testFilePath);
            if (!file.Exists)
                return null;

            return file;
        }
    }
}
