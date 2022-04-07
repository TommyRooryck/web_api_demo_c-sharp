using demo.Models;
using demo.Dto.CartItems;

namespace demo.Extensions.CartItems
{
    public static class Extension
    {
        public static CartItemDto asDto(this CartItem cartItem)
        {
            return new CartItemDto
            {
                item = cartItem.item
            };
        }
    }
}
