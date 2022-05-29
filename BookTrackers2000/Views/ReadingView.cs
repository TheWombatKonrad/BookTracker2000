
namespace BookTrackersApi.Views
{
    public class ReadingView
    {
        public int ReadingId { get; set; }
        public int UserId { get; set; }
        public int PagesRead { get; set; }
        public UserBookView Book { get; set; }

    }
}
