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
        Task<List<Review>> GetAllAsync();
        Task<Review> GetAsync(int id);
        Task<Review> PostAsync(Review review);
        Task<Review> PutAsync(Review review);
        Task<bool> DeleteAsync(int id);
    }
}
