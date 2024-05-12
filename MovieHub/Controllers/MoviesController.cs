using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MovieHub.Models;
using MovieHub.Models.PrincesTheatre;
using MovieHub.Services;

namespace MovieHub.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MoviesController(IMovieHubRepository repository, IMapper mapper, IPrincesTheatreService princesTheatreService) : ControllerBase
{
    private readonly IMovieHubRepository _movieHubRepository = repository  ?? throw new ArgumentNullException(nameof(repository));
    private readonly IMapper _mapper = mapper  ?? throw new ArgumentNullException(nameof(mapper));
    private readonly IPrincesTheatreService _princesTheatreService =
        princesTheatreService ?? throw new ArgumentNullException(nameof(princesTheatreService));

    [HttpGet]
    public async Task<ActionResult<IEnumerable<MovieDto>>> GetMovies(
        [FromQuery(Name = "title")] string? title,
        [FromQuery(Name = "genre")] string? genre
    )
    {
        var movieEntities = await _movieHubRepository.GetMoviesAsync(title, genre);
        var movies = _mapper.Map<IEnumerable<MovieWithoutDetailsDto>>(movieEntities);

        return Ok(movies);
    }
    
    [HttpGet("{movieId:int}")]
    public async Task<ActionResult> GetMovie(
        int movieId,
        [FromQuery(Name = "details")] bool details = false
    )
    {
        var movieEntity = await _movieHubRepository.GetMovieAsync(movieId, details);

        if (movieEntity == null)
        {
            return NotFound();
        }

        if (details)
        {
            try
            {
                var filmWorldResponse = await _princesTheatreService.GetPrincesTheatreMovies(MovieProvider.Filmworld);
                var cinemaWorldResponse =
                    await _princesTheatreService.GetPrincesTheatreMovies(MovieProvider.Cinemaworld);

                var filmworldMovie = _mapper
                    .Map<PrincesTheatreDto>(filmWorldResponse)
                    .Movies
                    .FirstOrDefault(movie => movie.ID.MovieId == movieEntity.PrincessTheatreMovieId);
                
                var cinemaworldMovie = _mapper
                    .Map<PrincesTheatreDto>(cinemaWorldResponse)
                    .Movies
                    .FirstOrDefault(movie => movie.ID.MovieId == movieEntity.PrincessTheatreMovieId);

                if (cinemaworldMovie == null)
                {
                    throw new ArgumentNullException(nameof(cinemaworldMovie));
                }
                
                if (filmworldMovie == null)
                {
                    throw new ArgumentNullException(nameof(filmworldMovie));
                }
                
                var movie = _mapper.Map<MovieWithPrincesTheatrePricesDto>(movieEntity);
                
                movie.CinemaWorldPrice = cinemaworldMovie.Price;
                movie.FilmWorldPrice = filmworldMovie.Price;
                
                return Ok(movie);
            }
            catch (Exception ex)
            {
                var movie = _mapper.Map<MovieDto>(movieEntity);
                return Ok(movie);
            }
        }

        {
            var movie = _mapper.Map<MovieWithoutDetailsDto>(movieEntity);
            return Ok(movie);
        }
    }
}