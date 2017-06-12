using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OdeToFood.Data.DomainClasses;

namespace OdeToFood.Data
{
    public class RestaurantDbRepository : IRestaurantRepository
    {
        private OdeToFoodContext _context;

        public RestaurantDbRepository(OdeToFoodContext context)
        {
            _context = context;
        }

        public IEnumerable<Restaurant> GetAll()
        {
            return _context.Restaurants.ToList();
        }

        public Restaurant Get(int id)
        {
            return _context.Restaurants.Find(id);
        }

        public Restaurant Post(Restaurant restaurant)
        {
            var addedRestaurant = _context.Restaurants.Add(restaurant);
            _context.SaveChanges();
            return addedRestaurant;
        }

        public Restaurant Put(Restaurant restaurant)
        {
            var original = _context.Restaurants.Find(restaurant.Id);
            var entry = _context.Entry(original);
            entry.CurrentValues.SetValues(restaurant);
            _context.SaveChanges();
            return restaurant;
        }

        public void Delete(int id)
        {
            _context.Restaurants.Remove(_context.Restaurants.Find(id));
            _context.SaveChanges();
        }
    }
}
