namespace ParkingApi.Interfaces;

public interface IParkingHistoryRepository : IBaseRepository<ParkingHistory>
{
    Task<ParkingHistory> CreateParkingHistory(ParkingHistory parkingHistory);

    Task<ParkingHistory?> FindOneParkingHistoryOpen(int vehicleId, int parkingLotId);

    Task<ParkingHistory> UpdateTimeAndCostInParkingHistory(
            float costPerHour,
            ParkingHistory parkingHistory
        );

    Task<ParkingHistory> UpdateParkingHistory(ParkingHistory parkingHistory);

    Task<(List<ParkingHistory>, int)> FindVehiclesByParkingLot(
            int page = 1,
            int limit = 10,
            int? parkingLotId = null
       );
}
