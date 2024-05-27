using AutoMapper;
using MovieHub.Entities;
using MovieHub.Models.Cinema;

namespace MovieHub.Profiles;

public class CinemaProfile : Profile
{
    public CinemaProfile()
    {
        CreateMap<Cinema, CinemaDto>();
    }
}