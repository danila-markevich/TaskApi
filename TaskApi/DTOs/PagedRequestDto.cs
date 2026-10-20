using System.ComponentModel.DataAnnotations;

namespace TaskApi.DTOs
{
    public class PagedRequestDto
    {
        [Range(1, int.MaxValue, ErrorMessage = "Page должна быть >= 1")]
        public int Page { get; set; } = 1;

        [Range(1, 100, ErrorMessage = "PageSize от 1 до 100")]
        public int PageSize { get; set; } = 10;

        public string? SortBy { get; set; }

        public bool SortDesc { get; set; } = false;

        public string? Search { get; set; }
    }
}