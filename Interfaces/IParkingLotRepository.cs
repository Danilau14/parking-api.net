using ParkingApi.Models;

namespace ParkingApi.Interfaces;

public interface IParkingLotRepository : IBaseRepository<ParkingLot>
{
    Task<ParkingLot> CreateParkingLot(ParkingLot parkingLot);
}
