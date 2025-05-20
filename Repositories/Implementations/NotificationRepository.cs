using LuanAnTotNghiep_TuanVu_TuBac.Data;
using LuanAnTotNghiep_TuanVu_TuBac.Models.DTOs;
using LuanAnTotNghiep_TuanVu_TuBac.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LuanAnTotNghiep_TuanVu_TuBac.Repositories.Implementations
{
    public class NotificationRepository : INotificationRepository
    {
        private readonly ApplicationDbContext _context;

        public NotificationRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<NotificationDto>> GetNotificationsByUserIdAsync(int userId)
        {
            return await _context.Notifications
                .Where(n => n.UserId == userId)
                .OrderByDescending(n => n.CreatedAt)
                .Select(n => new NotificationDto
                {
                    Id = n.Id,
                    Title = n.Title,
                    Message = n.Message,
                    Status = n.Status,
                    Type = n.Type,
                    CreatedAt = n.CreatedAt,
                    SenderName = n.SenderIsAdmin ? n.Admin.FullName : "Hệ thống"
                })
                .ToListAsync();
        }
    }

}
