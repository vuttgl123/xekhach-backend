namespace LuanAnTotNghiep_TuanVu_TuBac.Models.DTOs
{
    public class PaymentRequest
    {
        public int RideId { get; set; }
        public byte PaymentMethod { get; set; }
        public decimal Amount { get; set; }  
        public decimal DiscountAmount { get; set; } 
        public decimal TotalAmount { get; set; }  

        public int? PromotionId { get; set; }
    }
}
