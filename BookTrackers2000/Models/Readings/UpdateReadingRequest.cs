using System.ComponentModel.DataAnnotations;

namespace BookTrackersApi.Models.Readings;
public class UpdateReadingRequest
{
    [Range(0, 3000,
        ErrorMessage = "Value for {0} must be between {1} and {2}.")]
    public int PagesRead { get; set; }
}