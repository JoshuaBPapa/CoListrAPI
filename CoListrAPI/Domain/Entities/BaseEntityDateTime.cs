namespace CoListrAPI.Domain.Entities
{
    public class BaseEntityDateTime : BaseEntity
    {
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
