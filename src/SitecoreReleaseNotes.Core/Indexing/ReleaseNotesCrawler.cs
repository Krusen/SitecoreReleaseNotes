using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using HtmlAgilityPack;
using SitecoreReleaseNotes.Core.Extensions;
using SitecoreReleaseNotes.Core.Net;

namespace SitecoreReleaseNotes.Core.Indexing
{
    public class ReleaseNotesCrawler : IReleaseNotesCrawler
    {
        private const string ReleaseNotesRootPath = "https://dev.sitecore.net";
        private const string DownloadsListPageUrl = "https://dev.sitecore.net/Downloads/Sitecore_Experience_Platform.aspx";

        private IHttpClient Client { get; }

        public ReleaseNotesCrawler() : this(new HttpClientWrapper())
        {

        }

        public ReleaseNotesCrawler(IHttpClient client)
        {
            Client = client;
        }

        // TODO: Rename some of these methods
        public async Task<IList<ReleaseInfo>> GetReleaseAsync()
        {
            var releases = await GetReleaseInfosAsync();

            var tasks = releases.Select(FillReleaseNotesInfoAsync);
            await Task.WhenAll(tasks);

            return releases;
        }

        public async Task<IList<ReleaseInfo>> GetReleaseInfosAsync()
        {
            const string versionRegex = @"(\d\.\d).*";
            const string revisionRegex = @"rev\.?\s*?(\d{6})";

            var html = await Client.GetHtmlAsync(DownloadsListPageUrl);
            var downloadElements = html.QuerySelectorAll(".download-item > .download-item");

            var releases = new List<ReleaseInfo>();
            foreach (var element in downloadElements)
            {
                var anchor = element.QuerySelector("a");
                var url = anchor.Attributes["href"].Value;
                var name = anchor.InnerText;
                var description = element.QuerySelector("div").InnerText;

                // TODO: Parse version as Major, Minor, Revision and update number
                var version = Regex.Match(name, versionRegex).Value;
                var versionNumber = Regex.Match(name, versionRegex).Groups[1].Value;
                var revision  = Regex.Match(description, revisionRegex, RegexOptions.IgnoreCase).Groups[1].Value;
                var sortableVersion = $"{versionNumber}.{revision}";

                name = name.RemoveIgnoreCase("Initial Release").Trim();
                version = version.RemoveIgnoreCase("Initial Release").Trim();

                var id = Convert.ToBase64String(Encoding.UTF8.GetBytes(sortableVersion));

                var release = new ReleaseInfo
                {
                    Id = id,
                    ReleaseUrl = url,
                    Name = name,
                    Version = version,
                    Revision = revision,
                    SortableVersion = sortableVersion
                };

                releases.Add(release);
            }
            return releases;
        }

        public async Task FillReleaseNotesInfoAsync(ReleaseInfo release)
        {
            var url = ReleaseNotesRootPath + release.ReleaseUrl;
            var html = await Client.GetHtmlAsync(url);
            var releaseNotesUrl = GetReleaseNotesUrl(html);

            html = await Client.GetHtmlAsync(releaseNotesUrl);

            var releaseNotes = html.QuerySelector(".topic-content").InnerText;
            var issueNumbers = GetIssueNumbers(releaseNotes).SortAsIntegers().ToList();

            // Add '.' after issue numbers (in tables)
            // Workaround for Azure Search Highlighting returning long snippets
            releaseNotes = Regex.Replace(releaseNotes, @"(?<!rev\.\s|\d)\d{5,6}", "$0.");

            release.ReleaseNotesUrl = releaseNotesUrl;
            release.ReleaseNotes = releaseNotes;
            release.IssueNumbers = issueNumbers;
        }

        public static string GetReleaseNotesUrl(HtmlDocument html)
        {
            return html.QuerySelectorAll(".downloads-table a")
                    .First(x => string.Equals(x.InnerText, "Release Notes", StringComparison.OrdinalIgnoreCase))
                    .Attributes["href"].Value;
        }

        public static IList<string> GetIssueNumbers(string content)
        {
            const string issueNumberRegex = @"(?<!rev\.\s|\d)\d{5,6}";

            return
                Regex.Matches(content, issueNumberRegex, RegexOptions.IgnoreCase)
                    .Select(x => x.Value)
                    .ToList();
        }
    }
}
