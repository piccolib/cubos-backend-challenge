namespace CubosFinance.Application.DTOs.Common;

public class PagedResponseDto<T>
{
    public IEnumerable<T> Data { get; set; } = Enumerable.Empty<T>();

    public PaginationDto Pagination { get; set; } = new();
}
