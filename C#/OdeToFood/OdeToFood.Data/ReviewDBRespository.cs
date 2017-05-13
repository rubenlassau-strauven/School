using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using OdeToFood.Data.DomainClasses;

namespace OdeToFood.Data
{
    public class ReviewDBRespository : IReviewRepository
    {
        public OdeToFoodContext _context { get; set; }

        public ReviewDBRespository(OdeToFoodContext context)
        {
            _context = context;
        }

        public Task<IEnumerable<Review>> GetAllAsync()
        {
            return new Task<IEnumerable<Review>>(() => _context.Reviews.ToList());
        }

        public Task<Review> GetAsync(int id)
        {
            return new Task<Review>(() => _context.Reviews.ToList().Find(r => r.Id == id));
        }

        public Task<Review> AddAsync(Review review)
        {
            return new Task<Review>(() =>
            {
                _context.Reviews.Add(review);
                _context.SaveChanges();
                return review;
            });
        }

        public Task<Review> UpdateAync(Review review)
        {
            return new Task<Review>(() =>
            {
                var original = _context.Restaurants.First(r => r.Id == review.Id);
                var entry = _context.Entry(original);
                entry.CurrentValues.SetValues(review);
                _context.SaveChanges();
                return review;
            });
        }

        public Task<Review> DeleteAsync(int id)
        {
            return new Task<Review>(() =>
            {
                var reviewToDelete = this.GetAsync(id).Result;
                _context.Reviews.Remove(reviewToDelete);
                _context.SaveChanges();
                return reviewToDelete;
            });
        }
    }
}
