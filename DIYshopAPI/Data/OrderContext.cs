using DIYshopAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace DIYshopAPI.Data
{
    public class OrderContext : DbContext
    {
        public OrderContext(DbContextOptions<OrderContext> options) : base(options){ 
        
        }
        public DbSet<Order> Orders { get; set; }
    }
}
