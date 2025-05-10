namespace E_Commerce.APIS.DTOs
{
    public class PaginatedOrderResponseAdminDto:ResponsePaginationOrderDto
    {
        public List<GetOrdersForAdminDto> Orders { get; set; }

    }
}
