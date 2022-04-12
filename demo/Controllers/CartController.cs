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
        public async Task<ActionResult<CartDto>> getCart()
        {
            return createCart().asDto();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CartDto>> getCartById(Guid id)
        {
            Cart? cart = await _context.carts.FindAsync(id);

            if (cart == null)
            {
                return NotFound();
            }

            return cart.asDto();
        }

        [HttpPost]
        public async Task<ActionResult<CartDto>> addToCart([FromBody] CreateCartItemDto newCartItem)
        {
            Item? item = await _context.items.FindAsync(newCartItem.itemId);

            if (item == null)
            {
                return NotFound("item not found");
            }

            Cart currentCart = Cart.getInstance();

            if (currentCart.id == Guid.Empty)
            {
                currentCart = createCart();
            }

            Cart cart = _context.carts
                        .Include(x => x.items)
                        .ThenInclude(x => x.item)
                        .Single(c => c.id == currentCart.id);

            CartItem? existingCartItem = cart.items.SingleOrDefault(cartItem => cartItem.itemId == newCartItem.itemId);

            if (existingCartItem == null)
            {
                CartItem cartItem = new CartItem()
                {
                    cartId = cart.id,
                    cart = cart,
                    item = item,
                    itemId = item.id
                };
                
                cart.items.Add(cartItem);

            } else
            {
                cart.items.Single(cartItem => cartItem.id == existingCartItem.id ).increaseQuantity();
            }

            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(getCartById), new { id = cart.id }, cart.asDto());
        }

        private Cart createCart()
        {
            Cart cart = Cart.getInstance();

            if (cart.id != Guid.Empty)
            {
                return cart;
            }

            _context.carts.Add(cart);
            _context.SaveChanges();

            cart.update(cart);

            return cart;
        }
    }
}
