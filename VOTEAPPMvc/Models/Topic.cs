namespace VOTEAPPMvc.Models
{
    // Models/Topic.cs
    public class Topic
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public ICollection<Vote> Votes { get; set; } = new List<Vote>();
    }
}
