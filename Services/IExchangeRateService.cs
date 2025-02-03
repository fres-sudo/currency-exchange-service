using CurrencyExchangeService.Models;

namespace CurrencyExchangeService.Services
{
  public interface IExchangeRateService
  {
    Task<ExchangeRateResponse> GetExchangeRatesAsync(string baseCurrency);
    Task<ConvertResponse> ConvertCurrencyAsync(string fromCurrency, string toCurrency, decimal amount);
  }
}