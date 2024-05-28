namespace MovieHub.Models.MovieReview;

public class MovieReviewDto
{
    public int Id { get; set; }
    public decimal Score { get; set; }
    public string? Comment { get; set; }
    public DateTime ReviewDate { get; set; }
    public int MovieId { get; set; }
}