using LuanAnTotNghiep_TuanVu_TuBac.Data;
using LuanAnTotNghiep_TuanVu_TuBac.Models.Entities;
using LuanAnTotNghiep_TuanVu_TuBac.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LuanAnTotNghiep_TuanVu_TuBac.Repositories.Implementations
{
    public class RideRepository : IRideRepository
    {
        private readonly ApplicationDbContext _context;

        public RideRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Ride> CreateAsync(Ride ride, Ride returnRide = null)
        {
            _context.Rides.Add(ride);

            // Giảm ghế lượt đi
            var schedule = await _context.RouteTripSchedules.FindAsync(ride.RouteTripScheduleId);
            if (schedule != null && schedule.AvailableSeats > 0)
            {
                schedule.AvailableSeats--;
            }

            if (returnRide != null)
            {
                _context.Rides.Add(returnRide);

                // Giảm ghế lượt về
                var returnSchedule = await _context.RouteTripSchedules.FindAsync(returnRide.RouteTripScheduleId);
                if (returnSchedule != null && returnSchedule.AvailableSeats > 0)
                {
                    returnSchedule.AvailableSeats--;
                }
            }

            await _context.SaveChangesAsync();

            // Cập nhật ReturnRideId sau khi returnRide có Id
            if (returnRide != null)
            {
                ride.ReturnRideId = returnRide.Id;
                _context.Rides.Update(ride);
                await _context.SaveChangesAsync();
            }

            return ride;
        }
    }

}
