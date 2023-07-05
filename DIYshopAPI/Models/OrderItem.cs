using System.ComponentModel.DataAnnotations;

namespace DIYshopAPI.Models
{
    public class OrderItem
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int Order_Id { get; set; }
        [Required]
        public int Product_id { get; set; }
        [Required]
        public string N_Id { get; set; }
        [Required]
        public decimal Item_Price { get; set; }
        [Required]
        public int Item_Quantity { get; set;}

    }
}
