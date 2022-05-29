namespace BookTrackersApi.Models.Readings;

using System.ComponentModel.DataAnnotations;

public class RegisterReadingRequest
{
    [Required]
    public int BookId { get; set; }

    [Required]
    public int PagesRead { get; set; }
}