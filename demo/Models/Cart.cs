using demo.Enum;

namespace demo.Models
{
    public sealed class Cart
    {
        private static Cart instance;

        public Guid id { get; set; }
        public List<CartItem> items { get; set; }
        public Status status { get; set; }
        public DateTime createdDate { get; set; }
        

        public static Cart getInstance()
        {
            if (instance == null || instance.status == Status.Closed)
            {
                instance = new Cart();
            }

            return instance;
        }
    }
}
