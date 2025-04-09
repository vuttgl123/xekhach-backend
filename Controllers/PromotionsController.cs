using LuanAnTotNghiep_TuanVu_TuBac.Models.Entities;
using LuanAnTotNghiep_TuanVu_TuBac.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LuanAnTotNghiep_TuanVu_TuBac.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PromotionsController : ControllerBase
    {
        private readonly IPromotionService _promotionService;

        public PromotionsController(IPromotionService promotionService)
        {
            _promotionService = promotionService;
        }

        [HttpGet]
        [Route("active")]
        public async Task<ActionResult<IEnumerable<Promotion>>> GetActivePromotions()
        {
            var promotions = await _promotionService.GetActivePromotionsAsync();

            if (promotions == null || promotions.Count == 0)
            {
                return NotFound("Không có khuyến mãi nào đang áp dụng.");
            }

            return Ok(promotions);  // Trả về danh sách các khuyến mãi đang hoạt động
        }
    }
}
