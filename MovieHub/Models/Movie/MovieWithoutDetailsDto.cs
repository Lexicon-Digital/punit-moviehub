using Newtonsoft.Json;

namespace MovieHub.Models;

public class MovieWithoutDetailsDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public DateTime ReleaseDate { get; set; }
    public string Genre { get; set; } = string.Empty;
    public int RunTime { get; set; }
    public string Synopsis { get; set; } = string.Empty;
    public string Director { get; set; } = string.Empty;
    public string Rating { get; set; } = string.Empty;
    public string PrincessTheatreMovieId { get; set; } = string.Empty;
    
    public decimal AverageScore => Utils.Average.GetAverage(MovieReviews);
    
    [JsonIgnore]
    public ICollection<MovieReviewDto> MovieReviews { get; set; } = new List<MovieReviewDto>();
}
