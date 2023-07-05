﻿using DIYshopAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace DIYshopAPI.Data
{
    public class CustomerContext : DbContext
    {
        public CustomerContext(DbContextOptions<CustomerContext> options) : base(options){ 
        
        }
        public DbSet<Customer> Customers { get; set; }
    }
}
