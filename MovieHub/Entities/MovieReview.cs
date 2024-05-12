using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MovieHub.Entities;

[Table("MovieReview")]
public class MovieReview
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required]
    [Precision(4, 2)]
    public decimal Score { get; set; }

    [MaxLength] 
    public string? Comment { get; set; }

    [Required]
    [Precision(3)]
    public DateTime ReviewDate { get; set; } = DateTime.Now;
    
    [ForeignKey("MovieId")]
    public Movie? Movie { get; set; }

    public int MovieId { get; set; }
}