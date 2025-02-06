using CurrencyExchangeService.Exceptions;
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
            ValidateCurrencyCode(baseCurrency);
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
            throw new ApiException("Error fetching exchange rates from the API", ex);
        }
        catch (JsonException ex)
        {
            throw new ApiException("Error parsing exchange rates response", ex);
        }
        catch (InvalidOperationException ex)
        {
            throw new ApiException(ex.Message, ex);
        }
    }

    public async Task<ConvertResponse> ConvertCurrencyAsync(string fromCurrency, string toCurrency, decimal amount)
    {
        try
        {
            ValidateCurrencyCode(fromCurrency);
            ValidateCurrencyCode(toCurrency);
            var ratesResponse = await GetExchangeRatesAsync(fromCurrency);
            var rateProperty = ratesResponse.ConversionRates.GetType().GetProperty(toCurrency);

            if (rateProperty == null)
            {
                throw new InvalidCurrencyException("Invalid currency code");
            }

            var value = rateProperty.GetValue(ratesResponse.ConversionRates);
            if (value == null)
            {
                throw new InvalidCurrencyException("Exchange rate value is null");
            }
            var rate = (double)value;
            return new ConvertResponse
            {
                ConvertedAmount = amount * Convert.ToDecimal(rate),
            };
        }
        catch (ServiceException)
        {
            throw;
        }
        catch (Exception ex)
        {
            throw new ApiException("Error converting currency", ex);
        }
    }

    private static void ValidateCurrencyCode(string currencyCode)
    {
      if (string.IsNullOrWhiteSpace(currencyCode))
      {
        throw new InvalidCurrencyException("Currency code cannot be empty or whitespace");
      }

      if (currencyCode.Length != 3)
      {
        throw new InvalidCurrencyException($"Currency code '{currencyCode}' must be exactly 3 characters long");
      }

      var validCurrencies = typeof(ConversionRate).GetProperties().Select(p => p.Name);
      if (!validCurrencies.Contains(currencyCode))
      {
        throw new InvalidCurrencyException($"Currency code '{currencyCode}' is not supported. Please use a valid ISO 4217 currency code");
      }
    }
  }
}