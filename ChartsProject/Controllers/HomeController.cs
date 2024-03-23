using ChartsProject.Models;
using ChartsProject.Services;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace ChartsProject.Controllers
{
	public class HomeController : Controller
	{
		private readonly ILogger<HomeController> _logger;
        private readonly GithubHttpService _githubService;

        public HomeController(ILogger<HomeController> logger, GithubHttpService githubService)
		{
			_logger = logger;
            _githubService = githubService;
        }

		public async Task<IActionResult> Index()
		{
			//await _githubService.RetrieveStargazersCount();
			return View();
		}

		public IActionResult Privacy()
		{
			return View();
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}
