namespace Collectium.Model.Bean
{
    public abstract class PagedRequestBean
    {
        public int Page { get; set; }
        public int PageRow { get; set; }
    }

    public class Pagination {
        public int Skip { get; set; }
        public int Limit { get; set; }
    }
}
