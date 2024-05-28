using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Headers;
using Microsoft.AspNetCore.Mvc;
using Moq;
using MovieHub.Controllers;
using MovieHub.Models;
using MovieHub.Models.PrincesTheatre;
using MovieHub.Services;
using MovieHub.Tests.TestFactory;

namespace MovieHub.Tests.Controllers;

public class MoviesControllerTest
{
    private readonly Mock<IMovieHubRepository> _mockRepository;
    private readonly Mock<IMapper> _mockMapper;
    private readonly Mock<IPrincesTheatreService> _mockPrincesTheatreService;
    private readonly MoviesController _moviesController;

    public MoviesControllerTest()
    {
        _mockRepository = new Mock<IMovieHubRepository>();
        _mockMapper = new Mock<IMapper>();
        _mockPrincesTheatreService = new Mock<IPrincesTheatreService>();

        var mockHttpContext = new Mock<HttpContext>();
        var response = new Mock<HttpResponse>();
        var headerDictionary = new HeaderDictionary();
        var responseHeaders = new ResponseHeaders(headerDictionary);

        response.SetupGet(httpResponse => httpResponse.Headers).Returns(responseHeaders.Headers);
        mockHttpContext.SetupGet(httpContext => httpContext.Response).Returns(response.Object);
        
        _moviesController = new MoviesController(_mockRepository.Object, _mockMapper.Object, _mockPrincesTheatreService.Object)
        {
            ControllerContext = new ControllerContext
            {
                HttpContext = mockHttpContext.Object
            }
        };
    }
    
    [Fact]
    public async void GetMovies_Returns_Ok_Result_With_List_Of_Movies()
    {
        string? title = null;
        string? genre = null;
        const int pageNumber = 1;
        const int pageSize = 2;

        var paginationMetaData = new PaginationMetadata(MoviesFactory.GetMockMovieEntities().Count(), pageSize);

        _mockRepository.Setup(repository => repository.GetMoviesAsync(title, genre, pageNumber, pageSize))
            .ReturnsAsync((MoviesFactory.GetMockMovieEntities(), paginationMetaData));

        _mockMapper.Setup(mapper => mapper.Map<IEnumerable<MovieWithoutDetailsDto>>(MoviesFactory.GetMockMovieEntities()))
            .Returns(MoviesFactory.GetMockMoviesWithoutDetails());

        var result = await _moviesController.GetMovies(title, genre, pageNumber, pageSize);

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnedMovies = Assert.IsType<List<MovieWithoutDetailsDto>>(okResult.Value);
        
        Assert.Equal(2, returnedMovies.Count);
    }
    
    [Fact]
    public async void GetMovie_Returns_Ok_Result_With_A_Single_Movie()
    {
        const int movieId = 1;
        var movieEntity = MoviesFactory.GetMockMovieEntity(movieId);
        var movieWithoutDetails = MoviesFactory.GetMockMovieWithoutDetails(movieId);

        _mockRepository.Setup(repository => repository.GetMovieAsync(movieId, false))
            .ReturnsAsync(movieEntity);

        _mockMapper.Setup(mapper => mapper.Map<MovieWithoutDetailsDto>(movieEntity))
            .Returns(movieWithoutDetails!);

        var result = await _moviesController.GetMovie(movieId);
        
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedMovie = Assert.IsType<MovieWithoutDetailsDto>(okResult.Value);
        
        Assert.Equal(1, returnedMovie.Id);
    }
    
    [Fact]
    public async void GetMovie_Returns_Ok_Result_With_A_Single_Movie_With_Details()
    {
        const int movieId = 1;

        var princessTheatreCinemaWorldResponse = MoviesFactory.GetPrincesTheatreResponse(MovieProvider.Cinemaworld);
        var princessTheatreFilmWorldResponse = MoviesFactory.GetPrincesTheatreResponse(MovieProvider.Filmworld);
        var movieEntity = MoviesFactory.GetMockMovieEntity(movieId);

        _mockRepository.Setup(repository => repository.GetMovieAsync(movieId, true))
            .ReturnsAsync(movieEntity);
        
        _mockPrincesTheatreService.Setup(service => service.GetPrincesTheatreMovies(MovieProvider.Cinemaworld))
            .ReturnsAsync(princessTheatreCinemaWorldResponse);
        
        _mockPrincesTheatreService.Setup(service => service.GetPrincesTheatreMovies(MovieProvider.Filmworld))
            .ReturnsAsync(princessTheatreFilmWorldResponse);
        
        _mockMapper.Setup(mapper => mapper.Map<PrincesTheatreDto>(princessTheatreCinemaWorldResponse))
            .Returns(MoviesFactory.GetPrincesTheatreDto(MovieProvider.Cinemaworld));
        
        _mockMapper.Setup(mapper => mapper.Map<PrincesTheatreDto>(princessTheatreFilmWorldResponse))
            .Returns(MoviesFactory.GetPrincesTheatreDto(MovieProvider.Filmworld));

        _mockMapper.Setup(mapper => mapper.Map<MovieWithPrincesTheatrePricesDto>(movieEntity))
            .Returns(MoviesFactory.GetMovieWithPrincesTheatrePricesDto());

        var result = await _moviesController.GetMovie(movieId, details: true);

        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedMovie = Assert.IsType<MovieWithPrincesTheatrePricesDto>(okResult.Value);
        
        Assert.Equal(1, returnedMovie.Id);
        Assert.Equal((decimal) 25.00, returnedMovie.FilmWorldPrice);
        Assert.Equal((decimal) 26.00, returnedMovie.CinemaWorldPrice);
    }
    
    [Fact]
    public async void GetMovie_Returns_NotFound_Result()
    {
        const int movieId = 9999;
        var movieEntity = MoviesFactory.GetMockMovieEntity(movieId);

        _mockRepository.Setup(repository => repository.GetMovieAsync(movieId, false)).ReturnsAsync(movieEntity);

        var result = await _moviesController.GetMovie(movieId);

        Assert.IsType<NotFoundResult>(result);
    }
}