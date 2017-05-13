using OdeToFood.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.WebPages;
using OdeToFood.Data.DomainClasses;

namespace OdeToFood.Api.Controllers
{
    [RoutePrefix("api/Restaurants")]
    public class RestaurantsController : ApiController
    {
        private IRestaurantRepository _restaurantRepository;

        public RestaurantsController(IRestaurantRepository repository)
        {
            _restaurantRepository = repository;
        }

        // GET: api/Restaurants
        public IHttpActionResult Get()
        {
            try
            {
                return Ok(_restaurantRepository.GetAll());
            }
            catch (Exception e)
            {
                return InternalServerError(e);
            } 
        }

        // GET: api/Restaurants/5
        [Route("{id:int}")]
        public IHttpActionResult Get(int id)
        {
            Restaurant res = _restaurantRepository.Get(id);
            if (res != null)
                return Ok(res);
            return NotFound();
        }

        // POST: api/Restaurants
        public IHttpActionResult Post(Restaurant restaurant)
        {
            if (ModelState.IsValid)
            {
                var createdRestaurant = _restaurantRepository.Add(restaurant);
                return CreatedAtRoute("DefaultApi", new { controller = "Restaurants", id = createdRestaurant.Id}, createdRestaurant);
            }
            return BadRequest();
        }

        // PUT: api/Restaurants/5
        public IHttpActionResult Put(int id, Restaurant restaurant)     //aparte id is nodig => api/restaurants/5 <-
        {
            //this.Url.Request.
            if (!ModelState.IsValid)
                return BadRequest();

            if (id != restaurant.Id)
                return BadRequest();

            if (_restaurantRepository.Get(id) != null)
            {
                _restaurantRepository.Update(restaurant);
                return Ok();
            }
            return NotFound();
        }

        // DELETE: api/Restaurants/5
        public IHttpActionResult Delete(int id)
        {
            if (_restaurantRepository.Get(id) != null)
            {
                _restaurantRepository.Delete(id);
                return Ok();
            }
            return NotFound();
        }
    }
}
