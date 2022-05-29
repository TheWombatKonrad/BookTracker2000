namespace BookTrackersApi.Models.Users;

using System.ComponentModel.DataAnnotations;

public class RegisterUserRequest
{
    [Required]
    public string Username { get; set; }

    [Required]
    public string Password { get; set; }
    public string? Email { get; set; }
}