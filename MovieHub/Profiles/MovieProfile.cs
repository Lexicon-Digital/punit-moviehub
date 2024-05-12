using AutoMapper;
using MovieHub.Entities;
using MovieHub.Models;

public class MovieProfile : Profile
{
    public MovieProfile()
    {
        CreateMap<Movie, MovieDto>();
        CreateMap<Movie, MovieWithoutDetailsDto>();
        CreateMap<Movie, MovieWithPrincesTheatrePricesDto>();
    }
}