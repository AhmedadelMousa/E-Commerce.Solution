using AutoMapper;
using E_Commerce.APIS.DTOs;
using E_Commerce.APIS.Errors;
using E_Commerce.Core.Entities.Basket;
using E_Commerce.Core.Repositories.Contract;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.APIS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BasketController : ControllerBase
    {
        private readonly IBasketRepository _basket;
        private readonly IMapper _mapper;

        public BasketController(IBasketRepository basket,IMapper mapper)
        {
            _basket = basket;
            _mapper = mapper;
        }

        [HttpGet("{Id}")]
        public async Task<ActionResult<CustomerBasket>> GetBasketAsync(string Id)
        {
             
            return Ok(await _basket.GetBasketAsync(Id) ?? new CustomerBasket(Id));
        }

        [HttpPost]
        public async Task<ActionResult<CustomerBasket>> CreateOrUpdateBasketAsync(CustomerBasketDto basketDto)//To Add Or Update Basket
        {
            if (string.IsNullOrEmpty(basketDto.Id))
              basketDto.Id = Guid.NewGuid().ToString();
            var MappedBasket = _mapper.Map<CustomerBasketDto, CustomerBasket>(basketDto);
           
            var CreateOrUpdateBasket= await _basket.UpdateBasketAsync(MappedBasket);
            return CreateOrUpdateBasket is not null ? Ok(CreateOrUpdateBasket) : BadRequest(new ApiResponse(400));
        }
        [HttpDelete("{Id}")]
        public async Task DeleteAsync(string Id)
        {
            await _basket.DeleteBasketAsync(Id);
        }
    }
}
