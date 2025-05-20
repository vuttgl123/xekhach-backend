using LuanAnTotNghiep_TuanVu_TuBac.Models.DTOs;

namespace LuanAnTotNghiep_TuanVu_TuBac.Repositories.Interfaces
{
    public interface IPaymentRepository
    {
        // Phương thức để tạo bản ghi thanh toán
        Task<int> CreatePaymentAsync(PaymentRequest request);

        Task<decimal> GetTotalSpentByUserId(int userId);

    }
}
