namespace BookTrackersApi.Models.Authors;

using System.ComponentModel.DataAnnotations;

public class RegisterAuthorRequest
{
    [Required]
    [StringLength(16, ErrorMessage = "The {0} value cannot exceed {1} characters. ")]
    public string FirstName { get; set; }

    [Required]
    [StringLength(16, ErrorMessage = "The {0} value cannot exceed {1} characters. ")]
    public string LastName { get; set; }
}