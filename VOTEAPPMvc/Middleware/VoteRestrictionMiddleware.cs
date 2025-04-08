using System.Security.Cryptography;
using System.Text;
using VOTEAPPMvc.Services;

namespace VOTEAPPMvc.Middleware
{
    // Middleware/VoteRestrictionMiddleware.cs
    public class VoteRestrictionMiddleware
    {
        private readonly RequestDelegate _next;
        //private readonly IVoteService _voteService;


        //public VoteRestrictionMiddleware(RequestDelegate next, IVoteService voteService)
        //{
        //    _next = next;
        //    _voteService = voteService;
        //}

        public VoteRestrictionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var _voteService = context.RequestServices.GetRequiredService<IVoteService>(); // ✅ correct

            if (context.Request.Path.StartsWithSegments("/Vote/Submit") &&
               context.Request.Method == "POST" &&
               context.Request.RouteValues.TryGetValue("id", out var topicIdObj))
            {
                if (int.TryParse(topicIdObj?.ToString(), out int topicId))
                {
                    var ipAddress = context.Connection.RemoteIpAddress?.ToString() ?? "unknown";
                    var userAgent = context.Request.Headers["User-Agent"].ToString();
                    var userAgentHash = ComputeHash(userAgent);
                    var sessionId = context.Session.Id;

                    if (await _voteService.HasAlreadyVoted(topicId, ipAddress, userAgentHash, sessionId))
                    {
                        context.Response.Redirect($"/Vote/Index/{topicId}?error=already_voted");
                        return;
                    }
                }
            }

            await _next(context);

        }

        //public async Task InvokeAsync(HttpContext context)
        //{
        //    if (context.Request.Path.StartsWithSegments("/Vote/Submit") &&
        //        context.Request.Method == "POST" &&
        //        context.Request.RouteValues.TryGetValue("id", out var topicIdObj))
        //    {
        //        if (int.TryParse(topicIdObj?.ToString(), out int topicId))
        //        {
        //            var ipAddress = context.Connection.RemoteIpAddress?.ToString() ?? "unknown";
        //            var userAgent = context.Request.Headers["User-Agent"].ToString();
        //            var userAgentHash = ComputeHash(userAgent);
        //            var sessionId = context.Session.Id;

        //            if (await _voteService.HasAlreadyVoted(topicId, ipAddress, userAgentHash, sessionId))
        //            {
        //                context.Response.Redirect($"/Vote/Index/{topicId}?error=already_voted");
        //                return;
        //            }
        //        }
        //    }

        //    await _next(context);
        //}

        private static string ComputeHash(string input)
        {
            using var sha256 = SHA256.Create();
            var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(input));
            return Convert.ToBase64String(bytes);
        }
    }
}
