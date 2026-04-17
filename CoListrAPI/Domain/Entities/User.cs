using System.ComponentModel.DataAnnotations;

namespace CoListrAPI.Domain.Entities
{
    public class User : BaseEntity
    {
        [Required]
        [MaxLength(250)]
        public required string FirstName { get; set; }
        [Required]
        [MaxLength(250)]
        public required string LastName { get; set; }
        [Required]
        [MaxLength(250)]
        public required string Username { get; set; }
        [Required]
        public required string PasswordHash { get; set; }
        [Required]
        [MaxLength(6)]
        public required int ShareCode { get; set; }

        //TODO
        //[Required]
        //[ForeignKey("ProfilePictureId")]
        //public required ProfilePicture ProfilePicture { get; set; }
    }
}
