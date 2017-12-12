using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Search;
using SitecoreReleaseNotes.Core;
using SitecoreReleaseNotes.Core.Search;

namespace SitecoreReleaseNotes.Web.Controllers
{
    [Route("api")]
    public class SearchApiController : Controller
    {
        // TODO: Interface
        private ReleaseNotesSearchClient SearchClient { get; }

        public SearchApiController(AzureSearchSettings settings)
        {
            var credentials = new SearchCredentials(settings.ApiKey);
            var azureSearchClient = new SearchIndexClient(settings.ServiceName, settings.IndexName, credentials);

            SearchClient = new ReleaseNotesSearchClient(azureSearchClient);
        }

        // TODO: Filters etc.
        [HttpGet]
        [Route("search")]
        public Task<IEnumerable<ReleaseNotesResult>> SearchReleaseNotes(string query)
        {
            return SearchClient.SearchReleaseNotesAsync(query);
        }
    }
}
