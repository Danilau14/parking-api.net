namespace ParkingApi.Application.Features.ParkingHistories.Commands;

public record CreateParkingHistoryCommand(string LicensePlate, int ParkingLotId) : IRequest<ParkingHistory>;
