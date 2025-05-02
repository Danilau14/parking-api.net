namespace ParkingApi.Application.Features.ParkingLots.Commands;

public record  FindOneByIdParkingLotCommand(int Id) : IRequest<ParkingLot>;
