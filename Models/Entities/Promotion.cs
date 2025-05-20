using LuanAnTotNghiep_TuanVu_TuBac.Models.Enums;

namespace LuanAnTotNghiep_TuanVu_TuBac.Models.Entities
{
    public class Promotion
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public decimal Discount { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime ValidTo { get; set; }
        public PromotionStatus Status { get; set; }
        public int UsedCount { get; set; }
        public decimal MinRideAmount { get; set; }

    }
}
