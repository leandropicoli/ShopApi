using System.ComponentModel.DataAnnotations;

namespace Shop.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "This field is required.")]
        [MaxLength(60, ErrorMessage = "This field maximum allowed length is 60 characters.")]
        [MinLength(3, ErrorMessage = "This field must be at least 3 characters. ")]
        public string Title { get; set; }
    }
}