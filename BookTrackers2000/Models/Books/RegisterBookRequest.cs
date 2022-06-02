namespace BookTrackersApi.Models.Books;

using System.ComponentModel.DataAnnotations;

public class RegisterBookRequest
{
    [Required]
    [StringLength(100, ErrorMessage = "The {0} value cannot exceed {1} characters. ")]
    public string Title { get; set; }

    [Required]
    public int AuthorId { get; set; }
    
    [Required]
    [Range(0, 3000,
        ErrorMessage = "Value for {0} must be between {1} and {2}.")]
    public int Pages { get; set; }
}