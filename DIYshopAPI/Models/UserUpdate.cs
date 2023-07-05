using System.ComponentModel.DataAnnotations;

namespace DIYshopAPI.Models
{
    public class UserUpdate
    {
        [Required]
        public int Id { get; set; }
        public string? UserName { get; set; }
        public string? Firstname { get; set; }
        public string? Lastname { get; set; }
        public UserStatus? Status { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Proflieimg { get; set; }

    }
}
