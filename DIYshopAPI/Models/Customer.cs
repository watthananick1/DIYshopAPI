using System.ComponentModel.DataAnnotations;

namespace DIYshopAPI.Models
{
    public class Customer
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Firstname { get; set; }
        [Required]
        public string Lastname { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public DateTime? BridDate { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; } = string.Empty;
    }
}
