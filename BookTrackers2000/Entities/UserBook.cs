using System.ComponentModel.DataAnnotations;

namespace BookTrackersApi.Entities
{
    public class UserBook
    {
        public int Id { get; set; }
        public virtual User User { get; set; } = new User();
        public virtual Book Book { get; set; } = new Book();

        [Range(0, 3000,
        ErrorMessage = "Value for {0} must be between {1} and {2}.")]
        public int TotalPagesRead { get; set; }
        public bool Finished { get; set; }
        public virtual IEnumerable<Reading> Readings { get; set; } = new List<Reading>();

    }
}
