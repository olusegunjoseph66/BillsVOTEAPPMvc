using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using VOTEAPPMvc.Models;
using VOTEAPPMvc.Services;

namespace VOTEAPPMvc.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IVoteService _voteService;

        public HomeController(ILogger<HomeController> logger, IVoteService voteService)
        {
            _logger = logger;
            _voteService = voteService;
        }



        public async Task<IActionResult> Index()
        {
            var summaries = await _voteService.GetTopicSummaries();
            return View(summaries);
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
