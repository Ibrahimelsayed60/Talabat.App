using Talabat.APIs.DTOs;

namespace Talabat.APIs.Helpers
{
    public class Pagination<T>
    {

        public Pagination(int pageIndex, int pageSIze, IReadOnlyList<T> data)
        {
            PageIndex = pageIndex;
            PageSize = pageSIze;
            Data = data;
        }

        public int PageSize { get; set; }

        public int PageIndex { get; set; }

        public int Count { get; set; }

        public IReadOnlyList<T> Data { get; set; }

    }
}
