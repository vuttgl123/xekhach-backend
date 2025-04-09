using LuanAnTotNghiep_TuanVu_TuBac.Data;
using LuanAnTotNghiep_TuanVu_TuBac.Models.Entities;
using LuanAnTotNghiep_TuanVu_TuBac.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LuanAnTotNghiep_TuanVu_TuBac.Repositories.Implementations
{
    public class RouteTripScheduleRepository : IRouteTripScheduleRepository
    {
        private readonly ApplicationDbContext _context;

        public RouteTripScheduleRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<RouteTripSchedule>> SearchAsync(DateTime date, string origin, string destination)
        {
            // Chuẩn hóa input
            string originInput = origin?.Trim() ?? "";
            string destinationInput = destination?.Trim() ?? "";
            DateTime searchDate = date.Date;

            // Truy vấn
            return await _context.RouteTripSchedules
                .Include(s => s.RouteTrip)
                .Where(s =>
                    s.DepartureDate == searchDate &&
                    s.RouteTrip != null &&
                    s.RouteTrip.Origin != null &&
                    s.RouteTrip.Destination != null &&
                    EF.Functions.Collate(s.RouteTrip.Origin, "SQL_Latin1_General_CP1_CI_AI").Contains(originInput) &&
                    EF.Functions.Collate(s.RouteTrip.Destination, "SQL_Latin1_General_CP1_CI_AI").Contains(destinationInput)
                )
                .ToListAsync();
        }

        public async Task<RouteTripSchedule> GetByIdAsync(int id)
        {
            return await _context.RouteTripSchedules
                .Include(x => x.RouteTrip)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task DecreaseSeatAsync(int scheduleId)
        {
            var schedule = await _context.RouteTripSchedules.FindAsync(scheduleId);
            if (schedule != null && schedule.AvailableSeats > 0)
            {
                schedule.AvailableSeats--;
                await _context.SaveChangesAsync();
            }
        }

        public async Task RefreshSchedulesAsync()
        {
            var today = DateTime.Today;

            // ❌ Xoá lịch cũ nhưng phải kiểm tra chưa có Ride liên quan
            var oldSchedules = await _context.RouteTripSchedules
                .Where(s => s.DepartureDate < today &&
                            !_context.Rides.Any(r => r.RouteTripScheduleId == s.Id))
                .ToListAsync();

            _context.RouteTripSchedules.RemoveRange(oldSchedules);

            // ✅ Thêm mới 5 ngày tới
            for (int i = 0; i < 5; i++)
            {
                var date = today.AddDays(i);
                var trips = await _context.RouteTrips
                    .Where(rt => rt.Status != "Nghỉ")
                    .ToListAsync();

                foreach (var trip in trips)
                {
                    _context.RouteTripSchedules.Add(new Models.Entities.RouteTripSchedule
                    {
                        RouteTripId = trip.Id,
                        DepartureDate = date,
                        VehicleDriverId = trip.VehicleDriverId,
                        DepartureTime = trip.StartTime,
                        TotalSeats = 16,
                        AvailableSeats = 16,
                        CreatedAt = DateTime.Now
                    });
                }
            }

            await _context.SaveChangesAsync();
        }


    }
}
