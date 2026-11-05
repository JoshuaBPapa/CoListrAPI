using CoListrAPI.DTOs.User;

namespace CoListrAPI.DTOs.ShoppingList
{
    public class ShoppingListResponseDto
    {
        public required Guid Id { get; set; }
        public required string Title { get; set; }
        public required string Color { get; set; }
        public required string Icon { get; set; }
        public required List<UserResponseDto> Members { get; set; }
        public required DateTime CreatedAt { get; set; }
        public required DateTime UpdatedAt { get; set; }
        public required UserResponseDto CreatedBy { get; set; }
        public required int TotalItemCount { get; set; }
        public required int CompletedItemCount { get; set; }
    }
}
