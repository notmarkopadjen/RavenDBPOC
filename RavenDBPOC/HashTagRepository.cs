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
                            .Select(x => new { Entries = x.Entries.Skip(pageRequest.Skip).Take(pageRequest.Take) })
                            .FirstOrDefault();

                // Here I get only one entry, not 100, and it's "4999999" (last one in array)
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

                /* Throws Message: 
    System.NotSupportedException : Could not understand expression: from index 'HashTagNested/ById'.Where(x => (x.Id == value(RavenDBPOC.HashTagRepository+<>c__DisplayClass2_0).pageRequest.Filter)).Select(x => x.Entries.Skip(value(RavenDBPOC.HashTagRepository+<>c__DisplayClass2_0).pageRequest.Skip).Take(value(RavenDBPOC.HashTagRepository+<>c__DisplayClass2_0).pageRequest.Take)).FirstOrDefault()
    ---- System.NotSupportedException : Cannot understand how to translate method 'Take' of 'System.Linq.Enumerable' type. x.Entries.Skip(value(RavenDBPOC.HashTagRepository+<>c__DisplayClass2_0).pageRequest.Skip).Take(value(RavenDBPOC.HashTagRepository+<>c__DisplayClass2_0).pageRequest.Take)
                 */

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
