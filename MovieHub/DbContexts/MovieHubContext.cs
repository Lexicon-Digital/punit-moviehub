using Microsoft.EntityFrameworkCore;
using MovieHub.Entities;

namespace MovieHub.DbContexts;

public class MovieHubContext(DbContextOptions<MovieHubContext> options) : DbContext(options)
{
    public DbSet<Movie> Movies { get; set; }
    public DbSet<Cinema> Cinemas { get; set; }
    public DbSet<MovieCinema> MovieCinemas { get; set; }
}