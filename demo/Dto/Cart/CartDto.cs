using demo.Models;

namespace demo.Dto.Cart
{
    public class CartDto
    {
        public List<CartItem> items { get; set; }
        public decimal totalPrice
        {
            get
            {
                decimal total = 0;
                foreach (var item in items)
                {
                    total += item.item.price;
                }

                return total;
            }
        }
    }
}
