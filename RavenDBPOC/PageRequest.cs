namespace RavenDBPOC
{
    public class PageRequest<T>
    {
        public T Filter { get; set; }

        public int Skip { get; set; }
        public int Take { get; set; }
    }
}
