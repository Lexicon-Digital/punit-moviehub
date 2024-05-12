using MovieHub.Models;

namespace MovieHub.Utils;

public class Average
{
    public static decimal GetAverage(ICollection<MovieReviewDto> movieReviews)
    {
        return movieReviews.Count > 0 ? movieReviews.Average(review => review.Score) : decimal.Zero;
    }
}