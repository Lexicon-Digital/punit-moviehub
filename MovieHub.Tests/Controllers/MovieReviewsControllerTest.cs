using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.JsonPatch.Operations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Moq;
using MovieHub.Controllers;
using MovieHub.Entities;
using MovieHub.Models;
using MovieHub.Services;
using MovieHub.Tests.TestFactory;

namespace MovieHub.Tests.Controllers;

public class MovieReviewsControllerTest
{
    private readonly Mock<IMovieHubRepository> _mockRepository;
    private readonly Mock<IMapper> _mockMapper;
    private readonly MovieReviewsController _movieReviewsController;

    public MovieReviewsControllerTest()
    {
        _mockRepository = new Mock<IMovieHubRepository>();
        _mockMapper = new Mock<IMapper>();
        
        var mockModelMetadataProvider = new Mock<IModelMetadataProvider>();
        var mockServiceProvider = new Mock<IServiceProvider>();
        var mockHttpContext = new Mock<HttpContext>();
        var objectValidator = new Mock<IObjectModelValidator>();
        
        objectValidator.Setup(validator => validator.Validate(
                It.IsAny<ActionContext>(),
                It.IsAny<ValidationStateDictionary>(),
                It.IsAny<string>(),
                It.IsAny<object>()
            )
        );

        mockServiceProvider.Setup(provider => provider.GetService(typeof(IModelMetadataProvider)))
            .Returns(mockModelMetadataProvider.Object);
        
        _movieReviewsController = new MovieReviewsController(_mockRepository.Object, _mockMapper.Object)
        {
            ControllerContext = new ControllerContext
            {
                HttpContext = mockHttpContext.Object,
                ActionDescriptor = new ControllerActionDescriptor()
            }
        };

        _movieReviewsController.ControllerContext.HttpContext.RequestServices = mockServiceProvider.Object;
        _movieReviewsController.ObjectValidator = objectValidator.Object;
    }
    
    [Fact]
    public async void GetReviewsByMovieAsync_Returns_Ok_Result_With_List_Of_Reviews_For_A_Given_Movie()
    {
        const int movieId = 1;
        var movieReviewEntities = MovieReviewsFactory.GetMockMovieReviewEntities();
        var movieReviewsDto = MovieReviewsFactory.GetMockMovieReviewsDto();
        
        _mockRepository.Setup(repository => repository.MovieExistsAsync(movieId)).ReturnsAsync(true);

        _mockRepository.Setup(repository => repository.GetReviewsByMovieAsync(movieId))
            .ReturnsAsync(movieReviewEntities);

        _mockMapper.Setup(mapper => mapper.Map<IEnumerable<MovieReviewDto>>(movieReviewEntities))
            .Returns(movieReviewsDto);

        var result = await _movieReviewsController.GetReviewsByMovie(movieId);

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnedMovieReviews = Assert.IsType<List<MovieReviewDto>>(okResult.Value);
        
        Assert.Equal(2, returnedMovieReviews.Count);
    }
    
    [Fact]
    public async void GetReview_Returns_Ok_Result_With_A_Single_Review()
    {
        const int reviewId = 1;
        var movieReviewEntity = MovieReviewsFactory.GetMockMovieReviewEntity(reviewId);
        var movieReviewDto = MovieReviewsFactory.GetMockMovieReviewDto();
    
        _mockRepository.Setup(repository => repository.GetReviewAsync(reviewId))
            .ReturnsAsync(movieReviewEntity);
    
        _mockMapper.Setup(mapper => mapper.Map<MovieReviewDto>(movieReviewEntity))
            .Returns(movieReviewDto!);
    
        var result = await _movieReviewsController.GetReview(reviewId);
        
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnedReview = Assert.IsType<MovieReviewDto>(okResult.Value);
        
        Assert.Equal(1, returnedReview.Id);
    }
    
    [Fact]
    public async void GetReview_Returns_NotFound_Result()
    {
        const int reviewId = 9999;
        var movieEntity = MovieReviewsFactory.GetMockMovieReviewEntity(reviewId);
    
        _mockRepository.Setup(repository => repository.GetReviewAsync(reviewId)).ReturnsAsync(movieEntity);
    
        var result = await _movieReviewsController.GetReview(reviewId);
    
        Assert.IsType<NotFoundResult>(result.Result);
    }
    
    [Fact]
    public async void CreateReviewForMovie_Returns_CreatedAtRoute_Result_With_A_Single_Review()
    {
        const int movieId = 1;
        
        var movieReviewCreationDto = new MovieReviewCreationDto
        {
            Score = 8,
            Comment = "Movie was fantastic"
        };
    
        var movieReview = MovieReviewsFactory.CreateReviewForMovie(movieId, movieReviewCreationDto);
        var movieReviewDto = MovieReviewsFactory.CreateReviewDtoForMovie(movieId, movieReview!);
    
        _mockRepository.Setup(repository => repository.MovieExistsAsync(movieId)).ReturnsAsync(true);
        
        _mockMapper.Setup(mapper => mapper.Map<MovieReview>(movieReviewCreationDto))
            .Returns(movieReview!);
        
        _mockMapper.Setup(mapper => mapper.Map<MovieReviewDto>(movieReview))
            .Returns(movieReviewDto!);
        
        _mockRepository.Setup(repository => repository.CreateReviewForMovieAsync(movieId, movieReview!)).Returns(Task.CompletedTask);
        _mockRepository.Setup(repository => repository.SaveChangesAsync()).ReturnsAsync(true);
    
        var result = await _movieReviewsController.CreateReviewForMovie(movieId, movieReviewCreationDto);
    
        var createdAtRouteResult = Assert.IsType<CreatedAtRouteResult>(result.Result);
        var returnedMovieReview = Assert.IsType<MovieReviewDto>(createdAtRouteResult.Value);
        
        Assert.Equal(3, returnedMovieReview.Id);
        Assert.Equal(1, returnedMovieReview.MovieId);
        Assert.Equal(8, returnedMovieReview.Score);
        Assert.Equal("Movie was fantastic", returnedMovieReview.Comment);
    }
    
