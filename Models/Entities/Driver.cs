namespace LuanAnTotNghiep_TuanVu_TuBac.Models.Entities
{
    public class Driver
    {
        public int Id { get; set; }                // Id (PK)
        public string VehicleType { get; set; }    // Loại xe
        public decimal Earnings { get; set; }      // Thu nhập
        public DateTime CreatedAt { get; set; }    // Ngày tạo
        public DateTime? UpdatedAt { get; set; }   // Ngày cập nhật
        public byte? Status { get; set; }          // Trạng thái
        public string AvatarUrl { get; set; }      // URL ảnh đại diện
        public string NationalID { get; set; }     // Số CMND
        public DateTime NationalIDIssuedDate { get; set; } // Ngày cấp CMND
        public string NationalIDIssuedPlace { get; set; } // Nơi cấp CMND
        public string Address { get; set; }        // Địa chỉ
        public DateTime? BirthDate { get; set; }   // Ngày sinh
        public string PhoneNumber { get; set; }    // Số điện thoại
        public string LicenseType { get; set; }    // Loại giấy phép lái xe
        public DateTime LicenseExpiryDate { get; set; } // Hạn giấy phép lái xe
        public string BankAccountNumber { get; set; } // Số tài khoản ngân hàng
        public string BankName { get; set; }       // Tên ngân hàng
        public string FullName { get; set; }       // Họ và tên
        public string OperatingArea { get; set; }  // Khu vực hoạt động
    }
}
