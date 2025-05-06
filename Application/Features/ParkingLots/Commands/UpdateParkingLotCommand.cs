namespace ParkingApi.Application.Features.ParkingLots.Commands;

public record UpdateParkingLotCommand(int id, UpdatedParkingLotDto UpdatedParkingLotDto) : IRequest<ParkingLotDto>;
