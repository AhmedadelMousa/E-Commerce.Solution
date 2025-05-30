﻿using E_Commerce.Core.Entities.Identity;
using E_Commerce.Core.Entities;

namespace E_Commerce.APIS.DTOs
{
    public class ProductsDto
    {
        public string ProductId { get; set; }
        public string Name { get; set; }
      
        public string PictureUrl { get; set; }
        public string[] Sizes { get; set; }
        public string[] Colors { set; get; }
        public int StockQuantity { get; set; }
        public decimal Price { get; set; }
        public string CategoryId { set; get; }
        public string CategoryName { set; get; }
        public decimal Rate { set; get; }
        public bool IsInFav { get; set; }
        public bool IsInCart { get; set; }
    }
}
