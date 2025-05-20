using LuanAnTotNghiep_TuanVu_TuBac.Enums;

namespace LuanAnTotNghiep_TuanVu_TuBac.Models.Entities
{
    public class Payment
    {
        public int Id { get; set; }
        public byte PaymentMethod { get; set; }
        public decimal Amount { get; set; }
        public PaymentStatus PaymentStatus { get; set; } = PaymentStatus.Pending;
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal TotalAmount { get; set; }
        public int? CreatedByAdminId { get; set; }
        public int? UpdatedByAdminId { get; set; }

        public int? PromotionId { get; set; } // 👈 FK đến bảng Promotions

        public Promotion Promotion { get; set; }
    }
}
