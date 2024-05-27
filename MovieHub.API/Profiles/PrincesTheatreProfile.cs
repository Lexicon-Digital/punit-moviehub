using AutoMapper;
using MovieHub.Models.PrincesTheatre;

namespace MovieHub.Profiles;

public class PrincesTheatreProfile : Profile
{
    public PrincesTheatreProfile()
    {
        CreateMap<PrincesTheatreResponse, PrincesTheatreDto>()
            .ForMember(destination => destination.Provider,
                options => options.MapFrom(
                    source => ParseProvider(source.Provider)))
            .ForMember(
                destination => destination.Movies, 
                options => options.MapFrom(
                    source => source.Movies
                    )
                );
        
        CreateMap<PrincesTheatreResponse.Movie, PrincesTheatreDto.Movie>()
            .ForMember(
                destinationMovie => destinationMovie.ID, 
                options => options.MapFrom(
                    sourceMovie => ParseMovieId(sourceMovie.ID)
                    )
                );
    }
    
    private static PrincesTheatreDto.Movie.Id ParseMovieId(string movieId)
    {
        return new PrincesTheatreDto.Movie.Id
        {
            ProviderAcronym = movieId[..2], 
            MovieId = movieId[2..]
        }.Build();
    }
    
    private static MovieProvider ParseProvider(string provider)
    {
        var movieProvider = provider switch
        {
            "Film World" => MovieProvider.Filmworld,
            "Cinema World" => MovieProvider.Cinemaworld,
            _ => throw new ArgumentOutOfRangeException(nameof(provider))
        };
        return movieProvider;
    }
}