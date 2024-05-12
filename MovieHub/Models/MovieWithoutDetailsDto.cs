using MovieHub.Entities;
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
}