using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading;
using static System.Console;

namespace OrganizeFolders
{
    public class FolderHelper
    {

        public string LoggerPath { get; set; }
        private static readonly string documentsPath = ConfigurationManager.AppSettings["DocumentsPath"];
        private string picturePath = ConfigurationManager.AppSettings["PhotosPath"];
        private string executablesPath;
        private string subFoldersPath;
        private string musicPath;

        private static List<string> docExtensions = new List<string> { "doc", "docx", "odt", "pdf", "txt", "xlx", "xlxs", "xps", };
        private static List<string> photoExtensions = new List<string>() { "jpg", "jpeg", "png", "gif", "tiff", "psd", "eps", "ai", "raw", "indd", "cdr", "svg", "bmp" };

        [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
        internal void FileCreated(object source, FileSystemEventArgs e)
        {
            var extension = GetExtension(e);
            using (StreamWriter sw = File.AppendText(LoggerPath))
            {
                sw.WriteLine($"--> {e.ChangeType} a file on {DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")} : '{e.Name}' ");
                SortFiles(e, extension);
            }

        }

        internal void FileRenamed(object source, RenamedEventArgs e)
        {
            FileInfo file = new FileInfo(e.FullPath);
            using (StreamWriter sw = File.AppendText(LoggerPath))
            {
                sw.WriteLine($"--> File {e.ChangeType.ToString().ToLower()} on {DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")} : '{e.OldName}' => '{e.Name}'");
            }
        }

        internal void FileDeleted(object source, FileSystemEventArgs e)
        {
            using (StreamWriter sw = File.AppendText(LoggerPath))
            {
                sw.WriteLine($"--> File '{e.Name}' {e.ChangeType.ToString().ToLower()} from '{GetRootDirectory(e.FullPath)}' on {DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}");
            }
        }


        [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
        internal async void SortFiles(FileSystemEventArgs e, string ext)
        {
            try
            {
                System.Threading.Thread.Sleep(5000); // 5sec
                if (e.ChangeType == WatcherChangeTypes.Created && docExtensions.Contains(ext) && File.Exists(e.FullPath))
                {
                    var newCreatePath = GetRootDirectory(e.FullPath) + "\\" + documentsPath.Substring(0, documentsPath.Length - 4);
                    if (newCreatePath.Contains(GetRootDirectory(e.FullPath))) return; // if file created in a sorted folder, don't do anthing
                    using (Stream streamSource = File.Open(e.FullPath, FileMode.Open))
                    {
                        Directory.CreateDirectory(newCreatePath);
                        using (Stream destination = File.Create(newCreatePath + "\\" + e.Name))
                        {
                            await streamSource.CopyToAsync(destination);
                        }
                        FileMoved(e, newCreatePath);

                    }
                    File.Delete(e.FullPath);
                }
                else if (e.ChangeType == WatcherChangeTypes.Created && photoExtensions.Contains(ext) && File.Exists(e.FullPath))
                {
                    var newCreatePath = GetRootDirectory(e.FullPath) + "\\" + picturePath.Substring(0, picturePath.Length - 4);
                    using (Stream streamSource = File.Open(e.FullPath, FileMode.Open))
                    {
                        Directory.CreateDirectory(newCreatePath);
                        using (Stream destination = File.Create(newCreatePath + "\\" + e.Name))
                        {
                            await streamSource.CopyToAsync(destination);
                        }
                        FileMoved(e, newCreatePath);
                    }
                }
                
            }
            catch
            {
                throw;
            }

        }

        private string GetRootDirectory(string inputFilePath)
        {
            return new DirectoryInfo(inputFilePath).Parent.FullName;
        }

        private string GetExtension(FileSystemEventArgs e)
        {
            string output = Path.GetExtension(e.Name).Replace(".", "").ToLower();
            return output;
        }

        private void FileMoved(FileSystemEventArgs e, string newCreatedPath)
        {
            using (StreamWriter sw = File.AppendText(LoggerPath))
            {
                sw.WriteLine($"--> '{e.Name}' moved to '{newCreatedPath}'. ");
            }
        }

    }

}

