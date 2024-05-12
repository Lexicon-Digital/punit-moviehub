using MovieHub.Entities;

namespace MovieHub.Services;

public interface IMovieHubRepository
{
    Task<IEnumerable<Movie>> GetMoviesAsync();
    Task<IEnumerable<Movie>> GetMoviesAsync(string? title, string? genre);
    Task<Movie?> GetMovieAsync(int id, bool details);
    Task<MovieReview?> GetReviewAsync(int id);
    Task<IEnumerable<MovieReview>> GetReviewsByMovieAsync(int movieId);
    Task CreateReviewForMovieAsync(int movieId, MovieReview movieReview);
    void DeleteReviewAsync(MovieReview movieReview);
    void DeleteReviewsByMovieAsync(IEnumerable<MovieReview> movieReviews);
    Task<bool> MovieExistsAsync(int id);
    Task<bool> SaveChangesAsync();

}