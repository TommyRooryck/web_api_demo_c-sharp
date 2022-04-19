using demo.Context;
using demo.Models;
using demo.Enum;
using demo.Extensions.Orders;
using demo.Dto.Orders;
using Microsoft.AspNetCore.Mvc;

namespace demo.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrderController : Controller
    {
        private AppDbContext _context;

        public OrderController(AppDbContext _context)
        {
            this._context = _context;
        }

        [HttpGet]
        public async Task<IEnumerable<OrderDto>> getOrders()
        {
            var orders = (await _context.orders.ToListAsync()).Select(order => order.asDto());

            return orders;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<OrderDto>> getOrder(Guid id)
        {
            var order = await _context.orders.FindAsync(id);

            if (order == null)
            {
                return NotFound("Order was not found");
            }

            return order.asDto();
        }

        [HttpPost]
        public async Task<ActionResult<Order>>createOrder()
        {
            if (Cart.getInstance().id == Guid.Empty) {
                return BadRequest("There are no items in your cart.");
            }

            Cart currentCart = _context.carts
                                .Include(x => x.items)
                                .Single(c => c.id == Cart.getInstance().id);

            if (currentCart == null || currentCart.items.Count() == 0)
            {
                return BadRequest("There are no items in your cart.");
            }

            var order = new Order()
            {
                cart = currentCart,
                cartId = currentCart.id,
                status = Status.NEW
            };

            Cart.getInstance().status = Status.CLOSED;
            currentCart.status = Status.CLOSED;
            await _context.orders.AddAsync(order);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(getOrder), new { id = order.Id }, order.asDto());
        }
    }
}
