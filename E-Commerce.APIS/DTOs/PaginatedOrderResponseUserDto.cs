namespace E_Commerce.APIS.DTOs
{
    public class PaginatedOrderResponseUserDto:ResponsePaginationOrderDto
    {
        public List<GetAllOrdersDto> Orders { get; set; }
    }
}
