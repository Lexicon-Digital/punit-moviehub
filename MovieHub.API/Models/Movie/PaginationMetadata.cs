namespace MovieHub.Models.Movie;

public class PaginationMetadata(int totalItemCount, int pageSize)
{
    public int TotalItemCount { get; } = totalItemCount;
    public int TotalPageCount { get; } = (int) Math.Ceiling(totalItemCount / (double) pageSize);
}