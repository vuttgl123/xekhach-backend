namespace LuanAnTotNghiep_TuanVu_TuBac.Models.Entities
{
    public class Notification
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public byte? Status { get; set; }
        public byte? Type { get; set; }
        public DateTime? CreatedAt { get; set; }
        public int? AdminId { get; set; }
        public bool SenderIsAdmin { get; set; }

        // Navigation (tuỳ chọn nếu dùng User hoặc Admin)
        public User User { get; set; }
        public Admin Admin { get; set; }
    }

}
