using System.Data.Entity;
using TestApplication.Models;

namespace TestApplication
{
    public class UserContext : DbContext
    {
        public DbSet<Individual> Individual { get; set; }
        public DbSet<LegalEntity> LegalEntities { get; set; }
        public DbSet<Contracts> Contracts { get; set; }

        public UserContext() : base("name=MyDbContext")
        {
        }
    }
}