    [Fact]
    public async void UpdateReviewForMovie_Returns_NoContent_Result()
    {
        const int movieId = 1;
        const int reviewId = 1;

        var movieReviewUpdateDto = new MovieReviewUpdateDto
        {
            Score = 6,
            Comment = "Movie was okay"
        };
    
        var movieReviewEntity = MovieReviewsFactory.GetMockMovieReviewEntity(reviewId);
    
        _mockRepository.Setup(repository => repository.MovieExistsAsync(movieId)).ReturnsAsync(true);
        
        _mockRepository.Setup(repository => repository.GetReviewAsync(movieId)).ReturnsAsync(movieReviewEntity);
        
        _mockMapper.Setup(mapper => mapper.Map<MovieReview>(movieReviewUpdateDto))
            .Returns(movieReviewEntity!);
        
        _mockMapper.Setup(mapper => mapper.Map(movieReviewUpdateDto, movieReviewEntity))
            .Returns(movieReviewEntity);
        
        _mockRepository.Setup(repository => repository.SaveChangesAsync()).ReturnsAsync(true);
    
        var result = await _movieReviewsController.UpdateReviewForMovie(movieId, reviewId, movieReviewUpdateDto);
    
        Assert.IsType<NoContentResult>(result.Result);
    }
    
    [Fact]
    public async void PartiallyUpdateReviewForMovie_Returns_NoContent_Result()
    {
        const int movieId = 1;
        const int reviewId = 1;

        var patchDocument = new JsonPatchDocument<MovieReviewUpdateDto>()
        {
            Operations =
            {
                new Operation<MovieReviewUpdateDto>
                {
                    op = "replace",
                    path = "/score",
                    value = 7.0
                }
            }
        };
    
        var movieReviewEntity = MovieReviewsFactory.GetMockMovieReviewEntity(reviewId);
        var movieReviewToPatch = MovieReviewsFactory.UpdateReviewDtoForMovie(movieReviewEntity!);
    
        _mockRepository.Setup(repository => repository.MovieExistsAsync(movieId)).ReturnsAsync(true);
        _mockRepository.Setup(repository => repository.GetReviewAsync(reviewId)).ReturnsAsync(movieReviewEntity);
        
        _mockMapper.Setup(mapper => mapper.Map<MovieReviewUpdateDto>(movieReviewEntity))
            .Returns(movieReviewToPatch!);
        
        _mockMapper.Setup(mapper => mapper.Map(movieReviewToPatch, movieReviewEntity))
            .Returns(movieReviewEntity);
        
        _mockRepository.Setup(repository => repository.SaveChangesAsync()).ReturnsAsync(true);
    
        var result = await _movieReviewsController.PartiallyUpdateReviewForMovie(movieId, reviewId, patchDocument);
    
        Assert.IsType<NoContentResult>(result.Result);
    }
    
    [Fact]
    public async void DeleteReviewForMovie_Returns_NoContent_Result()
    {
        const int movieId = 1;
        const int reviewId = 1;

        var movieReviewEntity = MovieReviewsFactory.GetMockMovieReviewEntity(reviewId);
    
        _mockRepository.Setup(repository => repository.MovieExistsAsync(movieId)).ReturnsAsync(true);
        _mockRepository.Setup(repository => repository.GetReviewAsync(reviewId)).ReturnsAsync(movieReviewEntity);
        _mockRepository.Setup(repository => repository.DeleteReviewAsync(movieReviewEntity!));
        _mockRepository.Setup(repository => repository.SaveChangesAsync()).ReturnsAsync(true);

        var result = await _movieReviewsController.DeleteReviewForMovie(movieId, reviewId);
    
        Assert.IsType<NoContentResult>(result);
    }
    
    [Fact]
    public async void DeleteAllReviewsForMovie_Returns_NoContent_Result()
    {
        const int movieId = 1;
        var movieEntities = MovieReviewsFactory.GetMockMovieReviewEntities().ToList();
    
        _mockRepository.Setup(repository => repository.MovieExistsAsync(movieId)).ReturnsAsync(true);
        _mockRepository.Setup(repository => repository.GetReviewsByMovieAsync(movieId)).ReturnsAsync(movieEntities);
        _mockRepository.Setup(repository => repository.DeleteReviewsByMovieAsync(movieEntities));
        _mockRepository.Setup(repository => repository.SaveChangesAsync()).ReturnsAsync(true);

        var result = await _movieReviewsController.DeleteAllReviewsForMovie(movieId);
    
        Assert.IsType<NoContentResult>(result);
    }
}