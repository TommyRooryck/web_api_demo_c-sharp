using demo.Enum;

namespace demo.Models
{
    public class Order
    {
        public Guid Id { get; init; }

        public Guid cartId { get; set; }
        public Cart cart { get; set; }
        public Status status { get; set; }

        public DateTimeOffset createdDate { get; init; } = DateTimeOffset.UtcNow;
    }
}
