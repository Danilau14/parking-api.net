using ParkingApi.Models;

namespace ParkingApi.Interfaces;

public interface IParkingLotService
{
    Task<ParkingLot> CreateParkigLotAsync(ParkingLot parkingLot);
}
