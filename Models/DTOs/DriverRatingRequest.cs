namespace LuanAnTotNghiep_TuanVu_TuBac.DTOs
{
    public class DriverRatingRequest
    {
        public int RideId { get; set; }
        public int UserId { get; set; }
        public byte RatingValue { get; set; }  // Từ 1 đến 5
        public string Feedback { get; set; }
    }
}
