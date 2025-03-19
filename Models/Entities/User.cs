using System;
using System.ComponentModel.DataAnnotations;

namespace LuanAnTotNghiep_TuanVu_TuBac.Models.Entities
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Email không được để trống")]
        [EmailAddress(ErrorMessage = "Email không hợp lệ")]
        [MaxLength(320, ErrorMessage = "Email không được vượt quá 320 ký tự")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Mật khẩu không được để trống")]
        [MinLength(6, ErrorMessage = "Mật khẩu phải có ít nhất 6 ký tự")]
        [MaxLength(255, ErrorMessage = "Mật khẩu không được vượt quá 255 ký tự")]
        [RegularExpression(@"^\S+$", ErrorMessage = "Mật khẩu không được chứa khoảng trắng")]
        public string PasswordHash { get; set; }

        [Required(ErrorMessage = "Họ và tên không được để trống")]
        [MaxLength(255, ErrorMessage = "Họ và tên không được vượt quá 255 ký tự")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Giới tính không được để trống")]
        [RegularExpression("Nam|Nữ|Khác", ErrorMessage = "Giới tính phải là 'Nam', 'Nữ' hoặc 'Khác'")]
        [MaxLength(10)]
        public string Gender { get; set; }

        [Required(ErrorMessage = "Số điện thoại không được để trống")]
        [RegularExpression(@"^\d{10,15}$", ErrorMessage = "Số điện thoại phải có từ 10 đến 15 chữ số")]
        [MaxLength(15)]
        public string PhoneNumber { get; set; }

        [MaxLength(500, ErrorMessage = "Địa chỉ không được vượt quá 500 ký tự")]
        public string Address { get; set; }

        public DateTime? CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedAt { get; set; } = DateTime.UtcNow;

        [DataType(DataType.Date)]
        [CheckBirthDate(ErrorMessage = "Ngày sinh không hợp lệ")]
        public DateTime? BirthDate { get; set; }

        [Required(ErrorMessage = "Trạng thái không được để trống")]
        public bool? IsActive { get; set; }

        [Required(ErrorMessage = "Vai trò không được để trống")]
        [Range(0, 2, ErrorMessage = "Vai trò chỉ có thể là 0 (User), 1 (Admin), 2 (Driver)")]
        public byte? Role { get; set; }

        public int TokenVersion { get; set; } = 1;
        public string? OtpCode { get; set; }
        public DateTime? OtpExpiry { get; set; }

        public class CheckBirthDate : ValidationAttribute
        {
            public override bool IsValid(object value)
            {
                if (value is DateTime birthDate)
                {
                    return birthDate < DateTime.UtcNow; // Ngày sinh phải nhỏ hơn hiện tại
                }
                return true; // Cho phép null
            }
        }
    }
}
