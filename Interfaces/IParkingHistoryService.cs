using ParkingApi.Dto.ParkingHistory;
using ParkingApi.Models;

namespace ParkingApi.Interfaces;

public interface IParkingHistoryService
{
    Task<ParkingHistory> CreateParkingHistory(
        CreateParkingHistoryDto createParkingHistoryDto,
        int partnerId
        );

    Task<ParkingHistory> CloseParkingHistory(CreateParkingHistoryDto createParkingHistoryDto);
}
