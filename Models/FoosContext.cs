using Microsoft.EntityFrameworkCore;

namespace foosball_asp.Models 
{
    public class FoosContext : DbContext 
    {
        public FoosContext(DbContextOptions<FoosContext> options)
        : base (options)
        {}

        public DbSet<User> Users { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<Game> Games { get; set; }
        public DbSet<Player> Players { get; set; }
        public DbSet<Score> Scores { get; set; }
    }
}