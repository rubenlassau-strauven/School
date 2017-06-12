using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OdeToFood.Data.DomainClasses;

namespace OdeToFood.Data
{
    public interface IRestaurantRepository
    {
        IEnumerable<Restaurant> GetAll();
        Restaurant Get(int id);
        Restaurant Post(Restaurant restaurant);
        Restaurant Put(Restaurant restaurant);
        void Delete(int id);
    }
}
