using MovieHub.Models.PrincesTheatre;

namespace MovieHub.Tests.TestFactory;

public static class PrincesTheatreResponseFactory
{
    public static PrincesTheatreCurrencyResponse GetMockCurrencyResponse()
    {
        return new PrincesTheatreCurrencyResponse
        {
            Base = "USD",
            Rates = new PrincesTheatreCurrencyResponse.CurrencyRates
            {
                Au = "1.44058"
            }
        };
    }
}