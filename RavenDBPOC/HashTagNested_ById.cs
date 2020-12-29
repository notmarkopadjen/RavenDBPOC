using Raven.Client.Documents.Indexes;
using System.Linq;

namespace RavenDBPOC
{
    public class HashTagNested_ById : AbstractIndexCreationTask<HashTagNested>
    {
        public HashTagNested_ById()
        {
            Map = hashtags => from hashtag in hashtags
                               select new
                               {
                                   hashtag.Id
                               };
        }
    }
}
