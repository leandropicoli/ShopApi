using System.ComponentModel.DataAnnotations;

namespace Shop.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "This field is required")]
        [MaxLength(20, ErrorMessage = "This field maximum allowed length is 20 characters")]
        [MinLength(3, ErrorMessage = "This field must be at least 3 characters")]
        public string Username { get; set; }

        [Required(ErrorMessage = "This field is required")]
        [MaxLength(20, ErrorMessage = "This field maximum allowed length is 60 characters")]
        [MinLength(3, ErrorMessage = "This field must be at least 3 characters")]
        public string Password { get; set; }

        public string Role { get; set; }
    }
}