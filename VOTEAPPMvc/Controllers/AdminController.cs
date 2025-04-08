using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VOTEAPPMvc.Models;
using VOTEAPPMvc.Services;

namespace VOTEAPPMvc.Controllers
{
    // Controllers/AdminController.cs
    //[Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly IVoteService _voteService;

        public AdminController(IVoteService voteService)
        {
            _voteService = voteService;
        }

        public IActionResult CreateTopic()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateTopic(Topic model)
        {
            if (ModelState.IsValid)
            {
                await _voteService.CreateTopic(model);
                return RedirectToAction("ManageTopics");
            }
            return View(model);
        }

        public async Task<IActionResult> ManageTopics()
        {
            var topics = await _voteService.GetActiveTopics();
            return View(topics);
        }
    }
}
