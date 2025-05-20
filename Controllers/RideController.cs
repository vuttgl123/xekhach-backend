using Microsoft.AspNetCore.Mvc;
using LuanAnTotNghiep_TuanVu_TuBac.DTOs;
using LuanAnTotNghiep_TuanVu_TuBac.Repositories.Interfaces;
using LuanAnTotNghiep_TuanVu_TuBac.Models.DTOs;

namespace LuanAnTotNghiep_TuanVu_TuBac.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RidesController : ControllerBase
    {
        private readonly IRideRepository _rideRepository;

        public RidesController(IRideRepository rideRepository)
        {
            _rideRepository = rideRepository;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateRide([FromBody] CreateRideRequest request)
        {
            try
            {
                var rideId = await _rideRepository.CreateRideAsync(request);

                return Ok(new
                {
                    message = "✅ Đặt vé thành công",
                    rideId = rideId
                });
            }
            catch (Exception ex)
            {
                var inner = ex.InnerException?.Message ?? ex.Message;
                return BadRequest(new
                {
                    message = "❌ Đặt vé thất bại",
                    error = ex.Message
                });
            }
        }
        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetRidesByUserId(int userId)
        {
            try
            {
                var rides = await _rideRepository.GetRidesByUserIdAsync(userId);

                return Ok(rides.Select(r => new
                {
                    r.Id,
                    r.PickupLocation,
                    r.DropoffLocation,
                    r.PickupTime,
                    r.DropoffTime,
                    r.EstimatedFare,
                    r.Status,
                    r.IsRoundTrip,
                    r.TicketCountGo,
                    r.TicketCountReturn,
                    r.RouteTripScheduleId,
                    PaymentMethod = r.Payment?.PaymentMethod,
                    PaymentTime = r.Payment?.CreatedAt
                }));
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Lỗi khi lấy danh sách vé", error = ex.Message });
            }
        }



    }
}
