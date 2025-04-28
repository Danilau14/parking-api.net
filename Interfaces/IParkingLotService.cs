namespace ParkingApi.Interfaces;

public interface IParkingLotService
{
    Task<ParkingLot> CreateParkigLotAsync(ParkingLot parkingLot);
}
