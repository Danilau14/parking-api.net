namespace ParkingApi.Application.Features.Users.Commands;

public record RegisterUserCommand(string Email, string Password, UserRole Role) : IRequest<UserDynamo>;

