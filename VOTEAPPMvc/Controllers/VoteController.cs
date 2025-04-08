using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Text;
using VOTEAPPMvc.Services;

namespace VOTEAPPMvc.Controllers
{
            // Controllers/VoteController.cs
        public class VoteController : Controller
        {
            private readonly IVoteService _voteService;
            private readonly IReportService _reportService;

            public VoteController(IVoteService voteService, IReportService reportService)
            {
                _voteService = voteService;
                _reportService = reportService;
            }

            public async Task<IActionResult> Index(int id)
            {
                var result = await _voteService.GetVoteResult(id);
                if (result == null) return NotFound();

                if (TempData["Error"] != null)
                {
                    ViewBag.Error = TempData["Error"];
                }

                return View(result);
            }

            public async Task<IActionResult> Submit(int id)
            {
                var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";
                var userAgent = HttpContext.Request.Headers["User-Agent"].ToString();
                var userAgentHash = ComputeHash(userAgent);
                var sessionId = HttpContext.Session.Id;

                if (await _voteService.HasAlreadyVoted(id, ipAddress, userAgentHash, sessionId))
                {
                    TempData["Error"] = "You have already voted on this topic.";
                    return RedirectToAction("Index", new { id });
                }

                var topic = await _voteService.GetActiveTopics();
                var currentTopic = topic.FirstOrDefault(t => t.Id == id);
                if (currentTopic == null) return NotFound();

                ViewBag.Topic = currentTopic;
                return View();
            }

            [HttpPost]
            public async Task<IActionResult> Submit(int id, bool voteValue)
            {
                var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";
                var userAgent = HttpContext.Request.Headers["User-Agent"].ToString();
                var userAgentHash = ComputeHash(userAgent);
                var sessionId = HttpContext.Session.Id;

                var success = await _voteService.SubmitVote(id, voteValue, ipAddress, userAgentHash, sessionId);
                if (!success)
                {
                    TempData["Error"] = "Failed to submit your vote. Please try again.";
                    return RedirectToAction("Submit", new { id });
                }

                return RedirectToAction("Index", new { id });
            }

            [HttpGet("/api/vote/report/{topicId}")]
            public async Task<IActionResult> GetReport(int topicId)
            {
                var report = await _reportService.GenerateExcelReport(topicId);
                if (report == null) return NotFound();

                var topic = (await _voteService.GetVoteResult(topicId))?.TopicTitle ?? "VotingResults";
                return File(report,
                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    $"{topic}_Report.xlsx");
            }

            private static string ComputeHash(string input)
            {
                using var sha256 = SHA256.Create();
                var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(input));
                return Convert.ToBase64String(bytes);
            }
        }
}
