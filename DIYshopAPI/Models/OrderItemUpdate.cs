using System.ComponentModel.DataAnnotations;

namespace DIYshopAPI.Models
{
    public class OrderItemUpdate
    {
        public int? Order_Id { get; set; }
        public int? Product_id { get; set; }
        public string? N_Id { get; set; }
        public decimal? Item_Price { get; set; }
        public int? Item_Quantity { get; set; }

    }
}
