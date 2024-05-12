using MovieHub.Entities;
namespace MovieHub.Models;

public class CinemaDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;

    public ICollection<MovieCinema> MovieCinemas { get; set; } = new List<MovieCinema>();
}