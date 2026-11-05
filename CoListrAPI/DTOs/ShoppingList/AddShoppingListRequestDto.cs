using System.ComponentModel.DataAnnotations;

namespace CoListrAPI.DTOs.ShoppingList
{
    public class AddShoppingListRequestDto
    {
        [MaxLength(255)]
        public required string Title { get; set; }
        public required string Color { get; set; }
        public required string Icon { get; set; }
        public Guid[] MemberIds { get; set; } = [];
    }
}
