using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.Search;
using Microsoft.Azure.WebJobs;
using SitecoreReleaseNotes.Core.Indexing;

namespace SitecoreReleaseNotes.Jobs.Indexing
{
    // To learn more about Microsoft Azure WebJobs SDK, please see https://go.microsoft.com/fwlink/?LinkID=320976
    class Program
    {
        // Please set the following connection strings in app.config for this WebJob to run:
        // AzureWebJobsDashboard and AzureWebJobsStorage
        static void Main()
        {
            var config = new JobHostConfiguration();

            if (config.IsDevelopment)
            {
                config.UseDevelopmentSettings();
            }

            var host = new JobHost();
            // The following code ensures that the WebJob will be running continuously
            //host.RunAndBlock();

            // The following code will invoke a function called ManualTrigger and
            // pass in data (value in this case) to the function
            //host.Call(typeof(Functions).GetMethod("ManualTrigger"));

            var task = host.CallAsync(typeof(Functions).GetMethod(nameof(Functions.ManualTrigger)));

            Console.WriteLine("Waiting for index operation...");
            task.Wait();
            Console.WriteLine("Indexing complete: " + task.Status);
        }


    }
}
