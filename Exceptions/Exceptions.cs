namespace CurrencyExchangeService.Exceptions
{
    public class ServiceException : Exception
    {
        public ServiceException(string message) : base(message) { }
        public ServiceException(string message, Exception innerException) : base(message, innerException) { }
    }

    public class InvalidCurrencyException(string message) : ServiceException(message) { }

    public class ApiException(string message, Exception innerException) : ServiceException(message, innerException) { }
}