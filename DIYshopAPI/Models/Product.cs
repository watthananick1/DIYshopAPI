using System.ComponentModel.DataAnnotations;

namespace DIYshopAPI.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string N_Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public decimal Price { get; set; }
        [Required]
        public int Stock { get; set; }
        [Required]
        public ProductType Type { get; set; } = ProductType.Apple;
        public string? ImgPoduct { get; set; }

    }
}
