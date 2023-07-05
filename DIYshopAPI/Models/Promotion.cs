using System.ComponentModel.DataAnnotations;

namespace DIYshopAPI.Models
{
    public class Promotion
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        public string NameProduct_One { get; set; }
        [Required]
        public string NameProduct_Two { get; set; }
        [Required]
        public string IdProduct_One { get; set; }
        [Required]
        public string IdProduct_Two { get; set; }
        [Required]
        public decimal Discount { get; set; }


    }
}
