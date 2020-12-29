using System;
using System.Collections.Concurrent;
using System.Linq;
using Xunit;

namespace RavenDBPOC
{
    public class HashTagRepositoryTests
    {
        private ConcurrentBag<string> UsedHashTagNestedIds = new ConcurrentBag<string>();

        const int batchSize = 5_000_000;
        const int pageSize = 100;

        [Fact]
        public void Create_Should_Create_Proper_Nested_Entries()
        {
            var list = new HashTagNested
            {
                Name = "Test_" + DateTime.UtcNow.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss.fffffffK"),
                Entries = Enumerable.Range(0, batchSize).Select(i => new EntryNested
                {
                    Attributes = new[]
                     {
                         new EntryAttribute
                         {
                              Name = "Attribute 1",
                              Value = i.ToString()
                         }
                     }
                }).ToArray()
            };

            UsedHashTagNestedIds.Add(list.Id);

            HashTagRepository.CreateNested(list);
        }

        [Fact]
        public void Paging_Should_Load_Proper_Nested_Entites()
        {
            const string hashTagId = "96f9722aa3ff42cfbe0cad3a2dcaa9e5";
            const int hashTagSize = 5_000_000;

            var firstPage = HashTagRepository.GetNestedEntriesPage(new PageRequest<string>
            {
                Filter = hashTagId,
                Skip = 0,
                Take = pageSize
            });
            Assert.Equal(pageSize, firstPage.Entities.Count());
            Assert.Equal(hashTagSize, firstPage.Total);
            Assert.Equal(0.ToString(), firstPage.Entities.First().Attributes.First().Value);

            var hundredthPage = HashTagRepository.GetNestedEntriesPage(new PageRequest<string>
            {
                Filter = hashTagId,
                Skip = 99 * pageSize,
                Take = pageSize
            });
            Assert.Equal(pageSize, hundredthPage.Entities.Count());
            Assert.Equal(hashTagSize, hundredthPage.Total);
            Assert.Equal(9900.ToString(), hundredthPage.Entities.First().Attributes.First().Value);
        }
    }
}
