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
    public int Id { get; set; }
    
    [Required]
    [MaxLength(128)]
    public string Title { get; set; } = title;

    [Required]
    [Precision(3)]
    public DateTime ReleaseDate { get; set; } = releaseDate;

    [Required]
    [MaxLength(64)]
    public string Genre { get; set; } = genre;
    
    [Required]
    public int RunTime { get; set; } = runTime;
    
    [Required]
    [MaxLength]
    public string Synopsis { get; set; } = synopsis;
    
    [Required]
    [MaxLength(64)]
    public string Director { get; set; } = director;
    
    [Required]
    [MaxLength(8)]
    public string Rating { get; set; } = rating;
    
    [Required]
    [MaxLength(16)]
    public string PrincessTheatreMovieId { get; set; } = princessTheatreMovieId;

    public ICollection<MovieCinema> MovieCinemas { get; set; } = new List<MovieCinema>();
}