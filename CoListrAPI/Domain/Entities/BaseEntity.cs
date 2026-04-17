using System.ComponentModel.DataAnnotations.Schema;

namespace CoListrAPI.Domain.Entities
{
    public class BaseEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
    }
}
