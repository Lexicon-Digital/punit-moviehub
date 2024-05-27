using Asp.Versioning;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using MovieHub.Entities;
using MovieHub.Models;
using MovieHub.Services;

namespace MovieHub.Controllers;

[ApiController]
[Authorize]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion(1)]
[Produces("application/json")]
public class MovieReviewsController(IMovieHubRepository repository, IMapper mapper) : ControllerBase
{
    private readonly IMovieHubRepository _movieHubRepository = repository  ?? throw new ArgumentNullException(nameof(repository));
    private readonly IMapper _mapper = mapper  ?? throw new ArgumentNullException(nameof(mapper));

    [HttpGet("movies/{movieId:int}")]
    public async Task<ActionResult<IEnumerable<MovieReviewDto>>> GetReviewsByMovie(int movieId)
    {
        var movieExists = await _movieHubRepository.MovieExistsAsync(movieId);

        if (!movieExists) return NotFound();
        
        var movieReviewEntities = await _movieHubRepository.GetReviewsByMovieAsync(movieId);
        var movieReviews = _mapper.Map<IEnumerable<MovieReviewDto>>(movieReviewEntities);
        
        return Ok(movieReviews);
    }
    
    [HttpGet("{reviewId:int}", Name = "GetReviewById")]
    public async Task<ActionResult<MovieReviewDto>> GetReview(int reviewId)
    {
        var movieReviewEntity = await _movieHubRepository.GetReviewAsync(reviewId);

        if (movieReviewEntity == null) return NotFound();
        
        var movieReview = _mapper.Map<MovieReviewDto>(movieReviewEntity);
        
        return Ok(movieReview);
    }

    [HttpPost("movies/{movieId:int}")]
    public async Task<ActionResult<MovieReviewDto>> CreateReviewForMovie(
        int movieId,
        [FromBody] MovieReviewCreationDto movieReview
    ) {
        if (!ModelState.IsValid) return BadRequest();

        var movieExists = await _movieHubRepository.MovieExistsAsync(movieId);

        if (!movieExists) return NotFound();

        var finalMovieReview = _mapper.Map<MovieReview>(movieReview);

        await _movieHubRepository.CreateReviewForMovieAsync(movieId, finalMovieReview);

        await _movieHubRepository.SaveChangesAsync();

        var createdMovieReview = _mapper.Map<MovieReviewDto>(finalMovieReview);
        
        return CreatedAtRoute("GetReviewById", new
        {
            reviewId = createdMovieReview.Id
        }, createdMovieReview);
    }
    
    [HttpPut("movies/{movieId:int}/reviews/{reviewId:int}")]
    public async Task<ActionResult<MovieReviewDto>> UpdateReviewForMovie(
        int movieId,
        int reviewId,
        [FromBody] MovieReviewUpdateDto movieReview
    ) {
        if (!ModelState.IsValid) return BadRequest();

        var movieExists = await _movieHubRepository.MovieExistsAsync(movieId);

        if (!movieExists) return NotFound();

        var movieReviewEntity = await _movieHubRepository.GetReviewAsync(reviewId);

        if (movieReviewEntity == null) return NotFound();

        _mapper.Map(movieReview, movieReviewEntity);

        await _movieHubRepository.SaveChangesAsync();

        return NoContent();
    }
    
    [HttpPatch("movies/{movieId:int}/reviews/{reviewId:int}")]
    public async Task<ActionResult<MovieReviewDto>> PartiallyUpdateReviewForMovie(
        int movieId,
        int reviewId,
        [FromBody] JsonPatchDocument<MovieReviewUpdateDto> patchDocument
    ) {
        if (!ModelState.IsValid) return BadRequest();

        var movieExists = await _movieHubRepository.MovieExistsAsync(movieId);

        if (!movieExists) return NotFound();

        var movieReviewEntity = await _movieHubRepository.GetReviewAsync(reviewId);

        if (movieReviewEntity == null) return NotFound();

        var movieReviewToPatch = _mapper.Map<MovieReviewUpdateDto>(movieReviewEntity);
        
        patchDocument.ApplyTo(movieReviewToPatch, ModelState);

        if (!ModelState.IsValid || !TryValidateModel(movieReviewToPatch)) return BadRequest(ModelState);

        _mapper.Map(movieReviewToPatch, movieReviewEntity);
        await _movieHubRepository.SaveChangesAsync();

        return NoContent();
    }
    
    [HttpDelete("movies/{movieId:int}/reviews/{reviewId:int}")]
    public async Task<ActionResult> DeleteReviewForMovie(int movieId, int reviewId)
    {
        var movieExists = await _movieHubRepository.MovieExistsAsync(movieId);
    
        if (!movieExists) return NotFound();

        var movieReviewEntity = await _movieHubRepository.GetReviewAsync(reviewId);
    
        if (movieReviewEntity == null) return NotFound();
        
        _movieHubRepository.DeleteReviewAsync(movieReviewEntity);
        await _movieHubRepository.SaveChangesAsync();
        
        return NoContent();
    }
    
    [HttpDelete("movies/{movieId:int}")]
    public async Task<ActionResult> DeleteAllReviewsForMovie(int movieId, int reviewId)
    {
        var movieExists = await _movieHubRepository.MovieExistsAsync(movieId);
    
        if (!movieExists) return NotFound();

        var movieReviews = await _movieHubRepository.GetReviewsByMovieAsync(movieId);

        _movieHubRepository.DeleteReviewsByMovieAsync(movieReviews);
        await _movieHubRepository.SaveChangesAsync();
        
        return NoContent();
    }
}