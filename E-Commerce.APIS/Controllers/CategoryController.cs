using AutoMapper;
using E_Commerce.APIS.DTOs;
using E_Commerce.APIS.Errors;
using E_Commerce.Core.Entities;
using E_Commerce.Core.Repositories.Contract;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Query;

namespace E_Commerce.APIS.Controllers
{
   
    public class CategoryController : BaseApiController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CategoryController(IUnitOfWork unitOfWork,IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoriesDto>>> GetAllCategories()
        {
            var categories = await _unitOfWork.Repository<Category>().GetAllAsync();
            var categoryIds= categories.Select(c => c.Id).ToList();
            var products= await _unitOfWork.Repository<Product>().GetAllAsync();
            var countByCategory= products.GroupBy(p=>p.CategoryId).ToDictionary(g => g.Key, g => g.Count());


            var data =  _mapper.Map<IReadOnlyList<Category>, IReadOnlyList<CategoriesDto>>(categories, opt => opt.Items["countByCategory"] = countByCategory);
            return Ok(data);
        }
        [HttpPost]
        public async Task<ActionResult> CreateCategory(CreateCategoryDto categoryDto)
        {
            if(!ModelState.IsValid) return BadRequest(ModelState);
            var existingCategory= await _unitOfWork.Repository<Category>().GetByConditionAsync(c=>c.Name==categoryDto.Name);
            if (existingCategory != null)
                return BadRequest(new ApiResponse(400, "Category already exists."));
            var category = new Category
            {
                Id = Guid.NewGuid().ToString(),
                Name = categoryDto.Name,
                Description = categoryDto.Description,
            };
            await _unitOfWork.Repository<Category>().AddAsync(category);
            await _unitOfWork.CompleteAsync();
            return Ok(new { message = "Category created successfully", category });
        }
        [HttpPut("{Id}")]
        public async Task<ActionResult> UpdateCategory([FromRoute]string Id , [FromBody] UpdateCategoryDto updateCategory)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var category = await _unitOfWork.Repository<Category>().GetByIdAsync(Id);
            if (category == null)
                return NotFound(new ApiResponse(404, "Category Not Found"));
            if (!string.IsNullOrWhiteSpace(updateCategory.Name)) category.Name = updateCategory.Name;
            if (!string.IsNullOrWhiteSpace(updateCategory.Description)) category.Description = updateCategory.Description;
             _unitOfWork.Repository<Category>().UpdateAsync(category);
            await _unitOfWork.CompleteAsync();
            return Ok(new {message="Updated Created Successfuly"});
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteCategory([FromRoute] string id)
        {
            var category = await _unitOfWork.Repository<Category>().GetByIdAsync(id);
            if (category == null)
                return NotFound(new ApiResponse(404, "Category Not Found"));

             _unitOfWork.Repository<Category>().DeleteAsync(category);
            await _unitOfWork.CompleteAsync();

            return Ok(new { message = "Deleted   Successfuly" });
        }
    }
}
