using System;
using System.ComponentModel.DataAnnotations;

namespace LuanAnTotNghiep_TuanVu_TuBac.Models.DTOs
{
    public class UpdateUserRequest
    {
        [Required(ErrorMessage = "Họ và tên không được để trống")]
        [MaxLength(255, ErrorMessage = "Họ và tên không được vượt quá 255 ký tự")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Số điện thoại không được để trống")]
        [RegularExpression(@"^\d{10,15}$", ErrorMessage = "Số điện thoại phải có từ 10 đến 15 chữ số")]
        public string PhoneNumber { get; set; }

        public DateTime? BirthDate { get; set; } // Có thể để null nếu người dùng không cập nhật

        [Required(ErrorMessage = "Giới tính không được để trống")]
        [RegularExpression("Nam|Nữ|Khác", ErrorMessage = "Giới tính phải là 'Nam', 'Nữ' hoặc 'Khác'")]
        public string Gender { get; set; }

        [MaxLength(500, ErrorMessage = "Địa chỉ không được vượt quá 500 ký tự")]
        public string Address { get; set; }
    }
}
