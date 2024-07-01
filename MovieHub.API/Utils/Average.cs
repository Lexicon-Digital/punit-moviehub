using MovieHub.Models.MovieReview;

namespace MovieHub.Utils;

public static class Average
{
    public static decimal GetAverageScore(ICollection<MovieReviewDto> movieReviews)
    {
        return movieReviews.Count > 0 ? movieReviews.Average(review => review.Score) : decimal.Zero;
    }
}