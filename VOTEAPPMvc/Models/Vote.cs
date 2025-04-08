namespace VOTEAPPMvc.Models
{
    // Models/Vote.cs
    public class Vote
    {
        public int Id { get; set; }
        public int TopicId { get; set; }
        public bool VoteValue { get; set; }
        public string IPAddress { get; set; }
        public string UserAgentHash { get; set; }
        public string SessionId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public Topic Topic { get; set; }
    }
}
