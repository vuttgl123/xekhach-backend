using LuanAnTotNghiep_TuanVu_TuBac.Models.DTOs;
using LuanAnTotNghiep_TuanVu_TuBac.Models.Entities;
using LuanAnTotNghiep_TuanVu_TuBac.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LuanAnTotNghiep_TuanVu_TuBac.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RideController : ControllerBase
    {
        private readonly IRideRepository _rideRepository;
        private readonly IRouteTripScheduleRepository _scheduleRepository;

        public RideController(IRideRepository rideRepository, IRouteTripScheduleRepository scheduleRepository)
        {
            _rideRepository = rideRepository;
            _scheduleRepository = scheduleRepository;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateRideRequest request)
        {
            var schedule = await _scheduleRepository.GetByIdAsync(request.RouteTripScheduleId);
            if (schedule == null || schedule.AvailableSeats <= 0)
            {
                return BadRequest(new { message = "Lịch trình đi không hợp lệ hoặc đã hết ghế." });
            }

            var ride = new Ride
            {
                UserId = request.UserId,
                RouteTripScheduleId = request.RouteTripScheduleId,
                PickupLocation = request.PickupLocation,
                DropoffLocation = request.DropoffLocation,
                EstimatedFare = request.EstimatedFare,
                Status = 0,
                IsRoundTrip = request.IsRoundTrip,
                DistanceKm = (decimal)request.DistanceKm,
                VehicleDriverId = schedule.VehicleDriverId,
                CreatedAt = DateTime.UtcNow
            };

            Ride? returnRide = null;

            if (request.IsRoundTrip && request.ReturnRouteTripScheduleId.HasValue)
            {
                var returnSchedule = await _scheduleRepository.GetByIdAsync(request.ReturnRouteTripScheduleId.Value);
                if (returnSchedule == null || returnSchedule.AvailableSeats <= 0)
                {
                    return BadRequest(new { message = "Lịch trình về không hợp lệ hoặc đã hết ghế." });
                }

                returnRide = new Ride
                {
                    UserId = request.UserId,
                    RouteTripScheduleId = request.ReturnRouteTripScheduleId.Value,
                    PickupLocation = request.DropoffLocation,
                    DropoffLocation = request.PickupLocation,
                    EstimatedFare = request.EstimatedFare,
                    Status = 0,
                    IsRoundTrip = false, // Lượt về không cần gắn lại khứ hồi
                    DistanceKm = (decimal)request.DistanceKm,
                    VehicleDriverId = returnSchedule.VehicleDriverId,
                    CreatedAt = DateTime.UtcNow
                };
            }

            var result = await _rideRepository.CreateAsync(ride, returnRide);
            return Ok(result);
        }

    }
}
