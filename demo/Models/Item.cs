using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace demo.Models
{
    [Table("Item")]
    public record Item
    {
        [Key]
        public Guid id { get; init; }
        public string name { get; init; }
        public decimal price { get; init; }

        [JsonIgnore]
        public List<CartItem> carts { get; set; }

        public DateTimeOffset createdDate { get; init; } = DateTimeOffset.UtcNow;
    }
}
