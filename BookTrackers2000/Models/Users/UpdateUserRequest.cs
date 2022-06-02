using System.ComponentModel.DataAnnotations;

namespace BookTrackersApi.Models.Users;

public class UpdateUserRequest
{
    public string? Username { get; set; }
    public string? Password { get; set; }
    [EmailAddress]
    public string? Email { get; set; }
}