using System.Threading.Tasks;
using System.Web.Http;
using OdeToFood.Data;
using OdeToFood.Data.DomainClasses;

namespace OdeToFood.Api.Controllers
{
    [RoutePrefix("api/Reviews")]
    public class ReviewsController : ApiController
    {
        private IReviewRepository _repository;

        public ReviewsController(IReviewRepository _repository)
        {
            this._repository = _repository;
        }

        // GET: api/Review
        public async Task<IHttpActionResult> Get()
        {
            var reviews = await _repository.GetAllAsync();
            return Ok(reviews);
        }

        // GET: api/Review/5
        [Route("{id:int}")]
        public async Task<IHttpActionResult> Get(int id)
        {
            var review = await _repository.GetAsync(id);
            if (review != null)
                return Ok(review);
            return NotFound();
        }

        // POST: api/Review
        public async Task<IHttpActionResult> Post(Review review)
        {
            if (ModelState.IsValid)
            {
                var addedReview = await _repository.AddAsync(review);
                return CreatedAtRoute("DefaultApi", new {controller = "Review", id = addedReview.Id}, addedReview);
            }
            return BadRequest();
        }

        // PUT: api/Review/5
        public async Task<IHttpActionResult> Put(int id, Review review)
        {
            if (!ModelState.IsValid || id != review.Id)
                return BadRequest();
            if (_repository.GetAsync(id).Result != null)
            {
                var updatedReview = await _repository.UpdateAync(review);
                return Ok();
            }
            return NotFound();
        }

        // DELETE: api/Review/5
        public async Task<IHttpActionResult> Delete(int id)
        {
            if (_repository.GetAsync(id).Result != null)
            {
                await _repository.DeleteAsync(id);
                return Ok();
            }
            return NotFound();
        }
    }
}
