using DIYshopAPI.Data;
using DIYshopAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DIYshopAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly OrderContext _context;
        public OrdersController(OrderContext context)
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
            var customer = await _context.Orders.ToListAsync();
            return customer == null ? BadRequest("Order Not Found.") : Ok(customer);
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

            var customer = await _context.Orders.FindAsync(id);
            return customer == null ? BadRequest("Order Not Found.") : Ok(customer);
        }

        [HttpPost]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> Create(Order order)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(Get), new { id = order.Id }, order);
        }

        [HttpPut("{id}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Update(int id, OrderUpdate orderUpdate)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var order = await _context.Orders.FindAsync(id);
            var dataOrder = order;
            if (order == null) return BadRequest();

            order.Date = (DateTime)orderUpdate.Date;
            order.Total_Price = orderUpdate.Total_Price ?? dataOrder.Total_Price;
            order.User_Id = orderUpdate.User_Id ?? dataOrder.User_Id;
            order.Customer_Id = orderUpdate.Customer_Id ?? dataOrder.Customer_Id;
            order.Promotion_id = orderUpdate.Promotion_id ?? dataOrder.Promotion_id;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            var customer = await _context.Orders.FindAsync(id);
            if (customer == null) return NotFound();

            _context.Orders.Remove(customer);
            await _context.SaveChangesAsync();
            return NoContent();

        }
    }
}
