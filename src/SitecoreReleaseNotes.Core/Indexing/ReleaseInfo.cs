using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.Azure.Search;
using Microsoft.Azure.Search.Models;

namespace SitecoreReleaseNotes.Core.Indexing
{
    [SerializePropertyNamesAsCamelCase]
    public class ReleaseInfo
    {
        [Key]
        public string Id { get; set; }

        public string Version { get; set; }

        [IsFilterable, IsSortable]
        public string SortableVersion { get; set; }

        [IsFilterable, IsSortable]
        public string Revision { get; set; }

        public string Name { get; set; }

        [IsSearchable]
        [Analyzer(AnalyzerName.AsString.EnMicrosoft)]
        public string ReleaseNotes { get; set; }

        [IsSearchable]
        public IList<string> IssueNumbers { get; set; }

        public string ReleaseUrl { get; set; }

        public string ReleaseNotesUrl { get; set; }
    }
}
