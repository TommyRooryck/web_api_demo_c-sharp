using Xunit;
using Moq;
using demo.Context;
using demo.Models;
using demo.Dto.CartItems;
using demo.Controllers;
using demo.Dto.Cart;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Collections.Generic;
using System;
using Microsoft.AspNetCore.Mvc;
using FluentAssertions;
using System.Collections;

namespace demo.Tests
{
    public class CartControllerTest
    {
        private readonly Random rng = new Random();

        [Fact]
        public async Task getCart_ReturnsCart()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase("getCart_returnsCart")
                .Options
            ;
            var context = new AppDbContext(options);

            var controller = new CartController(context);

            // Act
            var result = await controller.getCart();
            var dto = (result as ActionResult<CartDto>).Value;

            // Assert
            result.Value.Should().NotBeNull();
            result.Value.Should().BeOfType<CartDto>();
        }

        [Fact]
        public async Task addToCart_newItem_ReturnsExpectedCart()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase("addToCart_newItem_ReturnsExpectedCart")
                .Options
            ;
            var context = new AppDbContext(options);

            Item item = createRandomItem();
            context.items.Add(item);
            await context.SaveChangesAsync();

            Cart cart = new Cart();
            context.carts.Add(cart);
            await context.SaveChangesAsync();

            CartItem cartItem = createCartItem(cart, item);
            Cart dbCart = context.carts
                     .Include(x => x.items)
                     .ThenInclude(x => x.item)
                     .Single(c => c.id == cart.id);

            dbCart.items.Add(cartItem);
            await context.SaveChangesAsync();

            var controller = new CartController(context);

            // Act
            var result = await controller.addToCart(
                new CreateCartItemDto()
                {
                    itemId = item.id
                }
            );

            var dto = (result.Result as CreatedAtActionResult).Value as CartDto;

            // Assert
            result.Value.Should().BeOfType<CartDto>();
            result.Value.Should().BeEquivalentTo(
                dbCart,
                options => options.ComparingByMembers<Cart>().ExcludingMissingMembers()
            );
        }

        private CartItem createCartItem(Cart cart, Item item)
        {
            return new CartItem()
            {
                cart = cart,
                item = item,
                cartId = cart.id,
                itemId = item.id
            };
        }

        private Item createRandomItem()
        {
            return new Item()
            {
                id = Guid.NewGuid(),
                name = Guid.NewGuid().ToString(),
                price = rng.Next(1000),
                createdDate = DateTime.UtcNow,
            };
        }
    }
}
