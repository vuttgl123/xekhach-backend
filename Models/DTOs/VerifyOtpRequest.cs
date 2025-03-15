using System.ComponentModel.DataAnnotations;

namespace LuanAnTotNghiep_TuanVu_TuBac.Models.DTOs
{
    public class VerifyOtpRequest
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string OtpCode { get; set; }
    }

}
