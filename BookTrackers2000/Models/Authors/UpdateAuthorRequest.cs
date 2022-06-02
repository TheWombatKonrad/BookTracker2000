using System.ComponentModel.DataAnnotations;

namespace BookTrackersApi.Models.Authors;
public class UpdateAuthorRequest
{
    [StringLength(16, ErrorMessage = "The {0} value cannot exceed {1} characters. ")]
    public string? FirstName { get; set; }

    [StringLength(16, ErrorMessage = "The {0} value cannot exceed {1} characters. ")]
    public string? LastName { get; set; }
}