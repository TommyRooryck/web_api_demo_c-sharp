namespace demo.Dto.Orders
{
    public class OrderDto
    {
        public Guid id { get; init; }
        public Guid cartId { get; init; }
        public DateTimeOffset createdAt { get; init; } = DateTimeOffset.Now;
    }
}
