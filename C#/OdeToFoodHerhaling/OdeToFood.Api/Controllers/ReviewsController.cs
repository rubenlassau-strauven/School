using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;
using OdeToFood.Data;
using OdeToFood.Data.DomainClasses;

namespace OdeToFood.Api.Controllers
{
    [Authorize(Roles = "User")]
    public class ReviewsController : ApiController
    {
        private IReviewRepository _repository;

        public ReviewsController(IReviewRepository repository)
        {
            _repository = repository;
        }

        // GET: api/Reviews
        public async Task<IHttpActionResult> Get()
        {
            return Ok(await _repository.GetAllAsync());
        }

        // GET: api/Reviews/5
        public async Task<IHttpActionResult> Get(int id)
        {
            var review = await _repository.GetAsync(id);
            if (review == null)
                return NotFound();
            return Ok(review);
        }

        // POST: api/Reviews
        public async Task<IHttpActionResult> Post(Review review)
        {
            if (!ModelState.IsValid)
                return BadRequest();
            var createdReview = await _repository.PostAsync(review);
            return CreatedAtRoute("DefaultApi", new { controller = "Reviews", id = createdReview.Id }, createdReview);
        }

        // PUT: api/Reviews/5
        public async Task<IHttpActionResult> Put(int id, Review review)
        {
            if (!ModelState.IsValid || id != review.Id)
                return BadRequest();
            if (await _repository.GetAsync(id) == null)
                return NotFound();
            var updatedReview = await _repository.PutAsync(review);
            return Ok();
        }

        // DELETE: api/Reviews/5
        public async Task<IHttpActionResult> Delete(int id)
        {
            if (await _repository.GetAsync(id) == null)
                return NotFound();
            await _repository.DeleteAsync(id);
            return Ok();
        }
    }
}
