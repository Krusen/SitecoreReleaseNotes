using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Azure.Search;
using Microsoft.Azure.Search.Models;
using SitecoreReleaseNotes.Core.Extensions;

namespace SitecoreReleaseNotes.Core.Indexing
{
    public class ReleaseNotesIndexer
    {
        public const string IndexName = "release-notes";

        public ISearchServiceClient SearchService { get; }

        public IReleaseNotesCrawler Crawler { get; }

        public ReleaseNotesIndexer(ISearchServiceClient searchService, IReleaseNotesCrawler crawler)
        {
            SearchService = searchService;
            Crawler = crawler;
        }

        // TODO: Maybe rename methods
        public async Task IndexReleaseNotes()
        {
            await EnsureIndexExists();

            var releases = await Crawler.GetReleaseAsync();

            var client = SearchService.Indexes.GetClient(IndexName);
            var batch = IndexBatch.Upload(releases);
            await client.Documents.IndexAsync(batch);
        }

        public Task EnsureIndexExists()
        {
            var index = new Index
            {
                Name = IndexName,
                Fields = FieldBuilder.BuildForType<ReleaseInfo>(),
                //Suggesters =
                //{
                //    // TODO: Suggester names as constants for easy reference
                //    new Suggester("issue number", SuggesterSearchMode.AnalyzingInfixMatching,
                //        nameof(ReleaseInfo.IssueNumbers).ToCamelCase()),
                //    new Suggester("release notes", SuggesterSearchMode.AnalyzingInfixMatching,
                //        nameof(ReleaseInfo.ReleaseNotes).ToCamelCase())
                //},
                //Analyzers =
                //{
                //    // TODO: Analyzer name as constant
                //    new CustomAnalyzer(
                //        "lowercase_keyword",
                //        TokenizerName.Keyword,
                //        new List<TokenFilterName> {TokenFilterName.Lowercase})
                //}
            };

            return SearchService.Indexes.CreateOrUpdateAsync(index);
        }
    }
}
