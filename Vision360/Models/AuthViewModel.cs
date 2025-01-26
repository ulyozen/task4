using System.ComponentModel.DataAnnotations;

namespace Vision360.Models;

public class AuthViewModel
{
    public bool RememberMe { get; set; }
    
    public bool IsRegistration { get; set; }
    
    public string? Name { get; set; }
    
    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress(ErrorMessage = "Invalid email format.")]
    public string? Email { get; set; }
    
    [Required(ErrorMessage = "Password is required.")]
    [StringLength(100, ErrorMessage = "Password must be one character long", MinimumLength = 1)]
    public string? Password { get; set; }
    
    
}