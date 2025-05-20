namespace LuanAnTotNghiep_TuanVu_TuBac.Models.DTOs
{
    public class DriverRatingResponse
    {
        public int Id { get; set; }
        public int RatingValue { get; set; }
        public string Feedback { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
