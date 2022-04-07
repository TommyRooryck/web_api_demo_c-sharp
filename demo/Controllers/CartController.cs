using demo.Context;
using demo.Models;
using demo.Dto.Items;
using demo.Dto.CartItems;
using demo.Extensions.Items;
using demo.Extensions.Carts;
using demo.Dto.Cart;
using Microsoft.AspNetCore.Mvc;

namespace demo.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CartController: Controller
    {
        private AppDbContext _context;

        public CartController(AppDbContext _context)
        {
            this._context = _context;
        }

        [HttpGet]
        public ActionResult<CartDto> getCart()
        {
            return Cart.getInstance().asDto();
        }

        [HttpPost]
        public async Task<ActionResult<CartDto>> addToCart([FromBody] CreateCartItemDto newCartItem)
        {
            Item? item = await _context.items.FindAsync(newCartItem.itemId);

            if (item == null)
            {
                return NotFound("item not found");
            }

            Cart cart = Cart.getInstance();

            if (cart.id == Guid.Empty)
            {
                _context.carts.Add(cart);
            }


            CartItem cartItem = new CartItem()
            {
                cartId = cart.id,
                cart = cart,
                item = item,
                itemId = item.id
            };

            _context.cartItems.Add(cartItem);


            return await save(cart);
        }

        private async Task<ActionResult<CartDto>> save(Cart cart)
        {
            if (cart.id != Guid.Empty)
            {
                var existingCart = await _context.carts.FindAsync(cart.id);
                _context.Entry<Cart>(existingCart).CurrentValues.SetValues(cart);
            }
            else
            {
                _context.Add(cart);
                await _context.SaveChangesAsync();
            }

            return CreatedAtAction(nameof(getCart), cart.asDto());
        }
    }
}
