using AutoMapper;
using E_Commerce.APIS.DTOs;
using E_Commerce.Core.Entities;
using E_Commerce.Core.Entities.Basket;
using E_Commerce.Core.Entities.Favorite;
using E_Commerce.Core.Order_Aggregrate;

namespace E_Commerce.APIS.Helpers
{
    public class PictureUrlResolver 
        : IValueResolver<Product, ProductDetailsResponseDto, string>, IValueResolver<Product, ProductsDto, string>,IValueResolver<ProductItemOrdered, ProductItemOrderedDto, string>,IValueResolver<BasketItem,BasketItemDto,string>,IValueResolver<FavoriteItem,FavoriteItem,string>

    {
        private readonly IConfiguration _configuration;

        public PictureUrlResolver(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public string Resolve(Product source, ProductDetailsResponseDto destination, string destMember, ResolutionContext context)
        {
            return GetProductPicture(source.PictureUrl);
        }

        public string Resolve(Product source, ProductsDto destination, string destMember, ResolutionContext context)
        {
            return GetProductPicture(source.PictureUrl);
        }

        public string Resolve(ProductItemOrdered source, ProductItemOrderedDto destination, string destMember, ResolutionContext context)
        {
            return GetProductPicture(source.ProductUrl);
        }

        public string Resolve(BasketItem source, BasketItemDto destination, string destMember, ResolutionContext context)
        {
            return GetProductPicture(source.PictureUrl);
        }

        public string Resolve(FavoriteItem source, FavoriteItem destination, string destMember, ResolutionContext context)
        {
            return GetProductPicture(source.PictureUrl);
        }

        private string GetProductPicture(string pictureUrl)
        {
            if (!string.IsNullOrEmpty(pictureUrl))
            {
                return $"{_configuration["ApiBaseUrl"]}/{pictureUrl.TrimStart('/')}";
            }
            return null;
        }
    }
}
