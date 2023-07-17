using DIYshopAPI.Data;
using DIYshopAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;

namespace DIYshopAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PromotionController : Controller
    {
        private readonly PromotionContext _context;
        public PromotionController(PromotionContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Get()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var promotion = await _context.Promotions.ToListAsync();
            return promotion == null ? BadRequest("Promotion Not Found.") : Ok(promotion);
        }

        [HttpGet("Item")]
        [Authorize]
        public async Task<IActionResult> GetItem()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var promotion = await _context.PromotionProducts.ToListAsync();
            return promotion == null ? BadRequest("Promotion item Not Found.") : Ok(promotion);
        }

        [HttpGet("{id}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Get(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var promotion = await _context.Promotions.FindAsync(id);
            return promotion == null ? BadRequest("Promotion Not Found.") : Ok(promotion);
        }

        [HttpGet("Code={id}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetByPromotionId(Guid id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var promotion = await _context.Promotions.Where(item => item.PromotionId == id).ToListAsync();
            return promotion == null ? BadRequest("Promotion Not Found.") : Ok(promotion);
        }

        [HttpGet("Item={id}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetItem(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var promotion = await _context.PromotionProducts.FindAsync(id);
            return promotion == null ? BadRequest("Promotion Item Not Found.") : Ok(promotion);
        }

        [HttpPost]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> Create(Promotion promotion)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            await _context.Promotions.AddAsync(promotion);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(Get), new { id = promotion.Id }, promotion);
        }

        [HttpPost("Item")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> CreateItem(PromotionProduct promotionProduct)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            await _context.PromotionProducts.AddAsync(promotionProduct);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(Get), new { id = promotionProduct.Id }, promotionProduct);
        }

        [HttpPut("{id}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Update(int id, PromotionUpdate promotionUpdate)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var promotion = await _context.Promotions.FindAsync(id);
            var dataPromotion = promotion;
            if (promotion == null) return BadRequest();

            promotion.PromotionId = promotionUpdate.PromotionId ?? dataPromotion.PromotionId;
            promotion.StartPromotion = promotionUpdate.StartPromotion ?? dataPromotion.StartPromotion;
            promotion.EndPromotion = promotionUpdate.EndPromotion ?? dataPromotion.EndPromotion;
            promotion.Discount = promotionUpdate.Discount ?? dataPromotion.Discount;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpPut("Item={id}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateItem(int id, PmPdUpdate pmPdUpdate)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var promotionItem = await _context.PromotionProducts.FindAsync(id);
            var dataPromotion = promotionItem;
            if (promotionItem == null) return BadRequest();

            promotionItem.Promotion_Id = pmPdUpdate.Promotion_Id ?? dataPromotion.Promotion_Id;
            promotionItem.Product_Id = pmPdUpdate.Product_Id ?? dataPromotion.Product_Id;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            var promotion = await _context.Promotions.FindAsync(id);
            if (promotion == null) return NotFound();

            var promotionProducts = await _context.PromotionProducts
                .Where(pp => pp.Promotion_Id == id)
                .ToListAsync();

            _context.PromotionProducts.RemoveRange(promotionProducts);

            _context.Promotions.Remove(promotion);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("Item={id}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteItem(int id)
        {
            var promotion = await _context.PromotionProducts.FindAsync(id);
            if (promotion == null) return NotFound();

            _context.PromotionProducts.Remove(promotion);
            await _context.SaveChangesAsync();
            return NoContent();

        }

    }
}
