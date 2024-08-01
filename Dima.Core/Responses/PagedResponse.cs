using System.Text.Json.Serialization;

namespace Dima.Core.Responses
{
    public class PagedResponse<T> : Response<T>
    {
        [JsonConstructor]
        public PagedResponse(T? data, int totalCount, int currentPage = Configurations.DefaultPageNumber, int pageSize = Configurations.DefaultPageSize) : base(data)
        {
            Data = data;
            TotalCount = totalCount;
            CurrentPage = currentPage;
            PageSize = pageSize;
        }

        public PagedResponse(T? data, int code = Configurations.DefaultStatusCode, string? message = null) : base(data, code, message)
        {

        }

        public int CurrentPage { get; set; }
        public int TotalPages => (int)Math.Ceiling(TotalCount / (double)PageSize);
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
    }
}
