using CoListrAPI.Domain.Entities;
using CoListrAPI.DTOs.ShoppingList;
using CoListrAPI.DTOs.User;
using Microsoft.EntityFrameworkCore;

namespace CoListrAPI.Data
{
    public class ShoppingListRepository : Repository<ShoppingList>, IShoppingListRepository
    {
        public ShoppingListRepository(CoListrDbContext context) : base(context)
        {
        }

        public async Task<Guid> AddShoppingListAsync(
            ShoppingList shoppingList, 
            List<Guid> shoppingListMembers, 
            CancellationToken cancellationToken
        )
        {
            _context.ShoppingLists.Add(shoppingList);

            await _context.SaveChangesAsync(cancellationToken);

            foreach (var member in shoppingListMembers)
            {
                var shoppingListUser = new ShoppingListUser
                {
                    UserId = member,
                    ShoppingListId = shoppingList.Id
                };
                _context.ShoppingListUser.Add(shoppingListUser);
            }

            await _context.SaveChangesAsync(cancellationToken);

            return shoppingList.Id;
        }

        public async Task<List<ShoppingListResponseDto>> GetShoppingListsAsync(
            string userId, 
            CancellationToken cancellationToken
        )
        {
            var userGuid = Guid.Parse(userId);

            return await (
                from shoppingList in _context.ShoppingLists
                where shoppingList.CreatedById == userGuid ||
                    _context.ShoppingListUser.Any(slu => slu.ShoppingListId == shoppingList.Id && slu.UserId == userGuid)

                join createdBy in _context.Users
                    on shoppingList.CreatedById equals createdBy.Id

                select new ShoppingListResponseDto
                {
                    Id = shoppingList.Id,
                    Title = shoppingList.Title,
                    CreatedAt = shoppingList.CreatedAt,
                    UpdatedAt = shoppingList.UpdatedAt,
                    Color = shoppingList.Color,
                    Icon = shoppingList.Icon,
                    CreatedBy = new UserResponseDto
                    {
                        Id = createdBy.Id,
                        Username = createdBy.Username,
                        FirstName = createdBy.FirstName,
                        LastName = createdBy.LastName,
                        ShareCode = createdBy.ShareCode
                    },

                    TotalItemCount = 0,
                    CompletedItemCount = 0,

                    Members = (
                        from shoppingListUser in _context.ShoppingListUser
                        join user in _context.Users
                            on shoppingListUser.UserId equals user.Id
                        where shoppingListUser.ShoppingListId == shoppingList.Id
                        select new UserResponseDto
                        {
                            Id = user.Id,
                            Username = user.Username,
                            FirstName = user.FirstName,
                            LastName = user.LastName,
                            ShareCode = user.ShareCode
                        }
                    ).ToList()
                }
            ).ToListAsync(cancellationToken);
        }

        public async Task<ShoppingListResponseDto?> GetShoppingListDetailsByIdAsync(
            string shoppingListId, 
            string userId,
            CancellationToken cancellationToken
        )
        {
            var id = Guid.Parse(shoppingListId);
            var userGuid = Guid.Parse(userId);

            return await (
                from shoppingList in _context.ShoppingLists
                where shoppingList.Id == id && 
                    (
                        shoppingList.CreatedById == userGuid ||
                        _context.ShoppingListUser.Any(slu => slu.ShoppingListId == shoppingList.Id && slu.UserId == userGuid)
                    )

                join createdBy in _context.Users
                    on shoppingList.CreatedById equals createdBy.Id

                select new ShoppingListResponseDto
                {
                    Id = shoppingList.Id,
                    Title = shoppingList.Title,
                    CreatedAt = shoppingList.CreatedAt,
                    UpdatedAt = shoppingList.UpdatedAt,
                    Color = shoppingList.Color,
                    Icon = shoppingList.Icon,
                    CreatedBy = new UserResponseDto
                    {
                        Id = createdBy.Id,
                        Username = createdBy.Username,
                        FirstName = createdBy.FirstName,
                        LastName = createdBy.LastName,
                        ShareCode = createdBy.ShareCode
                    },

                    TotalItemCount = 0,
                    CompletedItemCount = 0,

                    Members = (
                        from shoppingListUser in _context.ShoppingListUser
                        join user in _context.Users
                            on shoppingListUser.UserId equals user.Id
                        where shoppingListUser.ShoppingListId == shoppingList.Id
                        select new UserResponseDto
                        {
                            Id = user.Id,
                            Username = user.Username,
                            FirstName = user.FirstName,
                            LastName = user.LastName,
                            ShareCode = user.ShareCode
                        }
                    ).ToList()
                }
            ).FirstOrDefaultAsync(cancellationToken);
        }
    }
}
