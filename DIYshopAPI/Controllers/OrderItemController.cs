using DIYshopAPI.Data;
using DIYshopAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Security.Cryptography;

namespace DIYshopAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderItemController : ControllerBase
    {
        private readonly OrderItemContext _context;
        public OrderItemController(OrderItemContext context)
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
            var orderItems = await _context.OrderItems.ToListAsync();
            return orderItems == null ? BadRequest("Order Item Not Found.") : Ok(orderItems);
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

            var orderItem = await _context.OrderItems.FindAsync(id);
            return orderItem == null ? BadRequest("Order Item Not Found.") : Ok(orderItem);
        }

        public static int GetSeed()
        {
            using (RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider())
            {
                var intBytes = new byte[4];
                rng.GetBytes(intBytes);
                return BitConverter.ToInt32(intBytes, 0);
            }
        }

        [HttpPost]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> Create(OrderItem orderItem)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var random = new Random(GetSeed());
            orderItem.N_Id = "OR-" + random.Next();
            await _context.OrderItems.AddAsync(orderItem);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(Get), new { id = orderItem.Id }, orderItem);
        }

        [HttpPut("{id}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Update(int id, OrderItemUpdate orderItemUpdate)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var orderItem = await _context.OrderItems.FindAsync(id);
            var dataOrderItem = orderItem;
            if (orderItem == null) return BadRequest();

            orderItem.Order_Id = orderItemUpdate.Order_Id ?? dataOrderItem.Order_Id;
            orderItem.Product_id = orderItemUpdate.Product_id ?? dataOrderItem.Product_id;
            orderItem.N_Id = orderItemUpdate.N_Id ?? dataOrderItem.N_Id;
            orderItem.Item_Price = orderItemUpdate.Item_Price ?? dataOrderItem.Item_Price;
            orderItem.Item_Quantity = orderItemUpdate.Item_Quantity ?? dataOrderItem.Item_Quantity;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            var customer = await _context.OrderItems.FindAsync(id);
            if (customer == null) return NotFound();

            _context.OrderItems.Remove(customer);
            await _context.SaveChangesAsync();
            return NoContent();

        }

    }
}
