using LuanAnTotNghiep_TuanVu_TuBac.DTOs;
using LuanAnTotNghiep_TuanVu_TuBac.Models.DTOs;
using LuanAnTotNghiep_TuanVu_TuBac.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LuanAnTotNghiep_TuanVu_TuBac.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RatingsController : ControllerBase
    {
        private readonly IRatingRepository _ratingRepository;

        public RatingsController(IRatingRepository ratingRepository)
        {
            _ratingRepository = ratingRepository;
        }

        // GET: api/ratings/driver/5
        [HttpGet("driver/{driverId}")]
        public async Task<IActionResult> GetRatingsByDriverId(int driverId)
        {
            var ratings = await _ratingRepository.GetRatingsByDriverId(driverId);

            if (ratings == null || ratings.Count == 0)
                return NotFound("Không tìm thấy đánh giá nào cho tài xế này.");

            return Ok(ratings);
        }

        // GET: api/ratings/driver/5/average
        [HttpGet("driver/{driverId}/average")]
        public async Task<IActionResult> GetAverageRating(int driverId)
        {
            var (average, count) = await _ratingRepository.GetAverageRatingByDriverId(driverId);

            return Ok(new
            {
                driverId,
                averageRating = average,
                reviewCount = count
            });
        }

        // POST: api/ratings/add
        [HttpPost("add")]
        public async Task<IActionResult> AddRating([FromBody] DriverRatingRequest request)
        {
            try
            {
                var ratingId = await _ratingRepository.AddRatingAsync(request);
                return Ok(new
                {
                    message = "Đánh giá đã được gửi thành công!",
                    ratingId
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    message = "Gửi đánh giá thất bại.",
                    error = ex.Message
                });
            }
        }
    }
}
