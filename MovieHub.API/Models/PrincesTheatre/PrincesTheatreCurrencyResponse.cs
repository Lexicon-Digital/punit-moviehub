using Newtonsoft.Json;

namespace MovieHub.Models.PrincesTheatre;

public class PrincesTheatreCurrencyResponse
{
    [JsonProperty("base")] public string Base { get; set; } = string.Empty;

    [JsonProperty("rates")] public CurrencyRates Rates { get; init; } = new();

    public class CurrencyRates
    {
        public string Au { get; init; } = string.Empty;
    }
}