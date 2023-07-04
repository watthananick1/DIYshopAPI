using DIYshopAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace DIYshopAPI.Data
{
    public class UserdbContext : DbContext
    {
        public UserdbContext(DbContextOptions<UserdbContext> options) :base(options){ 

        }
        public DbSet<User> Users { get; set; }
    }
}
