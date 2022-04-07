using demo.Models;
using demo.Dto.Items;

namespace demo.Extensions.Items
{
    public static class Extension
    {
        public static ItemDto asDto(this Item item)
        {
            return new ItemDto
            {
                id = item.id,
                name = item.name,
                price = item.price,
                createdDate = item.createdDate,
            };
        }
    }
}
