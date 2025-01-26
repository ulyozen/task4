using Humanizer;

namespace Vision360.Models;

public class AdminViewModel
{
    public string? Id { get; set; }
    
    public string? Name { get; set; }
    
    public string? Email { get; set; }
    
    public DateTime? LastLogin { get; set; }
    
    public bool Status { get; set; }
    
    public string? LastLoginRelative => LastLogin.HasValue 
        ? LastLogin.Value.Humanize(culture: new System.Globalization.CultureInfo("en")) 
        : "Never logged in";
}