using System;
using OdeToFood.Data.DomainClasses;

namespace OdeToFood.Api.Tests.Utilities
{
    public class RestaurantBuilder
    {
        Restaurant res = new Restaurant
        {
            City = Guid.NewGuid().ToString(),
            Country = Guid.NewGuid().ToString(),
            Name = Guid.NewGuid().ToString()
        };

        public Restaurant Build()
        {
            return res;
        }

        public RestaurantBuilder WithId(int id = 0)
        {
            if (id == 0)
            {
                res.Id = new Random().Next();
            }
            else
            {
                res.Id = id;
            }
            return this;
        }

        public RestaurantBuilder WithoutName()
        {
            res.Name = null;
            return this;
        }
    }
}
