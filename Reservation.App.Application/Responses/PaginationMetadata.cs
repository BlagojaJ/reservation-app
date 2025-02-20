using Reservation.App.Application.RequestHelpers;

namespace Reservation.App.Application.Responses;

public class PaginationMetadata(PaginationParameters paginationParameters, int totalItems)
{
    public int PageNumber { get; set; } = paginationParameters.PageNumber;
    public int PageSize { get; set; } = paginationParameters.PageSize;
    public int TotalPages { get; set; } =
        (int)Math.Ceiling(totalItems / (double)paginationParameters.PageSize);
    public int TotalItems { get; set; } = totalItems;
}
