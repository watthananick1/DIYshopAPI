using DIYshopAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace DIYshopAPI.Data
{
    public class OrderItemContext : DbContext
    {
        public OrderItemContext(DbContextOptions<OrderItemContext> options) : base(options){ 
        
        }
        public DbSet<OrderItem> OrderItems { get; set; }
    }
}
