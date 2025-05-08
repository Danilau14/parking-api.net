namespace ParkingApi.Application.Features.ParkingLots.Commands;

public record UpdateParkingLotCommand(string id, UpdatedParkingLotDto UpdatedParkingLotDto) : IRequest<ParkingLotDynamo>;
