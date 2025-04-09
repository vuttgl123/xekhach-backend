using LuanAnTotNghiep_TuanVu_TuBac.Models.Entities;

namespace LuanAnTotNghiep_TuanVu_TuBac.Repositories.Interfaces
{
    public interface IDriverRepository
    {
        Task<Driver> GetDriverFromVehicleDriverIdAsync(int vehicleDriverId);
    }

}
