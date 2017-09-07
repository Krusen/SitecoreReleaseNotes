using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.Search;
using Microsoft.Azure.Search.Models;
using SitecoreReleaseNotes.Core.Extensions;
using SitecoreReleaseNotes.Core.Indexing;

namespace SitecoreReleaseNotes.Core.Search
{
    public class ReleaseNotesSearchClient
    {
        private ISearchIndexClient SearchClient { get; }

        public ReleaseNotesSearchClient(ISearchIndexClient searchClient)
        {
            SearchClient = searchClient;
        }

        // TODO: Filters etc.
        public async Task<IEnumerable<ReleaseNotesResult>>  SearchReleaseNotesAsync(string query)
        {
            var parameters = new SearchParameters
            {
                Top = 5,
                HighlightPreTag = "<highlight>",
                HighlightPostTag = "</highlight>",
                HighlightFields = new List<string> { nameof(ReleaseInfo.ReleaseNotes).ToCamelCase() }
            };

            var searchResult = await SearchClient.Documents.SearchAsync<ReleaseInfo>(query, parameters);

            // TODO: Improve
            return searchResult.Results.Select(x => new ReleaseNotesResult
            {
                Title = x.Document.Name,
                Version = x.Document.Version,
                SortableVersion = x.Document.SortableVersion,
                Revision = x.Document.Revision,
                ReleaseUrl = x.Document.ReleaseUrl,
                ReleaseNotesUrl = x.Document.ReleaseNotesUrl,
                Content = x.Highlights.Select(h => h.Value).First(),
                IssueNumbers = x.Document.IssueNumbers
            });
        }
    }

    // TODO: Move out (and refactor?)
    public class ReleaseNotesResult
    {
        public string Title { get; set; }

        public string Version { get; set; }

        public string SortableVersion { get; set; }

        public string Revision { get; set; }

        public IList<string> Content { get; set; }

        public IList<string> IssueNumbers { get; set; }

        public string ReleaseUrl { get; set; }

        public string ReleaseNotesUrl { get; set; }
    }
}
