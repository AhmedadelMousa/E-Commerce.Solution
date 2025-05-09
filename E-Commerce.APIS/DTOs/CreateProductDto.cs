﻿using E_Commerce.Core.Entities;

namespace E_Commerce.APIS.DTOs
{
    public class CreateProductDto
    {
       
        public string Name { get; set; }
        public string Description { get; set; }
        public IFormFile Picture { get; set; }
        public string[] Sizes { get; set; }
        public string[] Colors { get; set; }
        public int StockQuantity { get; set; }
        public decimal Price { get; set; }
        public string CategoryId { get; set; }
       
    }
}
