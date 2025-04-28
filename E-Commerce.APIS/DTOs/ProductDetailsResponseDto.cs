namespace E_Commerce.APIS.DTOs
{
    public class ProductDetailsResponseDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string PictureUrl { get; set; }
        public string Size { get; set; }
        public string Colors { get; set; }
        public int StockQuantity { get; set; }
        public decimal Price { get; set; }
        public List<MakeReviewDto> Reviews { get; set; }
        public string CategoryName { get; internal set; }
        public string CategoryId { get; internal set; }
        public bool IsInFav { get; set; }
        public bool IsInCart { get; set; }
    }
}
