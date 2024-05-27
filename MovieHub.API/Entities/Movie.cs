using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MovieHub.Entities;

[Table("Movie")]
public class Movie(
    string title,
    DateTime releaseDate, 
    string genre, 
    int runTime, 
    string synopsis, 
    string director,
    string rating,
    string princessTheatreMovieId
) {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; init; }
    
    [Required]
    [MaxLength(128)]
    public string Title { get; init; } = title;

    [Required]
    [Precision(3)]
    public DateTime ReleaseDate { get; init; } = releaseDate;

    [Required]
    [MaxLength(64)]
    public string Genre { get; init; } = genre;
    
    [Required]
    public int RunTime { get; init; } = runTime;
    
    [Required]
    [MaxLength(1024)]
    public string Synopsis { get; init; } = synopsis;
    
    [Required]
    [MaxLength(64)]
    public string Director { get; init; } = director;
    
    [Required]
    [MaxLength(8)]
    public string Rating { get; init; } = rating;
    
    [Required]
    [MaxLength(16)]
    public string PrincessTheatreMovieId { get; init; } = princessTheatreMovieId;

    public ICollection<MovieCinema> MovieCinemas { get; init; } = new List<MovieCinema>();
    
    public ICollection<MovieReview> MovieReviews { get; init; } = new List<MovieReview>();
}