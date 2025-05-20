using LuanAnTotNghiep_TuanVu_TuBac.Data;
using LuanAnTotNghiep_TuanVu_TuBac.DTOs;
using LuanAnTotNghiep_TuanVu_TuBac.Models.Entities;
using LuanAnTotNghiep_TuanVu_TuBac.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

public class RideRepository : IRideRepository
{
    private readonly ApplicationDbContext _context;

    public RideRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<int> CreateRideAsync(CreateRideRequest request)
    {
        // 👉 1. Lấy lịch trình và tuyến đường lượt đi
        var scheduleGo = await _context.RouteTripSchedules
            .Include(x => x.RouteTrip)
            .FirstOrDefaultAsync(x => x.Id == request.RouteTripScheduleId);

        if (scheduleGo == null)
            throw new Exception("Không tìm thấy lịch trình lượt đi");

        if (scheduleGo.AvailableSeats < request.TicketCountGo)
            throw new Exception("Không đủ vé cho chuyến đi lượt đi");

        var routeGo = scheduleGo.RouteTrip;
        var pickupTimeGo = scheduleGo.DepartureDate.Date + routeGo.StartTime;
        var dropoffTimeGo = scheduleGo.DepartureDate.Date + routeGo.EndTime;

        // 👉 2. Tạo ride lượt đi
        var rideGo = new Ride
        {
            UserId = request.UserId,
            RouteTripScheduleId = request.RouteTripScheduleId,
            PickupLocation = request.PickupLocation,
            DropoffLocation = request.DropoffLocation,
            EstimatedFare = request.EstimatedFare,
            IsRoundTrip = request.IsRoundTrip,
            DistanceKm = request.DistanceKm,
            Status = 0,
            CreatedAt = DateTime.Now,
            TicketCountGo = request.TicketCountGo,
            PickupTime = pickupTimeGo,
            DropoffTime = dropoffTimeGo
        };
        _context.Rides.Add(rideGo);

        scheduleGo.AvailableSeats -= request.TicketCountGo;
        _context.RouteTripSchedules.Update(scheduleGo);

        // 👉 3. Nếu là khứ hồi
        if (request.IsRoundTrip && request.ReturnRouteTripScheduleId.HasValue)
        {
            var scheduleReturn = await _context.RouteTripSchedules
                .Include(x => x.RouteTrip)
                .FirstOrDefaultAsync(x => x.Id == request.ReturnRouteTripScheduleId.Value);

            if (scheduleReturn == null)
                throw new Exception("Không tìm thấy lịch trình lượt về");

            if (scheduleReturn.AvailableSeats < request.TicketCountReturn)
                throw new Exception("Không đủ vé cho chuyến về");

            var routeReturn = scheduleReturn.RouteTrip;
            var pickupTimeReturn = scheduleReturn.DepartureDate.Date + routeReturn.StartTime;
            var dropoffTimeReturn = scheduleReturn.DepartureDate.Date + routeReturn.EndTime;

            var rideReturn = new Ride
            {
                UserId = request.UserId,
                RouteTripScheduleId = request.ReturnRouteTripScheduleId.Value,
                PickupLocation = request.DropoffLocation,
                DropoffLocation = request.PickupLocation,
                EstimatedFare = request.EstimatedFare,
                IsRoundTrip = request.IsRoundTrip,
                DistanceKm = request.DistanceKm,
                Status = 0,
                CreatedAt = DateTime.Now,
                TicketCountReturn = request.TicketCountReturn,
                PickupTime = pickupTimeReturn,
                DropoffTime = dropoffTimeReturn
            };

            _context.Rides.Add(rideReturn);
            scheduleReturn.AvailableSeats -= request.TicketCountReturn;
            _context.RouteTripSchedules.Update(scheduleReturn);

            await _context.SaveChangesAsync();

            rideGo.ReturnRideId = rideReturn.Id;
            _context.Rides.Update(rideGo);
        }

        await _context.SaveChangesAsync();
        return rideGo.Id;
    }

    public async Task<List<Ride>> GetRidesByUserIdAsync(int userId)
    {
        return await _context.Rides
            .Include(r => r.RouteTripSchedule)
                .ThenInclude(rts => rts.RouteTrip)
            .Include(r => r.Payment)
            .Where(r => r.UserId == userId)
            .OrderByDescending(r => r.CreatedAt)
            .ToListAsync();
    }

}
