using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using OdeToFood.Data.DomainClasses;

namespace OdeToFood.Data.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<OdeToFood.Data.OdeToFoodContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(OdeToFood.Data.OdeToFoodContext context)
        {
            context.Restaurants.AddOrUpdate(r => r.City, 
                new Restaurant {City = "Sint-Truiden", Country = "België", Name = "Noen"},
                new Restaurant { City = "Zoutleeuw", Country = "België", Name = "Peppermill" });
            context.SaveChanges();

            context.Reviews.AddOrUpdate(r => r.ReviewerName,
                new Review {Body = "I rate 8/8", Rating = 8, RestaurantId = context.Restaurants.First().Id, ReviewerName = "Orne"});
            context.SaveChanges();

            var roleMananger = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            roleMananger.Create(new IdentityRole { Name = "User" });
            context.SaveChanges();
        }
    }
}
