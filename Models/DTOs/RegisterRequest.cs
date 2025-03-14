using System.ComponentModel.DataAnnotations;

namespace LuanAnTotNghiep_TuanVu_TuBac.Models.DTOs
{
    public class RegisterRequest
    {
        [Required(ErrorMessage = "Họ và tên không được để trống")]
        [MaxLength(255, ErrorMessage = "Họ và tên không được vượt quá 255 ký tự")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Email không được để trống")]
        [EmailAddress(ErrorMessage = "Email không hợp lệ")]
        [MaxLength(320, ErrorMessage = "Email không được vượt quá 320 ký tự")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Mật khẩu không được để trống")]
        [MinLength(6, ErrorMessage = "Mật khẩu phải có ít nhất 6 ký tự")]
        [MaxLength(255, ErrorMessage = "Mật khẩu không được vượt quá 255 ký tự")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Xác nhận mật khẩu không được để trống")]
        [Compare("Password", ErrorMessage = "Mật khẩu xác nhận không khớp")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Giới tính không được để trống")]
        [RegularExpression("Nam|Nữ|Khác", ErrorMessage = "Giới tính phải là 'Nam', 'Nữ' hoặc 'Khác'")]
        public string Gender { get; set; }

        [Required(ErrorMessage = "Số điện thoại không được để trống")]
        [RegularExpression(@"^\d{10,15}$", ErrorMessage = "Số điện thoại phải có từ 10 đến 15 chữ số")]
        public string PhoneNumber { get; set; }

        [MaxLength(500, ErrorMessage = "Địa chỉ không được vượt quá 500 ký tự")]
        public string Address { get; set; }
    }
}
