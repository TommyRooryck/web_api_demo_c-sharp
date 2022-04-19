using demo.Dto.Orders;
using demo.Models;

namespace demo.Extensions.Orders
{
    public static class Extension
    {
        public static OrderDto asDto(this Order order)
        {
            return new OrderDto
            {
                id = order.Id,
                cartId = order.cartId,
                createdAt = order.createdDate
            };
        }
    }
}
