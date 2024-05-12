using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MovieHub.Models;
using MovieHub.Services;

namespace MovieHub.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MoviesController(IMovieHubRepository repository, IMapper mapper) : ControllerBase
{
    private readonly IMovieHubRepository _movieHubRepository = repository  ?? throw new ArgumentNullException(nameof(repository));
    private readonly IMapper _mapper = mapper  ?? throw new ArgumentNullException(nameof(mapper));

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
    
    [HttpGet("{id:int}")]
    public async Task<ActionResult<IEnumerable<MovieDto>>> GetMovie(
        int id,
        [FromQuery(Name = "details")] bool details = false
    )
    {
        var movieEntity = await _movieHubRepository.GetMovieAsync(id, details);

        if (movieEntity == null)
        {
            return NotFound();
        }

        if (details)
        {
            var movie = _mapper.Map<MovieDto>(movieEntity);
            return Ok(movie);
        }
        else
        {
            var movie = _mapper.Map<MovieWithoutDetailsDto>(movieEntity);
            return Ok(movie);
        }
    }
}