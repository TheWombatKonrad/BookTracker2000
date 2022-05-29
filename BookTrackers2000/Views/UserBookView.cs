using BookTrackersApi.Entities;

namespace BookTrackersApi.Views
{
    public class UserBookView
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public IEnumerable<Author> Authors { get; set; }
        public bool Finished { get; set; }
        public int TotalPagesRead { get; set; }
        public int TotalPages { get; set; }
        public IEnumerable<Reading> Readings { get; set; }
    }
}
