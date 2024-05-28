using System.ComponentModel.DataAnnotations;

namespace MovieHub.Models.MovieReview;

public class MovieReviewCreationDto
{
    [Range(1.0, 10.0)]
    public decimal Score { get; set; }

    [Required(ErrorMessage = "A review comment is required")]
    public string Comment { get; set; } = string.Empty;
}