using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OdeToFood.Data.DomainClasses;

namespace OdeToFood.Data
{
    public class ReviewDbRepository : IReviewRepository
    {
        private OdeToFoodContext _context;

        public ReviewDbRepository(OdeToFoodContext context)
        {
            _context = context;
        }

        public async Task<List<Review>> GetAllAsync()
        {
            return await _context.Reviews.ToListAsync();
        }

        public async Task<Review> GetAsync(int id)
        {
            return await _context.Reviews.FindAsync(id);
        }

        public async Task<Review> PostAsync(Review review)
        {
            _context.Reviews.Add(review);
            await _context.SaveChangesAsync();
            return review;
        }

        public async Task<Review> PutAsync(Review review)
        {
            var original = _context.Restaurants.FindAsync(review.Id);
            var entry = _context.Entry(original);
            entry.CurrentValues.SetValues(review);
            await _context.SaveChangesAsync();
            return review;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var deleted = _context.Reviews.Remove(_context.Reviews.Find(id));
            await _context.SaveChangesAsync();
            if (deleted == null)
                return false;
            return true;
        }
    }
}
