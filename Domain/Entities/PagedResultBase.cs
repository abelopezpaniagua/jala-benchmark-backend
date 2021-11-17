namespace Domain.Entities
{
    public abstract class PagedResultBase
    {
        public int RowCount { get; set; }
        public int PageCount { get; set; }
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
    }
}
