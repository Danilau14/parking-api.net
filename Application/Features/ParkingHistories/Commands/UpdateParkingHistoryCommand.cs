namespace ParkingApi.Application.Features.ParkingHistories.Commands;

public record UpdateParkingHistoryCommand(string LicensePlate, int ParkingLotId) : IRequest<ParkingHistoryDto>;

