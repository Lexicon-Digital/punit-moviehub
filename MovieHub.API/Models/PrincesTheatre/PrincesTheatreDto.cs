namespace MovieHub.Models.PrincesTheatre;

public class PrincesTheatreDto
{
    public MovieProvider Provider { get; init; }
    public ICollection<Movie> Movies = new List<Movie>();

    public class Movie
    {
        public Id ID { get; init; }
        public string Title { get; init; } = string.Empty;
        public string Type { get; init; } = string.Empty;
        public string Poster { get; init; } = string.Empty;
        public string Actors { get; init; } = string.Empty;
        public decimal Price { get; init; }

        public class Id
        {
            public string ProviderAcronym { get; init; } = string.Empty;
            public MovieProvider Provider { get; set; }
            public string MovieId { get; init; } = string.Empty;

            public Id Build()
            {
                MovieProvider provider = ProviderAcronym switch
                {
                    "fw" => MovieProvider.Filmworld,
                    "cw" => MovieProvider.Cinemaworld,
                    _ => throw new ArgumentOutOfRangeException(nameof(provider))
                };

                return new Id()
                {
                    ProviderAcronym = ProviderAcronym,
                    Provider = provider,
                    MovieId = MovieId
                };
            }
        }
    }
}