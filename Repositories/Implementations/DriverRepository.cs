using LuanAnTotNghiep_TuanVu_TuBac.Data;
using LuanAnTotNghiep_TuanVu_TuBac.Models.DTOs;
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

        public async Task<DriverForCustomerResponse> GetDriverFromVehicleDriverIdAsync(int vehicleDriverId)
        {
            var vehicleDriver = await _context.VehicleDrivers
                .FirstOrDefaultAsync(vd => vd.Id == vehicleDriverId);

            if (vehicleDriver == null)
                return null;

            var driver = await _context.Drivers
                .FirstOrDefaultAsync(d => d.Id == vehicleDriver.DriverId);

            if (driver == null)
                return null;

            return new DriverForCustomerResponse
            {
                Id = driver.Id,
                FullName = driver.FullName,
                VehicleType = driver.VehicleType,
                AvatarUrl = driver.AvatarUrl,
                PhoneNumber = driver.PhoneNumber, // 👉 Giữ nguyên số
                LicenseType = driver.LicenseType,
                LicenseExpiryDate = driver.LicenseExpiryDate,
                OperatingArea = driver.OperatingArea
            };

        }

    }

}
