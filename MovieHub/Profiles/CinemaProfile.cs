using AutoMapper;
using MovieHub.Entities;
using MovieHub.Models;

public class CinemaProfile : Profile
{
    public CinemaProfile()
    {
        CreateMap<Cinema, CinemaDto>();
    }
}