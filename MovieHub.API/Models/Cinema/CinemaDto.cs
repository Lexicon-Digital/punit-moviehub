namespace MovieHub.Models.Cinema;
using Entities;

public class CinemaDto
{
    public int Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string Location { get; init; } = string.Empty;

    public ICollection<MovieCinema> MovieCinemas { get; init; } = new List<MovieCinema>();
}