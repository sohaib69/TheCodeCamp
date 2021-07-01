using System.Data.Entity;
using TheCodeCamp.Migrations;

namespace TheCodeCamp.Data
{
    public class CampContext : DbContext
  {
    public CampContext() : base("CodeCampConnectionString")
    {
      Database.SetInitializer(new MigrateDatabaseToLatestVersion<CampContext, Configuration>());
    }

    public DbSet<Camp> Camps { get; set; }
    public DbSet<Speaker> Speakers { get; set; }
    public DbSet<Talk> Talks { get; set; }

  }
}
