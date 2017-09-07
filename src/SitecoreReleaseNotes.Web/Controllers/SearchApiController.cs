using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.Azure.Search;
using SitecoreReleaseNotes.Core.Search;

namespace SitecoreReleaseNotes.Web.Controllers
{
    [RoutePrefix("api")]
    public class SearchApiController : ApiController
    {
        // TODO: Interface
        private ReleaseNotesSearchClient SearchClient { get; }

        // TODO: Remove and use dependency injection
        public SearchApiController()
        {
            // TODO: Store settings somewhere else
            var credentials = new SearchCredentials("***REMOVED***");
            // TODO: Use constants instead
            var azureSearchClient = new SearchIndexClient("sitecore-release-notes", "release-notes", credentials);

            SearchClient = new ReleaseNotesSearchClient(azureSearchClient);
        }

        public SearchApiController(ReleaseNotesSearchClient searchClient)
        {
            SearchClient = searchClient;
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
