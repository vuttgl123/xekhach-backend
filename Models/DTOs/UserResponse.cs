using System;
using System.ComponentModel.DataAnnotations;

namespace LuanAnTotNghiep_TuanVu_TuBac.Models.DTOs
{
    public class UserResponse
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Email không được để trống")]
        [EmailAddress(ErrorMessage = "Email không hợp lệ")]
        [MaxLength(320, ErrorMessage = "Email không được vượt quá 320 ký tự")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Họ và tên không được để trống")]
        [MaxLength(255, ErrorMessage = "Họ và tên không được vượt quá 255 ký tự")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Số điện thoại không được để trống")]
        [RegularExpression(@"^\d{10,15}$", ErrorMessage = "Số điện thoại phải có từ 10 đến 15 chữ số")]
        [MaxLength(15)]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Ngày sinh không được để trống")]
        public string BirthDate { get; set; } // Chuyển đổi từ DateTime? sang string để dễ kiểm soát trên FE

        [Required(ErrorMessage = "Giới tính không được để trống")]
        [RegularExpression("Nam|Nữ|Khác", ErrorMessage = "Giới tính phải là 'Nam', 'Nữ' hoặc 'Khác'")]
        [MaxLength(10)]
        public string Gender { get; set; }

        [MaxLength(500, ErrorMessage = "Địa chỉ không được vượt quá 500 ký tự")]
        public string Address { get; set; }
    }
}
