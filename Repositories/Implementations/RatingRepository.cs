using LuanAnTotNghiep_TuanVu_TuBac.Data;
using LuanAnTotNghiep_TuanVu_TuBac.DTOs;
using LuanAnTotNghiep_TuanVu_TuBac.Models.DTOs;
using LuanAnTotNghiep_TuanVu_TuBac.Models.Entities;
using LuanAnTotNghiep_TuanVu_TuBac.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

public class RatingRepository : IRatingRepository
{
    private readonly ApplicationDbContext _context;

    public RatingRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<DriverRatingResponse>> GetRatingsByDriverId(int driverId)
    {
        var ratings = await _context.Rides
            .Where(r => r.RouteTripSchedule.VehicleDriver.DriverId == driverId) 
            .Join(_context.Ratings,
                ride => ride.Id,
                rating => rating.RideId,
                (ride, rating) => new DriverRatingResponse
                {
                    RatingValue = rating.RatingValue,
                    Feedback = rating.Feedback,
                    CreatedAt = rating.CreatedAt ?? DateTime.Now
                })
            .OrderByDescending(r => r.CreatedAt)
            .ToListAsync();

        return ratings;
    }


    public async Task<(double average, int count)> GetAverageRatingByDriverId(int driverId)
    {
        var ratings = await _context.Rides
            .Where(r => r.RouteTripSchedule.VehicleDriver.DriverId == driverId)
            .Join(_context.Ratings,
                ride => ride.Id,
                rating => rating.RideId,
                (ride, rating) => rating.RatingValue)
            .ToListAsync();

        if (!ratings.Any()) return (0, 0);

        double average = ratings.Average(x => x);
        return (average, ratings.Count);
    }

    public async Task<int> AddRatingAsync(DriverRatingRequest request)
    {
        var rating = new Rating
        {
            RideId = request.RideId,
            UserId = request.UserId,
            RatingValue = request.RatingValue,
            Feedback = request.Feedback,
            CreatedAt = DateTime.Now
        };

        _context.Ratings.Add(rating);
        await _context.SaveChangesAsync();
        return rating.Id;
    }

    public async Task<IEnumerable<Rating>> GetRatingsByUserIdAsync(int userId)
    {
        return await _context.Ratings
            .Where(r => r.UserId == userId)
            .ToListAsync();
    }

    public async Task<IEnumerable<Rating>> GetRatingsByRideIdAsync(int rideId)
    {
        return await _context.Ratings
            .Where(r => r.RideId == rideId)
            .ToListAsync();
    }
}
