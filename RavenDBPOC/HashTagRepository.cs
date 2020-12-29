using Raven.Client.Documents.Session;
using System.Linq;

namespace RavenDBPOC
{
    public class HashTagRepository
    {
        public static void CreateNested(HashTagNested list)
        {
            DocumentStoreHolder.RunAndSave(session => session.Store(list, list.Id));
        }

        public static Page<EntryNested> GetNestedEntriesPage(PageRequest<string> pageRequest) =>
            DocumentStoreHolder.Run(session =>
            {
                var allEntities = session
                            .Query<HashTagNested, HashTagNested_ById>()
                            .Statistics(out QueryStatistics stats)
                            .Where(x => x.Id == pageRequest.Filter)
                            .Select(x => new { Entries = x.Entries.Skip(pageRequest.Skip).Take(pageRequest.Take).ToList() })
                            .FirstOrDefault();
                return new Page<EntryNested>
                {
                    Entities = allEntities
                            .Entries
                            //.Skip(pageRequest.Skip)
                            //.Take(pageRequest.Take)
                            ,
                    Requested = pageRequest.Take,
                    Skipped = pageRequest.Skip,
                    Total = stats.TotalResults
                };
            });

        public static Page<EntryNested> GetNestedEntriesPage2(PageRequest<string> pageRequest) =>
            DocumentStoreHolder.Run(session =>
            {
                var allEntities = session
                            .Query<HashTagNested, HashTagNested_ById>()
                            .Statistics(out QueryStatistics stats)
                            .Where(x => x.Id == pageRequest.Filter)
                            .Select(x => x.Entries.Skip(pageRequest.Skip).Take(pageRequest.Take))
                            .FirstOrDefault();
                return new Page<EntryNested>
                {
                    Entities = allEntities
                            //.Skip(pageRequest.Skip)
                            //.Take(pageRequest.Take)
                            ,
                    Requested = pageRequest.Take,
                    Skipped = pageRequest.Skip,
                    Total = stats.TotalResults
                };
            });
    }
}
