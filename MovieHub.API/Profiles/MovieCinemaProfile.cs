using AutoMapper;
using MovieHub.Entities;
using MovieHub.Models.MovieCinema;

namespace MovieHub.Profiles;

public class MovieCinemaProfile : Profile
{
    public MovieCinemaProfile()
    {
        CreateMap<MovieCinema, MovieCinemaDto>()
            .ForMember(
                destination => destination.CinemaName, 
                options => options.MapFrom(
                    source => source.Cinema!.Name
                )
            );
    }
}