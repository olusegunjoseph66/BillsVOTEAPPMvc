using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using VOTEAPPMvc.Models;

namespace VOTEAPPMvc.Data
{
    public class VotingDbContext : DbContext
    {
        public VotingDbContext(DbContextOptions<VotingDbContext> options) : base(options) { }

        public DbSet<Topic> Topics { get; set; }
        public DbSet<Vote> Votes { get; set; }
    }
}
