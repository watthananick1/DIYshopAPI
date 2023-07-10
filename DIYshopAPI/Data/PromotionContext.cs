using DIYshopAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace DIYshopAPI.Data
{
    public class PromotionContext : DbContext
    {
        public PromotionContext(DbContextOptions<PromotionContext> options) : base(options){ 
        
        }
        public DbSet<Promotion> Promotions { get; set; }
        public DbSet<PromotionProduct> PromotionProducts { get; set; }
    }
}
