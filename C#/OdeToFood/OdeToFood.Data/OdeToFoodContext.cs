using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OdeToFood.Data.DomainClasses;

namespace OdeToFood.Data
{
    public class OdeToFoodContext : DbContext
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
