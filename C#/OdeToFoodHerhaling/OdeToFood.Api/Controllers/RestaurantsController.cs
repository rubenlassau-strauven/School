using OdeToFood.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Results;
using Microsoft.Owin.Security.Provider;
using OdeToFood.Data.DomainClasses;

namespace OdeToFood.Api.Controllers
{
    [System.Web.Mvc.Authorize(Roles = "User")]
    public class RestaurantsController : ApiController
    {
        private IRestaurantRepository _restaurantRepository;

        public RestaurantsController(IRestaurantRepository repo)
        {
            _restaurantRepository = repo;
        }
        // GET: api/Restaurants
        public IHttpActionResult Get()
        {
            return Ok(_restaurantRepository.GetAll());
        }

        // GET: api/Restaurants/5
        public IHttpActionResult Get(int id)
        {
            var restaurant = _restaurantRepository.Get(id);
            if (restaurant == null)
                return NotFound();
            return Ok(restaurant);
        }

        // POST: api/Restaurants
        public IHttpActionResult Post(Restaurant restaurant)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var createdRestaurant = _restaurantRepository.Post(restaurant);
            return CreatedAtRoute("DefaultApi",
                new {controller = "Restaurants", id = createdRestaurant.Id},
                createdRestaurant);
        }

        // PUT: api/Restaurants/5
        public IHttpActionResult Put(int id, Restaurant restaurant)
        {
            if (!ModelState.IsValid || id != restaurant.Id)
                return BadRequest();
            if (_restaurantRepository.Get(id) == null)
                return NotFound();
            var updatedRestaurant = _restaurantRepository.Put(restaurant);
            return Ok();
        }

        // DELETE: api/Restaurants/5
        public IHttpActionResult Delete(int id)
        {
            if (_restaurantRepository.Get(id) == null)
                return NotFound();
            _restaurantRepository.Delete(id);
            return Ok();
        }
    }
}
