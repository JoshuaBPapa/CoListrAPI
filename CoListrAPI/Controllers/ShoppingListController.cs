using CoListrAPI.DTOs.ShoppingList;
using CoListrAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CoListrAPI.Controllers
{
    [Route("api/shopping-lists")]
    [ApiController]
    [Authorize]
    public class ShoppingListController : ControllerBase
    {
        private readonly IShoppingListService _shoppingListService;

        public ShoppingListController(IShoppingListService shoppingListService)
        {
            _shoppingListService = shoppingListService;
        }

        [HttpPost]
        public async Task<ActionResult<Guid>> Add(AddShoppingListRequestDto dto, CancellationToken ct)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value!;

            var result = await _shoppingListService.CreateShoppingListAsync(dto, Guid.Parse(userIdClaim), ct);

            return CreatedAtAction(
                nameof(GetShoppingList),
                new { shoppingListId = result }, new { id = result }
            );
        }

        [HttpGet]
        public async Task<ActionResult<List<ShoppingListResponseDto>>> GetShoppingLists(CancellationToken ct)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value!;

            var result = await _shoppingListService.GetShoppingListsAsync(userIdClaim, ct);
            
            return Ok(result.Value);
        }

        [HttpGet("{shoppingListId}", Name = "GetShoppingList")]
        public async Task<ActionResult<ShoppingListResponseDto>> GetShoppingList(
            string shoppingListId, 
            CancellationToken ct
        )
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value!;

            var result = await _shoppingListService.GetShoppingListAsync(shoppingListId, userIdClaim, ct);

            if (result.Error == "Shopping list not found") return NotFound();

            return Ok(result.Value);
        }
    }
}
