namespace VOTEAPPMvc.ViewModels
{
    // ViewModels/TopicSummaryViewModel.cs
    public class TopicSummaryViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int TotalVotes { get; set; }
        public int YesCount { get; set; }
        public int NoCount { get; set; }
        public double YesPercentage => TotalVotes > 0 ? (YesCount * 100.0) / TotalVotes : 0;
        public double NoPercentage => TotalVotes > 0 ? (NoCount * 100.0) / TotalVotes : 0;
    }
}
