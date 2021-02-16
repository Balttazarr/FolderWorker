using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Permissions;
using System.Text;
using static System.Console;

namespace OrganizeFolders
{
    public class FolderWatcher
    {
        public string PathToWatch { get; set; }
        public string LoggerFile = "Logger.txt";
        [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
        public FolderWatcher(string newDirectoryPath)
        {
            //Create the path to watch
            PathToWatch = newDirectoryPath;
            if (!Directory.Exists(newDirectoryPath))
            {
                Directory.CreateDirectory(newDirectoryPath);
            }

            //create the Logger.txt file if doesn't exists
            LoggerFile = PathToWatch + "\\" + LoggerFile;
            try
            {
                if (File.Exists(LoggerFile))
                {
                    File.Delete(LoggerFile);
                }
                using (FileStream fs = File.Create(LoggerFile))
                {
                    byte[] info = new UTF8Encoding(true).GetBytes("Logger - saving all actions done on the chosen folder.\n________________________________________\n");
                    //add infos to file
                    fs.Write(info, 0, info.Length);
                }
                
            }
            catch (Exception ex)
            {
                WriteLine(ex.Message.ToString()); 
            }
        }
        public void Start()
        {
            StartWatcher();
        }

        private void StartWatcher()
        {
            var folderHelper = new FolderHelper();
            folderHelper.LoggerPath = LoggerFile;

            var inputFileWatcher = new FileSystemWatcher(PathToWatch);
            inputFileWatcher.IncludeSubdirectories = true;
            inputFileWatcher.InternalBufferSize = 32768; // 32 KB
            inputFileWatcher.Filter = "*.*"; // default
            inputFileWatcher.NotifyFilter = NotifyFilters.LastWrite
                                          | NotifyFilters.FileName
                                          | NotifyFilters.DirectoryName
                                          | NotifyFilters.Size;

            inputFileWatcher.Created += folderHelper.FileCreated;
            inputFileWatcher.Renamed += folderHelper.FileRenamed;
            inputFileWatcher.Deleted += folderHelper.FileDeleted;

            //inputFileWatcher.Error += WatcherError;

            inputFileWatcher.EnableRaisingEvents = true;
        }

        public void Stop()
        {
            WriteLine("Service: FolderOrganier has been stopped");
        }



    }
}
