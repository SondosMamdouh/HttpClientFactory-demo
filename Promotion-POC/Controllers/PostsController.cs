using Microsoft.AspNetCore.Mvc;
using Promotion_POC.Services;

namespace Promotion_POC.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class PostsController : Controller
	{
		private readonly JsonPlaceholderClientService _jsonPlaceholderClient;

		public PostsController(JsonPlaceholderClientService jsonPlaceholderClient)
		{
			_jsonPlaceholderClient = jsonPlaceholderClient;
		}

		[HttpGet]
		public async Task<IActionResult> GetPosts()
		{
			var posts = await _jsonPlaceholderClient.GetPostsAsync();
			return Ok(posts);
		}

		[HttpGet("{id}")]
		public async Task<IActionResult> GetPost(int id)
		{
			var post = await _jsonPlaceholderClient.GetPostByIdAsync(id);
			return Ok(post);
		}
	}
}
