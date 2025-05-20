namespace LuanAnTotNghiep_TuanVu_TuBac.Enums
{
    public enum PaymentStatus : byte
    {
        Pending = 0,        // 💬 Chờ xác nhận (áp dụng cho bank / wallet)
        Confirmed = 1,      // ✅ Đã thanh toán (áp dụng cho COD hoặc đã duyệt)
        Failed = 2,         // ❌ Thất bại, giao dịch bị huỷ hoặc sai thông tin
        Refunded = 3        // 💸 Đã hoàn tiền (nếu có cơ chế huỷ)
    }
}
