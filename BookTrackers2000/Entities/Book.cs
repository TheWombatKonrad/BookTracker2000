using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace BookTrackersApi.Entities
{
    public class Book
    {
        public int Id { get; set; }

        [StringLength(50, ErrorMessage = "The {0} value cannot exceed {1} characters. ")]
        public string Title { get; set; }
        [JsonIgnore]
        public virtual IList<Author> Authors { get; set; } = new List<Author>();

        [Range(0, 3000,
        ErrorMessage = "Value for {0} must be between {1} and {2}.")]
        public int Pages { get; set; }
    }
}
