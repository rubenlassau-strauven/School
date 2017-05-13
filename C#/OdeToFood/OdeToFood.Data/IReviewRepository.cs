using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OdeToFood.Data.DomainClasses;

namespace OdeToFood.Data
{
    public interface IReviewRepository
    {
        //alle namen eindigen met Async
        //alle methodes geven een Task terug
        Task<List<Review>> GetAllAsync();
        Task<Review> GetAsync(int id);
        Task<Review> AddAsync(Review review);
        Task<Review> UpdateAync(Review review);
        Task<Review> DeleteAsync(int id);
    }
}
