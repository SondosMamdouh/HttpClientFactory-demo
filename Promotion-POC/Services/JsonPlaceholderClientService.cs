using Promotion_POC.CustomExceptions;
using Promotion_POC.Models;
using System.Text.Json;

namespace Promotion_POC.Services
{
	public class JsonPlaceholderClientService
	{
		private readonly HttpClient _httpClient;
		private readonly ILogger<JsonPlaceholderClientService> _logger;

		public JsonPlaceholderClientService(HttpClient httpClient, ILogger<JsonPlaceholderClientService> logger)
		{
			_httpClient = httpClient;
			_httpClient.BaseAddress = new Uri("https://jsonplaceholder.typicode.com/");
			_logger = logger;
		}

		public async Task<IEnumerable<Post>> GetPostsAsync()
		{
			try
			{
				_logger.LogInformation("Starting API request to get data from Public Posts API...");

				var response = await _httpClient.GetAsync("posts");
				response.EnsureSuccessStatusCode();

				var contentStream = await response.Content.ReadAsStreamAsync();
				var posts = await JsonSerializer.DeserializeAsync<IEnumerable<Post>>(contentStream);

				return posts;
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

		public async Task<Post> GetPostByIdAsync(int id)
		{
			try
			{

				_logger.LogInformation("Starting API request to get data from Public Post by ID API...");

				var response = await _httpClient.GetAsync($"posts/{id}");
				response.EnsureSuccessStatusCode();

				var contentStream = await response.Content.ReadAsStreamAsync();
				var post = await JsonSerializer.DeserializeAsync<Post>(contentStream);
				return post;
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
