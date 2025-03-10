using System.ComponentModel.DataAnnotations;

namespace LuanAnTotNghiep_TuanVu_TuBac.Models.DTOs
{
    public class LoginRequest
    {
        [Required, EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
