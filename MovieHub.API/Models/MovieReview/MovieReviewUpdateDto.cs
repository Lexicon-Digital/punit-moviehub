using System.ComponentModel.DataAnnotations;

namespace MovieHub.Models;

public class MovieReviewUpdateDto
{
    [Range(1.0, 10.0)]
    public decimal Score { get; set; }
    
    public string Comment { get; set; } = string.Empty;
}