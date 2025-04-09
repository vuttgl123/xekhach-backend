namespace LuanAnTotNghiep_TuanVu_TuBac.Models.Entities
{
    public class RouteTrip
    {
        public int Id { get; set; }
        public string Code { get; set; } // Mã tuyến (T01, T02...)
        public string Origin { get; set; }
        public string Destination { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public string Status { get; set; } // "HN-ĐN", "NĐ-HN", "Nghỉ"
        public int? VehicleDriverId { get; set; }
        public int? AdminId { get; set; }
        public DateTime CreatedAt { get; set; }
    }

}
