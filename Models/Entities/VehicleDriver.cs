namespace LuanAnTotNghiep_TuanVu_TuBac.Models.Entities
{
    public class VehicleDriver
    {
        public int Id { get; set; }               // Id (PK)
        public int VehicleId { get; set; }        // VehicleId (FK)
        public int DriverId { get; set; }         // DriverId (FK)
        public DateTime CreatedAt { get; set; }   // Ngày tạo
        public DateTime AssignedAt { get; set; }  // Ngày phân công
        public DateTime? UnassignedAt { get; set; } // Ngày bỏ phân công (nếu có)
        public int? AdminId { get; set; }         // AdminId (nếu có)

        // Các liên kết giữa các bảng khác (nếu có)
        public virtual Driver Driver { get; set; } // Mối quan hệ với bảng Driver
    }
}
