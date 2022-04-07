using System.Text.Json.Serialization;

namespace demo.Models
{
    public class CartItem
    {
        public int id { get; set; }

        [JsonIgnore]
        public Guid cartId { get; set; }
        [JsonIgnore]
        public Cart cart { get; set; }
        public Guid itemId { get; set; }
        public Item item { get; set; }

    }
}
