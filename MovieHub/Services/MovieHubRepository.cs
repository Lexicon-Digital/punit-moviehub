using Microsoft.EntityFrameworkCore;
using MovieHub.DbContexts;
using MovieHub.Entities;

namespace MovieHub.Services;

public class MovieHubRepository(MovieHubContext context) : IMovieHubRepository
{
    private readonly MovieHubContext _context = context ?? throw new ArgumentNullException(nameof(context));

    public async Task<IEnumerable<Movie>> GetMoviesAsync()
    {
        return await _context.Movies.ToListAsync();
    }

    public async Task<IEnumerable<Movie>> GetMoviesAsync(string? title, string? genre)
    {
        if (string.IsNullOrEmpty(title) && string.IsNullOrWhiteSpace(genre))
        {
            return await GetMoviesAsync();
        }

        var collection = _context.Movies as IQueryable<Movie>;

        if (!string.IsNullOrWhiteSpace(title))
        {
            title = title.Trim();
            collection = collection.Where(movie => movie.Title.ToLower().Contains(title.ToLower()));
        }
        
        if (!string.IsNullOrWhiteSpace(genre))
        {
            genre = genre.Trim();
            collection = collection.Where(movie => movie.Genre.ToLower().Contains(genre.ToLower()));
        }

        return await collection.ToListAsync();
    }

    public async Task<Movie?> GetMovieAsync(int id, bool details)
    {
        var query = _context.Movies.Where(movie => movie.Id == id);
        
        if (details)
        {
            query = query
                .Include(movie => movie.MovieCinemas)
                .ThenInclude(movieCinema => movieCinema.Cinema);
        }
        
        return await query.FirstOrDefaultAsync();
    }
}