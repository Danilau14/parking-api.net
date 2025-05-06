namespace ParkingApi.Core.Interfaces;

public interface IUnitOfWork
{
    ApplicationDbContext Context { get; }
    IUserRepository UserRepository { get; }
    IRevokedTokenRepository RevokedTokenRepository { get; }
    IParkingLotRepository ParkingLotRepository { get; }
    IParkingHistoryRepository ParkingHistoryRepository { get; }
    IVehicleRepository VehicleRepository { get; }
}
