namespace LuanAnTotNghiep_TuanVu_TuBac.Models.DTOs
{
    public class NotificationDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public byte? Status { get; set; }
        public byte? Type { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string SenderName { get; set; }
    }

}
