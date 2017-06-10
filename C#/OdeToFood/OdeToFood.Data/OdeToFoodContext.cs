using System.Data.Entity;
using Microsoft.AspNet.Identity.EntityFramework;
using OdeToFood.Data.DomainClasses;

namespace OdeToFood.Data
{
    public class OdeToFoodContext : IdentityDbContext<ApplicationUser>
    {
        public OdeToFoodContext() : base("OdeToFoodContext") { }
        public DbSet<Restaurant> Restaurants { get; set; }
        public DbSet<Review> Reviews { get; set; }

        public static void SetInitializer()
        {
            System.Data.Entity.Database.SetInitializer(new MigrateDatabaseToLatestVersion<OdeToFoodContext, Migrations.Configuration>());
        }
    }
}
