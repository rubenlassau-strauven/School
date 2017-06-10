using System;
using OdeToFood.Data.DomainClasses;

namespace OdeToFood.Api.Tests.Utilities
{
    public class ReviewBuilder
    {
        private Review review;
        private Restaurant restaurant;

        public ReviewBuilder()
        {
            restaurant = new Restaurant
            {
                Id = new Random().Next(1, int.MaxValue),
                Name = Guid.NewGuid().ToString(),
                Country = Guid.NewGuid().ToString(),
                City = Guid.NewGuid().ToString()
            };
            review = new Review
            {
                Body = Guid.NewGuid().ToString(),
                Rating = new Random().Next(1, 11),
                RestaurantId = restaurant.Id,
                Restaurant = restaurant,
                ReviewerName = Guid.NewGuid().ToString()
            };
        }

        public Review Build()
        {
            return review;
        }

        public ReviewBuilder WithId(int id = 0)
        {
            if (id == 0)
            {
                review.Id = new Random().Next(1, int.MaxValue);
            }
            else
            {
                review.Id = id;
            }
            return this;
        }

        public ReviewBuilder WithoutName()
        {
            review.ReviewerName = null;
            return this;
        }
    }
}
