namespace ParkingApi.Application.Features.Auth.Commands;

public record LogoutCommand(string token) : IRequest<bool>;
