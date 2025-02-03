namespace CurrencyExchangeService.Models
{
  public class ConvertRequest
  {
    public required string FromCurrency { get; set; }
    public required string ToCurrency { get; set; }
    public decimal Amount { get; set; }
  }
}