namespace demo.Dto.Items
{
    public record ItemDto
    {
        public Guid id { get; init; }
        public string name { get; init; }
        public decimal price { get; init; }
        public DateTimeOffset createdDate { get; init; } = DateTimeOffset.UtcNow;
    }
}
