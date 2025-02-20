namespace Reservation.App.Application.RequestHelpers;

public enum SortOrder
{
    asc,
    desc,
}

public class SortParameters
{
    private string? _sortBy;

    public string? SortBy
    {
        get { return _sortBy; }
        set
        {
            if (!string.IsNullOrEmpty(value))
            {
                var parts = value.Split('_');
                for (int i = 0; i < parts.Length; i++)
                {
                    parts[i] = char.ToUpper(parts[i][0]) + parts[i].Substring(1);
                }

                _sortBy = string.Join("_", parts);
            }
            else
            {
                _sortBy = null;
            }
        }
    }
    public SortOrder? SortOrder { get; set; }
}
