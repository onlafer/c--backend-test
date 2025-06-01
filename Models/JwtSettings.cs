using System.ComponentModel.DataAnnotations;

namespace AuthDemo.Models;

public class JwtSettings
{
    [Required]
    public string SecretKey { get; set; } = string.Empty;
    
    [Required]
    public string Issuer { get; set; } = string.Empty;
    
    [Required]
    public string Audience { get; set; } = string.Empty;
    
    [Required]
    public int ExpirationInMinutes { get; set; }
}
