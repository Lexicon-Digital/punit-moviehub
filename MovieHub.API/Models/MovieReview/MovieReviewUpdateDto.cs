using System.ComponentModel.DataAnnotations;

namespace MovieHub.Models.MovieReview;

public class MovieReviewUpdateDto
{
    [Range(1.0, 10.0)]
    public decimal Score { get; init; }
    
    public string Comment { get; init; } = string.Empty;
}