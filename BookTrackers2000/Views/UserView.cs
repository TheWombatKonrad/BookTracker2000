namespace BookTrackersApi.Views
{
    public class UserView
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public IEnumerable<UserBookView> BookList { get; set; }
    }
}
