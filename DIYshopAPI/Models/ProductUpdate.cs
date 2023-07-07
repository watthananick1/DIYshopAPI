using System.ComponentModel.DataAnnotations;

namespace DIYshopAPI.Models
{
    public class ProductUpdate
    {
        public string? N_Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public decimal? Price { get; set; }
        public int? Stock { get; set; }
        public ProductType? Type { get; set; } 
        public string? ImgPoduct { get; set; }

    }
}
