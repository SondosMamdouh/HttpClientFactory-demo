using Microsoft.AspNetCore.Mvc;
using Promotion_POC.Services;

namespace Promotion_POC.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class HomeController : Controller
	{
		private readonly FakeApiService _fakeApiService;
		private readonly WeatherService _weatherService;
		public HomeController(FakeApiService fakeApiService, WeatherService weatherService)
		{
			_fakeApiService = fakeApiService;
			_weatherService = weatherService;
		}

		[HttpGet("Getdata")]
		public async Task<IActionResult> GetData()
		{
			var data = await _fakeApiService.GetFakeDataAsync();
			return Ok(data);			
		}

		[HttpGet("GetWeather")]
		public async Task<IActionResult> GetWeather()
		{
			var data = await _weatherService.GetWeatherAsync(35.6895, 139.6917);
			return Ok(data);
		}

	}
}
