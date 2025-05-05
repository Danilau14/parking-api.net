using ParkingApi.Core.Models;

namespace ParkingApi.Core.Interfaces;

public interface IJwtTokenGenerator
{
    string GenerateToken(User user);
}
