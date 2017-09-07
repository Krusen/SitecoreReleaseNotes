using System.Collections.Generic;
using System.Threading.Tasks;

namespace SitecoreReleaseNotes.Core.Indexing
{
    public interface IReleaseNotesCrawler
    {
        Task<IList<ReleaseInfo>> GetReleaseAsync();
    }
}