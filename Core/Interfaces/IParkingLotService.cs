using ParkingApi.Core.Models;

namespace ParkingApi.Core.Interfaces;

public interface IParkingLotService
{
    Task<ParkingLot> CreateParkigLotAsync(ParkingLot parkingLot);
}
