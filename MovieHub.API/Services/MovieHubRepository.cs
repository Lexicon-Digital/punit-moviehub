using Microsoft.EntityFrameworkCore;
using MovieHub.DbContexts;
using MovieHub.Entities;
using MovieHub.Models.Movie;

namespace MovieHub.Services;

public class MovieHubRepository(MovieHubContext context) : IMovieHubRepository
{
    private readonly MovieHubContext _context = context ?? throw new ArgumentNullException(nameof(context));

    public async Task<(IEnumerable<Movie>, PaginationMetadata)> GetMoviesAsync(
        string? title, 
        string? genre,
        int pageNumber, 
        int pageSize
    ) {
        var collection = _context.Movies.Include(movie => movie.MovieReviews) as IQueryable<Movie>;

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

        var totalItemCount = await collection.CountAsync();
        
        var paginationMetadata = new PaginationMetadata(totalItemCount, pageSize);

        var movies = await collection
            .Skip(pageSize * (pageNumber - 1))
            .Take(pageSize)
            .ToListAsync();
        
        return (movies, paginationMetadata);
    }

    public async Task<Movie?> GetMovieAsync(int id, bool details)
    {
        var query = _context.Movies
            .Include(movie => movie.MovieReviews)
            .Where(movie => movie.Id == id);
        
        if (details)
        {
            query = query
                .Include(movie => movie.MovieCinemas)
                .ThenInclude(movieCinema => movieCinema.Cinema);
        }
        
        return await query.FirstOrDefaultAsync();
    }

    public async Task<MovieReview?> GetReviewAsync(int id)
    {
        return await _context.MovieReviews
            .Where(movieReview => movieReview.Id == id)
            .FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<MovieReview>> GetReviewsByMovieAsync(int movieId)
    {
        return await _context.MovieReviews
            .Where(movieReview => movieReview.MovieId == movieId)
            .ToListAsync();
    }

    public async Task CreateReviewForMovieAsync(int movieId, MovieReview movieReview)
    {
        var movie = await GetMovieAsync(movieId, false);
        movie?.MovieReviews.Add(movieReview);
    }

    public void DeleteReviewAsync(MovieReview movieReview)
    {
        _context.MovieReviews.Remove(movieReview);
    }

    public void DeleteReviewsByMovieAsync(IEnumerable<MovieReview> movieReviews)
    {
        _context.MovieReviews.RemoveRange(movieReviews);
    }

    public async Task<bool> MovieExistsAsync(int id)
    {
        return await _context.Movies.AnyAsync(movie => movie.Id == id);
    }

    public async Task<Dictionary<string, object>?> RunQuery(string query)
    {
        var command = _context.Database.GetDbConnection().CreateCommand();
        command.CommandText = query;
        await _context.Database.OpenConnectionAsync();

        await using var result = await command.ExecuteReaderAsync();
        
        var entities = new List<Dictionary<string, object?>>();
        while (await result.ReadAsync())
        {
            var entity = new Dictionary<string, object?>();
            for (var i = 0; i < result.FieldCount; i++)
            {
                entity.Add(result.GetName(i), result.IsDBNull(i) ? null : result.GetValue(i));
            }
            entities.Add(entity);
        }

        var finalResult = new Dictionary<string, object>
        {
            { "_query", query },
            { "data", entities }
        };

        return finalResult;
    }

    public async Task<bool> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync() >= 0;
    }
}