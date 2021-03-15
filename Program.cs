using System;
using System.IO;
using System.Reflection;
using log4net;
using log4net.Config;
using Topshelf;

namespace SentientStatus
{
    public class Program
    {

        public static void Main(string[] args)
        {
            var exitCode = HostFactory.Run(x =>
            {
                
                var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
                XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));

                x.UseLog4Net();
                
                x.Service<SentientStatus>(s =>
                {
                    s.ConstructUsing(service => new SentientStatus());
                    s.WhenStarted(service => service.Start());
                    s.WhenStopped(service => service.Stop());
                });

                x.RunAsLocalSystem();

                x.SetServiceName("SentientStatus");
                x.SetDisplayName("Sentient Status");
                x.SetDescription("Discord status but sentient! Changes every 30 minutes.");
            });

            int exitCodeValue = (int)Convert.ChangeType(exitCode, exitCode.GetTypeCode());
            Environment.ExitCode = exitCodeValue;
        }
    }
}
