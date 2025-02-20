namespace Reservation.App.Application.RequestHelpers;

public class SearchParameters
{
    // Nullable to allow "query=" in url
    public string? QueryProperty { get; set; }
    public string? Query { get; set; }
}
