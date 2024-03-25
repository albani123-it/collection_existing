using Collectium.Model.Bean;

namespace Collectium.Model.Helper
{
    public class PaginationHelper
    {
        public Pagination getPagination(PagedRequestBean bean)
        {
            var ret = new Pagination();
            ret.Skip = 0;
            ret.Limit = 30;

            if (bean.PageRow > 0)
            {
                ret.Limit = bean.PageRow;
            }

            if (bean.Page > 1)
            {
                var page = bean.Page;
                if (page < 2)
                {
                    page = 0;
                } else
                {
                    page = ((bean.Page - 1) * ret.Limit);
                }
                ret.Skip = page;
            }

            return ret;
        }

        public int getMaxPage(PagedRequestBean bean, int number)
        {
            var limit = 30;

            if (bean.PageRow > 0)
            {
                limit = bean.PageRow;
            }

            var maxPage = number / limit;
            if ((number % limit) > 0)
            {
                maxPage++;
            }


            return maxPage;
        }
    }
}
