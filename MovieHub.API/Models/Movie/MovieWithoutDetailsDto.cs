using MovieHub.Models.MovieReview;
using Newtonsoft.Json;

namespace MovieHub.Models.Movie;

public class MovieWithoutDetailsDto
{
    public int Id { get; init; }
    public string Title { get; init; } = string.Empty;
    public DateTime ReleaseDate { get; init; }
    public string Genre { get; init; } = string.Empty;
    public int RunTime { get; init; }
    public string Synopsis { get; init; } = string.Empty;
    public string Director { get; init; } = string.Empty;
    public string Rating { get; init; } = string.Empty;
    public string PrincessTheatreMovieId { get; init; } = string.Empty;
    
    public decimal AverageScore => Utils.Average.GetAverageScore(MovieReviews);
    
    [JsonIgnore]
    public ICollection<MovieReviewDto> MovieReviews { get; init; } = new List<MovieReviewDto>();
}
