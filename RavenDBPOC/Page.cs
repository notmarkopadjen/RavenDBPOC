using System.Collections.Generic;

namespace RavenDBPOC
{
    public class Page<T>
    {
        public IEnumerable<T> Entities { get; set; }

        public int Skipped { get; set; }
        public int Requested { get; set; }
        public int Total { get; set; }
    }
}
