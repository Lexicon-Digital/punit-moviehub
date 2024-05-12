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
    public int Id { get; set; }

    [Required]
    [Precision(3)]
    public DateTime ShowTime { get; set; } = showTime;
    
    [Required]
    [Precision(4,2)]
    public decimal TicketPrice { get; set; } = ticketPrice;
    
    [ForeignKey("MovieId")]
    public Movie? Movie { get; set; }

    public int MovieId { get; set; } = movieId;
    
    [ForeignKey("CinemaId")]
    public Cinema? Cinema { get; set; }

    public int CinemaId { get; set; } = cinemaId;
}
