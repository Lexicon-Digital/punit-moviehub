using Asp.Versioning;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieHub.Models;
using MovieHub.Models.Movie;
using MovieHub.Models.PrincesTheatre;
using MovieHub.Services;

namespace MovieHub.Controllers;

[ApiController]
[Authorize]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion(1)]
[Produces("application/json")]
public class MoviesController(IMovieHubRepository repository, IMapper mapper, IPrincesTheatreService princesTheatreService) : ControllerBase
{
    private readonly IMovieHubRepository _movieHubRepository = repository  ?? throw new ArgumentNullException(nameof(repository));
    private readonly IMapper _mapper = mapper  ?? throw new ArgumentNullException(nameof(mapper));
    private readonly IPrincesTheatreService _princesTheatreService =
        princesTheatreService ?? throw new ArgumentNullException(nameof(princesTheatreService));

    private const int MaxMoviesPageSize = 10;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<MovieDto>>> GetMovies(
        [FromQuery(Name = "title")] string? title,
        [FromQuery(Name = "genre")] string? genre,
        [FromQuery(Name = "pageNumber")] int pageNumber = 1,
        [FromQuery(Name = "pageSize")] int pageSize = MaxMoviesPageSize
    )
    {
        pageSize = Math.Min(pageSize, MaxMoviesPageSize);
        var (movieEntities, paginationMetadata) = await _movieHubRepository.GetMoviesAsync(title, genre, pageNumber, pageSize);
        var movies = _mapper.Map<IEnumerable<MovieWithoutDetailsDto>>(movieEntities);

        Response.Headers.Append("X-Pagination-TotalItemCount", paginationMetadata.TotalItemCount.ToString());
        Response.Headers.Append("X-Pagination-TotalPageCount", paginationMetadata.TotalPageCount.ToString());
        Response.Headers.Append("X-Pagination-PageSize", pageSize.ToString());
        Response.Headers.Append("X-Pagination-CurrentPage", pageNumber.ToString());

        return Ok(movies);
    }
    
    [HttpGet("{movieId:int}")]
    public async Task<ActionResult> GetMovie(
        int movieId,
        [FromQuery(Name = "details")] bool details = false
    )
    {
        var movieEntity = await _movieHubRepository.GetMovieAsync(movieId, details);

        if (movieEntity == null) return NotFound();
        
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

                if (cinemaworldMovie == null) throw new ArgumentNullException(nameof(cinemaworldMovie));
                
                if (filmworldMovie == null) throw new ArgumentNullException(nameof(filmworldMovie));
                
                var movie = _mapper.Map<MovieWithPrincesTheatrePricesDto>(movieEntity);
                
                movie.CinemaWorldPrice = cinemaworldMovie.Price;
                movie.FilmWorldPrice = filmworldMovie.Price;
                
                return Ok(movie);
            }
            catch
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