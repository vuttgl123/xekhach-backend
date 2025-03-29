using LuanAnTotNghiep_TuanVu_TuBac.Models.Entities;

namespace LuanAnTotNghiep_TuanVu_TuBac.Repositories.Interfaces
{
    public interface IRideRepository
    {
        Task<Ride> CreateAsync(Ride ride, Ride returnRide = null);
    }
}
