using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DIYshopAPI.Models
{
    public class Promotion
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public Guid PromotionId { get; set; } = Guid.NewGuid();

        [Required]
        public DateTime StartPromotion { get; set; } = DateTime.Now;

        [Required]
        public DateTime EndPromotion { get; set; } = DateTime.Now.AddMonths(1);

        [Required]
        public decimal Discount { get; set; } = decimal.Zero;
    }

    public class PromotionProduct
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int Promotion_Id { get; set; }

        [Required]
        public int Product_Id { get; set; }
    }
}
