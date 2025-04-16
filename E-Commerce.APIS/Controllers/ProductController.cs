using AutoMapper;
using E_Commerce.APIS.DTOs;
using E_Commerce.APIS.Errors;
using E_Commerce.APIS.Helpers;
using E_Commerce.Core.Entities;
using E_Commerce.Core.Repositories.Contract;
using E_Commerce.Core.Specification.ProdutSpecification;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.APIS.Controllers
{

    public class ProductController : BaseApiController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ProductController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<ProductsDto>>> GetProducts([FromQuery] ProductSpecParam specParam)
        {
            var spec = new ProductWithCategorySpec(specParam);
            var products = await _unitOfWork.Repository<Product>().GetAllWithSpecAsync(spec);
            var data = _mapper.Map<IReadOnlyList<Product>, IReadOnlyList<ProductsDto>>(products);
            var countSpec = new ProductWithFilterationForCountSpec(specParam);
            var count = await _unitOfWork.Repository<Product>().GetCountAsync(countSpec);
            return Ok(new Pagination<ProductsDto>(specParam.PageSize, specParam.PageIndex, count, data));


        }
        [HttpPost]
        public async Task<ActionResult> CreateProduct([FromForm] CreateProductDto productDto)
        {
            if (productDto == null) return BadRequest(new ApiResponse(400, "Invalid Data"));
            string imgUrl = null;
            if (productDto.PictureUrl != null && productDto.PictureUrl.Length > 0)
            {
                //Route Location for img
                string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Img", "Products");
                //ensure the folder is exist , if not exist create it 
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }
                //Create UniqueFileName
                string fileName = $"{Guid.NewGuid()}{Path.GetExtension(productDto.PictureUrl.FileName)}";
                string filePath = Path.Combine(uploadsFolder, fileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await productDto.PictureUrl.CopyToAsync(fileStream);

                }
                imgUrl = $"Img/Products/{fileName}";
            }



            var Product = new Product
            {
                Id = Guid.NewGuid().ToString("N"),
                Name = productDto.Name,
                Colors = productDto.Colors,
                Description = productDto.Description,
                CategoryId = productDto.CategoryId,
                Price = productDto.Price,
                Size = productDto.Size,
                StockQuantity = productDto.StockQuantity,
                PictureUrl = imgUrl
            };
            await _unitOfWork.Repository<Product>().AddAsync(Product);
            await _unitOfWork.CompleteAsync();

            return Ok(new { message = "Product Created Succesfully" });
        }
        [HttpGet("{Id}")]
        public async Task<ActionResult<ProductDetailsResponseDto>>GetProductDetails([FromRoute]string Id)
        {
            var spec = new ProductWithCategorySpec(Id);
           
            var product= await _unitOfWork.Repository<Product>().GetWithSpecAsync(spec);
            if (product == null)
                return NotFound(new ApiResponse(404, "Product Not Found"));
            var specDto = _mapper.Map<Product, ProductDetailsResponseDto>(product);
            return Ok(specDto);

        }
        [HttpPut("{Id}")]
        public async Task<ActionResult> UpdateProduct([FromRoute] string Id, [FromForm] UpdateProductDto productDto)
        {
            var product = await _unitOfWork.Repository<Product>().GetByIdAsync(Id);
            if (product == null)
                return NotFound(new ApiResponse(404, "Product Not Found"));

            // تحديث البيانات فقط إذا تم إرسالها
            if (!string.IsNullOrEmpty(productDto.Name)) product.Name = productDto.Name;
            if (!string.IsNullOrEmpty(productDto.Description)) product.Description = productDto.Description;
            if (!string.IsNullOrEmpty(productDto.Size)) product.Size = productDto.Size;
            if (!string.IsNullOrEmpty(productDto.Colors)) product.Colors = productDto.Colors;
            if (productDto.StockQuantity.HasValue && productDto.StockQuantity > 0)
                product.StockQuantity = productDto.StockQuantity.GetValueOrDefault();

            if (productDto.Price.HasValue && productDto.Price > 0)
                product.Price = productDto.Price.GetValueOrDefault();
            if (!string.IsNullOrEmpty(productDto.CategoryId)) product.CategoryId = productDto.CategoryId;

            // ✅ تحديث الصورة إذا تم إرسال صورة جديدة
            if (productDto.PictureUrl != null && productDto.PictureUrl.Length > 0)
            {
                var uploadFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Img/products");
                if (!Directory.Exists(uploadFolder))
                {
                    Directory.CreateDirectory(uploadFolder);
                }

                // ✅ حذف الصورة القديمة إذا كانت موجودة
                if (!string.IsNullOrEmpty(product.PictureUrl))
                {
                    var oldImagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", product.PictureUrl);
                    if (System.IO.File.Exists(oldImagePath))
                    {
                        System.IO.File.Delete(oldImagePath);
                    }
                }

                var fileName = $"{Guid.NewGuid()}_{productDto.PictureUrl.FileName}";
                var filePath = Path.Combine(uploadFolder, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await productDto.PictureUrl.CopyToAsync(stream);
                }

                // تحديث رابط الصورة
                product.PictureUrl = $"Img/products/{fileName}";
            }

            _unitOfWork.Repository<Product>().UpdateAsync(product);
            await _unitOfWork.CompleteAsync();

            return Ok(new { message = "Product Updated Successfully" });
        }
        [HttpDelete("{Id}")]
        public async Task<ActionResult> DeleteProduct([FromRoute]string Id)
        {
            var product= await _unitOfWork.Repository<Product>().GetByIdAsync(Id);
            if (product == null) return NotFound(new ApiResponse(404, "Product Not Found"));
            //Delete Img
            if(!string.IsNullOrEmpty(product.PictureUrl))
            {
                var imagePath= Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", product.PictureUrl);
                if (System.IO.File.Exists(imagePath))
                {
                    System.IO.File.Delete(imagePath);
                }
            }
            _unitOfWork.Repository<Product>().DeleteAsync(product);
            await _unitOfWork.CompleteAsync();
            return Ok(new { message = "Product Deleted Successfully" });
        }
    }
}
