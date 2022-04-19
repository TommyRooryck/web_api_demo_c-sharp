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
            Item item2 = createRandomItem();
            context.items.Add(item);
            context.items.Add(item2);
            await context.SaveChangesAsync();

            var controller = new CartController(context);

            // Act
            var result = await controller.addToCart(
                new CreateCartItemDto()
                {
                    itemId = item.id
                }
            );

            result = await controller.addToCart(
                new CreateCartItemDto()
                {
                    itemId = item2.id
                }
            );

            var dto = (result.Result as CreatedAtActionResult).Value as CartDto;

            // Assert
            result.Result.Should().NotBeNull();
            dto.id.Should().NotBeEmpty();
            dto.items.Should().HaveCount(2);
        }

        [Fact]
        public async Task addToCart_existingItem_shouldIncreaseQuantity()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase("addToCart_existingItem_shouldIncreaseQuantity")
                .Options
            ;
            var context = new AppDbContext(options);

            Item item = createRandomItem();
            context.items.Add(item);
            await context.SaveChangesAsync();

            var controller = new CartController(context);

            // Act
            var result = await controller.addToCart(
                new CreateCartItemDto()
                {
                    itemId = item.id
                }
            );

            result = await controller.addToCart(
                new CreateCartItemDto()
                {
                    itemId = item.id
                }
            );

            var dto = (result.Result as CreatedAtActionResult).Value as CartDto;

            // Assert
            result.Result.Should().NotBeNull();
            dto.id.Should().NotBeEmpty();
            dto.items.Should().HaveCount(1);
            dto.items.First().quantity.Should().Be(2);
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
                name = Guid.NewGuid().ToString(),
                price = rng.Next(1000),
                createdDate = DateTime.UtcNow,
            };
        }
    }
}
