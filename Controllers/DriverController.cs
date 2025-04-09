using LuanAnTotNghiep_TuanVu_TuBac.Data;
using LuanAnTotNghiep_TuanVu_TuBac.Models.Entities;
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

        // API lấy thông tin tài xế từ vehicleDriverId
        [HttpGet("fromVehicleDriver/{vehicleDriverId}")]
        public async Task<ActionResult<Driver>> GetDriverFromVehicleDriverId(int vehicleDriverId)
        {
            // Lấy thông tin từ bảng dbo.VehicleDrivers
            var vehicleDriver = await _context.VehicleDrivers
                .Where(vd => vd.Id == vehicleDriverId)
                .FirstOrDefaultAsync();

            if (vehicleDriver == null)
            {
                return NotFound(new { message = "VehicleDriver not found" });
            }

            // Lấy thông tin tài xế từ bảng dbo.Drivers dựa trên DriverId từ vehicleDriver
            var driver = await _context.Drivers
                .Where(d => d.Id == vehicleDriver.DriverId)
                .FirstOrDefaultAsync();

            if (driver == null)
            {
                return NotFound(new { message = "Driver not found" });
            }

            // Trả về thông tin tài xế
            return Ok(driver);
        }
    }
}
