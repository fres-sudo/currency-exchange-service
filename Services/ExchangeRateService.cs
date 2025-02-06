using CurrencyExchangeService.Models;
using System.Text.Json;

namespace CurrencyExchangeService.Services
{
  public class ExchangeRateService(HttpClient httpClient) : IExchangeRateService
  {
    private readonly HttpClient _httpClient = httpClient;
    private readonly string _baseUrl = "https://v6.exchangerate-api.com/v6";
    private readonly string _apiKey = "48a9d1015303465feab64f44";

    public async Task<ExchangeRateResponse> GetExchangeRatesAsync(string baseCurrency)
    {
            try
            {
                var response = await _httpClient.GetAsync($"{_baseUrl}/{_apiKey}/latest/{baseCurrency}");
                response.EnsureSuccessStatusCode();

                var responseContent = await response.Content.ReadAsStringAsync();
                var parsedResponse = JsonSerializer.Deserialize<ExchangeRateResponse>(responseContent);

                if (parsedResponse?.ConversionRates == null)
                {
                    throw new InvalidOperationException("Failed to get conversion rates from API");
                }

                return parsedResponse;
            }
            catch (HttpRequestException ex)
            {
                // Log the exception (logging not implemented in this example)
                throw new Exception("Error fetching exchange rates from the API", ex);
            }
            catch (JsonException ex)
            {
                // Log the exception (logging not implemented in this example)
                throw new Exception("Error parsing exchange rates response", ex);
            }
        }    public async Task<ConvertResponse> ConvertCurrencyAsync(string fromCurrency, string toCurrency, decimal amount)
    {
      var ratesResponse = await GetExchangeRatesAsync(fromCurrency);
      var rate = ratesResponse.ConversionRates.GetType()
          .GetProperty(toCurrency)?
          .GetValue(ratesResponse.ConversionRates);

      if (rate != null)
      {
        return new ConvertResponse
        {
          ConvertedAmount = amount * Convert.ToDecimal(rate),
        };
      }
      throw new Exception("Invalid currency code.");
    }
  }
}