using E_Commerce.Core.Order_Aggregrate;
using E_Commerce.Core.Repositories.Contract;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.APIS.Controllers
{
   
    public class DeliveryMethodController :BaseApiController
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeliveryMethodController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<DeliveryMethod>>> GetAllDeliveryMethod()
        {
            var delivery= await _unitOfWork.Repository<DeliveryMethod>().GetAllAsync();
            return Ok(delivery);
        }
    }
}
