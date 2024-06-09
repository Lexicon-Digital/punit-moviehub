using MovieHub.Entities;
using MovieHub.Models.Movie;

namespace MovieHub.Services;

public interface IMovieHubRepository
{
    Task<(IEnumerable<Movie>, PaginationMetadata)> GetMoviesAsync(
        string? title, 
        string? genre, 
        int pageNumber,
        int pageSize
    );
    Task<Movie?> GetMovieAsync(int id, bool details);
    Task<MovieReview?> GetReviewAsync(int id);
    Task<IEnumerable<MovieReview>> GetReviewsByMovieAsync(int movieId);
    Task CreateReviewForMovieAsync(int movieId, MovieReview movieReview);
    void DeleteReviewAsync(MovieReview movieReview);
    void DeleteReviewsByMovieAsync(IEnumerable<MovieReview> movieReviews);
    Task<bool> MovieExistsAsync(int id);
    Task<Dictionary<string, object>?> RunQuery(string query);
    Task<bool> SaveChangesAsync();
}