namespace E_Commerce.APIS.DTOs
{
    public class MakeReviewDto
    {
        public string Comment { get; set; }
        public int NumberOfPoint { get; set; }
        public DateTime CreatedAt { get; set; }
        public string AppUserId { get; set; }
    }
}
