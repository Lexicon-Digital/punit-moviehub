using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MovieHub.Entities;

[Table("MovieCinema")]
public class MovieCinema(
    DateTime showTime,
    decimal ticketPrice,
    int movieId,
    int cinemaId
) {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; init; }

    [Required]
    [Precision(3)]
    public DateTime ShowTime { get; init; } = showTime;
    
    [Required]
    [Precision(4,2)]
    public decimal TicketPrice { get; init; } = ticketPrice;
    
    [ForeignKey("MovieId")]
    public Movie? Movie { get; init; }

    public int MovieId { get; init; } = movieId;
    
    [ForeignKey("CinemaId")]
    public Cinema? Cinema { get; init; }

    public int CinemaId { get; init; } = cinemaId;
}
