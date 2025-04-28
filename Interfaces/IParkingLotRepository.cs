namespace ParkingApi.Interfaces;

public interface IParkingLotRepository : IBaseRepository<ParkingLot>
{
    Task<ParkingLot> CreateParkingLot(ParkingLot parkingLot);
    Task<ParkingLot?> FindByParkingLotAndUser(int parkingLotId, int partnerId);
    Task<ParkingLot> UpdatedParkingLot(ParkingLot parkingLot);
    Task<(List<ParkingLot>, int)> FindAndCountAsync(int page, int limit, int? userId = null); 
}
