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
      var response = await _httpClient.GetStringAsync($"{_baseUrl}/{_apiKey}/latest/{baseCurrency}");
      Console.WriteLine(response);
      var parsedResponse = JsonSerializer.Deserialize<ExchangeRateResponse>(response);

      if (parsedResponse?.ConversionRates == null)
      {
        throw new Exception("Failed to get conversion rates from API");
      }
      
      return parsedResponse;
    }

    public async Task<ConvertResponse> ConvertCurrencyAsync(string fromCurrency, string toCurrency, decimal amount)
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