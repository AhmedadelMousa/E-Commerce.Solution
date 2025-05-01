using E_Commerce.APIS.DTOs;
using E_Commerce.APIS.Errors;
using E_Commerce.Core.Entities;
using E_Commerce.Core.Repositories.Contract;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.APIS.Controllers
{
   
    public class ReviewController :BaseApiController
    {
        private readonly IUnitOfWork _unitOfWork;

        public ReviewController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        [HttpPost]
        public async Task<ActionResult>CreateReview(Review review)
        {
             if(!ModelState.IsValid)
                    return BadRequest(ModelState);
             _unitOfWork.Repository<Review>().AddAsync(review);
            await _unitOfWork.CompleteAsync();
            return Ok(new {message="Review Created "});
        }
        [HttpPut("{Id}")]
        public async Task<ActionResult> UpdateReview([FromRoute] string Id, Review review)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var Review = await _unitOfWork.Repository<Review>().GetByIdAsync(Id);
            _unitOfWork.Repository<Review>().UpdateAsync(review);
            await _unitOfWork.CompleteAsync();
            return Ok(new {message="Review Updated"} );

        }
        [HttpDelete("{Id}")]
        public async Task<ActionResult>DeleteReview(string Id)
        {
          var review= await _unitOfWork.Repository<Review>().GetByIdAsync(Id);
            if (review is null)
                return NotFound(new ApiResponse(404,"Review NotFound"));
            _unitOfWork.Repository<Review>().DeleteAsync(review);
            await _unitOfWork.CompleteAsync();
            return Ok(new { message = "Review Deleted" });
        }


    }
}
