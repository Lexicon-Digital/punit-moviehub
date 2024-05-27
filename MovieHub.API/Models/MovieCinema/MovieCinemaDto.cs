namespace MovieHub.Models.MovieCinema;

public class MovieCinemaDto
{
    public int Id { get; init; }
    public DateTime ShowTime { get; init; }
    public decimal TicketPrice { get; init; }
    public string CinemaName { get; init; } = string.Empty;
}