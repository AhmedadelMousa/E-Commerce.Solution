namespace E_Commerce.APIS.Errors
{
    public class ApiResponse
    {
        public int StatusCode { get; set; }
        public string? Message { get; set; }
        public ApiResponse(int statusCode, string? message = null)
        {
            StatusCode = statusCode;
            Message = message ?? GetDefaultMessageForStatusCode(statusCode);
        }
        private string? GetDefaultMessageForStatusCode(int statusCode)
        {
            return statusCode switch
            {
                400 => "BadRequest",
                401 => "UnAuthorized",
                404 => "NotFound",
                500 => "ServerError",

                _ => null
            };
        }
    }
}
