using LuanAnTotNghiep_TuanVu_TuBac.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LuanAnTotNghiep_TuanVu_TuBac.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationsController : ControllerBase
    {
        private readonly INotificationRepository _notificationRepository;

        public NotificationsController(INotificationRepository notificationRepository)
        {
            _notificationRepository = notificationRepository;
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetUserNotifications(int userId)
        {
            var notifications = await _notificationRepository.GetNotificationsByUserIdAsync(userId);
            return Ok(notifications);
        }
    }

}
