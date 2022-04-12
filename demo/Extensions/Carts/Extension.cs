using demo.Models;
using demo.Dto.Cart;

namespace demo.Extensions.Carts
{
    public static class Extension
    {
        public static CartDto asDto(this Cart cart)
        {
            return new CartDto
            {
                id = cart.id,
                items = cart.items
            };
        }
    }
}
