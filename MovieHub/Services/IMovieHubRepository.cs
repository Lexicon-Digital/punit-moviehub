using MovieHub.Entities;

namespace MovieHub.Services;

public interface IMovieHubRepository
{
    Task<IEnumerable<Movie>> GetMoviesAsync();
    Task<IEnumerable<Movie>> GetMoviesAsync(string? title, string? genre);
    Task<Movie?> GetMovieAsync(int id, bool details);

}