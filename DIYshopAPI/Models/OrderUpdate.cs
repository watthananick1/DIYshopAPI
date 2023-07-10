using System.ComponentModel.DataAnnotations;

namespace DIYshopAPI.Models
{
    public class OrderUpdate
    {
        public DateTime? Date { get; set; }
        public decimal? Total_Price { get; set; }
        public int? User_Id { get; set;}
        public int? Customer_Id { get; set; }
        public int? Promotion_id { get; set; }
        
    }
}
