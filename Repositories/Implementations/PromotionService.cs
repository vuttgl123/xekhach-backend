using LuanAnTotNghiep_TuanVu_TuBac.Data;
using LuanAnTotNghiep_TuanVu_TuBac.Models.Entities;
using LuanAnTotNghiep_TuanVu_TuBac.Models.Enums;
using LuanAnTotNghiep_TuanVu_TuBac.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LuanAnTotNghiep_TuanVu_TuBac.Repositories.Implementations
{
    public class PromotionService : IPromotionService
    {
        private readonly ApplicationDbContext _context;

        public PromotionService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Promotion>> GetActivePromotionsAsync()
        {
            var promotions = await _context.Promotions
                .Where(p => p.Status == PromotionStatus.Active && p.ValidFrom <= DateTime.Now && p.ValidTo >= DateTime.Now)
                .ToListAsync();

            return promotions;
        }
    }
}
