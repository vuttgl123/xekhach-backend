using LuanAnTotNghiep_TuanVu_TuBac.Data;
using LuanAnTotNghiep_TuanVu_TuBac.Models.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LuanAnTotNghiep_TuanVu_TuBac.Controllers
{
    [Route("api/driver")]
    [ApiController]
    public class DriverController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public DriverController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("fromVehicleDriver/{vehicleDriverId}")]
        public async Task<ActionResult<DriverForCustomerResponse>> GetDriverFromVehicleDriverId(int vehicleDriverId)
        {
            var vehicleDriver = await _context.VehicleDrivers
                .FirstOrDefaultAsync(vd => vd.Id == vehicleDriverId);

            if (vehicleDriver == null)
            {
                return NotFound(new { message = "VehicleDriver not found" });
            }

            var driver = await _context.Drivers
                .FirstOrDefaultAsync(d => d.Id == vehicleDriver.DriverId);

            if (driver == null)
            {
                return NotFound(new { message = "Driver not found" });
            }

            var response = new DriverForCustomerResponse
            {
                Id = driver.Id,
                FullName = driver.FullName,
                VehicleType = driver.VehicleType,
                AvatarUrl = driver.AvatarUrl,
                PhoneNumber = driver.PhoneNumber, // ✅ Hiển thị rõ như bạn yêu cầu
                LicenseType = driver.LicenseType,
                LicenseExpiryDate = driver.LicenseExpiryDate,
                OperatingArea = driver.OperatingArea
            };

            return Ok(response);
        }

    }
}
