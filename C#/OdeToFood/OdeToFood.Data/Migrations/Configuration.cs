using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using OdeToFood.Data.DomainClasses;

namespace OdeToFood.Data.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<OdeToFoodContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override async void Seed(OdeToFoodContext context)
        {
            var roleStore = new RoleStore<IdentityRole>(context);
            var roleManager = new RoleManager<IdentityRole>(roleStore);

            context.Restaurants.AddOrUpdate(
                r => r.Name,
                new Restaurant {Name = "Peppermill", City = "Zoutleeuw", Country = "België"},
                new Restaurant { Name = "Noen", City = "Sint-Truiden", Country = "België" }
            );
            context.Reviews.AddOrUpdate(
                new Review() {Body = "I Rate 8/8", Rating = 8, RestaurantId = 2, ReviewerName = "Ruben Lassau"});

            await roleManager.CreateAsync(new IdentityRole { Name = "User" });
            context.SaveChanges();
        }
    }
}
