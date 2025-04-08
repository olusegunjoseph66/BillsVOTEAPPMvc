using VOTEAPPMvc.Models;
using VOTEAPPMvc.ViewModels;

namespace VOTEAPPMvc.Services
{
    // Services/IVoteService.cs
    public interface IVoteService
    {
        Task CreateTopic(Topic topic);
        Task<List<Topic>> GetActiveTopics();
        Task<List<TopicSummaryViewModel>> GetTopicSummaries();
        Task<VoteResult> GetVoteResult(int topicId);
        Task<bool> SubmitVote(int topicId, bool voteValue, string ipAddress, string userAgentHash, string sessionId);
        Task<bool> HasAlreadyVoted(int topicId, string ipAddress, string userAgentHash, string sessionId);
    }
}
