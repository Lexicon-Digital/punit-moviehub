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
    public decimal Score { get; init; }

    [MaxLength(512)]
    public string? Comment { get; init; }

    [Required]
    [Precision(3)]
    public DateTime ReviewDate { get; init; } = DateTime.Now;
    
    [ForeignKey("MovieId")]
    public Movie? Movie { get; init; }

    public int MovieId { get; init; }
}