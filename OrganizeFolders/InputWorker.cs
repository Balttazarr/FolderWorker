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
        private static readonly string pathToWatch = ConfigurationManager.AppSettings["WatchedPath"];

        static void Main(string[] args)
        {
            var exitCode = HostFactory.Run(x =>
            {
                //setup
                x.Service<FolderWatcher>(s =>
                {
                    s.ConstructUsing(fwatch => new FolderWatcher(pathToWatch));
                    s.WhenStarted(fwatch => fwatch.Start());
                    s.WhenStopped(fwatch => fwatch.Stop());
                });

                //how to run the service
                x.RunAsLocalSystem();

                x.SetServiceName("FolderOrganizerService");
                x.SetDisplayName("FolderOrganizer Serivce");
                x.SetDescription("gsgdfdf");
            });

            int exitCodeValue = (int)Convert.ChangeType(exitCode, exitCode.GetTypeCode());
            Environment.ExitCode = exitCodeValue;

        }
    }
}