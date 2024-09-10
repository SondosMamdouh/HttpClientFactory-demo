using Promotion_POC.CustomExceptions;
using System.Net.Http;

namespace Promotion_POC.Services
{
	public class WeatherService
	{
		private readonly IHttpClientFactory _httpClient;
		private readonly ILogger<WeatherService> _logger;

		public WeatherService(IHttpClientFactory httpClientFactory, ILogger<WeatherService> logger)
		{
			_httpClient = httpClientFactory;
			_logger = logger;
		}

		public async Task<string> GetWeatherAsync(double latitude, double longitude)
		{
			try
			{
				_logger.LogInformation("Starting API request to get data from Public Weather API...");
				var client = _httpClient.CreateClient("WeatherClient");
				var response = await client.GetAsync($"forecast?latitude={latitude}&longitude={longitude}&hourly=temperature_2m");

				if (!response.IsSuccessStatusCode)
				{
					_logger.LogWarning("API request failed with status code {StatusCode} and content: {Content}", response.StatusCode, response);
					throw new ApiException("Failed to retrieve weather data", (int)response.StatusCode);
				}

				_logger.LogInformation("API request succeeded.");
				return await response.Content.ReadAsStringAsync();
			}
			catch (HttpRequestException e)
			{
				_logger.LogError(e, "Network error occurred during API request.");
				throw new ApiException("Request error: " + e.Message, 500);
			}
			catch (Exception e)
			{
				_logger.LogError(e, "API error occurred during API request.");
				throw new ApiException($"error occurred: {e.Message}", e.HResult);
			}
		}
	}
}
