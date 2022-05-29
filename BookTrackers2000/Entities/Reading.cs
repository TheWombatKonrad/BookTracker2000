using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace BookTrackersApi.Entities
{
    public class Reading
    {
        public int Id { get; set; }

        [Range(0, 3000,
        ErrorMessage = "Value for {0} must be between {1} and {2}.")]
        public int PagesRead { get; set; }

        [JsonIgnore]
        public virtual UserBook UserBook { get; set; } = new UserBook();
    }
}
