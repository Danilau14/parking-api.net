namespace ParkingApi.Core.Interfaces;

public interface IParkingLotRepositoryDynamo : IBaseRepositoryDynamo<ParkingLotDynamo>
{
    Task<ParkingLotDynamo> CreateParkingLot(ParkingLotDynamo parkingLot);
    Task<ParkingLotDynamo?> FindByParkingLotAndUser(int parkingLotId, int partnerId);
    Task<ParkingLotDynamo> UpdatedParkingLot(ParkingLotDynamo parkingLot);
    Task<(List<ParkingLotDynamo>, int)> FindAndCountAsync(int page, int limit, int? userId = null);
}
