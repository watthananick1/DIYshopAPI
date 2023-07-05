using System.ComponentModel.DataAnnotations;

namespace DIYshopAPI.Models
{
    public class UserChangePassword
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Password { get; set; }

    }
}
