namespace ParkingApi.Core.Interfaces;

public interface IUnitOfWork
{
    ApplicationDbContext Context { get; }
    IUserRepository UserRepository { get; }
    IUserRepositoryDynamo UserRepositoryDynamo { get; }
    IRevokedTokenRepository RevokedTokenRepository { get; }
    IParkingLotRepositoryDynamo ParkingLotRepositoryDynamo { get; }
    IParkingLotRepository ParkingLotRepository { get; }
    IParkingHistoryRepository ParkingHistoryRepository { get; }
    IVehicleRepository VehicleRepository { get; }
}
