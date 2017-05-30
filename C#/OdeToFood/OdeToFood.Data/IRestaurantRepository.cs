using System.Collections.Generic;
using OdeToFood.Data.DomainClasses;

namespace OdeToFood.Data
{
    public interface IRestaurantRepository
    {
        IEnumerable<Restaurant> GetAll();
        Restaurant Get(int index);
        Restaurant Add(Restaurant restaurant);
        Restaurant Update(Restaurant restaurant);
        void Delete(int id);
    }
}
