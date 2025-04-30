using ParkingApi.Enums;

namespace ParkingApi.Dto.Users;

public class CreateUserDto
{
    public required string _email;

    [MinLength(6, ErrorMessage = "The password legt 6 charts")]
    public required string Password { get; set; }

    [EmailAddress]
    public required string Email {
        get => _email;
        set => _email = value.ToLower();
    }

    public UserRole Role { get; set; } = UserRole.PARTNER;
}
