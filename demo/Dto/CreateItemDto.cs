using System.ComponentModel.DataAnnotations;

namespace demo.Dto.Items
{
    public record CreateItemDto
    {
        [Required]
        public string name { get; set; }

        [Required]
        [Range(1, 10000)]
        public decimal price { get; set; }
    }
}
