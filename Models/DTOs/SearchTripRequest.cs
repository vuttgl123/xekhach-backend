namespace LuanAnTotNghiep_TuanVu_TuBac.Models.DTOs
{
    public class SearchTripRequest
    {
        public string Origin { get; set; }
        public string Destination { get; set; }
        public DateTime Date { get; set; }
    }
}
