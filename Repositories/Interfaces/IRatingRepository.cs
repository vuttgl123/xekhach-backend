using LuanAnTotNghiep_TuanVu_TuBac.DTOs;
using LuanAnTotNghiep_TuanVu_TuBac.Models.DTOs;
using LuanAnTotNghiep_TuanVu_TuBac.Models.Entities;

namespace LuanAnTotNghiep_TuanVu_TuBac.Repositories.Interfaces
{
    public interface IRatingRepository
    {
        Task<List<DriverRatingResponse>> GetRatingsByDriverId(int driverId);
        Task<(double average, int count)> GetAverageRatingByDriverId(int driverId);

        Task<int> AddRatingAsync(DriverRatingRequest request);
        Task<IEnumerable<Rating>> GetRatingsByUserIdAsync(int userId);
        Task<IEnumerable<Rating>> GetRatingsByRideIdAsync(int rideId);

    }
}
