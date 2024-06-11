using MovieHub.Entities;
using MovieHub.Models;
using MovieHub.Models.MovieReview;

namespace MovieHub.Tests.TestFactory;

public static class MovieReviewsFactory
{
    public static IEnumerable<MovieReview> GetMockMovieReviewEntities()
    {
        return MockMovieReviewEntities;
    }
    
    public static MovieReview? GetMockMovieReviewEntity(int reviewId)
    {
        return MockMovieReviewEntities.FirstOrDefault(review => review.Id == reviewId);
    }
    
    public static IEnumerable<MovieReviewDto> GetMockMovieReviewsDto()
    {
        return MockMovieReviewsDto;
    }
    
    public static MovieReviewDto? GetMockMovieReviewDto(int reviewId)
    {
        return MockMovieReviewsDto.FirstOrDefault(review => review.Id == reviewId);
    }

    public static MovieReview? CreateMockReviewForMovie(int movieId, MovieReviewCreationDto review, bool failed = false)
    {
        return failed ? null : new MovieReview
        {
            Id = MockMovieReviewEntities.Count() + 1,
            Score = review.Score,
            Comment = review.Comment,
            ReviewDate = DateTime.Parse("2024-05-13T12:34:51.947256"),
            MovieId = movieId
        };
    }
    
    public static MovieReviewDto? CreateMockReviewDtoForMovie(int movieId, MovieReview review, bool failed = false)
    {
        return failed ? null : new MovieReviewDto
        {
            Id = MockMovieReviewEntities.Count() + 1,
            Score = review.Score,
            Comment = review.Comment,
            ReviewDate = DateTime.Parse("2024-05-13T12:34:51.947256"),
            MovieId = movieId
        };
    }
    
    public static MovieReviewUpdateDto? UpdateMockReviewDtoForMovie(MovieReview review, bool failed = false)
    {
        return failed ? null : new MovieReviewUpdateDto
        {
            Score = review.Score
        };
    }
    
    private static readonly IEnumerable<MovieReview> MockMovieReviewEntities = new List<MovieReview>
    {
        new()
        {
            Id = 1,
            Score = 8,
            Comment = "This movie was great!",
            ReviewDate = DateTime.Parse("2024-05-12T22:44:50.947256"),
            MovieId = 1
        },
        new()
        {
            Id = 2,
            Score = 9,
            Comment = "This movie was excellent!",
            ReviewDate = DateTime.Parse("2024-05-13T12:34:51.947256"),
            MovieId = 1
        }
    };
    
    private static readonly IEnumerable<MovieReviewDto> MockMovieReviewsDto = new List<MovieReviewDto>
    {
        new()
        {
            Id = 1,
            Score = 8,
            Comment = "This movie was great!",
            ReviewDate = DateTime.Parse("2024-05-12T22:44:50.947256"),
            MovieId = 1
        },
        new()
        {
            Id = 2,
            Score = 9,
            Comment = "This movie was excellent!",
            ReviewDate = DateTime.Parse("2024-05-13T12:34:51.947256"),
            MovieId = 1
        }
    };
}