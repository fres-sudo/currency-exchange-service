# Currency Exchange Service

Currency Exchange Service is a .NET 8.0 web API that provides real-time currency exchange rates and currency conversion functionalities. This service fetches exchange rates from an external API and allows users to convert amounts between different currencies.

## Features

- **Fetch Exchange Rates**: Get the latest exchange rates for various currencies.
- **Currency Conversion**: Convert amounts from one currency to another.

## Endpoints

### Get Exchange Rates

- **URL**: `/exchange-rates`
- **Method**: `GET`
- **Query Parameter**: [`baseCurrency`](Controllers/ExchangeRatesController.cs) (e.g., USD)
- **Response**: Exchange rates for various currencies.

### Convert Currency

- **URL**: `/convert`
- **Method**: `POST`
- **Request Body**:
  ```json
  {
  	"FromCurrency": "USD",
  	"ToCurrency": "EUR",
  	"Amount": 100
  }
  ```
- **Response**: Converted amount.

## Getting Started

### Prerequisites

- .NET 8.0 SDK
- Docker

### Running the Service

1. Clone the repository:

```sh
git clone https://github.com/your-username/currency-exchange-service.git
cd currency-exchange-service
```

2. Build and run the service:

```sh
dotnet build
dotnet run
```

3. Access the API: Open your browser and navigate to `https://localhost:7215/swagger` to explore the API using Swagger UI.

### Docker

1. Build the Docker image:

```sh
docker build -t currency-exchange-service .
```

2. Run the Docker container:

```sh
docker run -p 8080:8080 currency-exchange-service
```

## Deployment

This project includes a **GitHub Actions** workflow for building and deploying the service to **Azure**. The workflow is defined in``azure.yml`.

## Configuration

Configuration settings are stored in `appsettings.json` and `appsettings.Development.json`. Modify these files to change the logging level, allowed hosts, and Kestrel endpoints.

## License

This project is licensed under the MIT License. See the LICENSE file for details.
