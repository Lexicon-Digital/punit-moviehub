using MovieHub.Models;
using MovieHub.Models.PrincesTheatre;

namespace MovieHub.Services;

public interface IPrincesTheatreService
{
    public Task<PrincesTheatreResponse?> GetPrincesTheatreMovies(MovieProvider provider);
}