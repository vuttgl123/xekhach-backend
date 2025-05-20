using LuanAnTotNghiep_TuanVu_TuBac.DTOs;
using LuanAnTotNghiep_TuanVu_TuBac.Models.Entities;

namespace LuanAnTotNghiep_TuanVu_TuBac.Repositories.Interfaces
{
    public interface IRideRepository
    {
        Task<int> CreateRideAsync(CreateRideRequest request);

        Task<List<Ride>> GetRidesByUserIdAsync(int userId);


    }
}
