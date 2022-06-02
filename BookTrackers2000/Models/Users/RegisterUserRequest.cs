namespace BookTrackersApi.Models.Users;

using System.ComponentModel.DataAnnotations;

public class RegisterUserRequest
{
    [StringLength(16, ErrorMessage = "The {0} value cannot exceed {1} characters. ")]
    [Required]
    public string Username { get; set; }

    [StringLength(16, ErrorMessage = "The {0} value cannot exceed {1} characters. ")]
    [Required]
    public string Password { get; set; }
    
    [EmailAddress]
    [Required]
    public string Email { get; set; }
}