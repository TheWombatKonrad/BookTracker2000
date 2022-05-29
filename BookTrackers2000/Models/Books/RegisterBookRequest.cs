namespace BookTrackersApi.Models.Books;

using System.ComponentModel.DataAnnotations;

public class RegisterBookRequest
{
    [Required]
    public string Title { get; set; }

    [Required]
    public int AuthorId { get; set; }

    [Required]
    public int Pages { get; set; }
}