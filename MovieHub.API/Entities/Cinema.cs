using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MovieHub.Entities;

[Table("Cinema")]
public class Cinema(
    string name, 
    string location
) {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; init; }
    
    [Required]
    [MaxLength(128)]
    public string Name { get; init; } = name;
    
    [Required]
    [MaxLength(512)]
    public string Location { get; set; } = location;

    public ICollection<MovieCinema> MovieCinemas { get; init; } = new List<MovieCinema>();
}