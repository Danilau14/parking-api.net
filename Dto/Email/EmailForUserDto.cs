using System.ComponentModel.DataAnnotations;

namespace ParkingApi.Dto.Email
{
    public class EmailForUserDto
    {
        [EmailAddress]
        public required string Email { get; set; }

        public required string Message { get; set; }

        public required string Subject { get; set; }
    }
}
