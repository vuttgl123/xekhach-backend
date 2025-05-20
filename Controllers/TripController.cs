using LuanAnTotNghiep_TuanVu_TuBac.Models.DTOs;
using LuanAnTotNghiep_TuanVu_TuBac.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LuanAnTotNghiep_TuanVu_TuBac.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TripController : ControllerBase
    {
        private readonly IRouteTripScheduleRepository _scheduleRepo;

        public TripController(IRouteTripScheduleRepository scheduleRepo)
        {
            _scheduleRepo = scheduleRepo;
        }

        [HttpPost("search")]
        public async Task<IActionResult> Search([FromBody] SearchTripRequest request)
        {
            Console.WriteLine($"[DEBUG] Origin: {request.Origin}, Destination: {request.Destination}, Date: {request.Date}");

            if (string.IsNullOrWhiteSpace(request.Origin) || string.IsNullOrWhiteSpace(request.Destination))
                return BadRequest("Origin và Destination không được để trống.");

            var result = await _scheduleRepo.SearchAsync(
                request.Date,
                request.Origin,
                request.Destination
            );

            if (!result.Any())
                return NotFound("Không tìm thấy chuyến phù hợp.");

            return Ok(result);
        }

        //[HttpPost("refresh-schedules")]
        //public async Task<IActionResult> RefreshSchedules()
        //{
        //    await _scheduleRepo.RefreshSchedulesAsync();
        //    return Ok("Đã làm mới lịch cho 5 ngày tới.");
        //}




    }

}
