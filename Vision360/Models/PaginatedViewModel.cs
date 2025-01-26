namespace Vision360.Models;

public class PaginatedViewModel<T>
{
    public IEnumerable<T> Data { get; set; }
    
    public int CurrentPage { get; set; }
    
    public int PageSize { get; set; }
    
    public int TotalRecords { get; set; }

    public int TotalPages => (int)Math.Ceiling((double)TotalRecords / PageSize);
    
    public string SearchQuery { get; set; } = string.Empty;
    
    public string SortColumn { get; set; } = "Name";
    
    public string SortDirection { get; set; } = "asc";
}