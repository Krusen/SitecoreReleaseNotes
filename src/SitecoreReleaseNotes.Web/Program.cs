using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace SitecoreReleaseNotes.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration(AddSecrets)
                .UseStartup<Startup>()
                .Build();

        private static void AddSecrets(IConfigurationBuilder builder)
        {
            if (File.Exists("secrets.json"))
                builder.AddJsonFile("secrets.json");
        }
    }
}
