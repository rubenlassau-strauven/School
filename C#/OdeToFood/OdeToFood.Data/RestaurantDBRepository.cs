using System.Collections.Generic;
using System.Linq;
using OdeToFood.Data.DomainClasses;

namespace OdeToFood.Data
{
    public class RestaurantDbRepository : IRestaurantRepository
    {
        private readonly OdeToFoodContext _context;

        public RestaurantDbRepository(OdeToFoodContext context)
        {
            _context = context;
        }

        public Restaurant Add(Restaurant restaurant)
        {
            _context.Restaurants.Add(restaurant);
            _context.SaveChanges();
            return restaurant;
        }

        public void Delete(int id)
        {
            _context.Restaurants.Remove(this.Get(id));
            _context.SaveChanges();
        }

        public Restaurant Get(int index)
        {
            return _context.Restaurants.First(r => r.Id == index);
        }

        public IEnumerable<Restaurant> GetAll()
        {
            return _context.Restaurants;
        }

        public Restaurant Update(Restaurant restaurant)
        {
            var original = _context.Restaurants.First(r => r.Id == restaurant.Id);
            var entry = _context.Entry(original);
            entry.CurrentValues.SetValues(restaurant);
            _context.SaveChanges();
            return restaurant;
        }
    }
}
