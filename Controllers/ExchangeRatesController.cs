using CurrencyExchangeService.Models;
using CurrencyExchangeService.Services;
using Microsoft.AspNetCore.Mvc;

namespace CurrencyExchangeService.Controllers
{
  [ApiController]
  [Route("[controller]")]
  public class ExchangeRatesController(IExchangeRateService exchangeRateService) : ControllerBase
  {
    private readonly IExchangeRateService _exchangeRateService = exchangeRateService;

    /// <summary>
    /// Fetch Exchange Rates
    /// </summary>
    /// <param name="baseCurrency">Base currency code (e.g., USD)</param>
    /// <returns>Exchange rates for various currencies</returns>
    [HttpGet]
    [Route("/exchange-rates")]
    public async Task<IActionResult> GetExchangeRates([FromQuery] string baseCurrency)
    {
      var rates = await _exchangeRateService.GetExchangeRatesAsync(baseCurrency);
      return Ok(rates);
    }

    /// <summary>
    /// Currency Conversion
    /// </summary>
    /// <param name="request">Conversion request details</param>
    /// <returns>Converted amount</returns>
    [HttpPost]
    [Route("/convert")]
    public async Task<IActionResult> ConvertCurrency([FromBody] ConvertRequest request)
    {
      var result = await _exchangeRateService.ConvertCurrencyAsync(
          request.FromCurrency,
          request.ToCurrency,
          request.Amount
      );
      return Ok(result);
    }
  }
}
