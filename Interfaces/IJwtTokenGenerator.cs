namespace ParkingApi.Interfaces;

public interface IJwtTokenGenerator
{
    string GenerateToken(User user);
}
