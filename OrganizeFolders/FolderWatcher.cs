using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Permissions;
using static System.Console;

namespace OrganizeFolders
{
    public class FolderWatcher
    {
        public string PathToWatch { get; set; }
        [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
        public FolderWatcher(string newDirectoryPath)
        {
            PathToWatch = newDirectoryPath;
            if (!Directory.Exists(newDirectoryPath))
            {
                Directory.CreateDirectory(newDirectoryPath);
            }
            WriteLine("Called ctor :/");
        }
        public void Start()
        {
            StartWatcher();
        }

        private void StartWatcher()
        {
            var inputFileWatcher = new FileSystemWatcher(PathToWatch);
            inputFileWatcher.IncludeSubdirectories = false;
            inputFileWatcher.InternalBufferSize = 32768; // 32 KB
            inputFileWatcher.Filter = "*.*"; // default
            inputFileWatcher.NotifyFilter = NotifyFilters.LastWrite
                                          | NotifyFilters.FileName
                                          | NotifyFilters.DirectoryName;

            inputFileWatcher.Created += FolderHelper.FileCreated;
            //inputFileWatcher.Changed += FileChanged;
            //inputFileWatcher.Deleted += FileDeleted;
            //inputFileWatcher.Renamed += FileRenamed;
            //inputFileWatcher.Error += WatcherError;

            inputFileWatcher.EnableRaisingEvents = true;
        }

        public void Stop()
        {

        }



    }
}
