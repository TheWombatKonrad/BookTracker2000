namespace BookTrackersApi.Models.Readings;

using System.ComponentModel.DataAnnotations;

public class RegisterReadingRequest
{
    [Required]
    public int BookId { get; set; }

    [Range(0, 3000,
        ErrorMessage = "Value for {0} must be between {1} and {2}.")]
    [Required]
    public int PagesRead { get; set; }
}