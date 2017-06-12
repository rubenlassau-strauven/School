using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity.EntityFramework;
using OdeToFood.Data.DomainClasses;
using OdeToFood.Data.Migrations;

namespace OdeToFood.Data
{
    public class OdeToFoodContext : IdentityDbContext<ApplicationUser>
    {
        public OdeToFoodContext() : base("OdeToFoodContext") { }

        public DbSet<Restaurant> Restaurants { get; set; }
        public DbSet<Review> Reviews { get; set; }

        public static void SetInitializer()
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<OdeToFoodContext, Configuration>());
        }
    }
}
