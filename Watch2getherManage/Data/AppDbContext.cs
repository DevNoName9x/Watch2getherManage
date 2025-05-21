using Microsoft.EntityFrameworkCore;
using Watch2getherManage.Models;

namespace Watch2getherManage.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
       : base(options)
        {
        }

        public DbSet<SteamKey> SteamKeys => Set<SteamKey>();

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=app.db");
        }
    }
}
