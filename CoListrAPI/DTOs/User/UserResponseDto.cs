using System.ComponentModel.DataAnnotations;

namespace CoListrAPI.DTOs.User
{
    public class UserResponseDto
    {
        public required Guid Id { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string Username { get; set; }
        public required int ShareCode { get; set; }
        //TODO
        //public required ProfilePicture ProfilePicture { get; set; }
    }
}
