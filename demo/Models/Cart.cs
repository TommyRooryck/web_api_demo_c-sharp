using demo.Enum;

namespace demo.Models
{
    public sealed class Cart
    {
        private static Cart instance;

        public Guid id { get; init; }
        public List<CartItem> items { get; set; } = new List<CartItem>();
        public Status status { get; set; }
        public Order order { get; set; }
        public DateTimeOffset createdDate { get; init; } = DateTimeOffset.UtcNow;


        public static Cart getInstance()
        {
            if (instance == null || instance.status == Status.CLOSED)
            {
                instance = new Cart();
            }

            return instance;
        }

        public void update(Cart cart)
        {
            instance = cart;
        }
    }
}
