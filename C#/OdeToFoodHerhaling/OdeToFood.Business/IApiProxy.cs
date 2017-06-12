using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OdeToFood.Data.DomainClasses;

namespace OdeToFood.Business
{
    public interface IApiProxy
    {
        void RegisterAsDummyUserAndUseBearerToken();
        Task<IEnumerable<Review>> GetReviewsAsync();
        Task<Restaurant> GetRestaurantByIdAsync(int id);
        Task<Review> GetReviewByIdAsync(int id);
        Task<bool> PutReviewAsync(int id, Review review);
        Task<bool> PostReviewAsync(Review review);
        Task<bool> DeleteReviewAsync(int id);
    }
}
