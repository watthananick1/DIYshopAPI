using System.ComponentModel.DataAnnotations;

namespace DIYshopAPI.Models
{
    public class PromotionUpdate
    {
        public Guid? PromotionId { get; set; }
        public DateTime? StartPromotion { get; set; }
        public DateTime? EndPromotion { get; set; }
        public decimal? Discount { get; set; }

    }

    public class PmPdUpdate
    {
        public int? Promotion_Id { get; set; }
        public int? Product_Id { get; set; }
    }
}
