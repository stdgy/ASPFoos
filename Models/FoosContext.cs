using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace foosball_asp.Models 
{
    public class FoosContext : IdentityDbContext<User>
    {
        public FoosContext(DbContextOptions<FoosContext> options)
        : base (options)
        {}
        
        public DbSet<Team> Teams { get; set; }
        public DbSet<Game> Games { get; set; }
        public DbSet<Player> Players { get; set; }
        public DbSet<Score> Scores { get; set; }
    }
}