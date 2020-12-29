using System;
using System.Collections.Generic;
using System.Text;

namespace RavenDBPOC
{
    public class HashTagNested
    {
        public string Id { get; set; } = Guid.NewGuid().ToString("N");
        public string Name { get; set; }
        public bool Active { get; set; } = true;
        public EntryNested[] Entries { get; set; }
    }
}
