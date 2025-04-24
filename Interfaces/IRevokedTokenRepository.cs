using ParkingApi.Models;

namespace ParkingApi.Interfaces;

public interface IRevokedTokenRepository : IBaseRepository<RevokedToken>
{
    Task<bool> IsTokenRevoked(string token);

    Task<bool> SaveTokenRevoked(RevokedToken revokedToken);
}
