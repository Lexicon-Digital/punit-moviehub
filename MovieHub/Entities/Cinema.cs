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
    public int Id { get; set; }
    
    [Required]
    [MaxLength(128)]
    public string Name { get; set; } = name;
    
    [Required]
    [MaxLength]
    public string Location { get; set; } = location;

    public ICollection<MovieCinema> MovieCinemas { get; set; } = new List<MovieCinema>();
}