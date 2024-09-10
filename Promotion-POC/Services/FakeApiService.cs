using Promotion_POC.CustomExceptions;

namespace Promotion_POC.Services
{
	public class FakeApiService 
	{
		private readonly IHttpClientFactory _httpClient;
		private readonly ILogger<FakeApiService> _logger;

		public FakeApiService(IHttpClientFactory httpClientFactory, ILogger<FakeApiService> logger)
		{
			_httpClient = httpClientFactory;
			_logger = logger;
		}

		public async Task<string> GetFakeDataAsync()
		{
			try
			{

				_logger.LogInformation("Starting API request to get data from fake API...");
				var client = _httpClient.CreateClient("FakeApiClient");

				var response = await client.GetAsync("endpoint");
				if (!response.IsSuccessStatusCode)
				{
					var content = await response.Content.ReadAsStringAsync();
					_logger.LogWarning("API request failed with status code {StatusCode} and content: {Content}", response.StatusCode, content);
					throw new ApiException("API request failed", (int)response.StatusCode);
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
				throw new ApiException($"API error occurred: {e.Message}", e.HResult);
			}
		}
	}
}
