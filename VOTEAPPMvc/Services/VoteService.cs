using Microsoft.EntityFrameworkCore;
using System;
using VOTEAPPMvc.Data;
using VOTEAPPMvc.Models;
using VOTEAPPMvc.ViewModels;

namespace VOTEAPPMvc.Services
{
    // Services/VoteService.cs
    public class VoteService : IVoteService
    {
        private readonly VotingDbContext _context;

        public VoteService(VotingDbContext context)
        {
            _context = context;
        }

        public async Task CreateTopic(Topic topic)
        {
            _context.Topics.Add(topic);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Topic>> GetActiveTopics()
        {
            return await _context.Topics
                .Where(t => t.IsActive)
                .Include(t => t.Votes)
                .ToListAsync();
        }

        public async Task<List<TopicSummaryViewModel>> GetTopicSummaries()
        {
            return await _context.Topics
                .Where(t => t.IsActive)
                .Select(t => new TopicSummaryViewModel
                {
                    Id = t.Id,
                    Title = t.Title,
                    TotalVotes = t.Votes.Count,
                    YesCount = t.Votes.Count(v => v.VoteValue),
                    NoCount = t.Votes.Count(v => !v.VoteValue)
                })
                .ToListAsync();
        }

        public async Task<VoteResult> GetVoteResult(int topicId)
        {
            var topic = await _context.Topics
                .Include(t => t.Votes)
                .FirstOrDefaultAsync(t => t.Id == topicId);

            if (topic == null) return null;

            return new VoteResult
            {
                TopicId = topic.Id,
                TopicTitle = topic.Title,
                YesCount = topic.Votes.Count(v => v.VoteValue),
                NoCount = topic.Votes.Count(v => !v.VoteValue)
            };
        }

        public async Task<bool> SubmitVote(int topicId, bool voteValue, string ipAddress, string userAgentHash, string sessionId)
        {
            var topic = await _context.Topics.FindAsync(topicId);
            if (topic == null) return false;

            var vote = new Vote
            {
                TopicId = topicId,
                VoteValue = voteValue,
                IPAddress = ipAddress,
                UserAgentHash = userAgentHash,
                SessionId = sessionId
            };

            _context.Votes.Add(vote);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> HasAlreadyVoted(int topicId, string ipAddress, string userAgentHash, string sessionId)
        {
            return await _context.Votes
                .AnyAsync(v => v.TopicId == topicId &&
                              (v.IPAddress == ipAddress ||
                               v.UserAgentHash == userAgentHash ||
                               v.SessionId == sessionId));
        }
    }
}
