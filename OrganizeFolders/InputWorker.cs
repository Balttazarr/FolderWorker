using System;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Topshelf;

namespace OrganizeFolders
{
    public class InputWorker
    {
        private static readonly string defaultPathToWatch = ConfigurationManager.AppSettings["WatchedPath"];

        static void Main(string[] args)
        {
            //string pathToWatch = "";
            //if (args[0] != "not")
            //{
            //    pathToWatch = args[3];
            //}
            //else
            //{
            //    pathToWatch = defaultPathToWatch;
            //}
            var exitCode = HostFactory.Run(x =>
            {
                //setup
                x.Service<FolderWatcher>(s =>
                {
                    s.ConstructUsing(fwatch => new FolderWatcher(defaultPathToWatch));
                    s.WhenStarted(fwatch => fwatch.Start());
                    s.WhenStopped(fwatch => fwatch.Stop());
                });

                //how to run the service
                x.RunAsLocalSystem();

                x.SetServiceName("FolderOrganizerService");
                x.SetDisplayName("FolderOrganizer Serivce");
                x.SetDescription("Service organizing a chosen folder. Sort files to correspoding folders of similar extension types.");
            });

            int exitCodeValue = (int)Convert.ChangeType(exitCode, exitCode.GetTypeCode());
            Environment.ExitCode = exitCodeValue;

        }
    }
}