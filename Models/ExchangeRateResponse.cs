using System.Text.Json.Serialization;

namespace CurrencyExchangeService.Models
{
  public class ExchangeRateResponse
  {
    [JsonPropertyName("result")]
    public required string Result { get; set; }
    
    [JsonPropertyName("documentation")]
    public required string Documentation { get; set; }
    
    [JsonPropertyName("terms_of_use")]
    public required string TermsOfUse { get; set; }

    [JsonPropertyName("time_last_update_unix")]    
    public required long TimeLastUpdateUnix { get; set; }

    [JsonPropertyName("time_last_update_utc")]
    public required string TimeLastUpdateUtc { get; set; }

    [JsonPropertyName("time_next_update_unix")]
    public required long TimeNextUpdateUnix { get; set; }

    [JsonPropertyName("time_next_update_utc")]
    public required string YimeNextUpdateUtc { get; set; }

    [JsonPropertyName("base_code")]
    public required string BaseCode { get; set; }

    [JsonPropertyName("conversion_rates")]
    public required ConversionRate ConversionRates { get; set; }
  }
}
