using System;
using System.Configuration;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Azure.Search;
using SitecoreReleaseNotes.Core.Indexing;

namespace SitecoreReleaseNotes.Functions.Indexing
{
    public static class Function
    {
        [FunctionName("Indexing")]
        public static Task Run([TimerTrigger("0 0 3 */1 * *")]TimerInfo myTimer, TraceWriter log)
        {
            log.Info($"C# Timer trigger function executed at: {DateTime.Now}");

            var serviceName = ConfigurationManager.AppSettings["AzureSearch:ServiceName"];
            var adminKey = ConfigurationManager.AppSettings["AzureSearch:AdminApiKey"];

            log.Info("Beginning indexing of release notes");

            var searchService = new SearchServiceClient(serviceName, new SearchCredentials(adminKey));
            var crawler = new ReleaseNotesCrawler();

            var indexer = new ReleaseNotesIndexer(searchService, crawler);

            // TODO: Maybe change method to take input of documents to index
            return indexer.IndexReleaseNotes();
        }
    }
}
