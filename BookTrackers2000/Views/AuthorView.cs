using BookTrackersApi.Entities;

namespace BookTrackersApi.Views
{
    public class AuthorView
    {
        public int AuthorId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public IEnumerable<Book> BookList { get; set; }
    }
}
