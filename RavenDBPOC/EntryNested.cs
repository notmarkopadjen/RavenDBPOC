using System;

namespace RavenDBPOC
{
    public class EntryNested
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public EntryAttribute[] Attributes { get; set; }
    }
}
