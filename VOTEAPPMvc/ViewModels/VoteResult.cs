namespace VOTEAPPMvc.ViewModels
{
    // ViewModels/VoteResult.cs
    public class VoteResult
    {
        public int TopicId { get; set; }
        public string TopicTitle { get; set; }
        public int YesCount { get; set; }
        public int NoCount { get; set; }
        public int TotalVotes => YesCount + NoCount;
    }
}
