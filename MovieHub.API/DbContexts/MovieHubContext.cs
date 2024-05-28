using Microsoft.EntityFrameworkCore;
using MovieHub.Entities;

namespace MovieHub.DbContexts;

public class MovieHubContext(DbContextOptions<MovieHubContext> options) : DbContext(options)
{
    public DbSet<Movie> Movies { get; init; }
    public DbSet<Cinema> Cinemas { get; init; }
    public DbSet<MovieCinema> MovieCinemas { get; init; }
    public DbSet<MovieReview> MovieReviews { get; init; }
}