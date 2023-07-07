using System.ComponentModel.DataAnnotations;

namespace DIYshopAPI.Models
{
    public class Order
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public DateTime Date { get; set; } = DateTime.Now;
        [Required]
        public string OrderId { get; set; }
        [Required]
        public decimal Total_Price { get; set; }
        [Required]
        public int User_Id { get; set;}
        public int? Customer_Id { get; set; }
        public int? Promotion_id { get; set; }
        
    }
}
