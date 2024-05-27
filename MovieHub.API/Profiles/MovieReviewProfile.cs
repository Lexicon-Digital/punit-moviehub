using AutoMapper;
using MovieHub.Entities;
using MovieHub.Models;

namespace MovieHub.Profiles;

public class MovieReviewProfile : Profile
{
    public MovieReviewProfile()
    {
        CreateMap<MovieReview, MovieReviewDto>();
        CreateMap<MovieReviewCreationDto, MovieReview>();
        CreateMap<MovieReviewUpdateDto, MovieReview>();
        CreateMap<MovieReview, MovieReviewUpdateDto>();
    }
}