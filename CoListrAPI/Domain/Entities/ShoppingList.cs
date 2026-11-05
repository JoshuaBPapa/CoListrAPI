using System.ComponentModel.DataAnnotations;

namespace CoListrAPI.Domain.Entities
{
    public class ShoppingList : BaseEntityDateTime
    {
        [Required]
        [MaxLength(255)]
        public required string Title { get; set; }
        [Required]
        public Guid CreatedById { get; set; }
        [Required]
        public required string Color { get; set; }
        [Required]
        public required string Icon { get; set; }
    }
}
