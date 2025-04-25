using ParkingApi.Models;

namespace ParkingApi.Interfaces;

public interface IParkingLotRepository : IBaseRepository<ParkingLot>
{
    Task<ParkingLot> CreateParkingLot(ParkingLot parkingLot);
    Task<ParkingLot?> FindByParkingLotAndUser(int parkingLotId, int partnerId);
    Task<ParkingLot> UpdatedParkingLot(ParkingLot parkingLot);
}
