using Xunit;
using Moq;
using demo.Context;
using demo.Models;
using demo.Controllers;
using demo.Dto.Items;
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
    public class ItemsControllerTests
    {
        private readonly Random rng = new Random();

        [Fact]
        public async Task getItem_WithUnexistingItem_ReturnsNotFound()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase("getItem_UnexistingItem")
                .Options
            ;


            var context = new AppDbContext(options);
            context.items.Add(createRandomItem());
            context.SaveChanges();

            var controller = new ItemsController(context);

            // Act
            var result = await controller.getItem(Guid.NewGuid());

            // Assert
            Assert.IsType<NotFoundObjectResult>(result.Result);

            /* Does the same as the above */
            result.Result.Should().BeOfType<NotFoundObjectResult>();
        }

        [Fact]
        public async Task getItem_WithExistingItem_ReturnsExpectedItem()
        {
            // Arrange
            var expectedItem = createRandomItem();
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase("getItem_ExistingItem")
                .Options
            ;
            var context = new AppDbContext(options);
            context.items.Add(expectedItem);
            context.SaveChanges();

            var controller = new ItemsController(context);

            // Act
            var result = await controller.getItem(expectedItem.id);
            var dto = (result as ActionResult<ItemDto>).Value;

            // Assert
            result.Value.Should().BeEquivalentTo(
                expectedItem,
                options => options.ComparingByMembers<Item>()
            );
        }

        [Fact]
        public async Task getItems_WithExistingItems_ReturnsAllItems()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase("getItems_ExistingItems")
                .Options
            ;
            var context = new AppDbContext(options);

            for (int i = 0; i < 10; i++)
            {
                Item createdItem = createRandomItem();
                context.items.Add(createdItem);
            }

            context.SaveChanges();

            var controller = new ItemsController(context);

            // Act
            var result = await controller.getItems();

            // Assert
            var model = Assert.IsAssignableFrom<IEnumerable<ItemDto>>(result);
            Assert.Equal(10, model.Count());
        }

        [Fact]
        public async Task createItem_WithItemToCreate_ReturnsCreatedItem()
        {
            // Arrange
            var itemToCreate = new CreateItemDto()
            {
                name = Guid.NewGuid().ToString(),
                price = rng.Next(1000)
            };

            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase("createItem")
                .Options
            ;

            var context = new AppDbContext(options);
            var controller = new ItemsController(context);

            // Act
            var result = await controller.createItem(itemToCreate);

            // Assert
            var createdItem = (result.Result as CreatedAtActionResult).Value as ItemDto;
            itemToCreate.Should().BeEquivalentTo(
                createdItem,
                options => options.ComparingByMembers<ItemDto>().ExcludingMissingMembers()
            );
            createdItem.id.Should().NotBeEmpty();
            createdItem.createdDate.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
        }

        [Fact]
        public async Task updateItem_WithExistingItem_ReturnsNoContent()
        {
            // Arrange
            Item existingItem = createRandomItem();
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase("updateItem")
                .Options
            ;
            var context = new AppDbContext(options);
            var controller = new ItemsController(context);
            
            context.items.Add(existingItem);
            context.SaveChanges();

            var itemId = existingItem.id;
            var itemToUpdate = new UpdateItemDto()
            {
                name = Guid.NewGuid().ToString(),
                price = existingItem.price + 3,
            };

            // Act
            var result = await controller.updateItem(itemId, itemToUpdate);

            // Assert
            result.Should().BeOfType<NoContentResult>();
        }

        [Fact]
        public async Task updateItem_WithUnexistingItem_ReturnsNotFound()
        {
            // Arrange
            Item existingItem = createRandomItem();
            
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase("updateItem_unexistingItem")
                .Options
            ;


            var context = new AppDbContext(options);
            context.items.Add(existingItem);
            context.SaveChanges();

            var itemId = existingItem.id;
            var itemToUpdate = new UpdateItemDto()
            {
                name = Guid.NewGuid().ToString(),
                price = existingItem.price + 3,
            };

            var controller = new ItemsController(context);

            // Act
            var result = await controller.updateItem(Guid.NewGuid(), itemToUpdate);

            // Assert
            result.Should().BeOfType<NotFoundObjectResult>();
        }

        [Fact]
        public async Task deleteItem_WithExistingItem_ReturnsNoContent()
        {
            // Arrange
            Item existingItem = createRandomItem();
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase("deleteItem")
                .Options
            ;
            var context = new AppDbContext(options);
            var controller = new ItemsController(context);

            context.items.Add(existingItem);
            context.SaveChanges();

            var itemId = existingItem.id;

            // Act
            var result = await controller.deleteItem(itemId);

            // Assert
            result.Should().BeOfType<NoContentResult>();
        }

        [Fact]
        public async Task deleteItem_WithUnexistingItem_ReturnsNotFound()
        {
            // Arrange
            Item existingItem = createRandomItem();

            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase("deleteItem_unexistingItem")
                .Options
            ;


            var context = new AppDbContext(options);
            context.items.Add(existingItem);
            context.SaveChanges();

            var itemId = existingItem.id;

            var controller = new ItemsController(context);

            // Act
            var result = await controller.deleteItem(Guid.NewGuid());

            // Assert
            result.Should().BeOfType<NotFoundObjectResult>();
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