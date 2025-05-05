namespace ParkingApi.Core.Interfaces;

public interface IUserContextService
{
    int? GetCurrentUserId();
    string? GetCurrentEmail();
}
