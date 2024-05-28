using AutoMapper;
using MovieHub.Entities;
using MovieHub.Models;
using MovieHub.Models.Movie;

namespace MovieHub.Profiles;

public class MovieProfile : Profile
{
    public MovieProfile()
    {
        CreateMap<Movie, MovieDto>();
        CreateMap<Movie, MovieWithoutDetailsDto>();
        CreateMap<Movie, MovieWithPrincesTheatrePricesDto>();
    }
}