using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace demo.Models
{
    [Table("Item")]
    public record Item
    {
        [Key]
        public Guid id { get; init; }
        public string name { get; init; }
        public decimal price { get; init; }
        public DateTimeOffset createdDate { get; init; } = DateTimeOffset.UtcNow;
    }
}
