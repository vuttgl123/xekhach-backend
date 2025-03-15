using System.ComponentModel.DataAnnotations;

namespace LuanAnTotNghiep_TuanVu_TuBac.Models.DTOs
{
    public class ResetPasswordRequest
    {
        [Required]
        public string Email { get; set; }

        [Required(ErrorMessage = "Mật khẩu không được để trống")]
        [MinLength(6, ErrorMessage = "Mật khẩu phải có ít nhất 6 ký tự")]
        public string NewPassword { get; set; }
    }

}
