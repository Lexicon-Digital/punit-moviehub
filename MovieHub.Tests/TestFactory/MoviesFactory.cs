using MovieHub.Entities;
using MovieHub.Models;
using MovieHub.Models.Movie;
using MovieHub.Models.PrincesTheatre;

namespace MovieHub.Tests.TestFactory;

public static class MoviesFactory
{
    public static IEnumerable<Movie> GetMockMovieEntities()
    {
        return MockMovieEntities;
    }
    
    public static Movie? GetMockMovieEntity(int movieId)
    {
        return MockMovieEntities.FirstOrDefault(movie => movie.Id == movieId);
    }
    
    public static MovieWithoutDetailsDto? GetMockMovieWithoutDetails(int movieId)
    {
        return MockMoviesWithoutDetails.FirstOrDefault(movie => movie.Id == movieId);
    }
    
    public static IEnumerable<MovieWithoutDetailsDto> GetMockMoviesWithoutDetails()
    {
        return MockMoviesWithoutDetails;
    }

    public static PrincesTheatreResponse GetMockPrincesTheatreResponse(MovieProvider provider)
    {
        return new PrincesTheatreResponse
        {
            Provider = provider == MovieProvider.Cinemaworld ? "Cinema World" : "Film World",
            Movies = new List<PrincesTheatreResponse.Movie?>
            {
                new()
                {
                    ID = $"{(provider == MovieProvider.Cinemaworld ? "cw" : "fw")}0120915",
                    Title = "Star Wars: Episode I - The Phantom Menace",
                    Type = "movie",
                    Poster =
                        "https://m.media-amazon.com/images/M/MV5BYTRhNjcwNWQtMGJmMi00NmQyLWE2YzItODVmMTdjNWI0ZDA2XkEyXkFqcGdeQXVyNTAyODkwOQ@@._V1_SX300.jpg",
                    Actors = "Liam Neeson, Ewan McGregor, Natalie Portman, Jake Lloyd",
                    Price = (decimal)(provider == MovieProvider.Cinemaworld ? 26.00 : 25.00)
                }
            }
        };
    }
    
    public static PrincesTheatreDto GetMockPrincesTheatreDto(MovieProvider provider)
    {
        return new PrincesTheatreDto
        {
            Provider = provider,
            Movies = new List<PrincesTheatreDto.Movie>
            {
                new()
                {
                    ID = new PrincesTheatreDto.Movie.Id
                    {
                        ProviderAcronym = provider == MovieProvider.Cinemaworld ? "cw" : "fw",
                        Provider = provider,
                        MovieId = "0120915"
                    },
                    Title = "Star Wars: Episode I - The Phantom Menace",
                    Type = "movie",
                    Poster =
                        "https://m.media-amazon.com/images/M/MV5BYTRhNjcwNWQtMGJmMi00NmQyLWE2YzItODVmMTdjNWI0ZDA2XkEyXkFqcGdeQXVyNTAyODkwOQ@@._V1_SX300.jpg",
                    Actors = "Liam Neeson, Ewan McGregor, Natalie Portman, Jake Lloyd",
                    Price = (decimal)(provider == MovieProvider.Cinemaworld ? 26.00 : 25.00)
                }
            }
        };
    }
    
    public static MovieWithPrincesTheatrePricesDto GetMockMovieWithPrincesTheatrePricesDto()
    {
        return MovieWithPrincesTheatrePricesDto;
    }
    
    private static readonly IEnumerable<Movie> MockMovieEntities = new List<Movie>
    {
        new
        (
            title: "Star Wars: The Phantom Menace (Episode I)",
            releaseDate: DateTime.Parse("1999-05-19T00:00:00"),
            genre: "Action, Adventure, Fantasy, Live Action, Science Fiction",
            runTime: 8160,
            synopsis: "Synopsis 1",
            director: "George Lucas",
            rating: "PG",
            princessTheatreMovieId: "0120915"
        )
        {
            Id = 1
        },
        new
        (
            title: "Star Wars: Attack of the Clones (Episode II)",
            releaseDate: DateTime.Parse("2002-05-16T00:00:00"),
            genre: "Action, Adventure, Fantasy, Live Action, Science Fiction",
            runTime: 8520,
            synopsis: "Synopsis 2",
            director: "George Lucas",
            rating: "PG-13",
            princessTheatreMovieId: "0121765"
        )
        {
            Id = 2
        }
    };
    
    private static readonly IEnumerable<MovieWithoutDetailsDto> MockMoviesWithoutDetails = new List<MovieWithoutDetailsDto>
    {
        new()
        {
            Id = 1,
            Title = "Star Wars: The Phantom Menace (Episode I)",
            ReleaseDate = DateTime.Parse("1999-05-19T00:00:00"),
            Genre = "Action, Adventure, Fantasy, Live Action, Science Fiction",
            RunTime = 8160,
            Synopsis = "Synopsis 1",
            Director = "George Lucas",
            Rating = "PG",
            PrincessTheatreMovieId = "0120915"
        },
        new()
        {
            Id = 2,
            Title = "Star Wars: Attack of the Clones (Episode II)",
            ReleaseDate = DateTime.Parse("2002-05-16T00:00:00"),
            Genre = "Action, Adventure, Fantasy, Live Action, Science Fiction",
            RunTime = 8520,
            Synopsis = "Synopsis 2",
            Director = "George Lucas",
            Rating = "PG-13",
            PrincessTheatreMovieId = "0121765"
        }
    };
    
    private static readonly MovieWithPrincesTheatrePricesDto MovieWithPrincesTheatrePricesDto = new()
    {
        Id = 1,
        Title = "Star Wars: The Phantom Menace (Episode I)",
        ReleaseDate = DateTime.Parse("1999-05-19T00:00:00"),
        Genre = "Action, Adventure, Fantasy, Live Action, Science Fiction",
        RunTime = 8160,
        Synopsis = "Synopsis 1",
        Director = "George Lucas",
        Rating = "PG",
        PrincessTheatreMovieId = "0120915",
        FilmWorldPrice = (decimal) 25.00,
        CinemaWorldPrice = (decimal) 26.00
    };
}