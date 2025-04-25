using ParkingApi.Models;

namespace ParkingApi.Interfaces;

public interface IParkingHistoryRepository : IBaseRepository<ParkingHistory>
{
    Task<ParkingHistory> CreateParkingHistory(ParkingHistory parkingHistory);

    Task<ParkingHistory?> FindOneParkingHistoryOpen(int vehicleId, int parkingLotId);
}
