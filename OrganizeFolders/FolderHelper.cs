using System.Collections.Generic;
using System.IO;
using static System.Console;

namespace OrganizeFolders
{
    public class FolderHelper
    {
        public string InputDirectoryString { get; }
        private string documentPath;
        private string picturePath;
        private string executablesPath;
        private string subFoldersPath;
        private string musicPath;

        private static List<string> docExtensions = new List<string> { "doc", "docx", "odt", "pdf", "txt", "xlx", "xlxs", "xps", };
        //dodatkowe rozszerania - lista

        //logger ustalić gdzie ma sie utworzyć i móc go zapisać

        internal async static void FileCreated(object source, FileSystemEventArgs e)
        {
            WriteLine($"* File created : {e.Name} - type: {e.ChangeType}");
            //logger - nazwa, co sie stalo, gdzie
        }

        internal async static void FileRenamed(object source, FileSystemEventArgs e)
        {

        }

        internal async static void SortFiles()
        {
        
        }



    }
}
