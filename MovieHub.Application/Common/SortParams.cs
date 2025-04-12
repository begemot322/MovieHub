namespace MovieHub.Application.Common;

public class SortParams
{
    public string? OrderBy { get; set; }
    public SortDirection? SortDirection { get; set; }
}

public enum SortDirection
{
    Ascending, 
    Descending
}

