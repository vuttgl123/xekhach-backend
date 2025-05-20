using LuanAnTotNghiep_TuanVu_TuBac.Models.DTOs;

namespace LuanAnTotNghiep_TuanVu_TuBac.Repositories.Interfaces
{
    public interface INotificationRepository
    {
        Task<List<NotificationDto>> GetNotificationsByUserIdAsync(int userId);
    }

}
