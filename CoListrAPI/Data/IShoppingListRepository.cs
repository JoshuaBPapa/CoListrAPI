using CoListrAPI.Domain.Entities;
using CoListrAPI.DTOs.ShoppingList;

namespace CoListrAPI.Data
{
    public interface IShoppingListRepository : IRepository<ShoppingList>
    {
        Task<ShoppingListResponseDto?> GetShoppingListDetailsByIdAsync(string shoppingListId, string userId, CancellationToken ct);
        Task<List<ShoppingListResponseDto>> GetShoppingListsAsync(string userId, CancellationToken ct);
        Task<Guid> AddShoppingListAsync(ShoppingList shoppingList, List<Guid> shoppingListMembers, CancellationToken ct);
    }
}
