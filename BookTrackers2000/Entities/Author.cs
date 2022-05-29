using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace BookTrackersApi.Entities
{
    public class Author
    {
        public int Id { get; set; }

        [StringLength(16, ErrorMessage = "The {0} value cannot exceed {1} characters. ")]
        public string FirstName { get; set; }

        [StringLength(16, ErrorMessage = "The {0} value cannot exceed {1} characters. ")]
        public string LastName { get; set; }
        [JsonIgnore]
        public virtual IEnumerable<Book> Books { get; set; } = new List<Book>();
    }
}
