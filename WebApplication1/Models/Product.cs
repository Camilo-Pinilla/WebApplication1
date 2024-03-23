using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "The field must be filled")]
        public required string Title { get; set; }

        [Required(AllowEmptyStrings = true, ErrorMessage = "The field must be filled")]
        public required string Description { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "The field must be filled")]
        public decimal Price { get; set; }
    }
}
