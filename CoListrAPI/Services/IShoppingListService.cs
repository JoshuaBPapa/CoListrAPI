using CoListrAPI.Domain.Result;
using CoListrAPI.DTOs.ShoppingList;

namespace CoListrAPI.Services
{
    public interface IShoppingListService
    {
        Task<Guid> CreateShoppingListAsync(AddShoppingListRequestDto dto, Guid createdById, CancellationToken cancellationToken);
        Task<Result<List<ShoppingListResponseDto>>> GetShoppingListsAsync(string shoppingListId, CancellationToken cancellationToken);
        Task<Result<ShoppingListResponseDto>> GetShoppingListAsync(string shoppingListId, string userId, CancellationToken cancellationToken);
    }
}
