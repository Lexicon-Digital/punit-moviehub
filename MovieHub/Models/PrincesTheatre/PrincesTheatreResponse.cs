namespace MovieHub.Models.PrincesTheatre;

public class PrincesTheatreResponse
{
    public string Provider { get; set; } = string.Empty;
    public ICollection<Movie?> Movies { get; set; } = new List<Movie?>();

    public class Movie
    {
        public string ID { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public string Poster { get; set; } = string.Empty;
        public string Actors { get; set; } = string.Empty;
        public decimal Price { get; set; }
    }
}