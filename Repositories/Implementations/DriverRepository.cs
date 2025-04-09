using LuanAnTotNghiep_TuanVu_TuBac.Data;
using LuanAnTotNghiep_TuanVu_TuBac.Models.Entities;
using LuanAnTotNghiep_TuanVu_TuBac.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LuanAnTotNghiep_TuanVu_TuBac.Repositories.Implementations
{
    public class DriverRepository : IDriverRepository
    {
        private readonly ApplicationDbContext _context;

        public DriverRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Driver> GetDriverFromVehicleDriverIdAsync(int vehicleDriverId)
        {
            // Lấy thông tin từ bảng dbo.VehicleDrivers
            var vehicleDriver = await _context.VehicleDrivers
                .Where(vd => vd.Id == vehicleDriverId)
                .FirstOrDefaultAsync();

            if (vehicleDriver == null)
            {
                return null;
            }

            // Lấy thông tin tài xế từ bảng dbo.Drivers
            return await _context.Drivers
                .Where(d => d.Id == vehicleDriver.DriverId)
                .FirstOrDefaultAsync();
        }
    }

}
