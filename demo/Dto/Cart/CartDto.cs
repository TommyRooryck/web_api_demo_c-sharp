using demo.Models;
using demo.Extensions.CartItems;

namespace demo.Dto.Cart
{
    public class CartDto
    {
        public Guid id { get; set; }
        public List<CartItem> items { get; set; } = new List<CartItem>();
        public decimal totalPrice
        {
            get
            {
                decimal total = 0;
                if (items.Count > 0)
                {
                    foreach (var item in items)
                    {
                        total += item.asDto().subtotal;
                    }
                }

                return total;
            }
        }
    }
}
