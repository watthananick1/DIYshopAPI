using System.ComponentModel.DataAnnotations;

namespace DIYshopAPI.Models
{
    public class CustomerUpdate
    {
        [Required]
        public int Id { get; set; }
        public string? Firstname { get; set; }
        public string? Lastname { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }

    }
}
