using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using static System.Console;

namespace OrganizeFolders
{
    public class FolderHelper
    {

        public string LoggerPath { get; set; }
        private string documentPath;
        private string picturePath;
        private string executablesPath;
        private string subFoldersPath;
        private string musicPath;

        private static List<string> docExtensions = new List<string> { "doc", "docx", "odt", "pdf", "txt", "xlx", "xlxs", "xps", };


        internal void FileCreated(object source, FileSystemEventArgs e)
        {
            using (StreamWriter sw = File.AppendText(LoggerPath))
            {
              sw.WriteLine($"--> File created on {DateTime.Now.ToString("yyyy-MM-dd")} : {e.Name} - type: {e.ChangeType}");
            }

        }

        internal async static void FileRenamed(object source, FileSystemEventArgs e)
        {

        }

        internal async static void SortFiles()
        {

        }



    }
}
