using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static System.Console;

namespace OrganizeFolders
{
    public class FolderHelper
    {

        public string LoggerPath { get; set; }
        private static readonly string documentsPath = ConfigurationManager.AppSettings["DocumentsPath"];
        private static readonly string picturePath = ConfigurationManager.AppSettings["PhotosPath"];
        private static readonly string musicPath = ConfigurationManager.AppSettings["MusicPath"];
        private string executablesPath = ConfigurationManager.AppSettings["ExecutablesPath"];
        private string otherPath = ConfigurationManager.AppSettings["OtherPath"];
        

        private static List<string> docExtensions = new List<string> { "doc", "docx", "odt", "pdf", "txt", "xlx", "xlxs", "xps", };
        private static List<string> photoExtensions = new List<string>() { "jpg", "jpeg", "png", "gif", "tiff", "psd", "eps", "ai", "raw", "indd", "cdr", "svg", "bmp" };
        private static List<string> musicExtensions = new List<string>() { "3gp", "aac", "aiff", "au", "awb", "dss", "flac", "m4a", "m4p", "mp3", "mpc", "ogg", "ra", "raw", "sln", "vox", "wav", "wma", "webm", "cda" };
        private static List<string> executableExtensions = new List<string>() { "apk","bin", "cmd", "csh", "inf", "msi", "exe", "com", "bat", "vb", "vbs", "wsd", "pif", "app" };

        [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
        internal async void FileCreated(object source, FileSystemEventArgs e)
        {
            var extension = GetExtension(e);
            using (StreamWriter sw = new FileInfo(LoggerPath).AppendText())
            {
                string pathMoveTo = "";

                sw.WriteLine($"--> {e.ChangeType} a file on {DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")} : '{e.Name}' ");
                SortFiles(e, extension);

                if (docExtensions.Contains(extension)) pathMoveTo = documentsPath;
                else if (photoExtensions.Contains(extension)) pathMoveTo = picturePath;
                sw.WriteLine($"--> '{e.Name}' moved to '{GetRootDirectory(e.FullPath) + "\\" + pathMoveTo.Substring(0, documentsPath.Length - 4)}'. ");

                //sw.Flush();
            }

        }

        internal async void FileRenamed(object source, RenamedEventArgs e)
        {

            using (StreamWriter sw = File.AppendText(LoggerPath))
            {
                sw.WriteLine($"--> File {e.ChangeType.ToString().ToLower()} on {DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")} : '{e.OldName}' => '{e.Name}'");
                sw.Flush();
            }
            Console.WriteLine("FileRenamed");


        }

        internal void FileDeleted(object source, FileSystemEventArgs e)
        {
            using (StreamWriter sw = File.AppendText(LoggerPath))
            {
                sw.WriteLine($"--> File '{e.Name}' {e.ChangeType.ToString().ToLower()} from '{GetRootDirectory(e.FullPath)}' on {DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}");
                sw.Close();
            }
        }


        [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
        internal async void SortFiles(FileSystemEventArgs e, string ext)
        {
            try
            {
                System.Threading.Thread.Sleep(1000); // 5sec

                //Documents
                if (e.ChangeType == WatcherChangeTypes.Created && docExtensions.Contains(ext) && File.Exists(e.FullPath))
                {
                    var newCreatePath = GetRootDirectory(e.FullPath) + "\\" + documentsPath.Substring(0, documentsPath.Length - 4);
                    //if (GetRootDirectory(e.FullPath).Contains(documentsPath.Substring(0, documentsPath.Length - 4))) return; // if file created in a sorted folder, don't do anthing
                    using (Stream streamSource = File.Open(e.FullPath, FileMode.Open))
                    {
                        Directory.CreateDirectory(newCreatePath);
                        using (Stream destination = File.Create(newCreatePath + "\\" + e.Name))
                        {
                            await streamSource.CopyToAsync(destination);
                        }

                    }
                    File.Delete(e.FullPath);
                }
                // Photos
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
                    }
                    File.Delete(e.FullPath);
                }
                // Music
                else if (e.ChangeType == WatcherChangeTypes.Created && musicExtensions.Contains(ext) && File.Exists(e.FullPath))
                {
                    var newCreatePath = GetRootDirectory(e.FullPath) + "\\" + musicPath.Substring(0, musicPath.Length - 4);
                    using (Stream streamSource = File.Open(e.FullPath, FileMode.Open))
                    {
                        Directory.CreateDirectory(newCreatePath);
                        using (Stream destination = File.Create(newCreatePath + "\\" + e.Name))
                        {
                            await streamSource.CopyToAsync(destination);
                        }
                    }
                    File.Delete(e.FullPath);
                }

                // Executables
                else if (e.ChangeType == WatcherChangeTypes.Created && executableExtensions.Contains(ext) && File.Exists(e.FullPath))
                {
                    var newCreatePath = GetRootDirectory(e.FullPath) + "\\" + executablesPath.Substring(0, executablesPath.Length - 4);
                    using (Stream streamSource = File.Open(e.FullPath, FileMode.Open))
                    {
                        Directory.CreateDirectory(newCreatePath);
                        using (Stream destination = File.Create(newCreatePath + "\\" + e.Name))
                        {
                            await streamSource.CopyToAsync(destination);
                        }
                    }
                    File.Delete(e.FullPath);
                }

                // Other
                else if (e.ChangeType == WatcherChangeTypes.Created )
                {
                    var newCreatePath = GetRootDirectory(e.FullPath) + "\\" + otherPath.Substring(0, otherPath.Length - 4);
                    using (Stream streamSource = File.Open(e.FullPath, FileMode.Open))
                    {
                        Directory.CreateDirectory(newCreatePath);
                        using (Stream destination = File.Create(newCreatePath + "\\" + e.Name))
                        {
                            await streamSource.CopyToAsync(destination);
                        }
                    }
                    File.Delete(e.FullPath);
                }
            }
            catch (Exception ex)
            {
                WriteLine(ex.Message);
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

    }

}

