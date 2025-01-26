using System.ComponentModel.DataAnnotations;

namespace Vision360.DTOs;

public class UserQueryDto
{
    [Range(1, int.MaxValue, ErrorMessage = "Page must be greater than 0.")]
    public int Page { get; set; } = 1;
    
    [Range(1, 100, ErrorMessage = "Size must be between 1 and 100.")]
    public int Size { get; set; } = 10;
    
    [Required]
    [MaxLength(10)]
    public string SortColumn { get; set; } = "Name";
    
    [RegularExpression("asc|desc", ErrorMessage = "SortDirection must be 'asc' or 'desc'.")]
    public string SortDirection { get; set; } = "asc";
    
    public string Search { get; set; } = string.Empty;
}