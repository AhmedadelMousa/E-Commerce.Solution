namespace E_Commerce.APIS.DTOs
{
    public class GetOrdersForAdminDto:GetAllOrdersDto
    {
        public string  UserName { get; set; }
    }
}
