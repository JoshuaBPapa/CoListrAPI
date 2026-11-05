using CoListrAPI.Data;
using CoListrAPI.Domain.Entities;
using CoListrAPI.Domain.Result;
using CoListrAPI.DTOs.ShoppingList;

namespace CoListrAPI.Services
{
    public class ShoppingListService : IShoppingListService
    {
        private readonly IShoppingListRepository _shoppingLists;

        public ShoppingListService (IShoppingListRepository shoppingLists)
        {
            _shoppingLists = shoppingLists;
        }

        public async Task<Guid> CreateShoppingListAsync(
            AddShoppingListRequestDto dto, 
            Guid createdById, 
            CancellationToken cancellationToken
        )
        {
            var shoppingList = new ShoppingList
            {
                Title = dto.Title,
                Color = dto.Color,
                Icon = dto.Icon,
                CreatedById = createdById
            };

            var memberIds = new List<Guid>(dto.MemberIds);

            var shoppingListId = await _shoppingLists.AddShoppingListAsync(shoppingList, memberIds, cancellationToken);

            return shoppingListId;
        }

        public async Task<Result<List<ShoppingListResponseDto>>> GetShoppingListsAsync(
            string userId, 
            CancellationToken cancellationToken
        )
        {
            var shoppingLists = await _shoppingLists.GetShoppingListsAsync(userId, cancellationToken);

            return new Result<List<ShoppingListResponseDto>>
            {
                IsSuccess = true,
                Value = shoppingLists,
                Error = null
            };
        }

        public async Task<Result<ShoppingListResponseDto>> GetShoppingListAsync(
            string shoppingListId,
            string userId,
            CancellationToken cancellationToken
        )
        {
            var shoppingList = await _shoppingLists.GetShoppingListDetailsByIdAsync(shoppingListId, userId, cancellationToken);

            if (shoppingList == null)
            {
                return new Result<ShoppingListResponseDto>
                {
                    IsSuccess = false,
                    Value = null,
                    Error = "Shopping list not found"
                };
            }

            return new Result<ShoppingListResponseDto>
            {
                IsSuccess = true,
                Value = shoppingList,
                Error = null
            };
        }
    }
}
