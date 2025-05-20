using LuanAnTotNghiep_TuanVu_TuBac.DTOs;
using LuanAnTotNghiep_TuanVu_TuBac.Models.DTOs;
using LuanAnTotNghiep_TuanVu_TuBac.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LuanAnTotNghiep_TuanVu_TuBac.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentRepository _paymentRepository;

        public PaymentController(IPaymentRepository paymentRepository)
        {
            _paymentRepository = paymentRepository;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreatePayment([FromBody] PaymentRequest request)
        {
            try
            {
                var paymentId = await _paymentRepository.CreatePaymentAsync(request);

                return Ok(new
                {
                    message = "✅ Thanh toán thành công",
                    paymentId = paymentId
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    message = "❌ Thanh toán thất bại",
                    error = ex.Message
                });
            }


        }
        [HttpGet("total-spent/{userId}")]
        public async Task<IActionResult> GetTotalSpent(int userId)
        {
            try
            {
                var total = await _paymentRepository.GetTotalSpentByUserId(userId);
                return Ok(new { userId, totalSpent = total });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Lỗi khi tính tổng tiền đã dùng", error = ex.Message });
            }
        }
    }
}
