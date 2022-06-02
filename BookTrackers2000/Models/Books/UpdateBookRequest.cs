using System.ComponentModel.DataAnnotations;

namespace BookTrackersApi.Models.Books;
public class UpdateBookRequest
{
    [StringLength(100, ErrorMessage = "The {0} value cannot exceed {1} characters. ")]
    public string? Title { get; set; }
    public int AuthorId { get; set; }

    [Range(0, 3000,
        ErrorMessage = "Value for {0} must be between {1} and {2}.")]
    public int Pages { get; set; }
}