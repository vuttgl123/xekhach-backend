using System;

namespace LuanAnTotNghiep_TuanVu_TuBac.Models.Entities
{
    public class Admin
    {
        public int Id { get; set; }

        public string FullName { get; set; }

        public string Email { get; set; }

        public DateTime? CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public string PasswordHash { get; set; }

        public string PhoneNumber { get; set; }

        public string AvatarUrl { get; set; }

        public bool IsActive { get; set; }

        public DateTime? LastLoginAt { get; set; }

        // Navigation properties (nếu cần)
        public ICollection<Notification> Notifications { get; set; }
    }
}
