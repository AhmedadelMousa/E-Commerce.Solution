using AutoMapper;
using E_Commerce.APIS.DTOs;
using E_Commerce.Core.Entities;
using E_Commerce.Core.Entities.Basket;
using E_Commerce.Core.Entities.Favorite;
using E_Commerce.Core.Order_Aggregrate;
using Microsoft.EntityFrameworkCore.Update.Internal;

namespace E_Commerce.APIS.Helpers
{
    public class MappingProfiles:Profile
    {
        public MappingProfiles()
        {
            CreateMap<Product, ProductsDto>()
                .ForMember(p => p.ProductId, d => d.MapFrom(s => s.Id))
                .ForMember(p => p.Name, d => d.MapFrom(s => s.Name))
                .ForMember(p => p.Price, d => d.MapFrom(s => s.Price))
                .ForMember(p => p.Rate, d => d.MapFrom(s => s.Reviews.Sum(r => Math.Floor((decimal)r.NumberOfPoint / s.Reviews.Count))))
                .ForMember(p => p.CategoryName, d => d.MapFrom(s => s.Category.Name))
                .ForMember(p => p.CategoryId, d => d.MapFrom(s => s.CategoryId))
                .ForMember(p => p.StockQuantity, d => d.MapFrom(s => s.StockQuantity))
                .ForMember(p => p.PictureUrl, d => d.MapFrom<PictureUrlResolver>())
                .ForMember(p => p.IsInFav, d => d.MapFrom((src, dest, _, context) =>
                {
                    if (context.Items.TryGetValue("FavoriteProductIds", out var favoriteProductIdsObj))
                    {
                        var favoriteProductIds = (List<string>)favoriteProductIdsObj;
                        return favoriteProductIds.Contains(src.Id);
                    }
                    return false;
                }))
                .ForMember(p => p.IsInCart, d => d.MapFrom((src, dest, _, context) =>
                {
                    if (context.Items.TryGetValue("CartProductIds", out var cartProductIdsObj))
                    {
                        var cartProductIds = (List<string>)cartProductIdsObj;
                        return cartProductIds.Contains(src.Id);
                    }
                    return false;
                }));
            CreateMap<Product, ProductDetailsResponseDto>()
                 .ForMember(p => p.Name, d => d.MapFrom(s => s.Name))
                 .ForMember(p => p.Price, d => d.MapFrom(s => s.Price))
                 .ForMember(p => p.CategoryName, d => d.MapFrom(s => s.Category.Name))
                 .ForMember(p => p.CategoryId, d => d.MapFrom(s => s.CategoryId))
                 .ForMember(p => p.PictureUrl, d => d.MapFrom<PictureUrlResolver>())
                 .ForMember(p => p.Reviews, d => d.MapFrom(s => s.Reviews))
                 .ForMember(p => p.StockQuantity, d => d.MapFrom(s => s.StockQuantity))
                 .ForMember(p => p.Colors, d => d.MapFrom(s => s.Colors))
                 .ForMember(p => p.Description, d => d.MapFrom(s => s.Description))
                 .ForMember(p => p.Size, d => d.MapFrom(s => s.Size))
                  .ForMember(p => p.IsInFav, d => d.MapFrom((src, dest, _, context) =>
                  {
                      if (context.Items.TryGetValue("FavoriteProductIds", out var favoriteProductIdsObj))
                      {
                          var favoriteProductIds = (List<string>)favoriteProductIdsObj;
                          return favoriteProductIds.Contains(src.Id);
                      }
                      return false;
                  }))
                .ForMember(p => p.IsInCart, d => d.MapFrom((src, dest, _, context) =>
                {
                    if (context.Items.TryGetValue("CartProductIds", out var cartProductIdsObj))
                    {
                        var cartProductIds = (List<string>)cartProductIdsObj;
                        return cartProductIds.Contains(src.Id);
                    }
                    return false;
                }));

            CreateMap<Review, MakeReviewDto>()
                .ForMember(p => p.Comment, d => d.MapFrom(s => s.Comment))
                .ForMember(p => p.NumberOfPoint, d => d.MapFrom(s => s.NumberOfPoint))
                .ForMember(p => p.CreatedAt, d => d.MapFrom(s => s.CreatedAt))
                .ForMember(p => p.AppUserId, d => d.MapFrom(s => s.AppUserId));
            CreateMap<UpdateProductDto,Product>()
                .ForMember(p => p.Name, d => d.MapFrom(s => s.Name))
                .ForMember(p => p.Description, d => d.MapFrom(s => s.Description))
                .ForMember(p => p.Price, d => d.MapFrom(s => s.Price))
                .ForMember(p => p.CategoryId, d => d.MapFrom(s => s.CategoryId))
                .ForMember(p => p.Size, d => d.MapFrom(s => s.Size))
                .ForMember(p => p.Colors, d => d.MapFrom(s => s.Colors))
                .ForMember(p => p.StockQuantity, d => d.MapFrom(s => s.StockQuantity))
                .ForMember(dest => dest.PictureUrl, opt => opt.Ignore());
            CreateMap<Category, CategoriesDto>()
                .ForMember(p => p.Name, d => d.MapFrom(s => s.Name))
                .ForMember(p => p.Id, d => d.MapFrom(s => s.Id))
                .ForMember(p => p.Description, d => d.MapFrom(s => s.Description))
                .ForMember(p =>p.Count,d=>d.MapFrom((src,dest, _,context)=>
                {
                    if(context.Items.TryGetValue("countByCategory",out var countByCategoryObj))
                      {
                        var countByCategory = (Dictionary<string, int>)countByCategoryObj;
                        return countByCategory.TryGetValue(src.Id, out var count) ? count : 0;
                      }
                    return 0;
                }
                ));
            CreateMap<AddressDto, AddressOrder>().ReverseMap();
            CreateMap<CustomerBasketDto, CustomerBasket>()
                .ForMember(p => p.Items, d => d.MapFrom(s => s.BasketItems));
            CreateMap<BasketItemDto, BasketItem>()
                .ReverseMap();
            CreateMap<Order, OrderToReturnDto>().ForMember(dest => dest.Total, opt => opt.MapFrom(src => src.Total))
                .ForMember(p => p.DeliveryMethodCost, d => d.MapFrom(s => s.DeliveryMethod.Cost))
                .ForMember(p => p.DeliveryMethod, d => d.MapFrom(s => s.DeliveryMethod.ShortName))
                ;
            CreateMap<OrderItem,OrderItemDto>()
                 .ForMember(p => p.Product, d => d.MapFrom(s => s.Product))
                  .ForMember(p => p.Quantity, d => d.MapFrom(s => s.Quantity))
                   .ForMember(p => p.Price, d => d.MapFrom(s => s.Price))
                ;
            CreateMap<Order, OrderDto>()
                .ForMember(p => p.DeliveryMethodId, d => d.MapFrom(s => s.DeliveryMethodId)).ReverseMap()
                ;
            CreateMap<Order, GetAllOrders>()
                .ForMember(p => p.OrderId, d => d.MapFrom(s => s.Id))
                .ForMember(p => p.DeliveryMethod, d => d.MapFrom(s => s.DeliveryMethod.ShortName))
                .ForMember(dest => dest.Total,
                opt => opt.MapFrom(src => src.SubTotal + src.DeliveryMethod.Cost))
                 .ForMember(p => p.PaymentMethod, d => d.MapFrom(s => s.paymentMethod))
                  .ForMember(p => p.Status, d => d.MapFrom(s => s.Status))
                ;
            CreateMap<Order, OrderDetailsDto>()
                .ForMember(p => p.PaymentMethod, d => d.MapFrom(s => s.paymentMethod.ToString()))
                .ForMember(p => p.ShippingAddress, d => d.MapFrom(s => s.ShippingAddress))
                .ForMember(dest => dest.Total,
                opt => opt.MapFrom(src => src.SubTotal + src.DeliveryMethod.Cost))
                .ForMember(p => p.DeliveryMethodId, d => d.MapFrom(s => s.DeliveryMethodId));
            CreateMap<ProductItemOrdered, ProductItemOrderedDto>()
                  .ForMember(p => p.ProductUrl, d => d.MapFrom<PictureUrlResolver>())
                   .ForMember(p => p.ProductId, d => d.MapFrom(s => s.ProductId))
                    .ForMember(p => p.ProductName, d => d.MapFrom(s => s.ProductName));
            CreateMap<AddItemDto, BasketItem>()
                .ForMember(p => p.Id, d => d.MapFrom(s => s.ProductId))
                 .ForMember(p => p.Quentity, d => d.MapFrom(s => s.Quantity));
            CreateMap<CustomerFavoriteDto, CustomerFavorite>();
            CreateMap<AddItemDto, FavoriteItem>()
               .ForMember(p => p.Id, d => d.MapFrom(s => s.ProductId))
                .ForMember(p => p.Quantity, d => d.MapFrom(s => s.Quantity));







        }
    }
}
