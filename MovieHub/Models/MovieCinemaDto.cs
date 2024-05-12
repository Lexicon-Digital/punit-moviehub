using MovieHub.Entities;

namespace MovieHub.Models;

public class MovieCinemaDto
{
    public int Id { get; set; }
    public DateTime ShowTime { get; set; }
    public decimal TicketPrice { get; set; }
    public string CinemaName { get; set; } = string.Empty;
}