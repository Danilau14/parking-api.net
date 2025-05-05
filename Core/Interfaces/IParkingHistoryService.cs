using ParkingApi.Core.Models;

namespace ParkingApi.Core.Interfaces;

public interface IParkingHistoryService
{
    Task<ParkingHistory> CreateParkingHistory(
        CreateParkingHistoryDto createParkingHistoryDto,
        int partnerId
        );

    Task<ParkingHistory> CloseParkingHistory(CreateParkingHistoryDto createParkingHistoryDto);
}
