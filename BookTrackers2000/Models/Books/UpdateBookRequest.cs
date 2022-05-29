namespace BookTrackersApi.Models.Books;
public class UpdateBookRequest
{
    public string Title { get; set; }
    public int AuthorId { get; set; }
    public int Pages { get; set; }
}