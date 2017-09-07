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
    public static class Functions
    {
        [NoAutomaticTrigger]
        public static Task ManualTrigger(TextWriter log)
        {
            // TODO: Move to config or something
            var serviceName = "sitecore-release-notes";
            var adminKey = "***REMOVED***";

            log.WriteLine("Beginning indexing of release notes");

            var searchService = new SearchServiceClient(serviceName, new SearchCredentials(adminKey));
            var crawler = new ReleaseNotesCrawler();

            var indexer = new ReleaseNotesIndexer(searchService, crawler);

            // TODO: Maybe change method to take input of documents to index
            return indexer.IndexReleaseNotes();
        }
    }
}
