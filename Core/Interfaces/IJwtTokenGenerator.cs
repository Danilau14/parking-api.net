namespace ParkingApi.Core.Interfaces;

public interface IJwtTokenGenerator
{
    string GenerateToken(User user);
}
