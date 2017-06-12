using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OdeToFood.Data.DomainClasses;

namespace OdeToFood.Api.Tests.TestServices
{
    public class ReviewBuilder
    {
        public Review review { get; }

        public ReviewBuilder()
        {
            review = new Review();
            review.Body = RandomGenerator.String();
            review.Rating = RandomGenerator.Integer();
            review.Restaurant = RandomGenerator.Restaurant();
            review.RestaurantId = review.Restaurant.Id;
            review.ReviewerName = RandomGenerator.String();
        }

        public Review Build()
        {
            return review;
        }

        public List<Review> BuildList(int amount)
        {
            List<Review> reviews = new List<Review>();
            for (int i = 0; i < amount; i++)
            {
                reviews.Add(new ReviewBuilder().Build());
            }
            return reviews;
        }

        public ReviewBuilder WithId(int id = -1)
        {
            if (id < 0)
            {
                review.Id = RandomGenerator.Integer();
            }
            else
            {
                review.Id = id;
            }
            return this;
        }
    }
}
