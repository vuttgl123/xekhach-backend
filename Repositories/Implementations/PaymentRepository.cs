using LuanAnTotNghiep_TuanVu_TuBac.Data;
using LuanAnTotNghiep_TuanVu_TuBac.Enums;
using LuanAnTotNghiep_TuanVu_TuBac.Models.DTOs;
using LuanAnTotNghiep_TuanVu_TuBac.Models.Entities;
using LuanAnTotNghiep_TuanVu_TuBac.Models.Enums;
using LuanAnTotNghiep_TuanVu_TuBac.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LuanAnTotNghiep_TuanVu_TuBac.Repositories.Implementations
{
    public class PaymentRepository : IPaymentRepository
    {
        private readonly ApplicationDbContext _context;

        public PaymentRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<int> CreatePaymentAsync(PaymentRequest request)
        {
            var ride = await _context.Rides.FindAsync(request.RideId);
            if (ride == null)
                throw new Exception("Không tìm thấy Ride để gán Payment");

            var payment = new Payment
            {
                // Không cần UserId
                PaymentMethod = request.PaymentMethod,
                Amount = request.Amount,
                DiscountAmount = request.DiscountAmount,
                TotalAmount = request.TotalAmount,
                PaymentStatus = request.PaymentMethod == (byte)PaymentMethodEnum.Cod
                    ? PaymentStatus.Confirmed
                    : PaymentStatus.Pending,
                CreatedAt = DateTime.Now,
                PromotionId = request.PromotionId
            };

            _context.Payments.Add(payment);
            await _context.SaveChangesAsync(); // lấy được payment.Id

            // Gán PaymentId cho Ride và (nếu có) rideReturn
            ride.PaymentId = payment.Id;
            _context.Rides.Update(ride);

            if (ride.ReturnRideId.HasValue)
            {
                var returnRide = await _context.Rides.FindAsync(ride.ReturnRideId.Value);
                if (returnRide != null)
                {
                    returnRide.PaymentId = payment.Id;
                    _context.Rides.Update(returnRide);
                }
            }

            await _context.SaveChangesAsync();
            return payment.Id;
        }

        public async Task<decimal> GetTotalSpentByUserId(int userId)
        {
            var total = await _context.Rides
                .Where(r => r.UserId == userId && r.PaymentId != null)
                .Join(_context.Payments,
                      ride => ride.PaymentId,
                      payment => payment.Id,
                      (ride, payment) => payment.TotalAmount)
                .SumAsync();

            return total;
        }


    }
}
