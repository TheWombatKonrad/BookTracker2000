using BookTrackersApi.Entities;

namespace BookTrackersApi.Views
{
    public class BookView
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int Pages { get; set; }
        public IEnumerable<Author> Authors { get; set; }
    }
}
