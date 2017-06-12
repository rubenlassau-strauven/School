using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OdeToFood.Data.DomainClasses;

namespace OdeToFood.Api.Tests.TestServices
{
    public class RestaurantBuilder
    {
        private Restaurant restaurant;
        public RestaurantBuilder()
        {
            restaurant = new Restaurant();
            restaurant.City = Guid.NewGuid().ToString();
            restaurant.Country = Guid.NewGuid().ToString();

            restaurant.Name = Guid.NewGuid().ToString();
        }

        public Restaurant Build()
        {
            return restaurant;
        }

        public RestaurantBuilder WithId(int id = -1)
        {
            if (id < 0)
            {
                restaurant.Id = new Random().Next(1, int.MaxValue);
            }
            else
            {
                restaurant.Id = id;
            }
            return this;
        }

        public List<Restaurant> BuildList(int amount)
        {
            List<Restaurant> restaurants = new List<Restaurant>();
            for (int i = 0; i < amount; i++)
            {
                restaurants.Add(this.WithId().Build());
            }
            return restaurants;
        }

        public RestaurantBuilder WithoutName()
        {
            restaurant.Name = null;
            return this;
        }
    }
}
