using demo.Models;
using demo.Dto.Items;

namespace demo.Dto.CartItems
{
    public class CartItemDto
    {
        public Item item { get; set; }
        public int quantity { get; set; }
        public decimal subtotal 
        { 
            get
            {
                return item.price * quantity;
            }
                
        }
    }
}
